using UnityEngine;

public class SnakeAction1:AbstractBossAction
{
    private Vector3 attackPos;
    private float coolTime;
    private int count;

    public SnakeAction1(Boss parent,Vector3 _attackPos):base(parent)
    {
        attackPos = _attackPos;
    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            float randomRotate = Random.Range(0, 360);
            for(int i = 0; i <= 14; i++)
            {
                float rotate = randomRotate + 360 / 15 * i;
                for(int j = 0; j <= 1; j++)
                {
                    BulletImageEnum imageEnum = BulletImageEnum.Red;
                    if (j == 1)
                    {
                        imageEnum = BulletImageEnum.Blue;
                    }
                    Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, imageEnum, TrajectoryEnum.MisuthiTra, attackPos);
                    BulletManager.SetUpMisuthiBullet(bullet, 4f, 20 * Mathf.Pow(-1, j), rotate);
                    bullet.SetDontDestroyCount(4.0f);
                }
            }
            SoundManager.PlaySE(SoundManager.SE_Type.BigShot4);
            count++;
            if (count == 5)
            {
                coolTime = 1.5f;
                count=0;
            }
            else
            {
                coolTime = 0.7f;
            }
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
    }
}
