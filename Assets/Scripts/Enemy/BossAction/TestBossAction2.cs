using System;
using UnityEngine;

public class TestBossAction2: AbstractBossAction
{
    private float coolTime;
    private Transform playerTransform;
    public TestBossAction2(Boss parent) : base(parent)
    {
        playerTransform = GameManager.GetPlayer().transform;
    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            float aimRotate = Utils.CalculateAimRotate(parent.GetTransform().position, playerTransform.position);
            for (int i = 0; i <= 2; i++)
            {
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Blue, TrajectoryEnum.Straight, parent.GetTransform().position);
                BulletManager.SetUpStraightBullet(bullet, 5 * Mathf.Cos((aimRotate-15+i*15) * Mathf.Deg2Rad), 5 * Mathf.Sin((aimRotate - 15 + i * 15) * Mathf.Deg2Rad));
            }
            coolTime = 1.0f;
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
    }
}
