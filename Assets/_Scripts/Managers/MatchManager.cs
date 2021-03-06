using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.Events;
using Cinemachine;

/// <summary>
/// Manages the beginning and end of an arena match start. On start, all players are teleported to the area where a countdown begins. Then the players try to reach the goal as fast as they can. The first player is the winner and gets a distribution from the betting pool funds.
/// </summary>
public class MatchManager : NetworkBehaviour
{
    public static MatchManager Instance { get; private set; }

    [SerializeField]
    private Transform hubSpawn;
    [SerializeField]
    private UnityEvent MatchStarted;
    [SerializeField]
    private UnityEvent<string> GameOver;
    [SerializeField]
    private List<Transform> spawnPoints;
    [SerializeField, Tooltip("Used to follow multiple Transforms, in this case players")]
    private CinemachineTargetGroup cinemachineTargetGroup;
    // [SyncObject, Tooltip("List of players in the queue")]
    // public readonly SyncList<Player> winners = new();
    [SyncObject, Tooltip("List of players in the queue")]
    public readonly SyncList<Player> players = new();

    private void Awake() {
        Instance = this;
    }

    [ContextMenu("Start Match")]
    public void StartMatchInspector() {
        StartMatchAllClients();
    }

    [ObserversRpc]
    private void StartMatchAllClients() {
        StartCoroutine(StartMatch(QueueManager.Instance.queue));
    }

    public IEnumerator StartMatch(SyncList<Player> queue) {
        // Set list of players to access after resetting the queue.
        // SyncList<Player> playerList = queue;
        
        // Teleport all of the players to the arena
        int index = 0;
        foreach (Player player in queue)
        {
            player.controlledPawn.controller.enabled = false;
            player.controlledPawn.character.transform.position = spawnPoints[index++].position;
            player.controlledPawn.character.transform.LookAt(Vector3.forward);
            players.Add(player);
            cinemachineTargetGroup.AddMember(player.controlledPawn.character.transform, 1, 2);
        }

        // // Reset the queue
        // QueueManager.Instance.ResetQueue();

        // Start Countdown
        yield return new WaitForSeconds(10);

        // Once countdown is done, enable movement
        // TODO - race condition. Make sure others can't join the queue or have 2 different queues.
        foreach (Player player in queue)
        {
            player.controlledPawn.controller.enabled = true;
        }

        // Reset the queue
        QueueManager.Instance.ResetQueue();

        MatchStarted.Invoke();
    }

    [Server]
    public void AddWinner(Player winner) {
        // Stop the game
        foreach (Player player in players)
        {
            player.controlledPawn.controller.enabled = false;
            player.controlledPawn.character.transform.position = hubSpawn.position;
            player.controlledPawn.character.transform.LookAt(Vector3.forward);
            players.Add(player);
            cinemachineTargetGroup.RemoveMember(player.controlledPawn.character.transform);
        }

        // Call the bet contract to distribute funds
        GameOver.Invoke(winner.userAddress);
    }

}
