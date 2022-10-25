using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppet : SubUnit
{
    [SerializeField] private Transform _transform;
    private bool isMove;
    private float rotate;
    private float speed;
    private float omega;
    public void SetIsMove(bool _isMove,float _rotate,float _speed,float _omega)
    {
        isMove = _isMove;
        rotate = _rotate;
        speed = _speed;
        omega = _omega;
    }
    public void SetIsMoveOnly(bool _isMove)
    {
        isMove = _isMove;
    }

    public float GetRotate()
    {
        return rotate;
    }


    protected override void Action()
    {
        if (isMove)
        {
            float xSpeed = -speed * Mathf.Sin(rotate * Mathf.Deg2Rad);
            float ySpeed = speed * Mathf.Cos(rotate * Mathf.Deg2Rad);
            _transform.position += new Vector3(xSpeed, ySpeed, 0)*Time.deltaTime;
            rotate += omega * Time.deltaTime;
        }
    }
}
