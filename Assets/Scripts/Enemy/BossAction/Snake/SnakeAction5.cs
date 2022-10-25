using System;
using UnityEngine;

public class SnakeAction5:AbstractBossAction
{
    private Vector3 attackPos1, attackPos2;
    private Transform playerTF;
    private float coolTime1;
    private float coolTime2;
    private int count1,count2;

    public SnakeAction5(Boss parent,Vector3 _attackPos1,Vector3 _attackPos2):base(parent)
    {
        attackPos1 = _attackPos1;
        attackPos2 = _attackPos2;
        playerTF = GameManager.GetPlayer().transform;
        coolTime2 = 1.0f;
    }

    public override void Update()
    {
        if (coolTime1 <= 0)
        {
            int bulletNum = 15;
            if (count1 % 2 == 0)
            {
                bulletNum = 16;
            }
            for(int i = 0; i < bulletNum; i++)
            {
                float rotate = 90 + i * (((float)180) / bulletNum)*Mathf.Deg2Rad;
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Yellow, TrajectoryEnum.Straight, attackPos2);
                BulletManager.SetUpStraightBullet(bullet, 5 * Mathf.Cos(rotate), 5 * Mathf.Sin(rotate));
            }
            coolTime1 = 0.3f;
            count1++;
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);

        }
        else
        {
            coolTime1 -= Time.deltaTime;
        }

        if (coolTime2 <= 0)
        {
            Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Snake1, TrajectoryEnum.Straight, attackPos1);
            float rotate = Utils.CalculateAimRotate(attackPos1, playerTF.position)*Mathf.Deg2Rad;
            BulletManager.SetUpStraightBullet(bullet, 10 * Mathf.Cos(rotate), 10 * Mathf.Sin(rotate));
            count2++;
            if (count2 <= 7)
            {
                coolTime2 = 0.1f;
            }
            else
            {
                count2 = 0;
                coolTime2 = 0.8f;
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
        }
        else
        {
            coolTime2 -= Time.deltaTime;
        }
    }
}
