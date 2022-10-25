using UnityEngine;

public class SnakeAction2:AbstractBossAction
{
    private Vector3 attackPos1;
    private Vector3 attackPos2;
    private float coolTime;
    private int count;
    private int count2;
    private float[] aimRotate=new float[5];
    private float coolTime2;
    private float aimRotate2;
    private Transform playerTF;

    public SnakeAction2(Boss parent, Vector3 _attackPos1,Vector3 _attackPos2) : base(parent)
    {
        attackPos1 = _attackPos1;
        attackPos2 = _attackPos2;
        playerTF = GameManager.GetPlayer().transform;
    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            for (int i = 0; i < aimRotate.Length; i++)
            {
                if (count == 0)
                {
                    aimRotate[i] = (Utils.CalculateAimRotate(attackPos2, playerTF.position) + Random.Range(0, 90) - 45) * Mathf.Deg2Rad;
                }
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Yellow2, TrajectoryEnum.Straight, attackPos2);
                BulletManager.SetUpStraightBullet(bullet, 12 * Mathf.Cos(aimRotate[i]), 12 * Mathf.Sin(aimRotate[i]));
            }
            count++;
            if (count >= 8)
            {
                count = 0;
            }
            coolTime = 0.07f;
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
        if (coolTime2 <= 0)
        {
            if (count2 == 0)
            {
                aimRotate2 = Utils.CalculateAimRotate(attackPos1, playerTF.position);
            }
            for (int i = 0; i <= 2; i++)
            {
                float rotate = (aimRotate2 + (i - 1) * 5) * Mathf.Deg2Rad;
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Snake1, TrajectoryEnum.Straight, attackPos1);
                BulletManager.SetUpStraightBullet(bullet, 6 * Mathf.Cos(rotate), 6 * Mathf.Sin(rotate));
            }
            count2++;
            if (count2 == 3)
            {
                count2 = 0;
                coolTime2 = 0.8f;
            }
            else
            {
                coolTime2 = 0.1f;
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
        }
        else
        {
            coolTime2 -= Time.deltaTime;
        }
    }
}
