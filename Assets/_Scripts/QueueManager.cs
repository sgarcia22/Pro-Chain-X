using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;

public class QueueManager : NetworkBehaviour
{
    public static QueueManager Instance { get; private set; }

    [SyncObject, Tooltip("List of players in the queue")]
    private readonly SyncList<Player> queue = new();

    [SyncVar]
    public int maximumAmountOfPlayers = 10;

    [SyncVar]
    public int roundTimeInMinutes = 5;

    private WaitForSeconds waitForSeconds;

    private void Awake() {
        Instance = this;
        waitForSeconds = new WaitForSeconds(roundTimeInMinutes);
        StartCoroutine(MatchLogic());
    }

    [Server]
    public void AddToQueue(Player player) {
        if (queue.Count < maximumAmountOfPlayers)
            queue.Add(player);
    }

    // Queue logic
    [Server]
    private IEnumerator MatchLogic() {
        while (true) {
            yield return waitForSeconds;
            while (queue.Count < 2) {
                yield return null;
            }
            MatchManager.Instance.StartMatch(queue);
        }
    }
}
