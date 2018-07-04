using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucy : MonoBehaviour {

    GameObject refObj;
	// Use this for initialization
	void Start () {
        refObj = GameObject.Find("Sphere");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Diamond.Sint = 72;

            Diamond d1 = GetComponent<Diamond>();
            d1.countUp();
            d1.sayInfo();

            Diamond d2 = refObj.GetComponent<Diamond>();
            d2.setCount(91);
            d2.sayInfo();
        }
	}
}
