using FishNet;
using UnityEngine;

/// <summary>
/// Start the server or client connection.
/// </summary>
public class StartConnection : MonoBehaviour
{
    public void AddPlayer() {
        // Start server if we are the first player
        if (InstanceFinder.ServerManager.Clients.Count == 0) {
         InstanceFinder.ServerManager.StartConnection();
        }
        InstanceFinder.ClientManager.StartConnection();
    }
}
