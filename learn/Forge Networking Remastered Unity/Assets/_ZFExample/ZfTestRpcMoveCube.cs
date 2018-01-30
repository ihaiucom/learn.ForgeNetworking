using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using System;
using BeardedManStudios.Forge.Networking.Unity;

public class ZfTestRpcMoveCube : ZfTestRpcMoveCubeBehavior
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            networkObject.SendRpc(RPC_MOVE, Receivers.All, Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            networkObject.SendRpc(RPC_MOVE, Receivers.All, Vector3.down);
        }
    }

    public override void Move(RpcArgs args)
    {
        MainThreadManager.Run(() => {
            transform.position += args.GetNext<Vector3>();
        }); 
    }
}
