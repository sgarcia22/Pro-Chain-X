using FishNet.Object;
using FishNet.Object.Synchronizing;

public class Pawn : NetworkBehaviour
{
    [SyncVar]
    public Player controllingPlayer;

    public override void OnStartClient() // server?
    {
        base.OnStartClient();
        // Let player control
        if (!IsOwner) Destroy(gameObject);
    }
}
