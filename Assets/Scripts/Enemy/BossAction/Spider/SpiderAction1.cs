using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAction1:AbstractBossAction
{
    private bool isStart;
    private GameObject _eyePrefab;
    private List<SpiderUnit1> units = new List<SpiderUnit1>();
    private float coolTime;
    private float coolTimne2;
    private Transform playerTF;
    private Transform bossTF;
    private int count;

    public SpiderAction1(Boss parent,GameObject eyePrefab) : base(parent)
    {
        _eyePrefab = eyePrefab;
        playerTF = GameManager.GetPlayer().transform;
        bossTF = parent.transform;
    }

    public override void Update()
    {
        if (!isStart)
        {
            isStart = true;
            CreateUnit();
        }
        if (coolTime <= 0)
        {
            coolTime = 0.05f;
            float rotate = UnityEngine.Random.Range(0, 360);
            for (int i = 0; i <= 2; i++)
            {
                float rotate2 = (rotate + (i - 1) *30.0f) * Mathf.Deg2Rad;
                Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Spider, TrajectoryEnum.Straight,bossTF.position+new Vector3(0,1.15f,0));
                BulletManager.SetUpStraightBullet(bullet, 4 * Mathf.Cos(rotate2), 4 * Mathf.Sin(rotate2));
            }
            count++;
            if (count % 3 == 0)
            {
                SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
            }
        }
        else
        {
            coolTime -= Time.deltaTime;
        }

    }

    private void CreateUnit()
    {
        for(int i = 0; i <= 1; i++)
        {
            SpiderUnit1 unit = UnityEngine.Object.Instantiate(_eyePrefab).GetComponent<SpiderUnit1>();
            unit.transform.position = bossTF.position + new Vector3(0, 1.15f, 0);

            Hashtable table = new Hashtable();
            table.Add("time", 1.0f);
            table.Add("position", new Vector3(0, 3.0f * Mathf.Pow(-1, i), 0));
            table.Add("oncomplete", "MoveFinish");
            table.Add("oncompletetarget", unit.gameObject);
            table.Add("easeType", "easeOutSine");
            iTween.MoveTo(unit.gameObject, table);

            units.Add(unit);
        }
        coolTime = 1.1f;
        SoundManager.PlaySE(SoundManager.SE_Type.Shot5);
    }

    public override void FinishAction()
    {
        ((Spider)parent).SetIsSwingContinue(false);
        foreach(var unit in units)
        {
            unit.DestroyDeal();
        }
        units.Clear();
    }
}
