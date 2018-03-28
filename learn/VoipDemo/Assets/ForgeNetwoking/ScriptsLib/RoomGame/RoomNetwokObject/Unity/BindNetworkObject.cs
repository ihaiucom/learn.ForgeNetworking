using Rooms.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindNetworkObject : MonoBehaviour {

    public UnitNetworkObject unitNetworkObject;
    public bool IsOwner;
    public float speed = 5;
    public uint myPlayerId;
    public uint ownerPlayerId;

    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(unitNetworkObject != null)
        {
            IsOwner = unitNetworkObject.IsOwner;
            myPlayerId = unitNetworkObject.MyPlayerId;
            ownerPlayerId = unitNetworkObject.Owner.NetworkId;

            if (unitNetworkObject.IsOwner)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                unitNetworkObject.position += new Vector3(h, 0, v).normalized * speed * Time.deltaTime;

            }

            transform.position = unitNetworkObject.position;
            transform.rotation = unitNetworkObject.rotation;


        }
	}
}
