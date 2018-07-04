using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {
    private GameObject mainCamera;
    //enums
    public enum cameraPosi { defalut ,near, middle, far, custom1, custom2,upper }
   
    public cameraPosi cam;

    public bool setCameraF;
    // Use this for initialization
    void Start () {
        mainCamera = GameObject.Find("Main Camera");
    }
	
	// Update is called once per frame
	void Update () {
        if (setCameraF)
        {
            setCameraPosi(cam);
            setCameraF = false;
        }
    }

    private void setCameraPosi(cameraPosi cam)
    {
        switch (cam)
        {
            case cameraPosi.defalut:
                mainCamera.transform.position = new Vector3(0.4f, -0.5f, 3f);
                mainCamera.transform.eulerAngles = new Vector3(30, 210, 0);
                break;
            case cameraPosi.near:
                mainCamera.transform.position = new Vector3(1.42f, -0.51f, 1.54f);
                break;
            case cameraPosi.middle:
                mainCamera.transform.position = new Vector3(0f, 1f, 3f);
                break;
            case cameraPosi.far:
                mainCamera.transform.position = new Vector3(0f,-0.5f,0f);
                break;
            case cameraPosi.custom1:
                mainCamera.transform.position = new Vector3(1.59f, -0.53f, 1.49f);
                mainCamera.transform.localEulerAngles = new Vector3(0, -45, 0);
                break;
            case cameraPosi.custom2:
                mainCamera.transform.position = new Vector3(0.4f, -1.3f, 2.01f);
                mainCamera.transform.eulerAngles = new Vector3(0, -90f, 0);
                break;
            case cameraPosi.upper:
                mainCamera.transform.position = new Vector3(-0.25f, 0.9f, 2.01f);
                mainCamera.transform.eulerAngles = new Vector3(90f, 0, 0);
                break;
        }
        return;
    }
}
