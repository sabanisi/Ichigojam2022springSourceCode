using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //フィールド
    protected Transform _transform;
    protected Trajectory _trajectory;//軌道(弾の移動制御)
    [SerializeField]protected SpriteRenderer _spriteRenderer;
    protected BulletBelongEnum _belongEnum;
    protected BulletImageEnum _imageEnum;

    public GameObject CircleCollider;
    public GameObject CapsuleCollider;
    public GameObject CapsuleCollider2;
    public GameObject CircleCollider2;

    protected bool _isStop;

    private float dontDestroyCount;

    public void SetIsStop(bool isStop)
    {
        _isStop = isStop;
        if (_isStop)
        {
            ShowHitCollision();
        }
        else
        {
            DeleteHitCollision();
        }
    }

    public bool IsStop()
    {
        return _isStop;
    }
    

    public Transform GetPos()
    {
        return _transform;
    }
    public Trajectory GetTrajectory()
    {
        return _trajectory;
    }

    public BulletBelongEnum GetBulletBelongEnum()
    {
        return _belongEnum;
    }

    public BulletImageEnum GetBulletImageEnum()
    {
        return _imageEnum;
    }
    
    public void SetSpeed(float xSpeed,float ySpeed)
    {
        _trajectory.SetSpeed(xSpeed, ySpeed);
    }

    public void SetDontDestroyCount(float _count)
    {
        dontDestroyCount = _count;
    }

    public void Initialize(BulletBelongEnum belngEnum,BulletImageEnum bulletImageEnum,Sprite sprite, Vector3 pos, Trajectory trajectory)
    {
        if (_transform == null)
        {
            _transform = transform;
        }
        _isStop = false;
        _belongEnum = belngEnum;
        _imageEnum = bulletImageEnum;
        
        _spriteRenderer.sprite = sprite;
        _transform.position = pos;
        _trajectory = trajectory;
        _trajectory.SetBulletTransform(_transform);

        if (_belongEnum != BulletBelongEnum.Bomb)
        {
            DeleteHitCollision();
            ShowHitCollision();

            if (_belongEnum == BulletBelongEnum.Enemy)
            {
                gameObject.tag = "EnemyBullet";
                gameObject.layer = LayerMask.NameToLayer("EnemyBullet");
                foreach (Transform childTF in _transform)
                {
                    childTF.gameObject.tag = "EnemyBullet";
                    childTF.gameObject.layer = LayerMask.NameToLayer("EnemyBullet");
                }
                _spriteRenderer.sortingOrder = 2;
            }
            else if (_belongEnum == BulletBelongEnum.Player)
            {
                gameObject.tag = "PlayerBullet";
                gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
                foreach (Transform childTF in _transform)
                {
                    childTF.gameObject.tag = "PlayerBullet";
                    childTF.gameObject.layer = LayerMask.NameToLayer("PlayerBullet");
                }
                _spriteRenderer.sortingOrder = 1;
            }
        }
    }

    private void DeleteHitCollision()
    {
        CircleCollider.SetActive(false);
        CapsuleCollider.SetActive(false);
        CapsuleCollider2.SetActive(false);
        CircleCollider2.SetActive(false);
    }

    private void ShowHitCollision()
    {
        switch (_imageEnum.GetColluderEnum())
        {
            case BulletColliderEnum.Capsule:
                CapsuleCollider.SetActive(true);
                break;
            case BulletColliderEnum.Circle:
                CircleCollider.SetActive(true);
                break;
            case BulletColliderEnum.Capsule2:
                CapsuleCollider2.SetActive(true);
                break;
            case BulletColliderEnum.Circle2:
                CircleCollider2.SetActive(true);
                break;
        }
    }

    void Start()
    {
        if (_transform == null)
        {
            _transform = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsPause())  return;
        if (GameManager.instance.IsGameOver()&&_belongEnum!=BulletBelongEnum.Bomb) return;
        if (_isStop) return;

        //移動処理
        if (_trajectory != null)
        {
            _trajectory.Update();
        }

        if (dontDestroyCount > 0)
        {
            dontDestroyCount -= Time.deltaTime;
        }
        else
        {
            //画面外ならactiveをfalseにする
            if (Utils.IsOut(_transform.position))
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (_belongEnum.Equals(BulletBelongEnum.Enemy))
        {
            if (collision.gameObject.tag == "Bomb")
            {
                gameObject.SetActive(false);
            }
        }
    }
}
