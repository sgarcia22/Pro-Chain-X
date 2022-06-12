using FishNet;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerMenu : MonoBehaviour
{
    [SerializeField]
    private Button hostButton;

    [SerializeField]
    private Button connectButton;    


    private void Start() {
        // Start a server and connect local client to
        hostButton.onClick.AddListener(() => {
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
        });

        connectButton.onClick.AddListener(() => {
            InstanceFinder.ClientManager.StartConnection();
        });
    }
}
