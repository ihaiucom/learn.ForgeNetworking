using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class GameLogic : MonoBehaviour {

	void Start ()
    {
        NetworkManager.Instance.InstantiateZfTestPlayerCube();
	}
	
}
