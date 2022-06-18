using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoralisUnity;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Web3Api.Models;

/// <summary>
/// Sample Moralis Unity SDK script to access values. Not used in the game.
/// </summary>
public class WalletLogin : MonoBehaviour
{
    public async void ConnectWallet() {
        Debug.Log("Wallet Connected");
        // if (MoralisState.Initialized.Equals(Moralis.State))
        //     {
        //         MoralisUser user = await Moralis.GetUserAsync();
        //         Debug.Log(user.ethAddress);

        //         if (user == null)
        //         {
        //             // User is null so go back to the authentication scene.
        //             // SceneManager.LoadScene(0);
        //         }

        //         // Display User's wallet address.
        //         // addressText.text = FormatUserAddressForDisplay(user.ethAddress);

        //         // Retrienve the user's native balance;
        //         NativeBalance balanceResponse = await Moralis.Web3Api.Account.GetNativeBalance(user.ethAddress, Moralis.CurrentChain.EnumValue);

        //         double balance = 0.0;
        //         float decimals = Moralis.CurrentChain.Decimals * 1.0f;
        //         string sym = Moralis.CurrentChain.Symbol;

        //         // Make sure a response to the balanace request weas received. The 
        //         // IsNullOrWhitespace check may not be necessary ...
        //         if (balanceResponse != null && !string.IsNullOrWhiteSpace(balanceResponse.Balance))
        //         {
        //             double.TryParse(balanceResponse.Balance, out balance);
        //         }

        //         // Display native token amount token in fractions of token.
        //         // NOTE: May be better to link this to chain since some tokens may have
        //         // more than 18 sigjnificant figures.
        //         // balanceText.text = string.Format("{0:0.####} {1}", (balance / (double)Mathf.Pow(10.0f, decimals)), sym);
        //     }
    }
}
