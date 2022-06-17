using UnityEngine;

public class QueueInteractable : MonoBehaviour, IClick
{
    public void OnClick()
    {
        // InstanceFinder.ClientManager.Connection.
        if (GameManager.Instance.currentPlayer.GetComponent<Player>().arenaAccess)
            QueueManager.Instance.AddToQueue(GameManager.Instance.currentPlayer);
    }
}
