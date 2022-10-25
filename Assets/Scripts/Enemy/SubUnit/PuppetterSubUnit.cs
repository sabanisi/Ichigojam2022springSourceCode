using System.Collections.Generic;
using UnityEngine;

public class PuppetterSubUnit:SubUnit
{
    [SerializeField] private Transform _transform;
    private int count;
    private float coolTime;
    private Vector3 Speeds;
    private List<Bullet> whiteBullte = new List<Bullet>();
    private List<Bullet> blackBullet = new List<Bullet>();
    private bool isMove = true;
    private bool isSound;

    public void SetSpeed(float speed,float rotate,bool _isSound)
    {
        _transform.rotation = Quaternion.Euler(0, 0, rotate+90);
        Speeds = new Vector3(speed * Mathf.Cos(rotate * Mathf.Deg2Rad), speed * Mathf.Sin(rotate * Mathf.Deg2Rad), 0);
        isSound = _isSound;
    }

    protected override void Action()
    {
        if (count >= 3 && Utils.IsOut(_transform.position))
        {
            isMove = false;
        }

        if (isMove)
        {
            _transform.position += Speeds * Time.deltaTime;
            if (coolTime <= 0)
            {
                count++;
                coolTime = 0.1f;
                BulletImageEnum imageEnum = BulletImageEnum.Black;
                if (count % 2 == 0)
                {
                    imageEnum = BulletImageEnum.White;
                }
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, imageEnum, TrajectoryEnum.Straight, _transform.position);
                BulletManager.SetUpStraightBullet(bullet, 0, 0);
                if (count % 2 == 0)
                {
                    whiteBullte.Add(bullet);
                }
                else
                {
                    blackBullet.Add(bullet);
                }
                if (isSound)
                {
                    SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
                }
            }
            else
            {
                coolTime -= Time.deltaTime;
            }
        } 
    }

    public void MoveStartOfBullet()
    {
        Transform playerTF = GameManager.GetPlayer().transform;
        foreach (var obj in whiteBullte)
        {
            if (obj.GetBulletImageEnum().Equals(BulletImageEnum.White))
            {
                float rotate = (Utils.CalculateAimRotate(obj.gameObject.transform.position, playerTF.position)-45+Random.Range(0,90)) * Mathf.Deg2Rad;
                obj.GetTrajectory().SetSpeed(2 * Mathf.Cos(rotate), 2 * Mathf.Sin(rotate));
            }
        }
        whiteBullte.Clear();
        foreach(var obj in blackBullet)
        {
            if (obj.GetBulletImageEnum().Equals(BulletImageEnum.Black))
            {
                float rotate = Utils.CalculateAimRotate(obj.gameObject.transform.position, playerTF.position) * Mathf.Deg2Rad;
                obj.GetTrajectory().SetSpeed(2 * Mathf.Cos(rotate), 2 * Mathf.Sin(rotate));
            }
        }
        blackBullet.Clear();
        Destroy(gameObject);
    }
}
