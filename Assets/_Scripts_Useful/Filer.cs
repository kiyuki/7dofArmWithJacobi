using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System.Threading;

//#define FILENAME "C:/Program Files/pcdData/realPoint.pcd"
public class planePath : MonoBehaviour
{
    public const string planePath0 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudPlane0.pcd";
    public const string planePath1 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudPlane1.pcd";
    public const string planePath2 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudPlane2.pcd";
    public const string planePath3 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudPlane3.pcd";
    public const string planePath4 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudPlane4.pcd";
    public const string planePath5 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudPlane5.pcd";
    public const string planePath6 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudPlane6.pcd";
    public const string planePath7 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudPlane7.pcd";
    public const string planePath8 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudPlane8.pcd";
    public const string planePath9 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudPlane9.pcd";
}

public class objPath : MonoBehaviour
{
    public const string objPath0 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster0.pcd";
    public const string objPath1 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster1.pcd";
    public const string objPath2 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster2.pcd";
    public const string objPath3 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster3.pcd";
    public const string objPath4 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster4.pcd";
    public const string objPath5 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster5.pcd";
    public const string objPath6 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster6.pcd";
    public const string objPath7 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster7.pcd";
    public const string objPath8 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster8.pcd";
    public const string objPath9 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster9.pcd";
    public const string objPath10 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster10.pcd";
    public const string objPath11 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster11.pcd";
    public const string objPath12 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster12.pcd";
    public const string objPath13 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster13.pcd";
    public const string objPath14 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster14.pcd";
    public const string objPath15 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster15.pcd";
    public const string objPath16 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster16.pcd";
    public const string objPath17 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster17.pcd";
    public const string objPath18 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster18.pcd";
    public const string objPath19 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster19.pcd";
    public const string objPath20 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster20.pcd";
    public const string objPath21 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster21.pcd";
    public const string objPath22 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster22.pcd";
    public const string objPath23 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster23.pcd";
    public const string objPath24 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster24.pcd";
    public const string objPath25 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster25.pcd";
    public const string objPath26 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster26.pcd";
    public const string objPath27 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster27.pcd";
    public const string objPath28 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster28.pcd";
    public const string objPath29 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster29.pcd";
    public const string objPath30 = @"C:\Users\tamia\Documents\7dofArmWithJacobi\pointCloudCluster30.pcd";
}

/*/////////////////////////////////////////////////////////
//                                                              
//Execute perception.exe and clustering.exe
//This scripts contain function which show point cloud.
//InstObj is olso constain
/////////////////////////////////////////////////////////*/

public class Filer : MonoBehaviour {

    public int[] clusterNum; //first element is palner, sencond element is object
    [Range(1,3)]
    public float robotPosiZ;
    public GameObject objectCluster;
    public GameObject planerCluster;
    private GameObject armSpace;
    private Vector3[] pcPointAddjusted;

    string fileNameBuf;

    //These lines and func moved to prefab. not used.
    private ParticleSystem.Particle[] psPoints; 
    private Vector3[] pointCloudPoints;
    
    private void Start()
    {
        armSpace = GameObject.Find("ArmSpace");
        armSpace.transform.position = new Vector3(-0.86f, -1.5293f, 2.04f);
        instObject();
    }

    public void exePerceptionAndClustering()
    {
        Process p = new Process();
        
        p.StartInfo.FileName = @"C:\clustering\perceptSurrounding.exe";
        p.Start();
        p.WaitForExit();

        p.StartInfo.FileName = @"C:\clustering\clusteringObject.exe";
        p.Start();
        p.WaitForExit();
    }

    public void exeClustering()
    {
        Process p = new Process();
        p.StartInfo.FileName = @"C:\clustering\clusteringObject.exe";
        p.Start();
        p.WaitForExit();
    }

    public void instObject()
    {
        clusterNum = readText2line();
        for(int i =0; i< clusterNum[0]; i++)
        {
            GameObject pla = (GameObject) Instantiate(planerCluster, Vector3.zero, transform.rotation );
            planerPrefab plaSc = pla.GetComponent<planerPrefab>();
            fileNameBuf = "pla" + i.ToString();
            pla.name = fileNameBuf;
            plaSc.objectNum = i;
            plaSc.getFilePath(i);
            plaSc.readPcd(plaSc.filePath);
            plaSc.getRadianGrandToPc();
            plaSc.showPointCloudRotateCresGraund(plaSc.pcPoints, -plaSc.Angle);
        }
        for (int i = 0; i < clusterNum[1]; i++)
        {
            if (i == 1) continue;
            GameObject obj = (GameObject)Instantiate(objectCluster, Vector3.zero, transform.rotation);
            objectPrefab objSc = obj.GetComponent<objectPrefab>();

            fileNameBuf = "obj" + i.ToString();
            obj.name = fileNameBuf;

            objSc.objectNum = i;
            objSc.getFilePath(i);
            objSc.readPcd(objSc.filePath);
            objSc.getRadianGrandToPc();
            objSc.showPointCloudRotateCresGraund(objSc.pcPoints, -objSc.Angle);
            objSc.rotatePointCloud(objSc.pcPoints, -objSc.Angle);
        }
        //armSpace.transform.position = calcuRobotPosi();
    }

    private Vector3 calcuRobotPosi()
    {
        // x coordinate is zero, z coodination is 1m, calculate y coordination.

        Vector3 coeff = new Vector3();
        Vector3 robotPosi;
        Vector3 robotPosiRotated;

        Vector3[] rotateMatrixx = new Vector3[3];
        float radi = -getRadianGrandToPc();
        string[] buffer;
        string line;
        StreamReader file = new StreamReader(@"C:\Users\tamia\Documents\7dofArmWithJacobi\PlanerCoefficients.txt");

        for (int i = 0; i < 4; i++)
        {
            line = file.ReadLine();
        }
        line = file.ReadLine();
        buffer = line.Split();
        UnityEngine.Debug.Log(buffer[0] + "\t" + buffer[1] + "\t" + buffer[2] + "\t" + buffer[3] + "\t" + buffer[4] + "\t" + buffer[5]);
        coeff.x = float.Parse(buffer[5]); //b value

        line = file.ReadLine();
        buffer = line.Split();
        UnityEngine.Debug.Log(buffer[0] + "\t" + buffer[1] + "\t" + buffer[2] + "\t" + buffer[3] + "\t" + buffer[4] + "\t" + buffer[5]);
        coeff.y = float.Parse(buffer[5]); //c value

        line = file.ReadLine();
        buffer = line.Split();
        UnityEngine.Debug.Log(buffer[0] + "\t" + buffer[1] + "\t" + buffer[2] + "\t" + buffer[3] + "\t" + buffer[4] + "\t" + buffer[5]);
        coeff.z = float.Parse(buffer[5]);// d value

        file.Close();

        //float radi = theta * (2 * Mathf.PI / 360);
        rotateMatrixx[0] = new Vector3(1, 0, 0);
        rotateMatrixx[1] = new Vector3(0, Mathf.Cos(radi), -Mathf.Sin(radi));
        rotateMatrixx[2] = new Vector3(0, Mathf.Sin(radi), Mathf.Cos(radi));
        robotPosi = new Vector3(0,
            (-1 / coeff.x) * (coeff.y * robotPosiZ + coeff.z)
            , robotPosiZ);
        robotPosiRotated = new Vector3(robotPosi.x,
                robotPosi.x * rotateMatrixx[0][1] + robotPosi.y * rotateMatrixx[1][1] + robotPosi.z * rotateMatrixx[2][1],
                robotPosi.x * rotateMatrixx[0][2] + robotPosi.y * rotateMatrixx[1][2] +robotPosi.z * rotateMatrixx[2][2]); ;
      
        return robotPosiRotated;
    }

    void fileCopy()
    {
        System.IO.File.Copy(@"C:\Users\tamia\Documents\Visual Studio 2015\Projects\perceptSurrounding\perceptSurrounding\pointCloudSurrounding.pcd", @"C:\Users\tamia\Documents\7dofArmWithJacobi\Assets",true);
    }

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

    void showPointCloud(Vector3[] pcPoints)
    {
        psPoints = new ParticleSystem.Particle[pcPoints.Length];

        for (int i = 0; i < pcPoints.Length; i++)
        {
            psPoints[i].position = pcPoints[i];
            psPoints[i].startColor = new Color(1f, 0, 0);
            psPoints[i].startSize = 0.01f;
        }
        GetComponent<ParticleSystem>().SetParticles(psPoints, psPoints.Length);
    }
    
    void showPointCloudRotateCresGraund(Vector3[] pcPoints, float radi)
    {
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
            psPoints[i].position = transform.InverseTransformPoint (new Vector3(pcPoints[i].x,
                pcPoints[i].x * rotateMatrixx[0][1] + pcPoints[i].y * rotateMatrixx[1][1] + pcPoints[i].z * rotateMatrixx[2][1],
                pcPoints[i].x * rotateMatrixx[0][2] + pcPoints[i].y * rotateMatrixx[1][2] + pcPoints[i].z * rotateMatrixx[2][2]));
            psPoints[i].startColor = new Color(1f, 0, 0);
            psPoints[i].startSize = 0.01f;
            pcPointAddjusted[i] = psPoints[i].position;
        }
        GetComponent<ParticleSystem>().SetParticles(psPoints, psPoints.Length);
    }

    public float getRadianGrandToPc()
    {
        float angle;
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
        return angle;
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
}