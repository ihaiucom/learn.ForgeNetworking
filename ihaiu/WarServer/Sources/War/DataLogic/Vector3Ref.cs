using System.Diagnostics;
using UnityEngine;
using System.Collections;

[DebuggerDisplay("x = {x} y = {y} z= {z}")]
public class Vector3Ref
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public Vector3 Value
    {
        get
        {
            return new Vector3(x, y, z);
        }
    }

    public Vector3Ref(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3Ref(Vector3 vector3)
    {
        this.x = vector3.x;
        this.y = vector3.y;
        this.z = vector3.z;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        if ((obj as Vector3Ref) == null) return false;
        Vector3Ref other = obj as Vector3Ref;
        return Mathf.Approximately(other.x, this.x) && Mathf.Approximately(other.y, this.y) && Mathf.Approximately(other.z, this.z);
    }
}
