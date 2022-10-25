using System.Collections.Generic;
using UnityEngine;

public class SpiderAction6:AbstractBossAction
{
    private Vector3 _mouthPos;
    private float coolTime;
    private int count;

    public SpiderAction6(Boss parent,Vector3 mouthPos) : base(parent)
    {
        _mouthPos = mouthPos;
    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            float aimRotate = Random.Range(0, 360);
            count++;
            for(int i = 0; i <= 7; i++)
            {
                for(int j = 0; j <= 9; j++)
                {
                    float rotate= j * (((float)45) / 10);
                    float localRotate = (aimRotate + i * 45 + rotate)*Mathf.Deg2Rad;
                    float speedCoeeicient = (Mathf.Sin(67.5f * Mathf.Deg2Rad) / Mathf.Sin((112.5f - rotate) * Mathf.Deg2Rad))/3;
                    BulletImageEnum imageEnum = BulletImageEnum.Yellow2;
                    switch (count % 3)
                    {
                        case 0:
                            imageEnum = BulletImageEnum.Orange2;
                            break;
                        case 1:
                            imageEnum = BulletImageEnum.White2;
                            break;
                        case 2:
                            break;
                    }
                    Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, imageEnum, TrajectoryEnum.Zigzag, _mouthPos);

                    float[] speed1 = new float[] { speedCoeeicient * 13 * Mathf.Cos(localRotate), speedCoeeicient * 13 * Mathf.Sin(localRotate) };
                    float[] speed2=new float[] {-speedCoeeicient*21*Mathf.Cos(localRotate),-speedCoeeicient*21*Mathf.Sin(localRotate)};
                    float[] speed3 = new float[] { speedCoeeicient * 14 * Mathf.Cos(localRotate), speedCoeeicient * 14 * Mathf.Sin(localRotate) };
                    float[] speed4 = new float[] { -speedCoeeicient * 14 * Mathf.Cos(localRotate), -speedCoeeicient * 14 * Mathf.Sin(localRotate) };
                    float[] stop = new float[] { 0, 0 };

                    List<float[]> speeds = new List<float[]> {speed1,stop,speed2,stop,speed3,stop,speed4};
                    List<float> times = new List<float> {2.1f,1.5f,2.1f,1.5f,2.1f,1.5f };
                    BulletManager.SetUpZigzagBullet(bullet, speeds, times);
                    bullet.SetDontDestroyCount(12.0f);
                }
            }

            coolTime = 3.6f;
            SoundManager.PlaySE(SoundManager.SE_Type.Shot5);
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
    }
}
