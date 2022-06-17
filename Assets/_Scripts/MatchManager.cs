using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class MatchManager : NetworkBehaviour
{
    public static MatchManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    [Server]
    public void StartMatch(SyncList<Player> queue) {
        // Teleport all of the players to the arena

        // Disable Movement

        // Start Countdown

        // Once countdown is done, enable movement

        //
    }
}
