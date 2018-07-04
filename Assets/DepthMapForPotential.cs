using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Windows.Kinect;

public class DepthMapForPotential : MonoBehaviour
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

    }

    void Update()
    {
        updateTexture();
        gameObject.GetComponent<Renderer>().material.mainTexture = texture;
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

    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
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
}
