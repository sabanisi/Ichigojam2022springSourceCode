using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAction6 : AbstractBossAction
{
    private GameObject cat1;
    private GameObject cat2;
    private Transform catTF1;
    private Transform catTF2;
    private Transform playerTransform;

    private float coolTime1;
    private float coolTime2;
    private bool isMoveBlcak;
    private Vector3 blackSpeed;
    private bool isStart;
    private float moveTime;

    private Coroutine whiteAttackCoroutine;
    private Coroutine blackAttackCoroutine;
    private bool isLaugh;

    public CatAction6(Boss parent, GameObject _cat1, GameObject _cat2) : base(parent)
    {
        cat1 = _cat1;
        cat2 = _cat2;
        catTF1 = _cat1.transform;
        catTF2 = _cat2.transform;
        playerTransform = GameManager.GetPlayer().transform;
        coolTime1 = 1.0f;
        isLaugh = false;
    }

    public override void Update()
    {
        if (!isLaugh)
        {
            isLaugh = true;
            SoundManager.PlaySE(SoundManager.SE_Type.Laugh);
        }
        if (coolTime1 <= 0)
        {
            whiteAttackCoroutine=CoroutineHandler.StartStaticCoroutine(WhiteAttack());
            coolTime1 = 1.0f;
        }
        else
        {
            coolTime1 -= Time.deltaTime;
        }

        if (isMoveBlcak)
        {
            moveTime += Time.deltaTime;
            catTF1.transform.position += blackSpeed * Time.deltaTime;
            if (moveTime>=0.2f&&Utils.IsOutWindow(catTF1.position))
            {
                isMoveBlcak = false;
                blackAttackCoroutine=CoroutineHandler.StartStaticCoroutine(BlackShot());
                moveTime = 0;
            }
        }
        if (!isStart)
        {
            isStart = true;
            CoroutineHandler.StartStaticCoroutine(StartOfBlack());
        }
    }

    private IEnumerator StartOfBlack()
    {
        yield return new WaitForSeconds(1.7f);
        SoundManager.PlaySE(SoundManager.SE_Type.Cat2);
        yield return new WaitForSeconds(0.3f);
        MoveBlackCat();
    }

    private IEnumerator BlackShot()
    {
        yield return new WaitForSeconds(0.2f);

        for(int j = 0; j <= 29; j++)
        {
            float randomRotate = Random.Range(0, 360);
            for (int i = 0; i <= 4; i++)
            {
                float rotate = (randomRotate + (i - 2) * 2.0f) * Mathf.Deg2Rad;
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Black2, TrajectoryEnum.Straight, catTF1.position);
                BulletManager.SetUpStraightBullet(bullet, 3 * Mathf.Cos(rotate), 3 * Mathf.Sin(rotate));
            }
            yield return new WaitForSeconds(0.05f);
            if (j % 4 == 0)
            {
                SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
            }
        }
        SoundManager.PlaySE(SoundManager.SE_Type.Cat2);
        yield return new WaitForSeconds(0.3f);
        MoveBlackCat();
    }

    private void MoveBlackCat()
    {
        float aimRotate = Utils.CalculateAimRotate(catTF1.position, playerTransform.position)*Mathf.Deg2Rad;
        float xSpeed = 12 * Mathf.Cos(aimRotate);
        float ySpeed = 12 * Mathf.Sin(aimRotate);
        if (xSpeed > 0)
        {
            catTF1.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            catTF1.rotation = Quaternion.Euler(0, 0, 0);
        }
        blackSpeed = new Vector3(xSpeed, ySpeed,0);
        isMoveBlcak = true;
    }

    private IEnumerator WhiteAttack()
    {
        float aimRotate = Utils.CalculateAimRotate(catTF2.position, playerTransform.position);
        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j <= 2; j++)
            {
                float rotate = (aimRotate + (j - 1) * 10.0f) * Mathf.Deg2Rad;
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.White, TrajectoryEnum.Straight, catTF2.position);
                BulletManager.SetUpStraightBullet(bullet, 8 * Mathf.Cos(rotate), 8 * Mathf.Sin(rotate));
            }
            SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
            yield return new WaitForSeconds(0.08f);
        }
    }

    public override void FinishAction()
    {
        if (whiteAttackCoroutine != null)
        {
            CoroutineHandler.StopStaticCoroutine(whiteAttackCoroutine);
        }
        if (blackAttackCoroutine!= null)
        {
            CoroutineHandler.StopStaticCoroutine(blackAttackCoroutine);
        }
        
        
    }
}