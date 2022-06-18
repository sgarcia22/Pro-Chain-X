using System.Net;
using System;
using System.IO;
using UnityEngine;
using System.Collections;
using TMPro;
using Newtonsoft.Json.Linq;

// TODO - make it run on the server
/// <summary>
/// Pulls the MATIC price from the blockchain using the Covalent API.
/// </summary>
public class CovalentAPI : MonoBehaviour
{
    
    [SerializeField]
    private TextMeshProUGUI priceText;

    private const string API_KEY = "ckey_b1502541d2da4c6e86c41d8aa76";

    private WaitForSeconds waitForSeconds;
    
    private void Start() {
        waitForSeconds = new WaitForSeconds(10f);
        StartCoroutine(getMATICPrice());
    }

    private IEnumerator getMATICPrice() {
        while (true) {
          HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.covalenthq.com/v1/pricing/historical_by_addresses_v2/1/USD/0x7D1AfA7B718fb893dB30A3aBc0Cfc608AaCfeBB0/?quote-currency=USD&format=JSON&key={0}", API_KEY));
          HttpWebResponse response = (HttpWebResponse)request.GetResponse();
          yield return new WaitUntil(() => response.StatusCode == HttpStatusCode.OK);
          StreamReader reader = new StreamReader(response.GetResponseStream());
          string jsonResponse = reader.ReadToEnd();
          var obj = JObject.Parse(jsonResponse);
          priceText.text = obj["data"][0]["prices"][0]["price"].ToString();
            yield return waitForSeconds;
        }
    }
}
