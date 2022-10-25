using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private GameObject BombPrefab;
    [SerializeField] private List<Sprite> BulletImages;//このリストの順番とBulletImageeEnumの順番が一致するようにすること
    private List<Bullet> bulletStock;

    [SerializeField] private GameObject HitAnimationPrefab;
    private List<HitAnimation> hitAnimationStock;
    [SerializeField] private GameObject BossDieAnimationPrefab;
    private List<GameObject> bossDieAnimationStock;

    public static BulletManager instance;

    public static Bullet CreateBullet(BulletBelongEnum belongEnum, BulletImageEnum imageEnum, TrajectoryEnum trajectoryEnum, Vector3 pos)
    {
        return instance.CreateBulletDeal(belongEnum, imageEnum, trajectoryEnum, pos);
    }

    private Bullet CreateBulletDeal(BulletBelongEnum belongEnum, BulletImageEnum imageEnum, TrajectoryEnum trajectoryEnum, Vector3 pos)
    {
        Sprite sprite = BulletImages[(int)imageEnum];
        Trajectory trajectory = CreateTrajectory(trajectoryEnum);

        foreach (var _bullet in bulletStock)
        {
            if (!_bullet.gameObject.activeSelf)
            {
                _bullet.gameObject.SetActive(true);
                _bullet.Initialize(belongEnum, imageEnum, sprite, pos, trajectory);

                return _bullet;
            }
        }

        GameObject bulletObject = Instantiate(BulletPrefab, transform);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Initialize(belongEnum, imageEnum, sprite, pos, trajectory);
        bulletStock.Add(bullet);
        return bullet;
    }

    public static Bullet CreateBomb(Vector3 pos)
    {
        return instance.CreateBombDeal(pos);
    }

    private Bullet CreateBombDeal(Vector3 pos)
    {
        Sprite sprite = BulletImages[(int)BulletImageEnum.Bomb];
        Trajectory trajectory = CreateTrajectory(TrajectoryEnum.MisuthiTra);
        GameObject bombObject = Instantiate(BombPrefab, transform);
        Bullet bullet = bombObject.GetComponent<Bullet>();
        bullet.Initialize(BulletBelongEnum.Bomb, BulletImageEnum.Bomb, sprite, pos, trajectory);
        return bullet;
    }

    private Trajectory CreateTrajectory(TrajectoryEnum trajectoryEnum)
    {
        Trajectory trajectory = null;
        switch (trajectoryEnum)
        {
            case TrajectoryEnum.Straight:
                trajectory = new StraightTra();
                break;
            case TrajectoryEnum.Zigzag:
                trajectory = new ZigzagTra();
                break;
            case TrajectoryEnum.MisuthiTra:
                trajectory = new MisuthiTra();
                break;
            default:
                Debug.Log("BulletManagerのCreateTrajectoryにてバグ");
                break;
        }
        return trajectory;
    }

    public static void SetUpStraightBullet(Bullet bullet, float xSpeed, float ySpeed)
    {
        bullet.GetTrajectory().SetSpeed(xSpeed, ySpeed);
    }

    public static void SetUpZigzagBullet(Bullet bullet, List<float[]> speeds, List<float> coolTimes)
    {
        ((ZigzagTra)bullet.GetTrajectory()).SetSpeeds(speeds);
        ((ZigzagTra)bullet.GetTrajectory()).SetTimes(coolTimes);
    }

    public static void SetUpMisuthiBullet(Bullet bullet,float rSpeed,float omegaSpeed,float rotate)
    {
        ((MisuthiTra)bullet.GetTrajectory()).SetSpeeds(rSpeed, omegaSpeed, rotate);
    }

    public void AllEnemyBulletDelete()
    {
        int checkCountForBulletDelete = 0;
        foreach (var _bullet in bulletStock)
        {
            if (_bullet.GetBulletBelongEnum() == BulletBelongEnum.Enemy)
            {
                bulletStock[checkCountForBulletDelete].gameObject.SetActive(false);
                //演出つける

            }
            checkCountForBulletDelete++;
        }
    }

    public void AllBulletDelete()
    {
        foreach(var _bullet in bulletStock)
        {
            _bullet.gameObject.SetActive(false);
        }
    }

    public static HitAnimation CreateHitAnimation(HitAnimationEnum animationEnum,Vector3 pos)
    {
        return instance.CreateHitAnimationDeal(animationEnum, pos);
    }

    private HitAnimation CreateHitAnimationDeal(HitAnimationEnum animationEnum, Vector3 pos)
    {
        foreach(var _hit in hitAnimationStock)
        {
            if (!_hit.gameObject.activeSelf)
            {
                _hit.gameObject.SetActive(true);
                _hit.Initialize(animationEnum, pos);
                return _hit;
            }
        }
        GameObject hitAnimation = Instantiate(HitAnimationPrefab, transform);
        HitAnimation script = hitAnimation.GetComponent<HitAnimation>();
        script.Initialize(animationEnum,pos);
        hitAnimationStock.Add(script);
        return script;
    }

    public static void CreateBossDieAnimation(Vector3 pos)
    {
        instance.CreateBossDieAnimationDeal(pos);
    }

    private void CreateBossDieAnimationDeal(Vector3 pos)
    {
        GameObject bossDieAnimation = Instantiate(BossDieAnimationPrefab);
        bossDieAnimation.transform.position = pos;
        bossDieAnimationStock.Add(bossDieAnimation);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            bulletStock = new List<Bullet>();
            hitAnimationStock = new List<HitAnimation>();
            bossDieAnimationStock = new List<GameObject>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DestroyDeal()
    {
        foreach (var _bullet in bulletStock)
        {
            Destroy(_bullet.gameObject);
        }
        bulletStock.Clear();

        foreach(var _hitAnimation in hitAnimationStock)
        {
            Destroy(_hitAnimation.gameObject);
        }
        hitAnimationStock.Clear();
        foreach(Transform childTransform in transform)
        {
            if (childTransform != null)
            {
                Destroy(childTransform.gameObject);
            }
        }

        foreach(var bossDieAnimation in bossDieAnimationStock)
        {
            Destroy(bossDieAnimation);
        }
        bossDieAnimationStock.Clear();
    }
}
