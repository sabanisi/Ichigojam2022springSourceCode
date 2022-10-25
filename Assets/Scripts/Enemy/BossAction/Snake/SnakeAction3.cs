using UnityEngine;

public class SnakeAction3:AbstractBossAction
{
    private Vector3 attackPos;
    private float coolTime;
    private bool isAim;
    private float[] aimRotate=new float[7];
    private int count;
    private Transform playerTF;

    public SnakeAction3(Boss parent,Vector3 _attackPos):base(parent)
    {
        attackPos = _attackPos;
        playerTF = GameManager.GetPlayer().transform;
    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            if (isAim)
            {
                if (count == 0)
                {
                    float playerAim = Utils.CalculateAimRotate(attackPos, playerTF.position);
                    for (int i = 0; i <= 2; i++)
                    {
                        aimRotate[i] = playerAim + (i - 1) * 5f;
                        Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Snake2, TrajectoryEnum.Straight, attackPos);
                        BulletManager.SetUpStraightBullet(bullet, 10 * Mathf.Cos(aimRotate[i] * Mathf.Deg2Rad), 10 * Mathf.Sin(aimRotate[i] * Mathf.Deg2Rad));
                    }
                    SoundManager.PlaySE(SoundManager.SE_Type.BigShot4);
                }
                else
                {
                    for (int i = 0; i <= 2; i++)
                    {
                        float rotate = (aimRotate[i] + Random.value*6 - 3f) * Mathf.Deg2Rad;
                        Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Yellow2, TrajectoryEnum.Straight, attackPos);
                        BulletManager.SetUpStraightBullet(bullet, 10 * Mathf.Cos(rotate), 10 * Mathf.Sin(rotate));
                    }

                }
                count++;
                if (count <= 24)
                {
                    coolTime = 0.03f;
                }
                else
                {
                    coolTime = 0.3f;
                    isAim = false;
                    count = 0;
                }
            }
            else
            {
                if (count == 0)
                {
                    float targetRotate = Random.Range(160, 200);
                    float thetaRotate = Random.Range(15, 20);
                    for (int i = 0; i <= 6; i++)
                    {
                        aimRotate[i] = targetRotate + (i - 3) * thetaRotate;
                        Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Snake2, TrajectoryEnum.Straight, attackPos);
                        BulletManager.SetUpStraightBullet(bullet, 8 * Mathf.Cos(aimRotate[i] * Mathf.Deg2Rad), 8 * Mathf.Sin(aimRotate[i] * Mathf.Deg2Rad));
                    }
                    SoundManager.PlaySE(SoundManager.SE_Type.BigShot4);
                }
                else
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        float rotate = (aimRotate[i] + Random.value*12 - 6f) * Mathf.Deg2Rad;
                        Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Yellow2, TrajectoryEnum.Straight, attackPos);
                        BulletManager.SetUpStraightBullet(bullet, 8 * Mathf.Cos(rotate), 8 * Mathf.Sin(rotate));
                    }

                }
                count++;
                if (count <= 24)
                {
                    coolTime = 0.03f;
                }
                else
                {
                    coolTime = 0.7f;
                    isAim = true;
                    count = 0;
                }
            }
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
    }
}
