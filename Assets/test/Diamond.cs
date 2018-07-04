using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour {

    private string Text;
    private int count;
    public static int Sint;

	// Use this for initialization
	void Start () {
        Text = transform.root.gameObject.name;
        count = 0;
        Sint = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void sayInfo()
    {
        Debug.Log("Text:" + Text + ",count: "+ count + ", Sint:" + Sint);
    }
    public void countUp()
    {
        count++;
    }
    public void setCount (int i)
    {
        count = i;
    }
}
