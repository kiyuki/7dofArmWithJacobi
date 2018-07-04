using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testComponent : MonoBehaviour {

	// Use this for initialization
	private void Start () {
        Scene scene = SceneManager.GetSceneByName("SceneA");
        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach( var obj in rootObjects)
        {
            Debug.LogFormat("RootObject = {0}", obj.name);
        }
	}
	
	// Update is called once per frame

}
