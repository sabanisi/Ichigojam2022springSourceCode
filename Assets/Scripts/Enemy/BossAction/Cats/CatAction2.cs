using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CatAction2 : AbstractBossAction
{
    private float coolTime;
    private float coolTime2;
    private int count;
    private float random1;
    private float random2;

    private GameObject cat1;
    private GameObject cat2;
    private Transform catTF1;
    private Transform catTF2;

    public CatAction2(Boss parent, GameObject _cat1, GameObject _cat2) : base(parent)
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
                if (count == 0)
                {
                    random1 = Random.Range(0, 360);
                    random2 = Random.Range(0, 360);
                }
                for (int i = 0; i <= 19; i++)
                {

                    float[] speed1 = new float[] { 5 * Mathf.Cos((random1+(i + count * 0.5f) * 18f )* Mathf.Deg2Rad), 5 * Mathf.Sin((random1+(i + count * 0.5f) * 18f) * Mathf.Deg2Rad) };
                    float[] speed2 = new float[] { 0, 0 };
                    float[] speed3 = new float[] { 10* Mathf.Cos((random1+(i - 3) * 18f) * Mathf.Deg2Rad), 10 * Mathf.Sin((random1+(i - 3) * 18f) * Mathf.Deg2Rad) };

                    Bullet bullet2 = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.White, TrajectoryEnum.Zigzag, catTF2.position);
                    BulletManager.SetUpZigzagBullet(bullet2, new List<float[]> { speed1, speed2, speed3 }, new List<float> { 0.5f, 0.1f+count*0.1f });

                    float[] speed4 = new float[] { 5 * Mathf.Cos((random2+(i - count * 0.5f) * 18f) * Mathf.Deg2Rad), 5 * Mathf.Sin((random2+(i - count * 0.5f) * 18f) * Mathf.Deg2Rad) };
                    float[] speed5 = new float[] { 0, 0 };
                    float[] speed6 = new float[] { 8* Mathf.Cos((random2+(i + 3) * 18f) * Mathf.Deg2Rad), 8 * Mathf.Sin((random2+(i + 3) * 18f) * Mathf.Deg2Rad) };

                    Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Black, TrajectoryEnum.Zigzag, catTF1.position);
                    BulletManager.SetUpZigzagBullet(bullet, new List<float[]> { speed4, speed5, speed6 }, new List<float> { 0.5f, 0.1f + count * 0.1f });

                }
                SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
                count++;
                if (count == 10)
                {
                    count = 0;
                    coolTime = 2.0f;
                }
                coolTime2 = 0.1f;
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

