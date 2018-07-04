using UnityEngine;

public class sigmoidLineCheck : MonoBehaviour
{
    [Range(10, 100)]
    public int resolution = 10;
    public Transform pointPrefab;

    private void Awake()
    {
        Vector3 position = new Vector3();
        Vector3 scale = Vector3.one*2 / resolution;

        float step = 2f / resolution;
        for (int i = 0; i < resolution ; i++)
        {
            Transform point = Instantiate(pointPrefab);
            position.x = (i + 0.5f)*step - 1f;
            position.y = 10/(1+Mathf.Exp(position.x*(2f/0.4f)-1f));
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform,false);
        }
    }
}
