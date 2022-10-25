using System;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagTra : Trajectory
{
    float[][] speeds;
    float[] times;
    int nowState = 0;
    float timeCount = 0;

    public override void Update()
    {
        if (nowState < times.Length)
        {
            timeCount += Time.deltaTime;
            if (timeCount >= times[nowState])
            {
                nowState++;
                timeCount = 0;
                SetSpeed(speeds[nowState][0], speeds[nowState][1]);
            }
        }
        Vector3 realSpeed = speed * Time.deltaTime;
        bulletTransform.position += realSpeed;
    }

    public void SetSpeeds(List<float[]> _speeds)
    {
        speeds = new float[_speeds.Count][];
        for(int i=0;i<_speeds.Count;i++)
        {
            speeds[i] = _speeds[i];
        }
        SetSpeed(speeds[0][0], speeds[0][1]);
    }

    public void SetTimes(List<float> _times)
    {
        times = new float[_times.Count];
        for (int i = 0; i < _times.Count; i++)
        {
            times[i] = _times[i];
        }
    }
}
