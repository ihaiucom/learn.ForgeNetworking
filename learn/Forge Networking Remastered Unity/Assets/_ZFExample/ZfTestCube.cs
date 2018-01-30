using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZfTestCube : ZfTestCubeBehavior
{
    public float speed = 5.0f;

    private void Update()
    {
        if(!networkObject.IsOwner)
        {
            transform.position = networkObject.position;
            transform.rotation = networkObject.rotation;
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.position += new Vector3(h, 0, v).normalized * speed * Time.deltaTime;
        networkObject.position = transform.position;
        networkObject.rotation = transform.rotation;
    }
}
