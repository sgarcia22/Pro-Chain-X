using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.Events;

/// <summary>
/// Manages the beginning and end of an arena match start. On start, all players are teleported to the area where a countdown begins. Then the players try to reach the goal as fast as they can. The first player is the winner and gets a distribution from the betting pool funds.
/// </summary>
public class MatchManager : NetworkBehaviour
{
    public static MatchManager Instance { get; private set; }

    [SerializeField]
    private UnityEvent MatchStarted;
    [SerializeField]
    private List<Transform> spawnPoints;
    [SyncObject, Tooltip("List of players in the queue")]
    public readonly SyncList<Player> winners = new();

    private void Awake() {
        Instance = this;
    }

    [ContextMenu("Start Match")]
    private void StartMatchInspector() {
        StartMatchAllClients();
    }

    [ObserversRpc]
    private void StartMatchAllClients() {
        StartCoroutine(StartMatch(QueueManager.Instance.queue));
    }

    public IEnumerator StartMatch(SyncList<Player> queue) {
        // Teleport all of the players to the arena
        int index = 0;
        foreach (Player player in queue)
        {
            player.controlledPawn.controller.enabled = false;
            player.controlledPawn.character.transform.position = spawnPoints[index++].position;
            player.controlledPawn.character.transform.LookAt(Vector3.forward);
        }

        // Start Countdown
        yield return new WaitForSeconds(10);

        // Once countdown is done, enable movement
        foreach (Player player in queue)
        {
            player.controlledPawn.controller.enabled = true;
        }

        MatchStarted.Invoke();
    }

    [Server]
    public void AddWinner(Player player) {
        winners.Add(player);
        // Stop the game
        
        // Call the bet contract to distribute funds

    }

}
