using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;


public partial class pcdReader : MonoBehaviour {

    private ParticleSystem.Particle[] psPoints;
    Vector3[] pcPoints;
	// Use this for initialization
	void Start () {
        pcPoints = readPcd();
        /*
        for(int i = 0; i< pcPoints.Length; i++)
        {
            Debug.Log(pcPoints[i]);
        }*/
        psPoints = new ParticleSystem.Particle[pcPoints.Length];

        for(int i = 0; i< pcPoints.Length; i++)
        {
            psPoints[i].position = pcPoints[i];
            psPoints[i].startColor = new Color(1f, 0, 0);
            psPoints[i].startSize = 0.01f;
        }
    }
	
	// Update is called once per frame
	void Update () {

        GetComponent<ParticleSystem>().SetParticles(psPoints, psPoints.Length);
    }
}

public partial class pcdReader : MonoBehaviour
{

    int numPoint = 0;
    Vector3[] readPcd()
    {
        Vector3[] points;
        string[] buffer;
        int counter = 0;
        string line;
        StreamReader file = new StreamReader(@"C:/Program Files/pcdData/realPoint.pcd");
        if(file == null)
        {
            Debug.Log("error");
        }
        for(int i = 0; i < 9; i++)
        {
            line = file.ReadLine();
        }
        line = file.ReadLine();
        buffer = line.Split();
        Debug.Log(buffer[0] + "\t" + buffer[1]);
        numPoint = int.Parse(buffer[1]);
        
        points = new Vector3[numPoint];
        line = file.ReadLine();

        while((line = file.ReadLine()) != null)
        {
            buffer = line.Split();
            points[counter] = new Vector3(float.Parse(buffer[0]), float.Parse(buffer[1]), float.Parse(buffer[2]));
            counter++;
        }
        file.Close();
        Debug.Log("There are " + counter + "lines.");
        return points;
    }
}
