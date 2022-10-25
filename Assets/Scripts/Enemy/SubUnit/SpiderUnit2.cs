using System;
using System.Collections.Generic;
using UnityEngine;

public class SpiderUnit2:SubUnit
{
    private float speed;
    private float time;
    private bool isMoveChange;
    private float direction;
    public void SetUp(float _direction,float _speed)
    {
        direction = _direction;
        speed = _speed;
        _transform.rotation = Quaternion.Euler(0, 0, direction);
        speedVector= new Vector3(speed * Mathf.Cos(direction * Mathf.Deg2Rad), speed * Mathf.Sin(direction * Mathf.Deg2Rad), 0);
    }
    [SerializeField] private Transform _transform;
    private Vector3 speedVector;
    private float coolTime;

    private readonly float MaxTime = 1.5f+3f * Mathf.Sin(22.5f * Mathf.Deg2Rad);

    protected override void Action()
    {
        time += Time.deltaTime;
        if (time >= 1.5f)
        {
            if (!isMoveChange)
            {
                isMoveChange = true;
                SetUp(direction + 112.5f,speed);
            }
        }

        transform.position += speedVector*Time.deltaTime;

        if (time > MaxTime)
        {
            DestroyDeal();
        }
        else
        {
            if (coolTime <= 0)
            {
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Yellow2, TrajectoryEnum.Zigzag, _transform.position);
                bullet.GetTrajectory().SetRotate(direction);
                List<float[]> speeds = new List<float[]>();
                speeds.Add(new float[] { 0, 0 });
                float randomRotate = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
                speeds.Add(new float[] { 2 * Mathf.Cos(randomRotate), 2 * Mathf.Sin(randomRotate) });
                List<float> times = new List<float>();
                times.Add(MaxTime + 2.5f - time);
                BulletManager.SetUpZigzagBullet(bullet,speeds, times);
               
                coolTime = 0.12f;
            }
            else
            {
                coolTime -= Time.deltaTime;
            }
        }
    }
}
