using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;


public partial class culcuMat7 : MonoBehaviour
{

    private float[,] transJaco;
    private float[] ang7;
    private float[] forceTip;

    public float distance;
    public float magnitude;

    //position Vectors
    public Vector3 targetPosi;
    public Vector3 targetCentroid;
    private Vector3 closestPointPla;
    private Vector3 closestPointObj;
    private Vector4 atracForce;
    private Vector4 sumOfRepul;
    private Vector4 repulForcePla;
    private Vector4 repulForceObj;
    
    private Vector4 force;

    //Objects

    private GameObject target;
    private GameObject targetNum2;
    private GameObject mainTarget;
    private GameObject tempObj;
    private GameObject mainPercep;

    private GameObject obstacle1;
    private GameObject obstacle2;
    private GameObject obstacle3;
    private GameObject obstacle4;
    private GameObject obstacle5;


    private GameObject arm7Spa;
    private GameObject arm6Spa;
    private GameObject arm5Spa;
    private GameObject arm4Spa;
    private GameObject arm3Spa;
    private GameObject arm2Spa;
    private GameObject arm1Spa;

    StreamWriter writer;
    //variables 
    public float threJacoInitial;
    public float threJaco;

    public float forceAttracValue;
    public float forceRepulValue;
    public float strengthOfRepulObj;
    public float strengthOfRepulPla;
    public float strengthOfAtrac;// This mean Vmax in thesis.

    public float atracComp;
    public float atracComp2;

    [Range(1, 5)]
    public int obstacleNum;
    int captureNum;
    float timeFromStart;
    //flags
    public bool[] obstacleActivateF;
    public bool targetActivateF;
    public bool captureModeF;
    public bool setObsAndTarF;
    public bool screenShot;
    public bool firstGoalF;
    public bool secondGoalF;
    public bool writerF;
    public bool forwardF;
    private bool pMrunningF;
}


public partial class culcuMat7 : MonoBehaviour { 
    // Use this for initialization
    void Start()
    {
        getObject();
        setInitialValue();
        setTar();

        writer = new StreamWriter(@"C:\Users\tamia\Documents\7dofArmWithJacobi\DistanceInfo.csv");
        if(writer == null)
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        closestPointPla = mostClosestPointPlane(arm1Spa.transform.position);
        closestPointObj = mostClosestPointObj(arm1Spa.transform.position);
        
        calInvKine();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawLine(target.transform.position, transform.TransformVector(arm1Spa.transform.position), Color.black, Color.blue);
        }

        
        if (captureModeF)
        {
            timeFromStart += Time.deltaTime;
            if (timeFromStart > 0.2)
            {
                ScreenCapture.CaptureScreenshot("screenShot\\MovingArm" + captureNum++ + ".jpg");
                timeFromStart = 0;
            }
        }
    }
}

public partial class culcuMat7
{
    IEnumerator potentialMethod()
    {
        if (pMrunningF) yield break;
        pMrunningF = true;
        yield return StartCoroutine(potentialFirstStep(arm1Spa.transform.position));
        StartCoroutine(potentialSecondStep());
        pMrunningF = false;
    }

    IEnumerator potentialFirstStep(Vector3 refPoint)
    {
        ParticleSystem obj_ps;
        ParticleSystem.Particle[] obj_p;
        int particleNum;
        float refDistance = float.MaxValue;

        int counter = 0;
        string filename;
        for (int i = 0; i < 1 /*mainPercep.GetComponent<Filer>().clusterNum[0]*/; i++)
        {
            filename = "pla" + i.ToString();
            tempObj = GameObject.Find(filename);
            obj_ps = tempObj.GetComponent<ParticleSystem>();
            obj_p = new ParticleSystem.Particle[obj_ps.particleCount];
            particleNum = obj_ps.GetParticles(obj_p);
            while (counter + 300 < particleNum)
            {
                for (int j = 0; j < 300; j++)
                {
                    if ((obj_p[counter].position - refPoint).magnitude < refDistance)
                    {
                        closestPointPla = obj_p[counter].position;
                        refDistance = (obj_p[counter].position - refPoint).magnitude;
                    }
                    counter++;
                }
                //DrawLine(refPoint, closestPointPla, Color.black, Color.red);
                yield return null;
            }
            yield return null;
        }

        for (int l = 0; l < 14/*mainPercep.GetComponent<Filer>().clusterNum[1]*/; l++)
        {
            if (l == 1) continue;
            if (l == 7) continue;
            counter = 0;
            refDistance = float.MaxValue;
            filename = "obj" + l.ToString();
            tempObj = GameObject.Find(filename);
            obj_ps = tempObj.GetComponent<ParticleSystem>();
            obj_p = new ParticleSystem.Particle[obj_ps.particleCount];
            particleNum = obj_ps.GetParticles(obj_p);
            while (counter + 10 < particleNum)
            {
                for (int k = 0; k < 10; k++)
                {
                    
                    if ((obj_p[counter].position - refPoint).magnitude < refDistance)
                    {
                        closestPointObj = obj_p[counter].position;
                        refDistance = (obj_p[counter].position - refPoint).magnitude;
                    }
                    counter++;
                }
               // DrawLine(refPoint, closestPointObj, Color.black, Color.red);
                yield return null;
            }
            yield return null;
        }
    }
    IEnumerator potentialSecondStep()
    {

        if ((target.transform.position - arm1Spa.transform.position).magnitude > threJacoInitial && firstGoalF)
        {
            forceAttracValue =
                  strengthOfAtrac / (1 + Mathf.Exp(-(transform.TransformVector(target.transform.position - arm1Spa.transform.position).magnitude * (2f / 0.4f) - 1f) * 4f));
            atracForce = transform.TransformVector(target.transform.position - arm1Spa.transform.position).normalized * forceAttracValue;
            /*
            repulForcePla = strengthOfRepul / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - obstacle1.transform.position).magnitude * (2f / 0.4f) - 1f) * 4f))
                    * transform.TransformVector(arm1Spa.transform.position - obstacle1.transform.position).normalized;
            */
            repulForceObj = strengthOfRepulObj / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - closestPointObj).magnitude * (2f / 0.4f) - 1f) * 4f))
                * transform.TransformVector(arm1Spa.transform.position - closestPointObj).normalized;
            
        }
        else if ((target.transform.position - arm1Spa.transform.position).magnitude < threJacoInitial)
        {
            firstGoalF = false;
            secondGoalF = true;
            mainTarget = GameObject.Find("obj7");

            targetCentroid = getCentroid2();
            targetNum2.transform.position = targetCentroid;
        }

        if ((targetCentroid - arm1Spa.transform.position).magnitude > threJaco && secondGoalF)
        {
            forceAttracValue =
                  strengthOfAtrac / (1 + Mathf.Exp(-(transform.TransformVector(targetCentroid - arm1Spa.transform.position).magnitude * (2f / 0.4f) - 1f) * 4f));
            atracForce = transform.TransformVector(targetCentroid - arm1Spa.transform.position).normalized * forceAttracValue;
            
            repulForceObj = strengthOfRepulObj / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - closestPointObj).magnitude * (2f / 0.4f) - 1f) * 4f))
                * transform.TransformVector(arm1Spa.transform.position - closestPointObj).normalized;

            DrawLine(arm1Spa.transform.position, closestPointObj, Color.red, Color.blue);
            DrawLine(arm1Spa.transform.position, mostClosestPointPlane(arm1Spa.transform.position), Color.red, Color.blue);
        }
        else if ((targetCentroid - arm1Spa.transform.position).magnitude < threJaco)
        {
            secondGoalF = false;
        }
        if (secondGoalF || firstGoalF)
        {

            forceTip = new float[] { atracForce[0] + repulForceObj.x, atracForce[1] + repulForceObj.y, atracForce[2] + repulForceObj.z };
            getTransJaco(arm7Spa.transform.localEulerAngles.y * 2 * Mathf.PI / 360,
                arm6Spa.transform.localEulerAngles.x * 2 * Mathf.PI / 360,
                arm5Spa.transform.localEulerAngles.y * 2 * Mathf.PI / 360,
                arm4Spa.transform.localEulerAngles.x * 2 * Mathf.PI / 360,
                arm3Spa.transform.localEulerAngles.y * 2 * Mathf.PI / 360,
                arm2Spa.transform.localEulerAngles.x * 2 * Mathf.PI / 360);

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
        yield return null;
    }
    private void calInvKine()
    {

        if (forwardF == true)
        {

                if ((target.transform.position - arm1Spa.transform.position).magnitude > threJacoInitial && firstGoalF)
                {
                    atracForce = strengthOfAtrac / (1 + Mathf.Exp((-transform.TransformVector(target.transform.position - arm1Spa.transform.position).magnitude * (2f / 1f) +0.3f) * 4f))
                        * transform.TransformVector(target.transform.position - arm1Spa.transform.position).normalized;


                    repulForceObj = strengthOfRepulObj / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - closestPointObj).magnitude * (2f / 0.3f) - 0.5f) * 6f))
                        * transform.TransformVector(arm1Spa.transform.position - closestPointObj).normalized;
                    repulForcePla = strengthOfRepulPla / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - closestPointPla).magnitude * (2f / 0.1f) - 1f) * 8f))
                        * transform.TransformVector(arm1Spa.transform.position - closestPointPla).normalized;

                    forceAttracValue = atracForce.magnitude;
                    forceRepulValue = repulForcePla.magnitude + repulForceObj.magnitude;
                    DrawLine(arm1Spa.transform.position, closestPointObj, Color.black, Color.red);
                }
                else if ((target.transform.position - arm1Spa.transform.position).magnitude < threJacoInitial)
                {
                    firstGoalF = false;
                    secondGoalF = true;
                    mainTarget = GameObject.Find("obj7");

                    targetCentroid = getCentroid2();
                    targetNum2.transform.position = targetCentroid;
                }

                if ((targetCentroid - arm1Spa.transform.position).magnitude > threJaco && secondGoalF)
                {
                    atracForce = strengthOfAtrac / (1 + Mathf.Exp((-transform.TransformVector(targetCentroid - arm1Spa.transform.position).magnitude * (2f / 1f) +0.3f)* 4f))
                        * transform.TransformVector(targetCentroid - arm1Spa.transform.position).normalized;

                    repulForceObj = strengthOfRepulObj / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - closestPointObj).magnitude * (2f / 0.3f) - 0.5f) * 6f))
                        * transform.TransformVector(arm1Spa.transform.position - closestPointObj).normalized;
                    repulForcePla = strengthOfRepulPla / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - closestPointPla).magnitude * (2f / 0.1f) - 1f) * 8f))
                        * transform.TransformVector(arm1Spa.transform.position - closestPointPla).normalized;

                    forceAttracValue = atracForce.magnitude;

                    forceRepulValue = repulForcePla.magnitude + repulForceObj.magnitude;
                    DrawLine(arm1Spa.transform.position, closestPointObj, Color.black, Color.red);
                }
                else if ((targetCentroid - arm1Spa.transform.position).magnitude < threJaco)
                {
                    secondGoalF = false;
                }
        }
        
        else
        {
            mainTarget = GameObject.Find("obj7");
            targetCentroid = getCentroid2();
            targetNum2.transform.position = targetCentroid;
            if ((targetCentroid - arm1Spa.transform.position).magnitude > threJacoInitial && secondGoalF)
            {
                magnitude = (targetCentroid - arm1Spa.transform.position).magnitude;
                distance = Vector3.Distance(closestPointObj, arm1Spa.transform.position);

                atracForce = strengthOfAtrac / (1 + Mathf.Exp((-transform.TransformVector(targetCentroid - arm1Spa.transform.position).magnitude * (2f / 1f) + 0.3f) * 4f))
                    * transform.TransformVector(targetCentroid - arm1Spa.transform.position).normalized;

                repulForceObj = strengthOfRepulObj / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - closestPointObj).magnitude * (2f / 0.3f) - 0.5f) * 6))
                    * transform.TransformVector(arm1Spa.transform.position - closestPointObj).normalized;
                repulForcePla = strengthOfRepulPla / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - closestPointPla).magnitude * (2f / 0.1f) - 1f) * 8f))
                    * transform.TransformVector(arm1Spa.transform.position - closestPointPla).normalized;

                forceAttracValue = atracForce.magnitude;

                forceRepulValue = repulForcePla.magnitude + repulForceObj.magnitude;
                DrawLine(arm1Spa.transform.position, closestPointObj, Color.black, Color.red);
            }
            else if ((targetCentroid - arm1Spa.transform.position).magnitude < threJacoInitial)
            {
                firstGoalF = true;
                secondGoalF = false;

            }

            if ((target.transform.position - arm1Spa.transform.position).magnitude > threJaco && firstGoalF)
            {

                magnitude = (targetCentroid - arm1Spa.transform.position).magnitude;
                distance = Vector3.Distance(closestPointObj, arm1Spa.transform.position);

                    atracForce = strengthOfAtrac / (1 + Mathf.Exp((-transform.TransformVector(target.transform.position - arm1Spa.transform.position).magnitude * (2f / 1f) + 0.3f) * 4f))
                        * transform.TransformVector(target.transform.position - arm1Spa.transform.position).normalized;


                    repulForceObj = strengthOfRepulObj / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - closestPointObj).magnitude * (2f / 0.3f) - 0.5f) * 6f))
                        * transform.TransformVector(arm1Spa.transform.position - closestPointObj).normalized;
                    repulForcePla = strengthOfRepulPla / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - closestPointPla).magnitude * (2f / 0.1f) - 1f) * 8f))
                        * transform.TransformVector(arm1Spa.transform.position - closestPointPla).normalized;

                    forceAttracValue = atracForce.magnitude;
                    forceRepulValue = repulForcePla.magnitude + repulForceObj.magnitude;
                    DrawLine(arm1Spa.transform.position, closestPointObj, Color.black, Color.red);
                }
            else if ((target.transform.position - arm1Spa.transform.position).magnitude < threJaco)
            {
                firstGoalF = false;
            }

        }

        if (secondGoalF || firstGoalF)
        {
            writer.WriteLine((closestPointObj - arm1Spa.transform.position).magnitude);

            forceTip = new float[] { atracForce[0] + repulForceObj.x + repulForcePla.x, atracForce[1] + repulForceObj.y + repulForcePla.y, atracForce[2] + repulForceObj.z + repulForcePla.z};
            getTransJaco(arm7Spa.transform.localEulerAngles.y * 2 * Mathf.PI / 360,
                arm6Spa.transform.localEulerAngles.x * 2 * Mathf.PI / 360,
                arm5Spa.transform.localEulerAngles.y * 2 * Mathf.PI / 360,
                arm4Spa.transform.localEulerAngles.x * 2 * Mathf.PI / 360,
                arm3Spa.transform.localEulerAngles.y * 2 * Mathf.PI / 360,
                arm2Spa.transform.localEulerAngles.x * 2 * Mathf.PI / 360);

            for (int i = 0; i < transJaco.GetLength(0); ++i) // row 7
            {
                ang7[i] = 0;
            }
            for (int i = 0; i < transJaco.GetLength(0); ++i) // row 7
            {
                for (int j = 0; j < transJaco.GetLength(1); ++j) // col 3
                {
                    ang7[i] += transJaco[i, j] * forceTip[j]*0.2f;
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
        if(writerF == false)
        {
            writer.Close();
        }
    }
    void setInitialValue()
    {
        if(forwardF == true)
        {

            firstGoalF = true;
            secondGoalF = false;
        }
        else
        {
            firstGoalF = false;
            secondGoalF = true;
        }
        threJaco = (float)0.01;
        threJacoInitial = (float)0.01;
        
        forceAttracValue = (float)1;
        strengthOfAtrac = 6f;
        strengthOfRepulObj = 1.5f;
        strengthOfRepulPla = 1f;
        forceTip = new float[] { 1, 2, 3 };
        transJaco = new float[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10, 11, 12 }, { 13, 14, 15 }, { 16, 17, 18 }, { 19, 20, 21 } };
        ang7 = new float[] { 0, 0, 0, 0, 0, 0, 0 };
        captureNum = 0;
        timeFromStart = 0;
        targetActivateF = true;
        pMrunningF = false;
        targetPosi = new Vector3(-0.447f, -1.447f, 2.431f);
        target.transform.position = targetPosi;
        target.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        closestPointObj = new Vector3();
        closestPointPla = new Vector3();
    }
    
    IEnumerator slowMethod()
    {
        yield return new WaitForSeconds(0.2f);
    }
    private void setTar()
    {
        target.transform.position = targetPosi;
    }
    private void getObject()
    {
        target = GameObject.Find("target");
        targetNum2 = GameObject.Find("targetNum2");

        mainPercep = GameObject.Find("mainPerceptor");
        arm7Spa = GameObject.Find("arm7Space");
        arm6Spa = GameObject.Find("arm6Space");
        arm5Spa = GameObject.Find("arm5Space");
        arm4Spa = GameObject.Find("arm4Space");
        arm3Spa = GameObject.Find("arm3Space");
        arm2Spa = GameObject.Find("arm2Space");
        arm1Spa = GameObject.Find("arm1Space");
    }

    private void logDistanceInfo()
    {
    }

    private void DrawLine(Vector3 start, Vector3 end, Color startColor, Color endColor)
    {
        LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
        renderer.startWidth = 0.1f;
        renderer.endWidth = 0.01f;
        renderer.SetPosition(0, start);
        renderer.SetPosition(1,end);
        renderer.startColor = startColor;
        renderer.endColor = endColor;
    }


    //mainCamera.transform.position = new Vector3(1.42f, -0.51f, 1.54f);

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

    private void confMat1(float[] mat)
    {
        Debug.Log("Value from here.");
        foreach(int x in mat)
        {
            Debug.Log(x);
        }
    }

    public Vector3 mostClosestPointPlane(Vector3 refPoint)
    {
        Vector3 closestPoint = new Vector3();
        ParticleSystem obj_ps;
        ParticleSystem.Particle[] obj_p;
        int particleNum;
        float refDistance = float.MaxValue;

        string filename;
        for(int i = 0; i< 1; i++)
        {
            filename = "pla" + i.ToString();
            tempObj = GameObject.Find(filename);
            obj_ps = tempObj.GetComponent<ParticleSystem>();
            obj_p = new ParticleSystem.Particle[obj_ps.particleCount];
            particleNum = obj_ps.GetParticles(obj_p);
            for(int j = 0; j < particleNum; j++)
            {
                if((obj_p[j].position - refPoint).magnitude < refDistance)
                {
                    closestPoint = obj_p[j].position;
                    refDistance = (obj_p[j].position - refPoint).magnitude;

                }
            }
            DrawLine(refPoint, closestPoint, Color.black, Color.red);
        }
        return closestPoint;
    }
    public Vector3 mostClosestPointObj(Vector3 refPoint)
    {
        Vector3 closestPoint = new Vector3();
        ParticleSystem obj_ps;
        ParticleSystem.Particle[] obj_p;
        int particleNum;
        float refDistance = float.MaxValue;

        string filename;
        for (int i = 0; i < 14/*mainPercep.GetComponent<Filer>().clusterNum[1]*/; i++)
        {
            if (i == 1) continue;
            if (i == 7) continue;
            filename = "obj" + i.ToString();
            tempObj = GameObject.Find(filename);
            obj_ps = tempObj.GetComponent<ParticleSystem>();
            obj_p = new ParticleSystem.Particle[obj_ps.main.maxParticles];
            particleNum = obj_ps.GetParticles(obj_p);
            for (int j = 0; j < particleNum; j++)
            {
                if ((obj_p[j].position - refPoint).magnitude < refDistance)
                {
                    
                    refDistance = (obj_p[j].position - refPoint).magnitude;
                    closestPoint = obj_p[j].position;
                    
                }
               // Debug.Log("Particle num" + j);
            }
        }
        return closestPoint;
    }
    private Vector3 getCentroid2()
    {
        Vector3 Centroid = new Vector3();
        ParticleSystem m_ps;
        m_ps = mainTarget.GetComponent<ParticleSystem>();
        ParticleSystem.Particle[] mainParticle;
        mainParticle = new ParticleSystem.Particle[m_ps.main.maxParticles];
        float xC, yC, zC;
        xC = 0;
        yC = 0;
        zC = 0;
        int numOfParticle;
        numOfParticle = m_ps.GetParticles(mainParticle);
        for (int i = 0; i < numOfParticle; i++){
            //Debug.Log(mainParticle[i].position);
            xC += mainParticle[i].position.x;
            yC += mainParticle[i].position.y;
            zC += mainParticle[i].position.z;
        }
        Centroid = new Vector3(xC / numOfParticle, yC / numOfParticle, zC / numOfParticle);
        return Centroid;
    }
    private Vector3 getCentroid()
    {
        int numPoint = 0;
        float numPoint2;
        Vector3[] points;
        Vector3 pointCentroid ;
        string[] buffer;
        int counter = 0;
        string line;
        StreamReader file = new StreamReader(@"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster7.pcd");
        float xCentroid, yCentroid, zCentroid;
        xCentroid = 0f;
        yCentroid = 0f;
        zCentroid = 0f;

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
        numPoint2 = (float)numPoint;
        points = new Vector3[numPoint];
        line = file.ReadLine();

        while ((line = file.ReadLine()) != null)
        {
            buffer = line.Split();
            points[counter] = new Vector3(float.Parse(buffer[0]), float.Parse(buffer[1]), float.Parse(buffer[2]));
            xCentroid += float.Parse(buffer[0]);
            yCentroid += float.Parse(buffer[1]);
            zCentroid += float.Parse(buffer[2]);
            counter++;
        }
        pointCentroid = new Vector3(xCentroid / numPoint2, yCentroid / numPoint2, zCentroid / numPoint2);
        file.Close();
        UnityEngine.Debug.Log("There are " + counter + "lines.");
        return pointCentroid;
    }

    Vector3[] readPcd(string filename)
    {
        int numPoint = 0;
        Vector3[] points;
        string[] buffer;
        int counter = 0;
        string line;
        StreamReader file = new StreamReader(filename);
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
            points[counter] = new Vector3(float.Parse(buffer[0]), float.Parse(buffer[1]), float.Parse(buffer[2]));
            counter++;
        }
        file.Close();
        UnityEngine.Debug.Log("There are " + counter + "lines.");
        return points;
    }
}


/*
 * 
        
        obstacleActivateF = new bool[5] { false, false, false, false, false };
        obstaclePos[0] = new Vector3(1, -1, 3);
        obstaclePos[1] = new Vector3(-1, -1, 3);
        obstaclePos[2] = new Vector3(0.5f, -0.5f, 3);
        obstaclePos[3] = new Vector3(0.5f, -0.3f, 2.8f);
        obstaclePos[4] = new Vector3(0.3f, -0.4f, 2.9f);
        public Vector3[] obstaclePos = new Vector3[5];
        obstacle1.transform.position = obstaclePos[0];
        obstacle2.transform.position = obstaclePos[1];
        obstacle3.transform.position = obstaclePos[2];
        obstacle4.transform.position = obstaclePos[3];
        obstacle5.transform.position = obstaclePos[4];    
        obstacle1 = GameObject.Find("obstacle1");
        obstacle2 = GameObject.Find("obstacle2");
        obstacle3 = GameObject.Find("obstacle3");
        obstacle4 = GameObject.Find("obstacle4");
        obstacle5 = GameObject.Find("obstacle5");

        obstaclePos[0] = new Vector3(1,-1,3);
        obstaclePos[1] = new Vector3(-1,-1,3);
        obstaclePos[2] = new Vector3(0.5f,-0.5f,3);
        obstaclePos[3] = new Vector3(0.5f,-0.3f,2.8f);
        obstaclePos[4] = new Vector3(0.3f, -0.4f, 2.9f);
        obstacleActivateF = new bool[5] { true , true, true, true, true };
        if (obstacleActivateF[0])
        {

            repulForce1 = strengthOfRepul / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - obstacle1.transform.position).magnitude * (2f / 0.4f) - 1f) * 4f))
                * transform.TransformVector(arm1Spa.transform.position - obstacle1.transform.position).normalized;
        }
        if (obstacleActivateF[1])
        {
            repulForce2 = strengthOfRepul / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - obstacle2.transform.position).magnitude * (2f / 0.4f) - 1f) * 4f))
                * transform.TransformVector(arm1Spa.transform.position - obstacle2.transform.position).normalized;
            
        }
        if (obstacleActivateF[2])
        {
            repulForce3 = strengthOfRepul / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - obstacle3.transform.position).magnitude * (2f / 0.4f) - 1f) * 4f))
                * transform.TransformVector(arm1Spa.transform.position - obstacle3.transform.position).normalized;
            
        }
        if (obstacleActivateF[3])
        {
            repulForce4 = strengthOfRepul / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - obstacle4.transform.position).magnitude * (2f / 0.4f) - 1f) * 4f))
                * transform.TransformVector(arm1Spa.transform.position - obstacle4.transform.position).normalized;
            
        }
        if (obstacleActivateF[4])
        {
            repulForce5 = strengthOfRepul / (1 + Mathf.Exp((transform.TransformVector(arm1Spa.transform.position - obstacle5.transform.position).magnitude * (2f / 0.4f) - 1f) * 4f))
                * transform.TransformVector(arm1Spa.transform.position - obstacle5.transform.position).normalized;
            
        }
        
        sumOfRepul = (repulForce1 + repulForce2 + repulForce3 + repulForce4 + repulForce5) * strengthOfRepul;
        */
