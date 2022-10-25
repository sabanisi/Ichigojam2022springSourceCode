using System;
using System.Collections.Generic;
using UnityEngine;

public class PuppetterAction6 : AbstractBossAction
{
    private Transform bossTF;
    private List<Puppet> puppets;
    private List<Transform> TFs = new List<Transform>();
    private bool isSetUp;
    private float coolTime;
    private bool isMove;
    private float coolTime2;

    public PuppetterAction6(Boss parent, List<Puppet> _puppets) : base(parent)
    {
        bossTF = parent.gameObject.transform;
        puppets = _puppets;
    }

    public override void Update()
    {
        if (!isSetUp)
        {
            isSetUp = true;
            for (int i = 0; i <= 5; i++)
            {
                TFs.Add(puppets[i].gameObject.transform);
            }
        }
        if (coolTime <= 0)
        {
            if (isMove)
            {
                isMove = false;
                coolTime = 0.9f;
            }
            else
            {
                isMove = true;
                coolTime = 0.65f;
            }
            foreach(var puppet in puppets)
            {
                puppet.SetIsMoveOnly(isMove);
            }
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
        if (coolTime2 <= 0)
        {
            for(int i = 0; i < puppets.Count; i++)
            {
                float rotate = puppets[i].GetRotate()-120;
                for(int j = 0; j <= 1; j++)
                {
                    float newRotate = (rotate +j * 300) * Mathf.Deg2Rad;
                    BulletImageEnum imageEnum=BulletImageEnum.Red;
                    switch (i)
                    {
                        case 1:
                            imageEnum = BulletImageEnum.Blue;
                            break;
                        case 2:
                            imageEnum = BulletImageEnum.Yellow;
                            break;
                        case 3:
                            imageEnum = BulletImageEnum.Green;
                            break;
                        case 4:
                            imageEnum = BulletImageEnum.Orange;
                            break;
                        case 5:
                            imageEnum = BulletImageEnum.Purple;
                            break;
                    }
                    Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, imageEnum, TrajectoryEnum.Straight, TFs[i].position);
                    BulletManager.SetUpStraightBullet(bullet, 9 * Mathf.Cos(newRotate), 9 * Mathf.Sin(newRotate));
                }
            }
            coolTime2 = 0.07f;
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
        }
        else
        {
            coolTime2 -= Time.deltaTime;
        }
    }

    public override void FinishAction()
    {
        foreach (var puppet in puppets)
        {
            puppet.DestroyDeal();
        }
    }
}
