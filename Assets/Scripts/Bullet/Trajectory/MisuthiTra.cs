using System;
using UnityEngine;

public class MisuthiTra:Trajectory
{
    private float rSpeed;
    private float r;
    private float omegaSpeed;
    private Vector3 centerPos;
    private bool isStart;
    public void SetSpeeds(float _rSpeed,float _omegaSpeed,float _rotate)
    {
        rSpeed = _rSpeed;
        omegaSpeed = _omegaSpeed;
        SetRotate(_rotate);
        r = 0;
    }

    public override void Update()
    {
        if (!isStart)
        {
            isStart = true;
            centerPos = bulletTransform.position;
        }
        SetRotate(rotate + omegaSpeed * Time.deltaTime);
        r += rSpeed * Time.deltaTime;
        bulletTransform.position = centerPos + new Vector3(r * Mathf.Cos(rotate * Mathf.Deg2Rad), r * Mathf.Sin(rotate * Mathf.Deg2Rad), 0);
    }
}
