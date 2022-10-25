using UnityEngine;
using System.Collections;

public abstract class Trajectory
{
    //フィールド
    protected float rotate { get; private set; }//度数法
   
    protected Vector2 speed;
    protected Transform bulletTransform;

    public float GetRotate()
    {
        return rotate;
    }
    public Vector2 GetSpeed()
    {
        return speed;
    }

    public void SetBulletTransform(Transform transform)
    {
        bulletTransform = transform;
    }

    public void SetSpeed(float xSpeed, float ySpeed)
    {
        speed = new Vector2(xSpeed, ySpeed);
        if (xSpeed != 0 | ySpeed != 0)
        {
            rotate = Mathf.Atan2(ySpeed, xSpeed) * Mathf.Rad2Deg;
            if (rotate < 0)
            {
                rotate += 360;
            }
            bulletTransform.rotation = Quaternion.Euler(0, 0, rotate);
        }
    }

    public void SetRotate(float _rotate)
    {
        bulletTransform.rotation = Quaternion.Euler(0, 0, rotate);
        rotate = _rotate;
    }

    public abstract void Update();
}
