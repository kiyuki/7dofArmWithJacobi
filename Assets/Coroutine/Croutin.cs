using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Croutin : MonoBehaviour {
    bool flagForRunning;
	// Use this for initialization
	void Start () {
        flagForRunning = false;
        /*
        IEnumerator e = Coroution();
        e.MoveNext();
        e.MoveNext();
        e.MoveNext();
        */
	}
	
	// Update is called once per frame
	void Update () {

        StartCoroutine(start());
    }
    IEnumerator start()
    {
        if (flagForRunning) yield break;
        flagForRunning = true;

        yield return StartCoroutine( Coroutin());
        StartCoroutine(processB());
        flagForRunning = false;
    }

    IEnumerator processB()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("わんわんお");
        }

            yield return null;
       
    }
    IEnumerator Coroutin()
    {
        int maxvalue = 2000;
        int counter = 0;
        Debug.Log("one");
        yield return null;
        Debug.Log("tow");
        yield return null;
        Debug.Log("three");
        yield return null;
        
        while(counter < maxvalue)
        {
            for(int i = 0; i < 200; i++)
            {
                Debug.Log(counter);
                counter++;
            }
            Debug.Log("Flame subdivision");
            yield return null;
        }
        yield return null;

        Debug.Log("End Coroutine");
    }
}
