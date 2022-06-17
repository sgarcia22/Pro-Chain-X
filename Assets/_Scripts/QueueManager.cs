using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using UnityEngine.Events;

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
        // StartCoroutine(MatchLogic());
    }


    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(MatchLogic());
        StartCoroutine(Countdown(roundTimeInSeconds));
    }

    // public override void OnStartClient()
    // {
    //     base.OnStartClient();
    //     if (!IsOwner) return;
    //     Debug.Log("Start Client");
    //     StartCoroutine(Countdown(roundTimeInSeconds));
    // }

    [Server]
    public void AddToQueue(Player player) {
        if (!player.inQueue && queue.Count < maximumAmountOfPlayers) {
            queue.Add(player);
            Debug.Log("Added player to queue");
            AddToQueueEvent.Invoke();
            player.inQueue = true;
        }
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
            // Debug.Log(counter);
            SendCountdownEvent();
                while (currentCountdownTime > 0) {
                    // Debug.Log(counter);
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
