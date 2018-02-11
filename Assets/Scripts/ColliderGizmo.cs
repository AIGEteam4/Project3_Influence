using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderGizmo : MonoBehaviour {

    // Draws the Light bulb icon at position of the object.
    void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, GetComponent<BoxCollider>().size);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
