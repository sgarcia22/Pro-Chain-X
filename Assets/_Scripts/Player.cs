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
/// Represents the connected networked player gameobject. Spawns the player into the scene and checks NFT ownership.
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
    [SyncVar]
    public bool arenaAccess = false;
    [SyncVar]
    public bool inQueue = false;
    [SyncVar]
    public string userAddress = "";

    // Arena NFT
    private const string contractAddress = "0xEE4b72cE7543b62a738E24519B27ac1775c90fCE";

    public override void OnStartServer()
    {
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
        userAddress = address;
        Debug.Log("Checking NFTs on: " + address);
        NftOwnerCollection nft = await Moralis.GetClient().Web3Api.Account.GetNFTsForContract(address.ToLower(), contractAddress, ChainList.mumbai);
        Debug.Log(nft.Total);
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

    [ServerRpc]
    public void ServerSetIsReady(bool value) {
        isReady = value;
    }

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

}
