using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using UnityEngine.Events;
public class QueueManager : NetworkBehaviour
{
    public static QueueManager Instance { get; private set; }

    [SyncObject, Tooltip("List of players in the queue")]
    private readonly SyncList<Player> queue = new();

    [SyncVar]
    public int maximumAmountOfPlayers = 10;

    [SyncVar]
    public int roundTimeInMinutes = 5*60;

    [SerializeField]

    private WaitForSeconds waitForSeconds;

    [SerializeField]
    private UnityEvent AddToQueueEvent;
    [SerializeField]
    private UnityEvent<int> countdownTime;

    private void Awake() {
        Instance = this;
        waitForSeconds = new WaitForSeconds(roundTimeInMinutes);
    StartCoroutine(Countdown(roundTimeInMinutes));
        // StartCoroutine(MatchLogic());
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(MatchLogic());
    }

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
            while (queue.Count < 2) {
                yield return null;
                countdownTime.Invoke(-1);
            }
            MatchManager.Instance.StartMatch(queue);
        }
    }

    IEnumerator Countdown (int seconds) {
        int counter = seconds;
        Debug.Log(counter);
        countdownTime.Invoke(counter);
        while (counter > 0) {
            yield return new WaitForSeconds (1);
            counter--;
            countdownTime.Invoke(counter);
        }
    }
}
