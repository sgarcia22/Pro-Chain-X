using UnityEngine;
using MoralisUnity;
using Nethereum.Hex.HexTypes;
using TMPro;
using FishNet.Object;
using FishNet.Object.Synchronizing;

/// <summary>
/// Manages the betting pool for the Arena.
/// </summary>
public class BettingPool : NetworkBehaviour
{

    [SerializeField]
    private TMP_InputField betAddress;
    [SerializeField]
    private TMP_InputField betAmount;

    string contractAddress = "";
    string contractABI = "[{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"bettersAddresses\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"winnerAddress\",\"type\":\"address\"}],\"name\":\"payout\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"bettingOn\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amountToBet\",\"type\":\"uint256\"}],\"name\":\"placeBet\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";

    // List of player wallet addresses and have it above the name... lol
    /// <summary>
    /// Calls the betting pool smart contract and locks funds in the pool for the wallet address player they are betting on.
    /// 
    /// There are many edge cases to consider such as if a player leaves the match or queue. In the outscope of a hackathon, the player will need to put a deposit to enter the match which they receive back upon successful completion.
    /// </summary>
    /// <param name="walletAddress">Wallet address of the player the current user wants to bet on.</param>
    /// <param name="amount">How much the user is willing to place in the pool to bet on the player with the specified wallet address.</param>
    [ServerRpc(RequireOwnership = false)]
    public async void AddFunds() {
        object[] parameters = {
            betAddress.text,
            betAmount.text
        };

        HexBigInteger value = new HexBigInteger(0);
        HexBigInteger gas = new HexBigInteger(0);
        HexBigInteger gasPrice = new HexBigInteger(0);
        
        // Call contract to mint arena access NFT
        try
        {
            string resp = await Moralis.ExecuteContractFunction(contractAddress, contractABI, "placeBet", parameters, value, gas, gasPrice);
            Debug.Log($"$Send Transaction respo: {resp}");
        }
        catch (System.Exception exp)
        {
            Debug.Log($"Send transaction failed: {exp.Message}");
        }
    }

    /// <summary>
    /// Called after the match ends and distributes the pool to the winning party and player.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public async void DistributeFunds(string winner) {
        object[] parameters = {
            winner
        };
        
        HexBigInteger value = new HexBigInteger(0);
        HexBigInteger gas = new HexBigInteger(0);
        HexBigInteger gasPrice = new HexBigInteger(0);
        
        // Call contract to mint arena access NFT
        try
        {
            string resp = await Moralis.ExecuteContractFunction(contractAddress, contractABI, "payout", parameters, value, gas, gasPrice);
            Debug.Log($"$Send Transaction respo: {resp}");
        }
        catch (System.Exception exp)
        {
            Debug.Log($"Send transaction failed: {exp.Message}");
        }
    }
}
