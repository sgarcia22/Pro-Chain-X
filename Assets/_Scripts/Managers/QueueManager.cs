using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using UnityEngine.Events;
using FishNet.Connection;

/// <summary>
/// Manages server and client side queue logic. Players click on the queue column to get added to the queue for the next Arena match if they have the Arena NFT for access.
/// </summary>
public class QueueManager : NetworkBehaviour
{
    public static QueueManager Instance { get; private set; }

    // TODO - set private
    [SyncObject, Tooltip("List of players in the queue")]
    public readonly SyncList<Player> queue = new();

    [SyncVar]
    public int maximumAmountOfPlayers = 10;

    [SyncVar]
    public int roundTimeInSeconds = 300;

    [SyncVar]
    public int currentCountdownTime;

    [SerializeField]

    private WaitForSeconds waitForSeconds;

    [SerializeField]
    private UnityEvent AddToQueueEvent;
    [SerializeField]
    private UnityEvent<int> countdownTime;
    

    private void Awake() {
        Instance = this;
        waitForSeconds = new WaitForSeconds(roundTimeInSeconds);
    }


    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(MatchLogic());
        StartCoroutine(Countdown(roundTimeInSeconds));
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddToQueue(Player player) {
        if (!player.inQueue && queue.Count < maximumAmountOfPlayers) {
            queue.Add(player);
            AfterAddToQueue(player);
        }
    }

    [ObserversRpc]
    private void AfterAddToQueue(Player player) {
        Debug.LogFormat("IN QUEUE {0}",queue.Count);
        Debug.Log("Added player to queue");
        // TODO - update client
        NotifyPlayerAddedToQueue(player.Owner);
        player.inQueue = true;
    }

    [TargetRpc]
    private void NotifyPlayerAddedToQueue(NetworkConnection conn) {
        AddToQueueEvent.Invoke();
    }

    // Queue logic
    [Server]
    private IEnumerator MatchLogic() {
        while (true) {
            yield return waitForSeconds;
            if (queue.Count >= 2) {
                // Only start the match if there are two or more players, otherwise restart the timer.
                MatchManager.Instance.StartMatch(queue);
            }
        }
    }
 
    IEnumerator Countdown (int seconds) {
        while (true) {
            currentCountdownTime = seconds;
            SendCountdownEvent();
                while (currentCountdownTime > 0) {
                    yield return new WaitForSeconds (1);
                    currentCountdownTime--;
                    SendCountdownEvent();
                }
        }
    }

    [ObserversRpc]
    private void SendCountdownEvent() {
        countdownTime.Invoke(currentCountdownTime);
    }
}
