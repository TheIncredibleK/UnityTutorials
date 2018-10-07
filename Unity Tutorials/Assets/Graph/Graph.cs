﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    //Constants
    const float pi = Mathf.PI;


    public Transform pointPrefab;
    [Range(10,500)]
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
        SineFunction, MultiSineFunction,
        TimeChangedMultiSineFunction, Sine2DFunction,
        MultiSine2DFunction, Ripple,
        Cylinder, StairCase, Star,
        ClayPot, AnimatedStar, AlmostSphere,
        TrueSphere, PulsingSphere, HornTorus,
        RingTorus, PulsingTorus
    };


    void Awake()
    {
        points = new Transform[resolution * resolution];
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        for(int i = 0; i < points.Length; i++)
        {
            var point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            functionToUse += 1;
            if((int)functionToUse > functions.Length - 1)
            {
                functionToUse = 0;
            }
        }


        float time = Time.time;
        GraphFunction update;
        update = functions[(int)functionToUse];
        float step = 2f / resolution;
        for(int i = 0, z = 0; z < resolution; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for(int x = 0; x < resolution; x++, i++)
            {
                float u = (x + 0.5f) * step - 1f;
                points[i].localPosition = update(u, v, time) * dampener;
            }
        }
    }

    static Vector3 SineFunction(float x, float z, float t)
    {
        // return Mathf.Sin(pi * (x + t));

        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.z = z;
        return p;
    }

    static Vector3 MultiSineFunction(float x, float z, float t)
    {
        //        float y = Mathf.Sin(pi * (x + t));
        //        y += Mathf.Sin(2f * pi * (x + t)) / 2f;
        //        y *= 2f / 3f;
        //        return y;
        Vector3 p;
        p.x = x;
        p.z = z; 
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (x + t)) / 2f;
        p.y *= 2f / 3f;
        return p;
    }

    static Vector3 TimeChangedMultiSineFunction(float x, float z, float t)
    {
        //        float y = Mathf.Sin(pi * (x + t));
        //        y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
        //        y *= 2f / 3f;
        //        return y;

        Vector3 p;
        p.x = x;
        p.z = z;
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
        p.y *= 2f / 3f;
        return p;
    }

    static Vector3 Sine2DFunction(float x, float z, float t)
    {
       // var y = Mathf.Sin(pi * (x + t));
       // y += Mathf.Sin(pi * (z + t));
       // return y *= 0.5f;

        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(pi * (z + t));
        p.y *= 0.5f;
        p.z = z;
        return p;

    }

    static Vector3 MultiSine2DFunction(float x, float z, float t)
    {
        //        float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
        //        y += Mathf.Sin(pi * (x + t));
        //       y += Mathf.Sin(2f * pi * (z + (2f * t))) * 0.5f;
        //        y *= 1f / 5.5f;
        //        return y;

        Vector3 p;
        p.x = x;
        p.z = z;
        p.y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
        p.y += Mathf.Sin(2f * pi * (z + (2f * t))) * 0.5f;
        p.y *= 1f / 5.5f;
        return p;
    }

    static Vector3 Ripple(float x, float z, float t)
    {
        //        float d = Mathf.Sqrt(x * x + z * z);
        //        float y = Mathf.Sin(4f * pi * d - t);
        //        y /= 1f + 10f * d;
        //
        //        return y;

        Vector3 p;
        p.x = x;
        p.z = z;
        float d = Mathf.Sqrt(x * x + z * z);
        p.y = Mathf.Sin(4f * pi * d - t);
        p.y /= 1f + 10f * d;
        return p;

    }



    static Vector3 StairCase(float u, float v, float t)
    {
        Vector3 p;
        p.x = Mathf.Sin(pi * u + t);
        p.y = u;
        p.z = Mathf.Cos(pi * u + t);
        return p;
    }

    static Vector3 Cylinder(float u, float v, float t)
    {
        float r = 1f;
        Vector3 p;
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 Star(float u, float v, float t)
    {
        float r = 1f + Mathf.Sin(6 * pi * u)/5f;
        Vector3 p;
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 ClayPot(float u, float v, float t)
    {
        float r = 1f + Mathf.Sin(2 * pi * v) / 5f;
        Vector3 p;
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 AnimatedStar(float u, float v, float t)
    {
        float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t)) * 0.2f;
        Vector3 p;
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 AlmostSphere(float u, float v, float t)
    {
        float r = Mathf.Cos(pi * 0.5f * v);
        Vector3 p;
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 TrueSphere(float u, float v, float t)
    {
        float r = Mathf.Cos(pi * 0.5f * v);
        Vector3 p;
        p.x = r * Mathf.Sin(pi * u);
        p.y = Mathf.Sin(pi * 0.5f * v);
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 PulsingSphere(float u, float v, float t)
    {
        float r = 0.8f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
        r += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
        float s = r * Mathf.Cos(pi * v * 0.5f);
        Vector3 p;
        p.x = s * Mathf.Sin(pi * u);
        p.y = r * Mathf.Sin(pi * 0.5f * v);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 HornTorus(float u, float v, float t)
    {
        Vector3 p;
        float r1 = 1f;
        float s = Mathf.Cos(pi * v) + r1;
        p.x = s * Mathf.Sin(pi * u);
        p.y = Mathf.Sin(pi * v);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 RingTorus(float u, float v, float t)
    {
        Vector3 p;
        float r1 = 1f;
        float r2 = 0.5f;
        float s = r2 * Mathf.Cos(pi * v) + r1;
        p.x = s * Mathf.Sin(pi * u);
        p.y = r2 * Mathf.Sin(pi * v);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 PulsingTorus(float u, float v, float t)
    {
        Vector3 p;
        float r1 = 0.65f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
        float r2 = 0.2f + Mathf.Sin(pi * (4f * v + t)) * 0.05f;
        float s = r2 * Mathf.Cos(pi * v) + r1;
        p.x = s * Mathf.Sin(pi * u);
        p.y = r2 * Mathf.Sin(pi * v);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }

    public enum TypeOfWave
    {
        RegularSine = 0,
        MultiSine = 1,
        TimeChangedMultiSine = 2,
        Sine2D = 3,
        MultiSine2D = 4,
        Ripple = 5,
        Cylinder = 6,
        StairCase = 7,
        Star = 8,
        ClayPot = 9,
        AnimatedStar = 10,
        AlmostSphere = 11,
        TrueSphere = 12,
        PulsingSphere = 13,
        HornTorus = 14,
        RingTorus = 15,
        PulsingTorus = 16
    };
}

