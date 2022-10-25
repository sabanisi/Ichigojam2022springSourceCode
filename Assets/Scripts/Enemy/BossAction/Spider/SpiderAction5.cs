using UnityEngine;
public class SpiderAction5:AbstractBossAction
{
    private Vector3 _mouthPos;
    private float coolTime1;
    private float coolTime2;
    private float aimRotate;
    private float aimRotate2;
    private int count;
    private int count2;
    public SpiderAction5(Boss parent,Vector3 mouthPos) : base(parent)
    {
        _mouthPos = mouthPos;
        coolTime2 = 1.5f;
    }

    public override void Update()
    {
        if (coolTime1 <= 0)
        {
            if (count == 0)
            {
                aimRotate = aimRotate2;
            }
            for(int i=0;i<=9; i++)
            {
                float rotate = (aimRotate + i * 36) * Mathf.Deg2Rad;
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Yellow2, TrajectoryEnum.Straight, _mouthPos);
                BulletManager.SetUpStraightBullet(bullet, 5 * Mathf.Cos(rotate), 5 * Mathf.Sin(rotate));
            }
            aimRotate += 6.0f;
            count++;
            if (count == 20)
            {
                coolTime1 = 1.5f;
                count = 0;
            }
            else
            {
                coolTime1 = 0.1f;
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
        }
        else
        {
            coolTime1 -= Time.deltaTime;
        }

        if (coolTime2 <= 0)
        {
            if (count2 == 0)
            {
                aimRotate2 = aimRotate;
            }
            for (int i = 0; i <= 9; i++)
            {
                float rotate = (aimRotate2 + i * 36) * Mathf.Deg2Rad;
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Orange2, TrajectoryEnum.Straight, _mouthPos);
                BulletManager.SetUpStraightBullet(bullet, 5 * Mathf.Cos(rotate), 5 * Mathf.Sin(rotate));
            }
            aimRotate2 -= 6.0f;
            count2++;
            if (count2 == 20)
            {
                coolTime2 = 1.5f;
                count2 = 0;
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
