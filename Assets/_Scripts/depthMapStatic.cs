using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Windows.Kinect;

public class depthMapStatic : MonoBehaviour
{

    private KinectSensor _Sensor;
    private DepthFrameReader _Reader;
    Texture2D texture;
    byte[] depthbuffer;
    FrameDescription frameDesc;
    private ushort[] _Data;
    private float scale;
    public ushort[] GetData()
    {
        return _Data;
    }

    void Start()
    {
        scale = 1.0f;
        _Sensor = KinectSensor.GetDefault();
        frameDesc = _Sensor.DepthFrameSource.FrameDescription;
        depthbuffer = new byte[frameDesc.LengthInPixels * 3];
        texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGB24, false);
        gameObject.transform.localScale =
            new Vector3(scale * frameDesc.Width / frameDesc.Height, scale, 1.0f);

        if (_Sensor != null)
        {
            _Reader = _Sensor.DepthFrameSource.OpenReader();
            _Data = new ushort[_Sensor.DepthFrameSource.FrameDescription.LengthInPixels];
            _Sensor.Open();
        }
    }

    void Update()
    {
        updateTexture();
        gameObject.GetComponent<Renderer>().material.mainTexture = texture;
    }
    /*

        if (Input.GetKeyDown(KeyCode.A))
        {
            for(int i = 0; i<_Data.Length; i++)
            {
                logSave(_Data[i]);
            }
        }
        */

    void getTexture()
    {

        ushort[] rawdata = GetData();


        // convert to byte data (
        for (int i = 0; i < rawdata.Length; i++)
        {
            // 0-8000を0-256に変換する
            byte value = (byte)(rawdata[i] * 255 / 8000);

            int colorindex = i * 3;
            depthbuffer[colorindex + 0] = value;
            depthbuffer[colorindex + 1] = value;
            depthbuffer[colorindex + 2] = value;
        }

        // make texture from byte array
        texture.LoadRawTextureData(depthbuffer);
        texture.Apply();
    }

    void updateTexture()
    {
        // get new depth data from DepthSourceManager.
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            if (frame != null)
            {
                frame.CopyFrameDataToArray(_Data);
                frame.Dispose();
                frame = null;
            }
        }
        ushort[] rawdata = GetData();


        // convert to byte data (
        for (int i = 0; i < rawdata.Length; i++)
        {
            // 0-8000を0-256に変換する
            byte value = (byte)(rawdata[i] * 255 / 8000);

            int colorindex = i * 3;
            depthbuffer[colorindex + 0] = value;
            depthbuffer[colorindex + 1] = value;
            depthbuffer[colorindex + 2] = value;
        }

        // make texture from byte array
        texture.LoadRawTextureData(depthbuffer);
        texture.Apply();
    }

    public void logSave(float fy)
    {
        StreamWriter sw;
        FileInfo fi;
        fi = new FileInfo(Application.dataPath + "/OutLog/DeLo1.csv");
        sw = fi.AppendText();
        sw.WriteLine(fy.ToString());
        sw.Flush();
        sw.Close();
    }

    Vector3[] readPcd()
    {
        int numPoint = 0;
        Vector3[] points;
        string[] buffer;
        int counter = 0;
        string line;
        StreamReader file = new StreamReader(@"C:/Program Files/pcdData/realPoint.pcd");
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

        points = new Vector3[numPoint];
        line = file.ReadLine();

        while ((line = file.ReadLine()) != null)
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
