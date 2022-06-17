using UnityEngine;
using FishNet.Object;

public class QueueInteractable : NetworkBehaviour, IClick
{
    [ServerRpc]
    public void OnClick()
    {
        // InstanceFinder.ClientManager.Connection.
        if (GameManager.Instance.currentPlayer.GetComponent<Player>().arenaAccess)
            AddToQueue();
    }

    [ServerRpc]
    private void AddToQueue(){ 
        QueueManager.Instance.AddToQueue(GameManager.Instance.currentPlayer);
    }
}
