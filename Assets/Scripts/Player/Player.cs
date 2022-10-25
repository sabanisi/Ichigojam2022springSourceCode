using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float SlowSpeed;
    [SerializeField] private List<Sprite> sprites;

    [SerializeField] private Transform _transform;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private float _coolTimeCount;
    [SerializeField] private float CoolTime;
    private float dieCoolTimeCount;
    [SerializeField] private float DieCoolTime;

    [SerializeField] private int MaxLife;
    [SerializeField] private int MaxBomb;
    private int lifeCount;
    [SerializeField] private Text LifeCountText;
    [SerializeField] private Text BomCountText;
    private int usedBombCount;//使ったボムの数
    private int diedCount;//死んだ数
    public int GetUsedBombCount() { return usedBombCount; }
    public int GetDiedCount() { return diedCount; }

    private int bombCount;
    private float bombCoolTime;

    public void Initialize()
    {
        lifeCount = MaxLife;
        bombCount = MaxBomb;
        _transform.position = new Vector3(-7, 0, 0);
        LifeCountText.text = lifeCount + "";
        BomCountText.text = bombCount + "";
        dieCoolTimeCount = 0;
        _spriteRenderer.color = new Color(1, 1, 1, 1);
        usedBombCount = 0;
        diedCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsPause())  return;
        if (GameManager.instance.IsGameOver()) return;

        //移動・画像制御
        ControlMove();

        //プレイヤーを画面内に納める
        Utils.FitWindow(_transform);

        //ショット発射処理
        if (_coolTimeCount > 0&&dieCoolTimeCount==0)
        {
            _coolTimeCount -= Time.deltaTime;
        }
        if (_coolTimeCount <= 0 && Input.GetButton("Fire"))
        {
            Shot();
        }

        //ボム発射処理
        if (bombCoolTime > 0)
        {
            bombCoolTime -= Time.deltaTime;
            if (bombCoolTime <= 0)
            {
                GameManager.instance.DissableBlackSquare();
            }
        }
        else
        {
            if (Input.GetButton("Bomb")&&bombCount>0&&GameManager.instance.IsGameStart())
            {
                Bomb();
            }
        }

        //被弾関連の処理
        if (dieCoolTimeCount > 0)
        {
            dieCoolTimeCount -= Time.deltaTime;
            if ((int)((float)(dieCoolTimeCount/DieCoolTime)*10)%2==1)
            {
                Color c = _spriteRenderer.color;
                c.a = 0.2f;
                _spriteRenderer.color = c;
            }
            else
            {
                Color c = _spriteRenderer.color;
                c.a = 1.0f;
                _spriteRenderer.color = c;
            }
            if (dieCoolTimeCount <= 0)
            {
                dieCoolTimeCount = 0;
                Color c = _spriteRenderer.color;
                c.a = 1.0f;
                _spriteRenderer.color = c;
            }
            
        }
    }

    //移動制御
    private void ControlMove()
    {
        float xSpeed = Input.GetAxis("Horizontal");
        float ySpeed = Input.GetAxis("Vertical");
        if (xSpeed > 0)
        {
            xSpeed = 1;
        }
        else if (xSpeed < 0)
        {
            xSpeed = -1;
        }
        if (ySpeed > 0)
        {
            ySpeed = 1;
        }
        else if (ySpeed < 0)
        {
            ySpeed = -1;
        }
        if (xSpeed != 0 && ySpeed != 0)
        {
            xSpeed = xSpeed * Mathf.Sqrt(2) / 2;
            ySpeed = ySpeed * Mathf.Sqrt(2) / 2;
        }
        if (Input.GetButton("Shift"))
        {
            xSpeed *= SlowSpeed;
            ySpeed *= SlowSpeed;
        }
        else
        {
            xSpeed *= Speed;
            ySpeed *= Speed;
        }

        _transform.position += new Vector3(xSpeed, ySpeed, 0)*Time.deltaTime;

        //画像処理
        if (ySpeed != 0)
        {
            _spriteRenderer.sprite = sprites[1];
        }else if (xSpeed != 0)
        {
            _spriteRenderer.sprite = sprites[2];
        }
        else
        {
            _spriteRenderer.sprite = sprites[0];
        }
    }

    //弾発射
    private void Shot()
    {
        float range = 40;
        BulletImageEnum image = BulletImageEnum.PlayerShot1;
        if (Input.GetButton("Shift"))
        {
            range /= 4;
            image = BulletImageEnum.PlayerShot2;
        }
        for(int i = 0; i <= 4; i++)
        {
            float rotate = range/4 * (i - 2) * Mathf.Deg2Rad;
            Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Player,image, TrajectoryEnum.Straight, _transform.position);
            BulletManager.SetUpStraightBullet(bullet,20*Mathf.Cos(rotate), 20*Mathf.Sin(rotate));
        }
        _coolTimeCount = CoolTime;
        SoundManager.PlaySE(SoundManager.SE_Type.Shot2);
    }

    //ボム発射
    private void Bomb()
    {
        for(int i = 0; i <= 19; i++)
        {
            for(int j = 0; j <= 1; j++)
            {
                Bullet bullet = BulletManager.CreateBomb(_transform.position);
                BulletManager.SetUpMisuthiBullet(bullet, 2, 30*Mathf.Pow(-1,j), i * (360 / 20));
            }
        }
        GameManager.instance.ShowBlackSquare();
        bombCoolTime = 3.0f;
        bombCount--;
        BomCountText.text = bombCount + "";
        SoundManager.PlaySE(SoundManager.SE_Type.Bomb);
        SoundManager.PlaySE(SoundManager.SE_Type.BombShot);
        usedBombCount++;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (dieCoolTimeCount <= 0)
        {
            if (collision.gameObject.tag == "EnemyBullet"|| collision.gameObject.tag == "Enemy")
            {
                if (bombCoolTime > 0)
                {
                    if (collision.gameObject.tag == "EnemyBullet")
                    {
                        collision.gameObject.transform.parent.gameObject.SetActive(false);
                    }
                }
                else
                {
                    BulletManager.instance.AllEnemyBulletDelete();
                    int r = Random.Range(0, 3);
                    switch (r)
                    {
                        case 0:
                            SoundManager.PlaySE(SoundManager.SE_Type.Damage1);
                            break;
                        case 1:
                            SoundManager.PlaySE(SoundManager.SE_Type.Damage2);
                            break;
                        case 2:
                            SoundManager.PlaySE(SoundManager.SE_Type.Damage3);
                            break;
                    }
                    lifeCount--;
                    LifeCountText.text = lifeCount + "";
                    diedCount++;
                    if (lifeCount == 0)
                    {
                        //ゲームオーバー時の処理
                        GameManager.instance.GameOver();
                        BulletManager.instance.Invoke("AllBulletDelete", 0.1f);
                    }
                    else
                    {
                        dieCoolTimeCount = 2.0f;
                        bombCount = 3;
                        BomCountText.text = bombCount + "";
                    }
                }
            }
        }
    }
}
