using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cats : Boss
{
    [SerializeField] private GameObject Cat1;
    [SerializeField] private GameObject Cat2;
    private Transform cat1Transform;
    private Transform cat2Transform;
    private SpriteRenderer cat1Sprite;
    private SpriteRenderer cat2Sprite;
    protected override void Start()
    {
        base.Start();
        cat1Transform = Cat1.transform;
        cat2Transform = Cat2.transform;
        cat1Sprite = Cat1.GetComponent<SpriteRenderer>();
        cat2Sprite = Cat2.GetComponent<SpriteRenderer>();

        //行動の追加 //順番2,3,1,4,5,6 HP 60,40,80,80,80,100
        maxActionNumber = 6;
        actions.Add(new CatAction2(this, Cat1, Cat2));
        actions.Add(new CatAction3(this, Cat1, Cat2));
        actions.Add(new CatAction1(this,Cat1,Cat2));
        actions.Add(new CatAction4(this, Cat1, Cat2));
        actions.Add(new CatAction5(this, Cat1, Cat2));
        actions.Add(new CatAction6(this, Cat1, Cat2));
    }

    public override void GameStartDeal()
    {
        EnterCat1();
        Invoke("EnterCat2", 0.25f);
    }

    private void EnterCat1()
    {
        Hashtable cat1Table = new Hashtable();
        Vector3[] path1 = new Vector3[3];
        path1[0] = new Vector3(9f, -3f, 0);
        path1[1] = new Vector3(7.5f, 0, 0);
        path1[2] = new Vector3(5.75f, 2.5f, 0);
        cat1Table.Add("path", path1);
        cat1Table.Add("time", 0.5f);
        cat1Table.Add("easeType", "easeOutSine");
        iTween.MoveTo(Cat1, cat1Table);
    }

    private void EnterCat2()
    {
        Hashtable cat2Table = new Hashtable();
        Vector3[] path2 = new Vector3[3];
        path2[0] = new Vector3(9f, 3f, 0);
        path2[1] = new Vector3(7.5f, 0, 0);
        path2[2] = new Vector3(5.75f, -2.5f, 0);
        cat2Table.Add("path", path2);
        cat2Table.Add("time", 0.5f);
        cat2Table.Add("easeType", "easeOutSine");
        cat2Table.Add("oncomplete", "GameStartDeal2");
        cat2Table.Add("oncompletetarget", gameObject);
        iTween.MoveTo(Cat2, cat2Table);
    }

    private void GameStartDeal2()
    {
        GameManager.instance.GameStartDeal1();
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
            if (cat1Sprite.color.a == 1)
            {
                cat1Sprite.color = new Color(1, 1, 1, 0.2f);
                cat2Sprite.color = new Color(1, 1, 1, 0.2f);
            }
            else
            {
                cat1Sprite.color = new Color(1, 1, 1, 1f);
                cat2Sprite.color = new Color(1, 1, 1, 1f);
            }
            count++;
            yield return new WaitForSeconds(0.1f);
        }
        cat1Sprite.color = new Color(1, 1, 1, 0f);
        cat2Sprite.color = new Color(1, 1, 1, 0f);
        yield return new WaitForSeconds(0.3f);
        GameManager.instance.GameClearDeal();
    }

    private IEnumerator BossDieAnimationCoroutine()
    {
        for (int i = 0; i <= 2; i++)
        {
            Vector3 pos1 = _transform.position + cat1Transform.position + new Vector3(Random.value*2-1.0f, Random.value*2-1.0f,0);
            Vector3 pos2 = _transform.position + cat2Transform.position + new Vector3(Random.value*2-1.0f, Random.value*2-1.0f,0);
            BulletManager.CreateBossDieAnimation(pos1);
            BulletManager.CreateBossDieAnimation(pos2);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
