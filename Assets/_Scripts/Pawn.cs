using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using System.Collections.Generic;
using Climbing;
using Cinemachine;

/// <summary>
/// A pawn is owned by a player, which is a NetworkedConnection. A pawn is the object the player controls. This script disables access to input and other scripts that do not belong to the current player.
/// </summary>
public class Pawn : NetworkBehaviour
{
    [SyncVar]
    public Player controllingPlayer;

    // TODO: make private
    public GameObject character;
    public InputCharacterController controller;

    [SerializeField]
    private List<GameObject> localObjects;
    [SerializeField]
    private List<Behaviour> localComponents;

    public override void OnStartClient()
    {
        base.OnStartClient();
        // Let the player control it's own pawn
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
        else {
            GameObject streamCamera = GameObject.FindGameObjectWithTag("CinemachineStreamCamera");
            if (streamCamera == null) return;
            streamCamera.GetComponent<CinemachineVirtualCamera>().Follow = character.transform;
            streamCamera.GetComponent<CinemachineVirtualCamera>().LookAt = character.transform;
        }
    }
}
