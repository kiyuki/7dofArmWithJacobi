using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefabManager : MonoBehaviour {
    public GameObject capusulePrefab;

	// Use this for initialization
	void Start () {
		for( int i =0; i<10; i++)
        {
            GameObject obj = (GameObject)Instantiate(capusulePrefab, Vector3.zero, transform.rotation);
            moveObject moveObject1 = obj.GetComponent<moveObject>();
            moveObject1.Vec = new Vector3(2f * i, 0f, 0f);
            moveObject1.Move();
        }
	}
	
	// Update is called once per frame

}
