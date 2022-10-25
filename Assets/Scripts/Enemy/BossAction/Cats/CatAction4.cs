using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAction4 : AbstractBossAction
{
    private GameObject cat1;
    private GameObject cat2;
    private Transform catTF1;
    private Transform catTF2;
    private bool isReady;
    private bool isReadyStart;
    private Transform playerTransform;

    private float coolTime1;
    private float coolTime2;

    private Coroutine whiteAttackCoroutine;

    public CatAction4(Boss parent, GameObject _cat1, GameObject _cat2) : base(parent)
    {
        cat1 = _cat1;
        cat2 = _cat2;
        catTF1 = _cat1.transform;
        catTF2 = _cat2.transform;
        playerTransform = GameManager.GetPlayer().transform;
    }

    public override void Update()
    {
        if (!isReadyStart)
        {
            isReadyStart = true;
            MoveCats();
        }
        if (!isReady) return;


        if (coolTime1 <= 0)
        {
            //黒猫の攻撃
            float randomRotate = Random.Range(0,360);
            for(int i = 0; i <= 2; i++)
            {
                float rotate = (randomRotate + (i - 1) * 0.8f)*Mathf.Deg2Rad;
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Black2, TrajectoryEnum.Straight, catTF1.position);
                BulletManager.SetUpStraightBullet(bullet, 4 * Mathf.Cos(rotate), 4 * Mathf.Sin(rotate));
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
            coolTime1 = 0.1f;
        }
        else
        {
            coolTime1 -= Time.deltaTime;
        }

        if (coolTime2 <= 0)
        {
            whiteAttackCoroutine=CoroutineHandler.StartStaticCoroutine(WhiteAttack());
            coolTime2 = 1.0f;
        }
        else
        {
            coolTime2 -= Time.deltaTime;
        }
    }

    private IEnumerator WhiteAttack()
    {
        float aimRotate = Utils.CalculateAimRotate(catTF2.position, playerTransform.position);
        for (int i = 0; i <= 2; i++)
        {
            for(int j = 0; j <= 2; j++)
            {
                float rotate = (aimRotate + (j - 1) * 10.0f) * Mathf.Deg2Rad ;
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.White, TrajectoryEnum.Straight, catTF2.position);
                BulletManager.SetUpStraightBullet(bullet, 7 * Mathf.Cos(rotate), 7 * Mathf.Sin(rotate));
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void MoveCats()
    {
        SoundManager.PlaySE(SoundManager.SE_Type.Laugh);
        Hashtable black = new Hashtable();
        black.Add("position", new Vector3(-7,0,0));
        black.Add("time", 0.8f);
        black.Add("easeType", "easeInOutSine");
        iTween.MoveTo(cat1, black);

        Hashtable white = new Hashtable();
        white.Add("position", new Vector3(7,0,0));
        white.Add("time", 0.8f);
        white.Add("easeType", "easeInOutSine");
        iTween.MoveTo(cat2, white);

        SoundManager.PlaySE(SoundManager.SE_Type.Cat2);
        CoroutineHandler.StartStaticCoroutine(MoveFinishDeal());
    }

    IEnumerator MoveFinishDeal()
    {
        yield return new WaitForSeconds(0.8f);
        catTF1.rotation = Quaternion.Euler(0, 180, 0);

        yield return new WaitForSeconds(0.6f);
        isReady = true;
    }

    public override void FinishAction()
    {
        if (whiteAttackCoroutine != null)
        {
            CoroutineHandler.StopStaticCoroutine(whiteAttackCoroutine);
        }
    }

}
