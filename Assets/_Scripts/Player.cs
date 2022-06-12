using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.InputSystem;
/// <summary>
/// Networked player gameobject.
/// </summary>
public class Player : NetworkBehaviour
{
    // When you change value on server will be sync to all clients
    [SyncVar]
    public string username;

    // Is the player ready to start the game (countdown, pre-condition). Make sure that all players are ready.
    [SyncVar]
    public bool isReady = false;

    public override void OnStartServer()
    {
        // Always call this first, can cause bugs otherwise
        base.OnStartServer();
        GameManager.Instance.players.Add(this);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        GameManager.Instance.players.Remove(this);
    }

    // Client to server side. Call code to be executed on server side.
    [ServerRpc]
    public void ServerSetIsReady(bool value) {
        isReady = value;
    }

    private void Update() {
        // Who is the owner of this player gameobject
        if (!IsOwner) return;

        if (Keyboard.current[Key.R].wasPressedThisFrame) {
            ServerSetIsReady(!isReady);
        }
    }
}
