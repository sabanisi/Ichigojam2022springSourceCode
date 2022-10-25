using System;
using UnityEngine;

public class TestBossAction : AbstractBossAction
{
    private float coolTime;
    public TestBossAction(Boss parent) : base(parent)
    {

    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            for (int i = 0; i <= 15; i++)
            {
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Bullet1, TrajectoryEnum.Straight, parent.GetTransform().position);
                BulletManager.SetUpStraightBullet(bullet, 5 * Mathf.Cos(i * 22.5f * Mathf.Deg2Rad), 5*Mathf.Sin(i * 22.5f * Mathf.Deg2Rad));
            }
            coolTime = 1.0f;
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
       
    }
}
