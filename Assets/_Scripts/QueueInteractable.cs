using UnityEngine;
using FishNet.Object;

/// <summary>
/// Attached to the pedastle to sign up for the arena queue. Calls the QueueManager for the current player to be added to the queue.
/// </summary>
public class QueueInteractable : NetworkBehaviour, IClick
{
    public void OnClick()
    {
        // InstanceFinder.ClientManager.Connection.
        if (GameManager.Instance.currentPlayer.GetComponent<Player>().arenaAccess)
             QueueManager.Instance.AddToQueue(GameManager.Instance.currentPlayer);
    }

}
