using UnityEngine;
using FishNet.Object;

/// <summary>
/// Called on the client upon successful reach of the goal and udpates the winners list in the server.
/// </summary>
public class GoalBehaviour : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other) {
        UpdateWinners( other.gameObject.transform.parent.GetComponent<Pawn>().controllingPlayer);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateWinners(Player player) { 
        MatchManager.Instance.AddWinner(player);
    }
}
