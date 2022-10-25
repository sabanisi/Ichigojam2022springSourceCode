using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    protected int maxActionNumber;//使う弾幕の数
    protected int nowActionNumber;//現在の弾幕の番号

    protected float stopActionCount;//行動を一時停止するためのフラグ

    protected float invisibleCount;//一時的に無敵にするためのフラグ

    private float gameStartAnimation;

    protected List<AbstractBossAction> actions = new List<AbstractBossAction>();
    [SerializeField] protected List<int> hps = new List<int>();
    private int nowHp;

    protected Transform _transform;
    public Transform GetTransform()
    {
        return _transform;
    }

    protected virtual void Start()
    {
        _transform = transform;
        nowActionNumber = 1;
        nowHp = hps[0];
    }

    public virtual void Update()
    {
        if (GameManager.instance.IsPause()) return;
        if (!GameManager.instance.IsGameStart()) return;
        if (GameManager.instance.IsGameOver()) return;
       /* if (!isGameStart)
        {
            isGameStart = true;
            GameStartDeal();
        }
        if (!isGameStartFinish) return;*/

        if (stopActionCount <= 0)
        {
            //BossActionのUpdateを更新
            actions[nowActionNumber - 1].Update();
        }
        else
        {
            stopActionCount -= Time.deltaTime;
            if (stopActionCount < 0)
            {
                stopActionCount = 0;
            }
        }

        if (invisibleCount > 0)
        {
            invisibleCount -= Time.deltaTime;
            if (invisibleCount < 0)
            {
                invisibleCount = 0;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.IsGameStart()) return;
        if (stopActionCount > 0) return;
        if (invisibleCount > 0) return;
        if (collision.gameObject.tag == "PlayerBullet")
        {
            collision.transform.parent.gameObject.SetActive(false);
            nowHp--;
            if (nowHp == 0)
            {
                BulletManager.instance.Invoke("AllEnemyBulletDelete",0.05f);
                actions[nowActionNumber - 1].FinishAction();
                if (nowActionNumber == maxActionNumber)
                {
                    //ボス撃破時処理
                    GameManager.instance.GameClear();
                    BulletManager.instance.Invoke("AllEnemyBulletDelete", 0.8f);
                    SoundManager.PlaySE(SoundManager.SE_Type.Die);
                    SoundManager.StopBGM();
                }
                else
                {
                    //次の攻撃に移る
                    nowActionNumber++;
                    nowHp = hps[nowActionNumber-1];
                    SoundManager.PlaySE(SoundManager.SE_Type.Change4);
                }
                stopActionCount = 2.0f;
            }
            else
            {
                HitAnimationEnum animationEnum = HitAnimationEnum.Yellow;
                if (collision.gameObject.transform.parent.GetComponent<Bullet>().GetBulletImageEnum().Equals(BulletImageEnum.PlayerShot1))
                {
                    animationEnum = HitAnimationEnum.Blue;
                }
                float randomX = Random.value * 1.0f-0.5f;
                float randomY = Random.value * 1.0f-0.5f;
                BulletManager.CreateHitAnimation(animationEnum, collision.gameObject.transform.position+new Vector3(1+randomX,randomY,0));
            }
        }
    }

    public abstract void GameStartDeal();

    public abstract void GameClearDeal();

}
