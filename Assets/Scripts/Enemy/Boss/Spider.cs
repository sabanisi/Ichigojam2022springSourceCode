using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Spider : Boss
{
    [SerializeField] private GameObject spider;
    private Transform spiderTransform;
    private SpriteRenderer spiderSprite;
    [SerializeField] private float PenduramRotate;//振り子のように揺れる角度
    private bool isSwingContinue;//振り子運動を続けるかどうか
    private bool isNowSwing;//今振り子運動をしているかどうか
    [SerializeField] private Vector3 CenterPos;
    [SerializeField] private float SwingSpeed;
    private float time=0;
    private float ThreadLong;

    public void SetIsSwingContinue(bool _isSwingContinue)
    {
        isSwingContinue = _isSwingContinue;
    }

    [SerializeField] private GameObject Unit1Prefab;
    [SerializeField] private GameObject Unit2Prefab;
    [SerializeField] private Vector3 mouthPos;
    [SerializeField] private Vector3 mouthPos2;

    protected override void Start()
    {
        base.Start();
        spiderTransform = spider.transform;
        spiderSprite = spider.GetComponent<SpriteRenderer>();

        maxActionNumber = 6;
        actions.Add(new SpiderAction1(this,Unit1Prefab));
        actions.Add(new SpiderAction2(this, mouthPos, Unit2Prefab));
        actions.Add(new SpiderAction3(this,_transform));
        actions.Add(new SpiderAction4(this, transform, mouthPos2));
        actions.Add(new SpiderAction5(this, mouthPos2));
        actions.Add(new SpiderAction6(this, mouthPos2));
    }

    public override void GameStartDeal()
    {
        EnterDeal();
    }

    private void EnterDeal()
    {
        Hashtable moveTable = new Hashtable();
        moveTable.Add("time", 1.2f);
        moveTable.Add("position", new Vector3(5,-1,0));
        moveTable.Add("oncomplete", "EnterDeal2");
        moveTable.Add("oncompletetarget", gameObject);
        moveTable.Add("easeType", "easeOutSine");
        iTween.MoveTo(gameObject, moveTable);
    }

    private void EnterDeal2()
    {
        StartCoroutine(StartSwing());
    }

    private IEnumerator StartSwing()
    {
        isSwingContinue = true;
        ThreadLong = transform.position.y - CenterPos.y;
        GameManager.instance.GameStartDeal1();
        yield return new WaitForSeconds(1.2f);
    }

    public override void GameClearDeal()
    {
        StartCoroutine(Blinking());
        StartCoroutine(BossDieAnimationCoroutine());
    }

    private IEnumerator Blinking()
    {
        int count = 0;
        while (count <= 9)
        {
            if (spiderSprite.color.a == 1)
            {
                spiderSprite.color = new Color(1, 1, 1, 0.2f);
            }
            else
            {
                spiderSprite.color = new Color(1, 1, 1, 1f);
            }
            count++;
            yield return new WaitForSeconds(0.1f);
        }
        spiderSprite.color = new Color(1, 1, 1, 0f);
        yield return new WaitForSeconds(0.3f);
        GameManager.instance.GameClearDeal();
    }

    public override void Update()
    {
        base.Update();
        if (GameManager.instance.IsPause()) return;
        if (isSwingContinue)
        {
            isNowSwing = true;
        }
        if (isNowSwing)
        {
            float rotate = PenduramRotate * Mathf.Sin(SwingSpeed * time);
            _transform.rotation = Quaternion.Euler(0, 0, -rotate);
            _transform.position = new Vector3(CenterPos.x + ThreadLong * Mathf.Sin(rotate*Mathf.Deg2Rad), CenterPos.y + ThreadLong * Mathf.Cos(rotate*Mathf.Deg2Rad), 0);
            time += Time.deltaTime;
            if (!isSwingContinue && Mathf.Abs(rotate) <= 0.1f)
            {
                isNowSwing = false;
                _transform.position = new Vector3(CenterPos.x, CenterPos.y + ThreadLong, 0);
                _transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
       
    }

    private IEnumerator BossDieAnimationCoroutine()
    {
        for (int i = 0; i <= 2; i++)
        {
            for(int j = 0; j <= 1; j++)
            {
                Vector3 pos = _transform.position + spiderTransform.position + new Vector3(Random.value * 2 - 1.0f, Random.value * 3 - 1.5f, 0);
                BulletManager.CreateBossDieAnimation(pos);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

}
