using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetterAction5:AbstractBossAction
{
    private Transform bossTF;
    private List<Puppet> puppets;
    private List<Transform> TFs = new List<Transform>();
    private bool isSetUp;
    private float startCount;
    private float coolTime;
    private int count;

    public PuppetterAction5(Boss parent, List<Puppet> _puppets) : base(parent)
    {
        bossTF = parent.gameObject.transform;
        puppets = _puppets;
    }
    
    public override void Update()
    {
        if (!isSetUp)
        {
            isSetUp = true;
            ((Puppetter)parent).PuppetCreate3();
            startCount = 0.9f;
            for (int i = 0; i <= 5; i++)
            {
                TFs.Add(puppets[i].gameObject.transform);
            }
        }
        if (startCount > 0)
        {
            startCount -= Time.deltaTime;
            if (startCount <= 0)
            {
                GoRoundPosition();
            }
            return;
        }
        if (coolTime <= 0)
        {
            count++;
            if (count == 12)
            {
                count = 0;
            }
            if (count != 0)
            {
                for (int i = 0; i < puppets.Count; i++)
                {
                    float rotate = puppets[i].GetRotate();
                    float newRotate = (rotate + 90) * Mathf.Deg2Rad;
                    if (count % 3 == 1)
                    {
                        newRotate = (rotate - 120) * Mathf.Deg2Rad;
                    }
                    else if (count % 3 == 2)
                    {
                        newRotate = (rotate + 150) * Mathf.Deg2Rad;
                    }
                    Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.White, TrajectoryEnum.Straight, TFs[i].position);
                    BulletManager.SetUpStraightBullet(bullet, 8 * Mathf.Cos(newRotate), 8 * Mathf.Sin(newRotate));
                }
                SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
            }
            else
            {
                float randomRotate = Random.Range(0, 360);
                for(int i = 0; i <= 35; i++)
                {
                    float newRotate = (randomRotate + i * 10) * Mathf.Deg2Rad;
                    Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Black, TrajectoryEnum.Straight,bossTF.position);
                    BulletManager.SetUpStraightBullet(bullet, 5 * Mathf.Cos(newRotate), 5 * Mathf.Sin(newRotate));
                }
                SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
            }
        
            coolTime = 0.08f;
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
    }

    private void GoRoundPosition()
    {
        Hashtable bossMove = new Hashtable();
        bossMove.Add("position", new Vector3(5.5f, 0, 0));
        bossMove.Add("time", 0.8f);
        bossMove.Add("oncomplete", "StartRound");
        bossMove.Add("oncompletetarget", parent.gameObject);
        iTween.MoveTo(parent.gameObject,bossMove);

        float r = 0.5f /(10* Mathf.Deg2Rad);
        for(int i=0; i < puppets.Count; i++)
        {
            Puppet puppet = puppets[i];
            float rotate = i * 60 * Mathf.Deg2Rad;
            float x = 5.5f + r * Mathf.Cos(rotate);
            float y = r * Mathf.Sin(rotate);
            
            Hashtable move = new Hashtable();
            move.Add("position", new Vector3(x, y, 0));
            move.Add("time", 0.8f);
            iTween.MoveTo(puppet.gameObject,move);

        }
        coolTime = 1.0f;
    }

    public override void FinishAction()
    {
        foreach(var puppet in puppets)
        {
            puppet.SetIsMove(false, puppet.GetRotate(), 10f, 200);
        }
    }

}
