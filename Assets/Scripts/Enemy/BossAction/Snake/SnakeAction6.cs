using System.Collections.Generic;
using UnityEngine;

public class SnakeAction6:AbstractBossAction
{
    private Vector3 attackPos1, attackPos2;
    private float coolTime,coolTime2;
    private int count,count2;

    public SnakeAction6(Boss parent, Vector3 _attackPos1, Vector3 _attackPos2):base(parent)
    {
        attackPos1 = _attackPos1;
        attackPos2 = _attackPos2;
        coolTime2 = 3.0f;
    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            for(int i = 0; i <= 19; i++)
            {
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Red, TrajectoryEnum.Zigzag, attackPos1);
                List<float[]> speeds = new List<float[]>();
                List<float> times = new List<float>();
                float rotate = 80 + i * 200 / 20+(count-5)*1;
                for(int j=0;j<=9; j++)
                {
                    float newRotate = (rotate + 10 * Mathf.Pow(-1, j % 2))*Mathf.Deg2Rad;
                    speeds.Add(new float[] { 7 * Mathf.Cos(newRotate), 7 * Mathf.Sin(newRotate) });
                    speeds.Add(new float[] { 0, 0 });
                    times.Add(0.6f);
                    times.Add(0.3f);
                }
                speeds.Add(new float[] { 4 * Mathf.Cos(rotate * Mathf.Deg2Rad), 4 * Mathf.Sin(rotate * Mathf.Deg2Rad) });
                BulletManager.SetUpZigzagBullet(bullet, speeds, times);
            }
            count++;
            if (count <= 10)
            {
                coolTime = 0.3f;
            }
            else
            {
                coolTime = 3.0f;
                count = 0;
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
        }
        else
        {
            coolTime -= Time.deltaTime;
        }

        if (coolTime2 <= 0)
        {
            for (int i = 0; i <= 19; i++)
            {
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Blue, TrajectoryEnum.MisuthiTra, attackPos1);
                BulletManager.SetUpMisuthiBullet(bullet, 3, 10, 80 + i * 200 / 20 + (count - 5) * 1);
            }
            count2++;
            if (count2 <= 10)
            {
                coolTime2 = 0.3f;
            }
            else
            {
                coolTime2 = 3.0f;
                count2 = 0;
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
        }
        else
        {
            coolTime2 -= Time.deltaTime;
        }
    }
}
