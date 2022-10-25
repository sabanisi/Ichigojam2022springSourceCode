using System.Collections.Generic;
using UnityEngine;

public class PuppetterAction1:AbstractBossAction
{
    private Transform bossTF;
    private List<Puppet> pupptes;

    private float coolTime;
    private int count;

    public PuppetterAction1(Boss parent,List<Puppet> _puppets):base(parent)
    {
        bossTF = parent.gameObject.transform;
        pupptes = _puppets;
    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            count++;
            Vector3 pos=new Vector3(0,0,0);
            BulletImageEnum imageEnum=BulletImageEnum.Black;
            switch (count % 3)
            {
                case 0:
                    pos = bossTF.position;
                    imageEnum = BulletImageEnum.Black;
                    break;
                case 1:
                    pos = pupptes[0].gameObject.transform.position;
                    imageEnum = BulletImageEnum.White;
                    break;
                case 2:
                    pos = pupptes[1].gameObject.transform.position;
                    imageEnum = BulletImageEnum.White;
                    break;
            }
            float straightTime = 0.2f + Random.value;
            float randomRotate = Random.Range(90, 270)*Mathf.Deg2Rad;
            for(int i = 0; i <= 39; i++)
            {
                float rotate = randomRotate + i * (360 / 40)*Mathf.Deg2Rad;
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, imageEnum, TrajectoryEnum.Zigzag, pos);
                float[] speed1 = new float[] { 10 * Mathf.Cos(rotate), 10 * Mathf.Sin(rotate ) };
                float[] speed2 = new float[] { 6 * Mathf.Cos(randomRotate), 6 * Mathf.Sin(randomRotate) };
                float[] speed3 = new float[] { -6 * Mathf.Cos(rotate), -6 * Mathf.Sin(rotate) };
                float[] stop = new float[] { 0, 0 };
                List<float[]> speeds = new List<float[]>() {speed1,stop,speed2,stop,speed3 };
                List<float> times = new List<float>() { 0.2f, 0.03f, straightTime, 0.05f };
                bullet.SetDontDestroyCount(2.2f);
                BulletManager.SetUpZigzagBullet(bullet, speeds, times);
            }
            coolTime = 0.6f;
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
        }
        else
        {
            coolTime -=Time.deltaTime;
        }
    }
}
