using System.Collections.Generic;
using UnityEngine;

public class PuppetterAction4:AbstractBossAction
{
    private Transform bossTF;
    private List<Puppet> puppets;
    private List<Transform> TFs=new List<Transform>();
    private float coolTime;
    private float coolTime2;
    private Transform playerTF;
    private int count;
    private int count2;
    private bool isSetUp;
    private float aimRotate;
    private float aimRotate2;

    public PuppetterAction4(Boss parent, List<Puppet> _puppets) : base(parent)
    {
        bossTF = parent.gameObject.transform;
        puppets = _puppets;
       
        playerTF = GameManager.GetPlayer().transform;
    }

    public override void Update()
    {
        if(!isSetUp)
        {
            isSetUp = true;
            for (int i = 0; i <= 3; i++)
            {
                TFs.Add(puppets[i].gameObject.transform);
            }
        }

        if (coolTime <= 0)
        {
            Vector3 pos=TFs[0].position;
            switch (count / 9)
            {
                case 0:
                    break;
                case 1:
                    pos = TFs[1].position;
                    break;
                case 2:
                    pos = TFs[2].position;
                    break;
                case 3:
                    pos=TFs[3].position;
                    break;
            }
            if (count % 9 == 0)
            {
                aimRotate = Utils.CalculateAimRotate(pos, playerTF.position) * Mathf.Deg2Rad;
                SoundManager.PlaySE(SoundManager.SE_Type.BigShot4);
            }
            for(int i = 0; i <= 2; i++)
            {
                float rotate = aimRotate + (i - 1) * 60 * Mathf.Deg2Rad;
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.White2, TrajectoryEnum.Straight, pos);
                BulletManager.SetUpStraightBullet(bullet, 10 * Mathf.Cos(rotate), 10 * Mathf.Sin(rotate));
            }
            if (count <= 34)
            {
                coolTime = 0.05f;
                count++;
            }
            else
            {
                count =0;
                coolTime = 0.2f;
            }
        }
        else
        {
            coolTime -= Time.deltaTime;
        }

        if (coolTime2 <= 0)
        {
            if (count2 == 0)
            {
                aimRotate2 = Utils.CalculateAimRotate(bossTF.position, playerTF.position)*Mathf.Deg2Rad;
                SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
            }
            for(int i = 0; i <= 5; i++)
            {
                float rotate = aimRotate2 + 10 * Mathf.Deg2Rad;
                switch (i)
                {
                    case 1:
                        rotate = aimRotate2 - 10 * Mathf.Deg2Rad;
                        break;
                    case 2:
                        rotate = aimRotate2 + 70 * Mathf.Deg2Rad;
                        break;
                    case 3:
                        rotate = aimRotate2 + 50 * Mathf.Deg2Rad;
                        break;
                    case 4:
                        rotate = aimRotate2 - 50 * Mathf.Deg2Rad;
                        break;
                    case 5:
                        rotate = aimRotate2 - 70 * Mathf.Deg2Rad;
                        break;
                    default:
                        break;
                     
                }
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Black, TrajectoryEnum.Straight, bossTF.position);
                BulletManager.SetUpStraightBullet(bullet, 5 * Mathf.Cos(rotate), 5 * Mathf.Sin(rotate));
            }
            count2++;
            if (count2 <= 10)
            {
                coolTime2 = 0.2f;
            }
            else
            {
                coolTime2 = 0.15f;
                count2 = 0;
            }
        }
        else
        {
            coolTime2 -= Time.deltaTime;
        }
    }
}
