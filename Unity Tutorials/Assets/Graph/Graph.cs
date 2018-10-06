using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    //Constants
    const float pi = Mathf.PI;


    public Transform pointPrefab;
    [Range(10,100)]
    public int resolution;
    Transform[] points;
    [Range(1, 100)]
    public float dampener;
    [Range(1,100)]
    public static float timeMultiplier;
    public TypeOfWave functionToUse;

    //Array of delegates
    static GraphFunction[] functions =
    {
        SineFunction, MultiSineFunction, TimeChangedMultiSineFunction, Sine2DFunction, MultiSine2DFunction
    };


    void Awake()
    {
        timeMultiplier = 5.0f;
        points = new Transform[resolution * resolution];
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position =  new Vector3(0, 0, 0);
        for(int i = 0, z=0; z < resolution; z++) {
            position.z = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                var point = Instantiate(pointPrefab);
                position.x = (x + 0.5f) * step - 1f;
                point.localPosition = position;
                point.localScale = scale;
                point.SetParent(transform, false);
                points[i] = point;
            }

        }
    }

    void Update()
    {
        float time = Time.time;
        GraphFunction update;
        update = functions[(int)functionToUse];
        for(int i = 0; i < points.Length; i++)
        {
            var point = points[i];
            Vector3 position = point.localPosition;
            position.y = update(position.x, position.z, time);
            position.y *= 1 / dampener; 
            point.localPosition = position;
        }
    }

    static float SineFunction(float x, float z, float t)
    {
        return Mathf.Sin(pi * (x + t));
    }

    static float MultiSineFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (x + t)) / 2f;
        y *= 2f / 3f;
        return y;
    }

    static float TimeChangedMultiSineFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
        y *= 2f / 3f;
        return y;
    }

    static float Sine2DFunction(float x, float z, float t)
    {
        var y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(pi * (z + t));
        return y *= 0.5f;
    }

    static float MultiSine2DFunction(float x, float z, float t)
    {
        float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
        y += Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (z + (2f * t))) * 0.5f;
        y *= 1f / 5.5f;
        return y;
    }


    public enum TypeOfWave
    {
        RegularSine = 0,
        MultiSine = 1,
        TimeChangedMultiSine = 2,
        Sine2D = 3,
        MultiSine2D = 4
    };
}

