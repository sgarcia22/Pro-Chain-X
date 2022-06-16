using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;
using MoralisUnity;
using MoralisUnity.Web3Api.Models;
using MoralisUnity.Platform.Objects;
using System.Collections.Generic;
using System.Linq;

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

    [SyncVar]
    public Pawn controlledPawn;

    public bool arenaAccess = false;

    // Arena NFT
    private const string contractAddress = "0x51729BCaaF96F08f8Dd0e3758821fc440503bBC3";

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

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner) return; // TODO - do we need this?
        SpawnPlayer();
        GameManager.Instance.currentPlayer = this;
        CheckArenaNFTOwnership();
    }

    private async void CheckArenaNFTOwnership() { 
        MoralisUser user = await Moralis.GetUserAsync();
        string address = user.ethAddress;
        Debug.Log("Checking NFTs on: " + address);
        // TODO - update to mumbai
        NftOwnerCollection nft = await Moralis.GetClient().Web3Api.Account.GetNFTsForContract(address.ToLower(), contractAddress, ChainList.ropsten); 
        List<NftOwner> nftOwners = nft.Result;
        if (!nftOwners.Any())
            {
                Debug.Log("You don't own any NFT");
                return;
            }
        else {
            arenaAccess = true;
        }
    }

    // Client to server side. Call code to be executed on server side.
    [ServerRpc]
    public void ServerSetIsReady(bool value) {
        isReady = value;
    }

    // TODO - player is ready when their wallet is connected

    [ServerRpc]
    public void SpawnPlayer() {
        GameObject pawnPrefab = Addressables.LoadAssetAsync<GameObject>("Pawn").WaitForCompletion();
        GameObject pawnInstance = Instantiate(pawnPrefab, transform.position, Quaternion.identity);
        // Spawn instance and make it's owner the current local connection
        Spawn(pawnInstance, Owner);
        controlledPawn = pawnInstance.GetComponent<Pawn>();
        controlledPawn.controllingPlayer = this;
    }

    public void StopGame() {
        if (controlledPawn != null && controlledPawn.IsSpawned) { // safety check
            controlledPawn.Despawn();
        }
    }

    // private void Update() {
    //     // Who is the owner of this player gameobject
    //     if (!IsOwner) return;

    //     if (Keyboard.current[Key.R].wasPressedThisFrame) {
    //         ServerSetIsReady(!isReady);
    //     }
    //      if (Keyboard.current[Key.I].wasPressedThisFrame) {
    //         StartGame();
    //     }
    // }
}
