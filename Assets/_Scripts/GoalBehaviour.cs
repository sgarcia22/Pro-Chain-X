using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public class GoalBehaviour : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other) {
        UpdateWinners( other.gameObject.GetComponent<Pawn>().controllingPlayer);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateWinners(Player player) { 
        MatchManager.Instance.AddWinner(player);
    }
}
