using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;

public class QueueInteractable : NetworkBehaviour, IClick
{
    public void OnClick()
    {
        // InstanceFinder.ClientManager.Connection.
        QueueManager.Instance.AddToQueue(GameManager.Instance.currentPlayer);
    }
}
