using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAction1 : AbstractBossAction
{
    private float coolTime;
    private float coolTime2;
    private float coolTime3;
    private int count1;
    private int count2;
    private GameObject cat1;
    private GameObject cat2;
    private Transform catTF1;
    private Transform catTF2;
    private float random1;
    private float random2;
    private bool isBlackFast;

    public CatAction1(Boss parent, GameObject _cat1, GameObject _cat2) : base(parent)
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
            if (coolTime3 <= 0)
            {
                if (count2 >=7&&count2<=16)
                {
                    for (int i = 0; i <= 19; i++)
                    {
                        if (!isBlackFast)
                        {
                            float[] speed1 = new float[] { 5 * Mathf.Cos((random1 + (i + count2 * 0.5f) * 18f) * Mathf.Deg2Rad), 5 * Mathf.Sin((random1 + (i + count2 * 0.5f) * 18f) * Mathf.Deg2Rad) };
                            float[] speed2 = new float[] { 0, 0 };
                            float[] speed3 = new float[] { 10 * Mathf.Cos((random1 + (i - 3) * 18f) * Mathf.Deg2Rad), 10 * Mathf.Sin((random1 + (i - 3) * 18f) * Mathf.Deg2Rad) };

                            Bullet bullet2 = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.White, TrajectoryEnum.Zigzag, catTF2.position);
                            BulletManager.SetUpZigzagBullet(bullet2, new List<float[]> { speed1, speed2, speed3 }, new List<float> { 0.5f, 0.1f + count2 * 0.1f });
                        }
                        else
                        {
                            float[] speed4 = new float[] { 5 * Mathf.Cos((random2 + (i - count2 * 0.5f) * 18f) * Mathf.Deg2Rad), 5 * Mathf.Sin((random2 + (i - count2 * 0.5f) * 18f) * Mathf.Deg2Rad) };
                            float[] speed5 = new float[] { 0, 0 };
                            float[] speed6 = new float[] { 8 * Mathf.Cos((random2 + (i + 3) * 18f) * Mathf.Deg2Rad), 8 * Mathf.Sin((random2 + (i + 3) * 18f) * Mathf.Deg2Rad) };

                            Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Black, TrajectoryEnum.Zigzag, catTF1.position);
                            BulletManager.SetUpZigzagBullet(bullet, new List<float[]> { speed4, speed5, speed6 }, new List<float> { 0.5f, 0.1f + count2 * 0.1f });
                        }
                    }
                    
                }
                count2++;
                coolTime3 = 0.1f;
            }
            else
            {
                coolTime3 -= Time.deltaTime;
            }

            if (coolTime2 <= 0)
            {
                for (int i = 0; i <= 15; i++)
                {
                    if (isBlackFast)
                    {
                        float[] speed4 = new float[] { 5 * Mathf.Cos((i + count1 * 0.2f) * 22.5f * Mathf.Deg2Rad), 5 * Mathf.Sin((i + count1 * 0.2f) * 22.5f * Mathf.Deg2Rad) };
                        float[] speed5 = new float[] { 0, 0 };
                        float[] speed6 = new float[] { 5 * Mathf.Cos((i - 90 - count1 * 0.2f) * 22.5f * Mathf.Deg2Rad), 5 * Mathf.Sin((i - 90 - count1 * 0.2f) * 22.5f * Mathf.Deg2Rad) };

                        Bullet bullet2 = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.White, TrajectoryEnum.Zigzag, catTF2.position);
                        BulletManager.SetUpZigzagBullet(bullet2, new List<float[]> { speed4, speed5, speed6 }, new List<float> { 0.6f - count1 * 0.02f, 0.3f });
                    }
                    else
                    {
                        float[] speed1 = new float[] { 5 * Mathf.Cos((i + count1 * 0.2f) * 22.5f * Mathf.Deg2Rad), 5 * Mathf.Sin((i + count1 * 0.2f) * 22.5f * Mathf.Deg2Rad) };
                        float[] speed2 = new float[] { 0, 0 };
                        float[] speed3 = new float[] { 7 * Mathf.Cos((i + 90 + count1 * 0.2f) * 22.5f * Mathf.Deg2Rad), 7 * Mathf.Sin((i + 90 + count1 * 0.2f) * 22.5f * Mathf.Deg2Rad) };

                        Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Black, TrajectoryEnum.Zigzag, catTF1.position);
                        BulletManager.SetUpZigzagBullet(bullet, new List<float[]> { speed1, speed2, speed3 }, new List<float> { 0.6f - count1 * 0.02f, 0.3f });
                    }
                }
                count1++;
                SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
                if (count1 == 10)
                {
                    count1 = 0;
                    coolTime = 3.0f;
                    count2 = 0;
                    if (isBlackFast)
                    {
                        isBlackFast = false;
                    }
                    else
                    {
                        isBlackFast = true;
                    }
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

