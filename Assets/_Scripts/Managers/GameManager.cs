using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;
using FishNet;

public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    // 2021.3+ C# 9
    [SyncObject]
    public readonly SyncList<Player> players = new();

    // Set by the server only
    [SyncVar]
    public bool canStart = false;

    [SyncVar]
    private bool gameRunning = false;

    private void Awake() {
        Instance = this;
    }

    // private void Update() {
    //     if (!IsServer) return;

    //     if (players.Count != 0) {
    //         canStart = players.All(player => player.isReady);
    //         Debug.LogFormat("Can start, {0}", canStart);
    //         if (canStart) {
    //             StartGame();
    //         }
    //     }
    // }

    public void AddPlayer() {
        // Start server if we are the first player
        if (players.Count == 0) {
         InstanceFinder.ServerManager.StartConnection();
        }
        InstanceFinder.ClientManager.StartConnection();
    }

    [Server]
    public void StartGame() {
        if (!canStart || gameRunning) return;
        gameRunning = true;
        
        foreach (Player player in players)
        {
            // player.StartGame();
        }
    }

    [Server]
    public void StopGame() {
        gameRunning = false;
        foreach (Player player in players)
        {
            player.StopGame();
        }
    }
}
