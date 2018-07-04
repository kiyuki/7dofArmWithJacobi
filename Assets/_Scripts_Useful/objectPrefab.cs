using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class objectPrefab : MonoBehaviour {

    public string filePath { get; set; }
    public int objectNum { get; set; }
    public Vector3[] pcPoints { get; set; }
    public Vector3[] pcPointsAddjusted { get; set; }
    private float angle;
    public float Angle
    {
        get
        {
            Debug.Log("AngleGetter: Not auto propaty, this is propaty firstTime.");
            return angle;
        }
        set
        {
            angle = value;
        }
    }
    private float[] colorPallet;

    public ParticleSystem.Particle[] psPoints;

    public void getFilePath(int fileNum)
    {
        switch (fileNum)
        {
            case 0:
                filePath = objPath.objPath0;
                break;
            case 1:
                filePath = objPath.objPath1;
                break;
            case 2:
                filePath = objPath.objPath2;
                break;
            case 3:
                filePath = objPath.objPath3;
                break;
            case 4:
                filePath = objPath.objPath4;
                break;
            case 5:
                filePath = objPath.objPath5;
                break;
            case 6:
                filePath = objPath.objPath6;
                break;
            case 7:
                filePath = objPath.objPath7;
                break;
            case 8:
                filePath = objPath.objPath8;
                break;
            case 9:
                filePath = objPath.objPath9;
                break;
            case 10:
                filePath = objPath.objPath10;
                break;
            case 11:
                filePath = objPath.objPath11;
                break;
            case 12:
                filePath = objPath.objPath12;
                break;
            case 13:
                filePath = objPath.objPath13;
                break;
            case 14:
                filePath = objPath.objPath14;
                break;
            case 15:
                filePath = objPath.objPath15;
                break;
            case 16:
                filePath = objPath.objPath16;
                break;
            case 17:
                filePath = objPath.objPath17;
                break;
            case 18:
                filePath = objPath.objPath18;
                break;
            case 19:
                filePath = objPath.objPath19;
                break;
            case 20:
                filePath = objPath.objPath20;
                break;
            case 21:
                filePath = objPath.objPath21;
                break;
            case 22:
                filePath = objPath.objPath22;
                break;
            case 23:
                filePath = objPath.objPath23;
                break;
            case 24:
                filePath = objPath.objPath24;
                break;
            case 25:
                filePath = objPath.objPath25;
                break;
            case 26:
                filePath = objPath.objPath26;
                break;
            case 27:
                filePath = objPath.objPath27;
                break;
            case 28:
                filePath = objPath.objPath28;
                break;
            case 29:
                filePath = objPath.objPath29;
                break;
            case 30:
                filePath = objPath.objPath30;
                break;
        }
    }

    public void sayInfo()
    {
        Debug.Log(filePath);
    }

    public void readPcd(string filename)
    {
        int numPoint = 0;

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

        pcPoints = new Vector3[numPoint];
        line = file.ReadLine();

        while ((line = file.ReadLine()) != null)
        {
            buffer = line.Split();
            pcPoints[counter] = new Vector3(float.Parse(buffer[0]), float.Parse(buffer[1]), float.Parse(buffer[2]));
            counter++;
        }
        file.Close();
        UnityEngine.Debug.Log("There are " + counter + "lines.");
    }

    public void getRadianGrandToPc()
    {
        Vector3 normal = new Vector3();
        Vector3 grandVec = new Vector3(0, -1, 0);

        string[] buffer;
        string line;
        StreamReader file = new StreamReader(@"C:\Users\tamia\Documents\7dofArmWithJacobi\PlanerCoefficients.txt");

        for (int i = 0; i < 3; i++)
        {
            line = file.ReadLine();
        }
        line = file.ReadLine();
        buffer = line.Split();
        UnityEngine.Debug.Log(buffer[0] + "\t" + buffer[1] + "\t" + buffer[2] + "\t" + buffer[3] + "\t" + buffer[4] + "\t" + buffer[5]);
        normal.x = float.Parse(buffer[5]);

        line = file.ReadLine();
        buffer = line.Split();
        UnityEngine.Debug.Log(buffer[0] + "\t" + buffer[1] + "\t" + buffer[2] + "\t" + buffer[3] + "\t" + buffer[4] + "\t" + buffer[5]);
        normal.y = float.Parse(buffer[5]);

        line = file.ReadLine();
        buffer = line.Split();
        UnityEngine.Debug.Log(buffer[0] + "\t" + buffer[1] + "\t" + buffer[2] + "\t" + buffer[3] + "\t" + buffer[4] + "\t" + buffer[5]);
        normal.z = float.Parse(buffer[5]);

        file.Close();
        angle = Mathf.Acos((normal.x * grandVec.x + normal.y * grandVec.y + normal.z * grandVec.z) / normal.magnitude * grandVec.magnitude);
        UnityEngine.Debug.Log(angle);
        UnityEngine.Debug.Log(angle * (360 / (2 * Mathf.PI)));
    }

    public void showPointCloudRotateCresGraund(Vector3[] pcPoints, float radi)
    {
        getColorPallet();
        Vector3[] rotateMatrixx = new Vector3[3];
        Vector3 bufVec;
        //float radi = theta * (2 * Mathf.PI / 360);
        rotateMatrixx[0] = new Vector3(1, 0, 0);
        rotateMatrixx[1] = new Vector3(0, Mathf.Cos(radi), -Mathf.Sin(radi));
        rotateMatrixx[2] = new Vector3(0, Mathf.Sin(radi), Mathf.Cos(radi));
        psPoints = new ParticleSystem.Particle[pcPoints.Length];

        for (int i = 0; i < pcPoints.Length; i++)
        {
            bufVec = pcPoints[i];

            psPoints[i].position = new Vector3(pcPoints[i].x,
                pcPoints[i].x * rotateMatrixx[0][1] + pcPoints[i].y * rotateMatrixx[1][1] + pcPoints[i].z * rotateMatrixx[2][1],
                pcPoints[i].x * rotateMatrixx[0][2] + pcPoints[i].y * rotateMatrixx[1][2] + pcPoints[i].z * rotateMatrixx[2][2]); ;
            //psPoints[i].startColor = new Color(colorPallet[0], colorPallet[1], colorPallet[2]);
            psPoints[i].startColor = Color.black;
            psPoints[i].startSize = 0.01f;
        }
        GetComponent<ParticleSystem>().SetParticles(psPoints, psPoints.Length);
    }

    private void getColorPallet()
    {
        colorPallet = new float[3];
        /*
        int product1;
        int product2;
        int remain1;
        int remain2;

        if (objectNum < 4)
        {
            colorPallet[2] = (float)(objectNum + 1) / 4;
            colorPallet[1] = 0f;
            colorPallet[0] = 0.5f;
        }
        else if (4 <= objectNum && objectNum < 16)
        {
            product1 = objectNum / 4;
            colorPallet[2] = (float)(product1 + 1) / 4;
            remain1 = objectNum - 4 * product1;
            colorPallet[1] = (float)(remain1 + 1) / 4;
            colorPallet[0] = 0.5f;

        }
        else if (16 <= objectNum && objectNum < 64)
        {
            product2 = objectNum / 16;
            remain2 = objectNum - 16 * product2;

            product1 = remain2 / 4;
            remain1 = remain2 - 4 * product1;

            colorPallet[2] = (float)(remain1 + 1) / 4;
            colorPallet[1] = (float)(remain2 + 1) / 4;
            colorPallet[0] = (float)(product2 + 1) / 4;
        }
        */
        colorPallet[2] = (float)0;
        colorPallet[1] = (float)0;
        colorPallet[0] = (float)1;
    }

    public void rotatePointCloud(Vector3[] pcPoints, float radi)
    {
        Vector3[] rotateMatrixx = new Vector3[3];
        pcPointsAddjusted = new Vector3[pcPoints.Length];
        //float radi = theta * (2 * Mathf.PI / 360);
        rotateMatrixx[0] = new Vector3(1, 0, 0);
        rotateMatrixx[1] = new Vector3(0, Mathf.Cos(radi), -Mathf.Sin(radi));
        rotateMatrixx[2] = new Vector3(0, Mathf.Sin(radi), Mathf.Cos(radi));
        for (int i = 0; i < pcPoints.Length; i++)
        {
            psPoints[i].position = transform.InverseTransformPoint(new Vector3(pcPoints[i].x,
                pcPoints[i].x * rotateMatrixx[0][1] + pcPoints[i].y * rotateMatrixx[1][1] + pcPoints[i].z * rotateMatrixx[2][1],
                pcPoints[i].x * rotateMatrixx[0][2] + pcPoints[i].y * rotateMatrixx[1][2] + pcPoints[i].z * rotateMatrixx[2][2]));
            psPoints[i].startColor = new Color(1f, 0, 0);
            psPoints[i].startSize = 0.01f;
            pcPointsAddjusted[i] = psPoints[i].position;
        }
    }
}

