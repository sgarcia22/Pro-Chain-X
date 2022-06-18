using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet;

/// <summary>
/// Manages the list of players on the server side and keeps track of the current player on the client side.
/// </summary>
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

    public Player currentPlayer;

    private void Awake() {
        Instance = this;
    }

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
        
        // foreach (Player player in players)
        // {
        //     // player.StartGame();
        // }
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
