using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnitSpawen : MonoBehaviour {

	void Start ()
    {
        TestUnitNetworkObject networkObject = new TestUnitNetworkObject(NetworkManager.Instance.Networker);
        networkObject.RegistrationComplete();
    }
	
}
