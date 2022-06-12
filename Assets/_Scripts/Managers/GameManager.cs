using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Linq;

public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    // 2021.3+ C# 9
    [SyncObject]
    public readonly SyncList<Player> players = new();

    // Set by the server only
    [SyncVar]
    public bool canStart = false;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (!IsServer) return;

        if (players.Count != 0) {
            canStart = players.All(player => player.isReady);
            Debug.LogFormat("Can start, {0}", canStart);
        }
    }
}
