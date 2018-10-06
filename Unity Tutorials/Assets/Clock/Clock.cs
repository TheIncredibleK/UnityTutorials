using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {

    public Transform hoursTransform;
    public Transform minutesTransform;
    public Transform secondsTransform;


    const float
            degreesPerHour = 30f,
            degreesPerMinute = 6f,
            degreesPerSecond = 6f;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        TimeSpan time = DateTime.Now.TimeOfDay;
        hoursTransform.localRotation =
                    Quaternion.Euler(0f, (float)time.TotalHours * degreesPerHour, 0f);
        minutesTransform.localRotation =
            Quaternion.Euler(0f, (float)time.TotalMinutes * degreesPerMinute, 0f);
        secondsTransform.localRotation =
            Quaternion.Euler(0f, (float)time.TotalSeconds * degreesPerSecond, 0f);
    }
}
