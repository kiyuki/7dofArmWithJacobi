using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveObject : MonoBehaviour {

    public Vector3 Vec;

    public void Move()
    {
        Debug.Log("vec: " + Vec);
        transform.position = Vec;
    }
}
