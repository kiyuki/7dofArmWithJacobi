using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    GameObject tempObj;
    Vector3 closestPoint;
    GameObject arm1Spa;
    private void Start()
    {
        closestPoint = new Vector3();
        arm1Spa = GameObject.Find("arm1Space");
        
    }


    void Update()
    {
      
         //StartCoroutine(mostClosestPointPlane(arm1Spa.transform.position));
         DrawLine(arm1Spa.transform.position, closestPoint, Color.black, Color.red);
     }
    IEnumerator startCo()
    {
        yield return StartCoroutine(mostClosestPointPlane(arm1Spa.transform.position));
        StartCoroutine(B());
        
    }
    private IEnumerator B()
    {
        LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
        renderer.startWidth = 0.01f;
        renderer.endWidth = 0.01f;
        renderer.SetPosition(0, arm1Spa.transform.position);
        renderer.SetPosition(1, closestPoint);
        renderer.startColor = Color.black;
        renderer.endColor = Color.red;

        yield return null;
    }
    IEnumerator mostClosestPointPlane(Vector3 refPoint)
    {

        ParticleSystem obj_ps;
        ParticleSystem.Particle[] obj_p;
        int particleNum;
        float refDistance = float.MaxValue;
        string filename;
        for (int i = 0; i < 1/*mainPercep.GetComponent<Filer>().clusterNum[0]*/; i++)
        {
            filename = "pla" + i.ToString();
            tempObj = GameObject.Find(filename);
            obj_ps = tempObj.GetComponent<ParticleSystem>();
            obj_p = new ParticleSystem.Particle[obj_ps.main.maxParticles];
            particleNum = obj_ps.GetParticles(obj_p);
            
            for(int j = 0; j<particleNum; j++)
            { 

                    if ((obj_p[j].position - refPoint).magnitude < refDistance)
                    {
                        closestPoint = obj_p[j].position;
                        refDistance = (obj_p[j].position - refPoint).magnitude;
                        Debug.Log("j num: " + j);
                        

                    }

                yield return null;
            }
             
                
            
        }
        Debug.Log(closestPoint);
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
    /*
    IEnumerator Fade()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            Color c = renderer.material.color;
            c.a = f;
            renderer.material.color = c;
            yield return null;
        }
    }*/

    IEnumerator fortest()
    {
        for(int i = 0; i< 50000; i++)
        {
            Debug.Log(i);
        }
        yield return null;
    }


    void OnMouseDown()
    {
        StartCoroutine(ChangeColorCoroutine());
    }



    void ChangeColorCoroutine(Renderer renderer, Color color)
    {
        renderer.material.color = color;
        renderer.UpdateGIMaterials();
    }

    bool isRunning = false;
    Coroutine coroutine;

    [SerializeField]
    Renderer leftRenderer = null, rightRenderer = null;
    private readonly int j;

    IEnumerator ChangeColorCoroutine()
    {
        if (isRunning) { yield break; }
        isRunning = true;

        var renderer = GetComponent<Renderer>();
        ChangeColor(renderer, Color.blue);

        var leftCoroutine = StartCoroutine(ChangeColorCoroutineWithRuntimetime(leftRenderer));
        var rightCoroutine = StartCoroutine(ChangeColorCoroutineWithRuntimetime(rightRenderer));

        yield return leftCoroutine;
        yield return rightCoroutine;

        yield return new WaitForSeconds(0.2f);
        ChangeColor(renderer, Color.white);
        isRunning = false;
    }

    IEnumerator ChangeColorCoroutineWithRuntimetime(Renderer renderer)
    {
        ChangeColor(renderer, Color.red);

        float waitSec = Random.Range(0.2f, 1f);
        yield return new WaitForSeconds(waitSec);

        ChangeColor(renderer, Color.green);
    }

    void ChangeColor(Renderer renderer, Color color)
    {
        renderer.material.color = color;
        renderer.UpdateGIMaterials();
    }
}


