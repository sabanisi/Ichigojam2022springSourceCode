using System.Collections.Generic;
using UnityEngine;

public class SnakeAction4:AbstractBossAction
{
    private Vector3 attackPos;
    private float coolTime;
    private int count;
    private bool isOdd;

    public SnakeAction4(Boss parent,Vector3 _attackPos):base(parent)
    {
        attackPos = _attackPos;
    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            int bulletCount=19;
            BulletImageEnum imageEnum = BulletImageEnum.Red;
            int j = 0;
            if (isOdd)
            {
                j = 1;
                bulletCount = 20;
                imageEnum = BulletImageEnum.Blue;
            }
            for(int i = 0; i <= bulletCount; i++)
            {
                float rotate = (80+ i * (((float)200) / bulletCount)+count*1.3f*Mathf.Pow(-1,j))*Mathf.Deg2Rad;
                Bullet bullet=BulletManager.CreateBullet(BulletBelongEnum.Enemy, imageEnum, TrajectoryEnum.Zigzag, attackPos);
                float[] speed1 = new float[] { (10-j*2) * Mathf.Cos(rotate), (10-j*2) * Mathf.Sin(rotate) };
                float[] stop = new float[] { 0, 0 };
                float[] speed2 = new float[] { (10 + j * 2) * Mathf.Cos(rotate), (10 + j * 2) * Mathf.Sin(rotate) };
                BulletManager.SetUpZigzagBullet(bullet, new List<float[]>() { speed1, stop, speed2 }, new List<float>() { 0.3f, 0.7f });
            }
            count++;
            if (count < 7)
            {
                coolTime = 0.052f;
            }
            else
            {
                coolTime = 0.4f;
                count = 0;
                if (isOdd)
                {
                    isOdd = false;
                }
                else
                {
                    isOdd = true;
                }
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
    }
}
