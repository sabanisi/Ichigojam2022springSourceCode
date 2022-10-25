using UnityEngine;

public class SpiderAction4:AbstractBossAction
{
    private bool isStart;
    private bool isDown;

    private Transform _bossTF;
    private Vector3 _mouthPos;

    private float coolTime;
    private int count;
    private float aimRotate;
    private float coolTime2;

    public SpiderAction4(Boss parent, Transform bossTF,Vector3 mouthPos) : base(parent)
    {
        _bossTF = bossTF;
        _mouthPos = mouthPos;
        aimRotate = Random.Range(0, 360);
    }

    public override void Update()
    {
        if (!isStart)
        {
            if (!isDown)
            {
                _bossTF.position += new Vector3(0, 16, 0)*Time.deltaTime;
                if (_bossTF.position.y >= 7.5f)
                {
                    isDown = true;
                    _bossTF.position = new Vector3(0, 7.5f, 0);
                }
            }
            else
            {
                _bossTF.position -= new Vector3(0.5f, 16, 0)*Time.deltaTime;
                if (_bossTF.position.y <= -1)
                {
                    isStart = true;
                    _bossTF.position = new Vector3(0.5f, -1, 0);
                }
            }
        }
        else
        {
            if (coolTime <= 0)
            {
                coolTime = 0.1f;
                for(int i = 0; i <= 7; i++)
                {
                    float rotate = (aimRotate + i * (360 / 4))*Mathf.Deg2Rad;
                    Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Bullet1, TrajectoryEnum.Straight, _mouthPos);
                    BulletManager.SetUpStraightBullet(bullet, 9* Mathf.Cos(rotate), 9 * Mathf.Sin(rotate));
                }
                aimRotate += 2.5f;
                SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
            }
            else
            {
                coolTime -= Time.deltaTime;
            }
            if (coolTime2 <= 0)
            {
                float randomY = Random.value * 10 - 5;
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Spider, TrajectoryEnum.Straight, new Vector3(9, randomY, 0));
                BulletManager.SetUpStraightBullet(bullet, -7, 0);
                coolTime2 = 0.1f;
            }
            else
            {
                coolTime2 -= Time.deltaTime;
            }
        }
    }
}
