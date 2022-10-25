using System.Collections.Generic;
using UnityEngine;

public class PuppetterAction3:AbstractBossAction
{
    private Transform bossTF;
    private List<Puppet> pupptes;
    private List<Transform> TFs = new List<Transform>();

    private bool isStart;
    private float coolTime;
    private int count;

    public PuppetterAction3(Boss parent, List<Puppet> _puppets) : base(parent)
    {
        bossTF = parent.gameObject.transform;
        pupptes = _puppets;
    }

    public override void Update()
    {
        if (!isStart)
        {
            isStart = true ;
            ((Puppetter)parent).PuppetCreate2();
            coolTime = 0.9f;
            foreach(var obj in pupptes)
            {
                TFs.Add(obj.gameObject.transform);
            }
        }
        else
        {
            if (coolTime <= 0)
            {
                count++;
                Vector3 pos = new Vector3(0, 0, 0);
                BulletImageEnum imageEnum = BulletImageEnum.Black;
                switch (count % 5)
                {
                    case 0:
                        pos = bossTF.position;
                        imageEnum = BulletImageEnum.Black;
                        break;
                    case 1:
                        pos = TFs[0].position;
                        imageEnum = BulletImageEnum.White;
                        break;
                    case 2:
                        pos = TFs[1].position;
                        imageEnum = BulletImageEnum.White;
                        break;
                    case 3:
                        pos = TFs[2].position;
                        imageEnum = BulletImageEnum.White;
                        break;
                    case 4:
                        pos = TFs[3].position;
                        imageEnum = BulletImageEnum.White;
                        break;
                }
                float straightTime = 0.6f + Random.value;
                float randomRotate = Random.Range(90, 270) * Mathf.Deg2Rad;
                for (int i = 0; i <= 23; i++)
                {
                    float rotate = randomRotate + i * (360 / 24) * Mathf.Deg2Rad;
                    Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, imageEnum, TrajectoryEnum.Zigzag, pos);
                    float[] speed1 = new float[] { 10 * Mathf.Cos(rotate), 10 * Mathf.Sin(rotate) };
                    float[] speed2 = new float[] { 6 * Mathf.Cos(randomRotate), 6 * Mathf.Sin(randomRotate) };
                    float[] speed3 = new float[] { -6 * Mathf.Cos(rotate), -6 * Mathf.Sin(rotate) };
                    float[] stop = new float[] { 0, 0 };
                    List<float[]> speeds = new List<float[]>() { speed1, stop, speed2, stop, speed3 };
                    List<float> times = new List<float>() { 0.4f, 0.03f, straightTime, 0.05f };
                    bullet.SetDontDestroyCount(3.0f);
                    BulletManager.SetUpZigzagBullet(bullet, speeds, times);
                }
                coolTime = 0.4f;
                SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
            }
            else
            {
                coolTime -= Time.deltaTime;
            }
        }
    }
}
