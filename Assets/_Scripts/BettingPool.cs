using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingPool : MonoBehaviour
{

    // List of player wallet addresses and have it above the name... lol
    /// <summary>
    /// Calls the betting pool smart contract and locks funds in the pool for the wallet address player they are betting on.
    /// 
    /// There are many edge cases to consider such as if a player leaves the match or queue. In the outscope of a hackathon, the player will need to put a deposit to enter the match which they receive back upon successful completion.
    /// </summary>
    /// <param name="walletAddress">Wallet address of the player the current user wants to bet on.</param>
    /// <param name="amount">How much the user is willing to place in the pool to bet on the player with the specified wallet address.</param>
    public void AddFunds(string walletAddress, float amount) {
        
    }

    /// <summary>
    /// Called after the match ends and distributes the pool to the winning party and player.
    /// </summary>
    public void DistributeFunds() {

    }
}
