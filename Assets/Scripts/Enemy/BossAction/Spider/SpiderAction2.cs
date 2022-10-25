using System.Collections.Generic;
using UnityEngine;

public class SpiderAction2:AbstractBossAction
{
    private Vector3 _mouthPos;
    private GameObject _spiderPrefab;
    private List<SpiderUnit2> units = new List<SpiderUnit2>();
    private float coolTime;

    public SpiderAction2(Boss parent, Vector3 mouthPos, GameObject spiderPrefab) : base(parent)
    {
        _mouthPos = mouthPos;
        _spiderPrefab = spiderPrefab;
    }

    public override void Update()
    {
        if (coolTime <= 0)
        {
            float randomRotate = Random.Range(0, 360);
            for(int i = 0; i <= 2; i++)
            {
                for(int j = 0; j <= 7; j++)
                {
                    SpiderUnit2 spider = UnityEngine.Object.Instantiate(_spiderPrefab).GetComponent<SpiderUnit2>();
                    spider.transform.position = _mouthPos;
                    spider.SetUp(randomRotate+j * 45, 3 * (i + 1));
                    units.Add(spider);
                }
            }
            coolTime = 4.0f;
            SoundManager.PlaySE(SoundManager.SE_Type.Shot5);
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
    }

    public override void FinishAction()
    {
        foreach (var unit in units)
        {
            if (unit != null)
            {
                unit.DestroyDeal();
            }
        }
        units.Clear();
    }
}
