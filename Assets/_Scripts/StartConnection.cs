using FishNet;
using UnityEngine;

public class StartConnection : MonoBehaviour
{
    // TODO - test if works properly
    public void AddPlayer() {
        // Start server if we are the first player
        if (InstanceFinder.ServerManager.Clients.Count == 0) {
         InstanceFinder.ServerManager.StartConnection();
        }
        InstanceFinder.ClientManager.StartConnection();
    }
}
