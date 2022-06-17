using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using System.Collections.Generic;
using Climbing;

public class Pawn : NetworkBehaviour
{
    [SyncVar]
    public Player controllingPlayer;

    // TODO: Make neater
    public GameObject character;
    public InputCharacterController controller;

    [SerializeField]
    private List<GameObject> localObjects;
    [SerializeField]
    private List<Behaviour> localComponents;

    public override void OnStartClient() // server?
    {
        base.OnStartClient();
        // Let player control
        if (!IsOwner) {
            foreach (GameObject item in localObjects)
            {
                item.SetActive(false);
            }
            foreach (Behaviour item in localComponents)
            {
                item.enabled = false;
            }
        }
    }
}
