using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Transform rootTransform = transform.root;
        GameObject rootObject = rootTransform.gameObject;
        Debug.LogFormat("Object ={0}, RootObject = {1}", gameObject.name, rootObject.name);
	}
	
}
