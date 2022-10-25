using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiderUnit1:SubUnit
{
    private bool canShot=false;
    private float coolTime;
    private float _aimRotate=0;
    private Vector3 pos;
    private int count;

    protected override void Action()
    {
        if (canShot)
        {
            if (coolTime <= 0)
            {
                for(int i = 0; i <= 6; i++)
                {
                    float rotate = _aimRotate + (i - 2) * (360/7)*Mathf.Deg2Rad;
                    Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Orange2, TrajectoryEnum.Straight, pos);
                    BulletManager.SetUpStraightBullet(bullet, 10 * Mathf.Cos(rotate), 10 * Mathf.Sin(rotate));
                }
                _aimRotate += 0.01f;
                coolTime = 0.05f;
                count++;
                if (count >= 80)
                {
                    count = 0;
                    coolTime = 1.5f;
                    _aimRotate = Random.Range(0, 360);
                }
            }
            else
            {
                coolTime -= Time.deltaTime;
            }
        } 
    }

    public void MoveFinish()
    {
        canShot = true;
        pos = transform.position;
        _aimRotate = Random.Range(0, 360);
    }
}
