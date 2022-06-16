using UnityEngine;
using MoralisUnity;
using Nethereum.Hex.HexTypes;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Platform.Queries;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;

public class ArenaNFT : MonoBehaviour, IClick
{
    public class ArenaGateEvent : MoralisObject
    {
        public bool result { get; set; }
        
        public ArenaGateEvent() : base("ArenaGateEvent") {}
    }

    // Deployed contract on Mumbai testnet
    private const string contractAddress = "0x51729BCaaF96F08f8Dd0e3758821fc440503bBC3";
    private const string contractABI = "[{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";

    //Database Queries
    private MoralisQuery<ArenaGateEvent> _getEventsQuery;
    private MoralisLiveQueryCallbacks<ArenaGateEvent> _queryCallbacks;

    private bool _listening;

    // Only for Editor using
    private bool _responseReceived;
    private bool _responseResult;

    public void StartGame()
    { 
        SubscribeToDatabaseEvents();
    }
    

    private async void SubscribeToDatabaseEvents()
    {
        _getEventsQuery = await Moralis.GetClient().Query<ArenaGateEvent>();
        _queryCallbacks = new MoralisLiveQueryCallbacks<ArenaGateEvent>();

        _queryCallbacks.OnUpdateEvent += HandleContractEventResponse;

        MoralisLiveQueryController.AddSubscription<ArenaGateEvent>("ArenaGateEvent", _getEventsQuery, _queryCallbacks);
    }

    public async void OnClick()
    {
        await CallContractFunction();
    }

    private async UniTask<string> CallContractFunction()
    {
        object[] parameters = {};
        HexBigInteger value = new HexBigInteger(0);
        HexBigInteger gas = new HexBigInteger(0);
        HexBigInteger gasPrice = new HexBigInteger(0);
        // Call contract to mint arena access NFT
        return await Moralis.ExecuteContractFunction(contractAddress, contractABI, "mint", parameters, value, gas, gasPrice);
    }

    private void HandleContractEventResponse(ArenaGateEvent newEvent, int requestId)
        {
            if (!_listening) return;

            // You will find this a bit different from the video. It's a quality improvement for Editor testing. Functionality continues in ShowResponsePanel() :)
            if (Application.isEditor)
            {
                _responseResult = newEvent.result;
                _responseReceived = true;

                return;
            }

            Debug.Log(newEvent.result);
        }

}
