using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using System.Collections.Generic;
using Climbing;

public class Pawn : NetworkBehaviour
{
    [SyncVar]
    public Player controllingPlayer;

    [SerializeField]
    private List<GameObject> localObjects;
    [SerializeField]
    private List<Behaviour> localComponents;
    
    public InputCharacterController controller;

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
