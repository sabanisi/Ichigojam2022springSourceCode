using System;
using System.Collections.Generic;
using UnityEngine;

public class CatAction3:AbstractBossAction
{
    private float coolTime;
    private float coolTime2;
    private int count;
    private GameObject cat1;
    private GameObject cat2;
    private Transform catTF1;
    private Transform catTF2;

    public CatAction3(Boss parent, GameObject _cat1, GameObject _cat2) : base(parent)
    {
        cat1 = _cat1;
        cat2 = _cat2;
        catTF1 = _cat1.transform;
        catTF2 = _cat2.transform;
    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            if (coolTime2 <= 0)
            {
                for (int i = 0; i <= 15; i++)
                {
                    float[] speed4 = new float[] { 5 * Mathf.Cos((i + count * 0.2f) * 22.5f * Mathf.Deg2Rad), 5 * Mathf.Sin((i + count * 0.2f) * 22.5f * Mathf.Deg2Rad) };
                    float[] speed5 = new float[] { 0, 0 };
                    float[] speed6 = new float[] { 5 * Mathf.Cos((i - 90 - count * 0.2f) * 22.5f * Mathf.Deg2Rad), 5 * Mathf.Sin((i - 90 - count * 0.2f) * 22.5f * Mathf.Deg2Rad) };

                    Bullet bullet2 = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.White, TrajectoryEnum.Zigzag, catTF2.position);
                    BulletManager.SetUpZigzagBullet(bullet2, new List<float[]> { speed4, speed5, speed6 }, new List<float> { 0.6f - count * 0.02f, 0.3f });

                    float[] speed1 = new float[] { 5 * Mathf.Cos((i + count * 0.2f) * 22.5f * Mathf.Deg2Rad), 5 * Mathf.Sin((i + count * 0.2f) * 22.5f * Mathf.Deg2Rad) };
                    float[] speed2 = new float[] { 0, 0 };
                    float[] speed3 = new float[] { 7 * Mathf.Cos((i + 90 + count * 0.2f) * 22.5f * Mathf.Deg2Rad), 7 * Mathf.Sin((i + 90 + count * 0.2f) * 22.5f * Mathf.Deg2Rad) };

                    Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Black, TrajectoryEnum.Zigzag, catTF1.position);
                    BulletManager.SetUpZigzagBullet(bullet, new List<float[]> { speed1, speed2, speed3 }, new List<float> { 0.6f - count * 0.02f, 0.3f });
                }
                SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
                count++;
                if (count == 10)
                {
                    count = 0;
                    coolTime = 3.0f;
                }
                coolTime2 = 0.2f;
            }
            else
            {
                coolTime2 -= Time.deltaTime;
            }

        }
        else
        {
            coolTime -= Time.deltaTime;
        }
    }
}

