using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZfTestPlayerCube : ZfTestPlayerCubeBehavior
{
    public float speed = 5f;
    private void Update()
    {
        if(!networkObject.IsOwner)
        {
            transform.position = networkObject.position;
            transform.rotation = networkObject.rotation;
            return;
        }

        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        dir *= speed * Time.deltaTime;
        transform.position += dir;
        transform.Rotate(new Vector3(speed, speed, speed) * 0.25f);

        networkObject.position = transform.position;
        networkObject.rotation = transform.rotation;
    }
}
