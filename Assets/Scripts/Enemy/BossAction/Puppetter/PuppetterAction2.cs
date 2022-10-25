using System.Collections.Generic;
using UnityEngine;

public class PuppetterAction2:AbstractBossAction
{
    private Transform bossTF;
    private List<Puppet> pupptes;
    private List<PuppetterSubUnit> subUnits=new List<PuppetterSubUnit>();
    private GameObject prefab;
    private Transform playerTF;

    private float coolTime;
    private int count;

    public PuppetterAction2(Boss parent, List<Puppet> _puppets, GameObject _prefab):base(parent)
    {
        bossTF = parent.gameObject.transform;
        pupptes = _puppets;
        prefab = _prefab;
        playerTF = GameManager.GetPlayer().transform;
    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            if (count == 0)
            {
                coolTime = 1.5f;
                for(int i = 0; i <= 1; i++)
                {
                    float rotate = 210 - i * 60;
                    GameObject subUnit = UnityEngine.Object.Instantiate(prefab);
                    subUnit.transform.position = new Vector3(9.7f, 4.5f * Mathf.Pow(-1, i), 0);
                    PuppetterSubUnit script = subUnit.GetComponent<PuppetterSubUnit>();
                    bool isSound = true;
                    if (i == 0)
                    {
                        isSound = false;
                    }
                    script.SetSpeed(9, rotate,isSound);
                    subUnits.Add(script);
                }
                count++;
            }
            else
            {
                coolTime = 0.2f;
                if (count <= 15)
                {
                    foreach (var puppet in pupptes)
                    {
                        Vector3 pos = puppet.gameObject.transform.position;

                        float aimRotate = Utils.CalculateAimRotate(pos, playerTF.position);
                        for (int i = 0; i <= 9; i++)
                        {
                            float rotate = (aimRotate + i * (360 / 10)) * Mathf.Deg2Rad;
                            Bullet bullet = BulletManager.CreateBullet(BulletBelongEnum.Enemy, BulletImageEnum.Red2, TrajectoryEnum.Straight, pos);
                            BulletManager.SetUpStraightBullet(bullet, 10 * Mathf.Cos(rotate), 10 * Mathf.Sin(rotate));
                        }
                    }
                    SoundManager.PlaySE(SoundManager.SE_Type.Shot4);
                }
               count++;
               if (count == 22)
               {
                    foreach(var obj in subUnits)
                    {
                        if (obj != null)
                        {
                            obj.MoveStartOfBullet();
                        }
                    }
                    subUnits.Clear();
                    count = 0;
                    coolTime = 0.3f;
                }
            }
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
    }

    public override void FinishAction()
    {
        foreach(var subUnit in subUnits)
        {
            if (subUnit != null)
            {
                subUnit.DestroyDeal();
            }
        }
        subUnits.Clear();
    }
}
