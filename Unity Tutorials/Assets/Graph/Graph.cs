using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    public Transform pointPrefab;
    [Range(10,100)]
    public int resolution;
    Transform[] points;
    [Range(1, 100)]
    public float dampener;


    void Awake()
    {
        points = new Transform[resolution];
        int i = 0;
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position;
        position.z = 0f;
        while (i < resolution)
        {
            var point = Instantiate(pointPrefab);
            position.x = (i + 0.5f) * step - 1f;
            position.y = position.x * position.x * position.x;
            point.localPosition = position;
            point.localScale = scale;
            points[i] = point;
            i++;
        }
    }

    void Update()
    {
        for(int i = 0; i < points.Length; i++)
        {
            var point = points[i];
            Vector3 position = point.localPosition;
            position.y = Mathf.Sin(Mathf.PI * position.x + Time.time)/dampener;
            position.z = Mathf.Sin(Mathf.PI * position.x + Time.time) / dampener;
            point.localPosition = position;
        }
    }
}
