using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public partial class culcuMat7ForReal : MonoBehaviour {


    private float[,] transJaco;
    private float[] ang7;
    private float[] forceTip;


    //position Vectors
    public Vector3 targetPosi;

    private Vector4 atracForce;
    private Vector4 sumOfRepul;
    private Vector4[] repulForce;

    private Vector4 force;

    //Objects

    private GameObject target;
    
    public  GameObject[] objectCluster = new GameObject[20];
   public  int[] clusterNum;
    private string objectName;

    private GameObject arm7Spa;
    private GameObject arm6Spa;
    private GameObject arm5Spa;
    private GameObject arm4Spa;
    private GameObject arm3Spa;
    private GameObject arm2Spa;
    private GameObject arm1Spa;

    //variables 
    public float threJaco;
    public float forceStrengthWithSig;
    public float strengthOfRepul;
    public float strengthOfAtrac;// This mean Vmax in thesis.
    public Vector3 pointFound;
    [Range(1, 5)]
    public int obstacleNum;
    int captureNum;
    float timeFromStart;
    //flags
    public bool captureModeF;
    public bool setObsAndTarF;
    public bool screenShotF;
    public bool startF; // after instantiate
    public bool startJacoF;
    public Vector3[] closestPoint;
}


public partial class culcuMat7ForReal : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startF)
        {
            getObject();
            setInitialValue();
            startF = false;
        }
        if (startJacoF)
        {
            if ((closestPoint[0] - arm1Spa.transform.position).magnitude > threJaco)
            {
                Invoke("calInvKine", 0.5f);
                Debug.Log("thre magnitude:  " + (closestPoint[0] - arm1Spa.transform.position).magnitude);
                DrawLine(closestPoint[0], arm1Spa.transform.position, Color.red, Color.black);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawLine(closestPoint[0], transform.TransformVector(arm1Spa.transform.position), Color.black, Color.blue);
        }

        if (screenShotF)
        {
            ScreenCapture.CaptureScreenshot("MovingArm" + captureNum++ + ".jpg");
            screenShotF = false;
        }

        if (captureModeF)
        {
            timeFromStart += Time.deltaTime;
            if (timeFromStart > 0.2)
            {
                ScreenCapture.CaptureScreenshot("MovingArm" + captureNum++ + ".jpg");
                timeFromStart = 0;
            }
        }


    }
}

public partial class culcuMat7ForReal
{
    void setInitialValue()
    {
        threJaco = (float)0.01; //10mm
        strengthOfAtrac = 3;
        forceStrengthWithSig = (float)1;
        strengthOfRepul = 3f;
        forceTip = new float[] { 1, 2, 3 };
        transJaco = new float[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10, 11, 12 }, { 13, 14, 15 }, { 16, 17, 18 }, { 19, 20, 21 } };
        ang7 = new float[] { 0, 0, 0, 0, 0, 0, 0 };
        captureNum = 0;
        timeFromStart = 0;
        targetPosi = new Vector3(0.2f, -0.4f, 2.7f);
    }
    private void calInvKine()
    {
        
            // Before transform.TransformVector(closestPoin trotate- arm1Spa.transform.position)
        forceStrengthWithSig =
             strengthOfAtrac / (1 + Mathf.Exp(-((closestPoint[0] - arm1Spa.transform.position).magnitude * (2f / 0.4f) - 1f) * 4f));
        atracForce = transform.TransformVector(closestPoint[0] - arm1Spa.transform.position).normalized * forceStrengthWithSig;
        pointFound = closestPoint[0];

        getTransJaco(arm7Spa.transform.localEulerAngles.y * 2 * Mathf.PI / 360,
            arm6Spa.transform.localEulerAngles.x * 2 * Mathf.PI / 360,
            arm5Spa.transform.localEulerAngles.y * 2 * Mathf.PI / 360,
            arm4Spa.transform.localEulerAngles.x * 2 * Mathf.PI / 360,
            arm3Spa.transform.localEulerAngles.y * 2 * Mathf.PI / 360,
            arm2Spa.transform.localEulerAngles.x * 2 * Mathf.PI / 360);

        forceTip = new float[] { atracForce[0] , atracForce[1] , atracForce[2]};
        //forceTip = new float[] { atracForce[0] + sumOfRepul[0], atracForce[1] + sumOfRepul[1], atracForce[2] + sumOfRepul[2] };
        for (int i = 0; i < transJaco.GetLength(0); ++i) // row 7
        {
            ang7[i] = 0;
        }
        for (int i = 0; i < transJaco.GetLength(0); ++i) // row 7
        {
            for (int j = 0; j < transJaco.GetLength(1); ++j) // col 3
            {
                ang7[i] += transJaco[i, j] * forceTip[j];
            }
        }

        arm7Spa.transform.localEulerAngles = new Vector3(0, arm7Spa.transform.localEulerAngles.y + ang7[0], 0);
        arm6Spa.transform.localEulerAngles = new Vector3(arm6Spa.transform.localEulerAngles.x + ang7[1], 0, 0);
        arm5Spa.transform.localEulerAngles = new Vector3(0, arm5Spa.transform.localEulerAngles.y + ang7[2], 0);
        arm4Spa.transform.localEulerAngles = new Vector3(arm4Spa.transform.localEulerAngles.x + ang7[3], 0, 0);
        arm3Spa.transform.localEulerAngles = new Vector3(0, arm3Spa.transform.localEulerAngles.y + ang7[4], 0);
        arm2Spa.transform.localEulerAngles = new Vector3(arm2Spa.transform.localEulerAngles.x + ang7[5], 0, 0);

        if (arm7Spa.transform.localEulerAngles.y > 360) { arm7Spa.transform.localEulerAngles = new Vector3(0, 0, 0); }
        if (arm6Spa.transform.localEulerAngles.x > 360) { arm6Spa.transform.localEulerAngles = new Vector3(0, 0, 0); }
        if (arm5Spa.transform.localEulerAngles.y > 360) { arm5Spa.transform.localEulerAngles = new Vector3(0, 0, 0); }
        if (arm4Spa.transform.localEulerAngles.x > 360) { arm4Spa.transform.localEulerAngles = new Vector3(0, 0, 0); }
        if (arm3Spa.transform.localEulerAngles.y > 360) { arm3Spa.transform.localEulerAngles = new Vector3(0, 0, 0); }
        if (arm2Spa.transform.localEulerAngles.x > 360) { arm2Spa.transform.localEulerAngles = new Vector3(0, 0, 0); }

    }



    private void getObject()
    {
        arm7Spa = GameObject.Find("arm7Space");
        arm6Spa = GameObject.Find("arm6Space");
        arm5Spa = GameObject.Find("arm5Space");
        arm4Spa = GameObject.Find("arm4Space");
        arm3Spa = GameObject.Find("arm3Space");
        arm2Spa = GameObject.Find("arm2Space");
        arm1Spa = GameObject.Find("arm1Space");

        clusterNum = readText2line();
        closestPoint = new Vector3[clusterNum[1]];
        for (int i = 0; i < clusterNum[1]; i++)
        {
            objectName = "obj" + i.ToString();
            objectCluster[i] = GameObject.Find(objectName);
        }
        for (int i = 0; i < clusterNum[1]; i++)
        {
            closestPoint[i] = findClosestPoint(i);
        }

    }
    private void DrawLine(Vector3 start, Vector3 end, Color startColor, Color endColor)
    {
        LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
        renderer.startWidth = 0.01f;
        renderer.endWidth = 0.01f;
        renderer.SetPosition(0, start);
        renderer.SetPosition(1, end);
        renderer.startColor = startColor;
        renderer.endColor = endColor;
    }

    public void instObject()
    {
 
    }
    //mainCamera.transform.position = new Vector3(1.42f, -0.51f, 1.54f);

    int[] readText2line()
    {
        int[] numPoint = new int[2];
        string[] buffer;
        string line;
        StreamReader file = new StreamReader(@"C:\Users\tamia\Documents\7dofArmWithJacobi\clusterNum.txt");
        if (file == null)
        {
            UnityEngine.Debug.Log("error");
        }
        for (int i = 0; i < 2; i++)
        {
            line = file.ReadLine();
            buffer = line.Split();
            UnityEngine.Debug.Log(buffer[0] + "\t" + buffer[1]);
            numPoint[i] = int.Parse(buffer[1]);
        }
        file.Close();
        return numPoint;
    }

    private void getTransJaco(float th7, float th6, float th5, float th4, float th3, float th2)  //complete change theata
    {
        transJaco = new float[,]{

                { (float)(((-0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th4) + (0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Cos(th4) + 0.4) * Mathf.Sin(th6) + (((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) + 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Cos(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Sin(th5)) * Mathf.Cos(th6)) * Mathf.Cos(th7) - (((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) + 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Sin(th5) + 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Cos(th5)) * Mathf.Sin(th7)) ,
                0,
                (float)(-((-0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th4) + (0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Cos(th4) + 0.4) * Mathf.Sin(th6) + (((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) + 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Cos(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Sin(th5)) * Mathf.Cos(th6)) * Mathf.Sin(th7) - (((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) + 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Sin(th5) + 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Cos(th5)) * Mathf.Cos(th7))  },

                { (float)((-0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th4) + (0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Cos(th4) + 0.4) * Mathf.Cos(th6) - (((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) + 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Cos(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Sin(th5)) * Mathf.Sin(th6))*Mathf.Sin(th7) ,
                (float)(-(-0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th4) + (0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Cos(th4) + 0.4) * Mathf.Sin(th6) - (((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) + 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Cos(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Sin(th5)) * Mathf.Cos(th6)) ,
                (float)((-0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th4) + (0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Cos(th4) + 0.4) * Mathf.Cos(th6) - (((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) + 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Cos(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Sin(th5)) * Mathf.Sin(th6))*Mathf.Cos(th7)  },

                { (float)((-((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) + 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Sin(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Cos(th5)) * Mathf.Cos(th6) * Mathf.Sin(th7) + (((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) + 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Cos(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Sin(th5)) * Mathf.Cos(th7)) ,
                 (float)-(-((0.126*Mathf.Cos(th2)+0.4)*Mathf.Sin(th4)+0.126*Mathf.Sin(th2)*Mathf.Cos(th3)*Mathf.Cos(th4))*Mathf.Sin(th5)-0.126*Mathf.Sin(th2)*Mathf.Sin(th3)*Mathf.Cos(th5))*Mathf.Sin(th6) ,
                (float)((-((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) + 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Sin(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Cos(th5)) * Mathf.Cos(th6) * Mathf.Cos(th7) - (((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) + 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Cos(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Sin(th5)) * Mathf.Sin(th7))  },

                { (float)(((-(0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Sin(th6) + ((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Cos(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th4)) * Mathf.Cos(th5) * Mathf.Cos(th6)) * Mathf.Sin(th7) + ((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Cos(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th4)) * Mathf.Sin(th5) * Mathf.Cos(th7)) ,
                (float)((-(0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Cos(th6) - ((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Cos(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th4)) * Mathf.Cos(th5) * Mathf.Sin(th6)) ,
                (float)(((-(0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Sin(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th4)) * Mathf.Sin(th6) + ((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Cos(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th4)) * Mathf.Cos(th5) * Mathf.Cos(th6)) * Mathf.Cos(th7) - ((0.126 * Mathf.Cos(th2) + 0.4) * Mathf.Cos(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th4)) * Mathf.Sin(th5) * Mathf.Sin(th7))  },

                { (float)((0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Sin(th4) * Mathf.Sin(th6) + (-0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Cos(th4) * Mathf.Cos(th5)) * Mathf.Cos(th6)) * Mathf.Sin(th7) + (0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Cos(th4) * Mathf.Sin(th5)) * Mathf.Cos(th7)) ,
                (float)(0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Sin(th4) * Mathf.Cos(th6) - (-0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Cos(th4) * Mathf.Cos(th5)) * Mathf.Sin(th6)) ,
                (float)((0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Sin(th4) * Mathf.Sin(th6) + (-0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Sin(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Cos(th4) * Mathf.Cos(th5)) * Mathf.Cos(th6)) * Mathf.Cos(th7) - (0.126 * Mathf.Sin(th2) * Mathf.Cos(th3) * Mathf.Cos(th5) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th3) * Mathf.Cos(th4) * Mathf.Sin(th5)) * Mathf.Sin(th7))  },

                { (float)(((-0.126 * Mathf.Cos(th2) * Mathf.Cos(th3) * Mathf.Sin(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Cos(th4)) * Mathf.Sin(th6) + ((0.126 * Mathf.Cos(th2) * Mathf.Cos(th3) * Mathf.Cos(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th4)) * Mathf.Cos(th5) - 0.126 * Mathf.Cos(th2) * Mathf.Sin(th3) * Mathf.Sin(th5)) * Mathf.Cos(th6)) * Mathf.Sin(th7) + ((0.126 * Mathf.Cos(th2) * Mathf.Cos(th3) * Mathf.Cos(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th4)) * Mathf.Sin(th5) + 0.126 * Mathf.Cos(th2) * Mathf.Sin(th3) * Mathf.Cos(th5)) * Mathf.Cos(th7)) ,
                (float)((-0.126 * Mathf.Cos(th2) * Mathf.Cos(th3) * Mathf.Sin(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Cos(th4)) * Mathf.Cos(th6) - ((0.126 * Mathf.Cos(th2) * Mathf.Cos(th3) * Mathf.Cos(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th4)) * Mathf.Cos(th5) - 0.126 * Mathf.Cos(th2) * Mathf.Sin(th3) * Mathf.Sin(th5)) * Mathf.Sin(th6)) ,
                (float)(((-0.126 * Mathf.Cos(th2) * Mathf.Cos(th3) * Mathf.Sin(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Cos(th4)) * Mathf.Sin(th6) + ((0.126 * Mathf.Cos(th2) * Mathf.Cos(th3) * Mathf.Cos(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th4)) * Mathf.Cos(th5) - 0.126 * Mathf.Cos(th2) * Mathf.Sin(th3) * Mathf.Sin(th5)) * Mathf.Cos(th6)) * Mathf.Cos(th7) - ((0.126 * Mathf.Cos(th2) * Mathf.Cos(th3) * Mathf.Cos(th4) - 0.126 * Mathf.Sin(th2) * Mathf.Sin(th4)) * Mathf.Sin(th5) + 0.126 * Mathf.Cos(th2) * Mathf.Sin(th3) * Mathf.Cos(th5)) * Mathf.Sin(th7)) },

                { 0,
                0,
                0 }
        };

    }
    private void callSinnbar()
    {
        Debug.Log("I don't know him");
    }
    private void callFos()
    {
        Debug.Log("Cheer up Sinnabar!");
    }
    private void confMat1(float[] mat)
    {
        Debug.Log("Value from here.");
        foreach (int x in mat)
        {
            Debug.Log(x);
        }
    }
    private Vector3 findClosestPoint(int clusterNum)
    {
        int closestPointNum = 0;
        float closestDis = 100;
        for(int i = 0; i < objectCluster[clusterNum].GetComponent<objectPrefab>().pcPointsAddjusted.Length; i++)
        {
            if((objectCluster[clusterNum].GetComponent<objectPrefab>().pcPointsAddjusted[i] - arm1Spa.transform.position).magnitude < closestDis)
            {
                closestPointNum = i;
                closestDis = (objectCluster[clusterNum].GetComponent<objectPrefab>().pcPointsAddjusted[i] - arm1Spa.transform.position).magnitude;
            }
        }
        return objectCluster[clusterNum].GetComponent<objectPrefab>().pcPointsAddjusted[closestPointNum];
    }

}
