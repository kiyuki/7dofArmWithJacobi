using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class planeTest : MonoBehaviour {
    public GameObject planeTes;
    Vector3[] planerPcd;
    Vector3[] rotateMatrixx = new Vector3[3];

	// Use this for initialization
	void Start () {
        planeTes = GameObject.Find("Plane");
        getRotateMat(42*(2*Mathf.PI /360));
        planerPcd = readPcd();
    }
	
	// Update is called once per frame
	void Update () {
	
    }

    void getRotateMat(float theta)
    {
        rotateMatrixx[0] = new Vector3( 1, 0, 0 );
        rotateMatrixx[1] = new Vector3(0, Mathf.Cos(theta), Mathf.Sin(theta));
        rotateMatrixx[2] = new Vector3(0, Mathf.Sin(theta), Mathf.Cos(theta));
    }
    

    Vector3[] readPcd()
    {
        int numPoint = 0;
        Vector3 bufVec;
        Vector3[] points;
        string[] buffer;
        int counter = 0;
        string line;
        StreamReader file = new StreamReader(@"C:/Program Files/pcdData/realPoint.pcd");
        if (file == null)
        {
            UnityEngine.Debug.Log("error");
        }
        for (int i = 0; i < 9; i++)
        {
            line = file.ReadLine();
        }
        line = file.ReadLine();
        buffer = line.Split();
        UnityEngine.Debug.Log(buffer[0] + "\t" + buffer[1]);
        numPoint = int.Parse(buffer[1]);
        points = new Vector3[numPoint];
        line = file.ReadLine();
        while ((line = file.ReadLine()) != null)
        {
            buffer = line.Split();
            bufVec = new Vector3(float.Parse(buffer[0]), float.Parse(buffer[1]), float.Parse(buffer[2]));
            
            points[counter] = new Vector3(float.Parse(buffer[0]),
                bufVec[0] * rotateMatrixx[0][1] + bufVec[1] * rotateMatrixx[1][1] + bufVec[2] * rotateMatrixx[2][1],
                bufVec[0] * rotateMatrixx[0][2] + bufVec[1] * rotateMatrixx[1][2] + bufVec[2] * rotateMatrixx[2][2]);
            counter++;
         }
         file.Close();
         UnityEngine.Debug.Log("There are " + counter + "lines.");
         return points;
     }
   
}
