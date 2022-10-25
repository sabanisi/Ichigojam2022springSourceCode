using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils:MonoBehaviour
{
	private Utils instance;
    public void Start()
    {
        if (instance == null)
        {
			instance = this;
        }
        else
        {
			Destroy(gameObject);
        }
    }

    // 移動可能な範囲
    public static Vector2 LimitLeftTop = new Vector2(-8.3f, 4.4f);
	public static Vector2 LimitRightBottom = new Vector2(8.3f, -4.4f);

	//オブジェクトが消える範囲かどうか
	public static bool IsOut(Vector3 position)
	{
		return (position.x < LimitLeftTop.x - 1.0f ||
			   LimitLeftTop.y + 1.0f < position.y ||
				LimitRightBottom.x + 1.0f < position.x ||
				position.y < LimitRightBottom.y - 1.0f);
	}

	//画面内かどうか
	public static bool IsOutWindow(Vector3 position)
	{
		return (position.x < LimitLeftTop.x ||
			   LimitLeftTop.y < position.y ||
				LimitRightBottom.x < position.x ||
				position.y < LimitRightBottom.y);
	}

	//画面内にオブジェクトを納める
	public static void FitWindow(Transform _transform)
    {
		Vector3 pos = _transform.position;
        if (pos.x < LimitLeftTop.x)//左にはみ出しているとき
        {
			pos.x = LimitLeftTop.x;
        }
        if (pos.x > LimitRightBottom.x)//右
        {
			pos.x = LimitRightBottom.x;
        }
        if (pos.y < LimitRightBottom.y)
        {
			pos.y = LimitRightBottom.y;
        }
        if (pos.y > LimitLeftTop.y)
        {
			pos.y = LimitLeftTop.y;
        }
		_transform.position = pos;
    }

	//fromPosからtoPosを見た角度を弧度法で求める
	public static float CalculateAimRotate(Vector3 fromPos,Vector3 toPos)
    {
		return Mathf.Atan2(toPos.y - fromPos.y, toPos.x - fromPos.x)*Mathf.Rad2Deg;
    }

	//オブジェクトを消す
	public static void StaticDestroy(GameObject gameObject)
    {
		Destroy(gameObject);
    }

}
