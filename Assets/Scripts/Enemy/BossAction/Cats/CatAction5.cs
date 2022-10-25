using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAction5: AbstractBossAction
{
    private GameObject cat1;
    private GameObject cat2;
    private Transform catTF1;
    private Transform catTF2;

    private float coolTime;

    private Coroutine attackCoroutine;

    public CatAction5(Boss parent, GameObject _cat1, GameObject _cat2) : base(parent)
    {
        cat1 = _cat1;
        cat2 = _cat2;
        catTF1 = _cat1.transform;
        catTF2 = _cat2.transform;
    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            attackCoroutine=CoroutineHandler.StartStaticCoroutine(Attack());
            coolTime = 1.5f;
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
    }

    IEnumerator Attack()
    {
        float random =Random.Range(0, 360);

        for (int i = 0; i <= 2; i++)
        {
            //l=0:黒 l=1:白
            for (int l = 0; l <= 1; l++)
            {
                for (int j = 0; j <= 15; j++)
                {
                    float rotate = random + j * (float)(360 / 16);
                    List<float[]> speeds = new List<float[]>();
                    List<float> times = new List<float>();
                    //速度作成
                    for (int k = 0; k <= 9; k++)
                    {
                        float newRotate = rotate + Mathf.Pow(-1, l) * (k % 2) * 30.0f;
                        float[] move = new float[] { (3+2*(l%2)) * Mathf.Cos(newRotate * Mathf.Deg2Rad), (3+2*(l%2)) * Mathf.Sin(newRotate * Mathf.Deg2Rad) };
                        speeds.Add(move);
                        if (k != 9)
                        {
                            float[] stop = new float[] { 0, 0 };
                            speeds.Add(stop);

                            float moveTimes = 0.7f;
                            times.Add(moveTimes);

                            float stopTimes = 0.4f;
                            times.Add(stopTimes);
                        }
                    }
                    Bullet bullet;
                    if (l == 0)
                    {
                        bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Black, TrajectoryEnum.Zigzag, catTF1.position);
                    }
                    else
                    {
                        bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.White, TrajectoryEnum.Zigzag, catTF2.position);
                    }
                    BulletManager.SetUpZigzagBullet(bullet, speeds, times);
                }
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void FinishAction()
    {
        if (attackCoroutine != null)
        {
            CoroutineHandler.StopStaticCoroutine(attackCoroutine);
        }
    }

}
