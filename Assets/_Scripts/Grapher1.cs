using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;

public class Grapher1 : MonoBehaviour {

    GameObject Mcam;
    
    public int resolution = 10;
    private int currentResolution;
    private ParticleSystem.Particle[] points;

    public enum FunctionOption
    {
        Exponential,
        Parabola,
        Sine
    }

    public FunctionOption function;

    private delegate float FunctionDelegate(float x);
    private static FunctionDelegate[] functionDelegates =
    {
        Exponential,
        Parabola,
        Sine
    };

    void Start()
    {
        GameObject Mcam = GameObject.Find("Main Camera");
        Mcam.transform.position = new Vector3(0, 0, -5);
        CreatePoints();

        ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
        color.mode = ParticleSystemGradientMode.Color;
        color.color = Color.red;
    }

    private void CreatePoints()
    {
        if (resolution < 10 || resolution > 100)
        {
            Debug.LogWarning("Grapher resolution out of bounds, resetting to minimum.", this);
            resolution = 10;
        }
        currentResolution = resolution;
        points = new ParticleSystem.Particle[resolution];
        float increment = 1f / (resolution - 1);
        for (int i = 0; i < resolution; i++)
        {
            float x = i * increment;
            points[i].position = new Vector3(x, 0f, 0f);
            points[i].startColor = new Color(x, 0f, 0f);
            points[i].startSize = 0.1f;
        }
        
    }

    void Update()
    {
        if (currentResolution != resolution || points == null)
        {
            CreatePoints();
        }
        FunctionDelegate f = functionDelegates[(int)function];

        for(int i = 0; i< resolution; i++)
        {
            Vector3 p = points[i].position;
            //p.y = Parabola(p.x);
            p.y = f(p.x);
            //p.y = Exponential(p.x);
            points[i].position = p;
            Color c = points[i].startColor;
            c.g = p.y;
            points[i].startColor = c;
        }
        GetComponent<ParticleSystem>().SetParticles(points, points.Length);

    }

    private static float Exponential (float x)
    {
        return x * x;
    }

    private static float Parabola (float x)
    {
        x = 2f * x - 1f;
        return x * x;
    }
    private static float Sine(float x)
    {
        return 0.5f + 0.5f * Mathf.Sin(2 * Mathf.PI * x + Time.timeSinceLevelLoad);
    }


    /*
    private ParticleSystem.Particle[] points = new ParticleSystem.Particle[100];
    private Vector3[] pointCloud;

    private void Start()
    {
        points[0].position = new Vector3(0, 0, 0);
        points[1].position = new Vector3(1, 0, 0);
        points[2].position = new Vector3(0, 1, 0);
        points[3].position = new Vector3(1, 1, 0);
        points[4].position = new Vector3(2, 0, 0);
    }
    void Update()
    {
        GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }*/
    /*
    private ParticleSystem.Particle[] points;
    private Vector3[] pointCloud;
    int numPoint = 0;

    // Use this for initialization
    void Start() {
        readPcd();
        Debug.Log(pointCloud[1]);
        Debug.Log(pointCloud[1111]);
        Debug.Log(pointCloud[1112]);
        Debug.Log("numPoint: " + numPoint);

        points = new ParticleSystem.Particle[numPoint];
               
        for (int i = 0; i < numPoint; i++)
        {
            //Debug.Log("how many times?" + i);
            points[i].position = pointCloud[i];
            points[i].startSize = 0.01f;
        }
    }
        // Update is called once per frame
    void Update () {
        GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }


    void readPcd(){
        string[] buffer;
        int counter = 0;
        string line;
        StreamReader file = new StreamReader(@"C:/Program Files/pcdData/Manoi.pcd");
        if (file == null)
        {
            Debug.Log("error");
        }
        for (int i = 0; i < 9; i++)
        {
            line = file.ReadLine();
        }
        line = file.ReadLine();
        buffer = line.Split();
        Debug.Log(buffer[0] + "\t" + buffer[1]);
        numPoint = int.Parse(buffer[1]);

        pointCloud = new Vector3[numPoint];
        line = file.ReadLine();

        while ((line = file.ReadLine()) != null)
        {
            buffer = line.Split();
            pointCloud[counter] = new Vector3(float.Parse(buffer[0]), float.Parse(buffer[1]), float.Parse(buffer[2]));
            counter++;
           // Debug.Log(counter);
        }
        file.Close();
        Debug.Log("There are " + counter + "lines.");
        Debug.Log(pointCloud[0]);
        Debug.Log(pointCloud[1]);
    }
    */
}