using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppetter : Boss
{
    [SerializeField] private GameObject _puppetter;
    [SerializeField] private GameObject PuppetPrefab;
    [SerializeField] private GameObject SubUnitPrefab;

    private SpriteRenderer puppetterSprite;
    private Transform puppetterTransform;

    private List<Puppet> puppets = new List<Puppet>();

    protected override void Start()
    {
        base.Start();
        puppetterSprite = _puppetter.GetComponent<SpriteRenderer>();
        puppetterTransform = _puppetter.transform;

        maxActionNumber = 6;
        actions.Add(new PuppetterAction1(this,puppets));
        actions.Add(new PuppetterAction2(this, puppets, SubUnitPrefab));
        actions.Add(new PuppetterAction3(this, puppets));
        actions.Add(new PuppetterAction4(this, puppets));
        actions.Add(new PuppetterAction5(this, puppets));
        actions.Add(new PuppetterAction6(this, puppets));
    }

    public override void GameStartDeal()
    {
        Hashtable moveTable = new Hashtable();
        moveTable.Add("time", 1.2f);
        moveTable.Add("position", new Vector3(7.5f,0, 0));
        moveTable.Add("oncomplete", "PuppetCreate1");
        moveTable.Add("oncompletetarget", gameObject);
        moveTable.Add("easeType", "easeOutCubic");
        iTween.MoveTo(gameObject, moveTable);
    }

    public void PuppetCreate1()
    {
        for(int i = 0; i <= 1; i++)
        {
            Puppet puppet = Instantiate(PuppetPrefab).GetComponent<Puppet>();
            puppet.gameObject.transform.position = new Vector3(9, 2.5f * Mathf.Pow(-1, i), 0);
            Hashtable moveTable = new Hashtable();
            moveTable.Add("time", 0.8f);
            moveTable.Add("position", new Vector3(6, 2.5f * Mathf.Pow(-1, i), 0));
            moveTable.Add("easeType", "easeOutCubic");
            iTween.MoveTo(puppet.gameObject, moveTable);
            puppets.Add(puppet);
        }
        Invoke("GameStartDeal2", 0.8f);
    }

    public void PuppetCreate2()
    {
        SoundManager.PlaySE(SoundManager.SE_Type.Laugh);
        for (int i = 0; i <= 1; i++)
        {
            Puppet puppet = Instantiate(PuppetPrefab).GetComponent<Puppet>();
            puppet.gameObject.transform.position = new Vector3(9, 2 * Mathf.Pow(-1, i), 0);
            Hashtable moveTable = new Hashtable();
            moveTable.Add("time", 0.8f);
            moveTable.Add("position", new Vector3(5.2f, 2 * Mathf.Pow(-1, i), 0));
            moveTable.Add("easeType", "easeOutCubic");
            iTween.MoveTo(puppet.gameObject, moveTable);
            puppets.Add(puppet);
        }
    }

    public void PuppetCreate3()
    {
        SoundManager.PlaySE(SoundManager.SE_Type.Laugh);
        for (int i = 0; i <= 1; i++)
        {
            Puppet puppet = Instantiate(PuppetPrefab).GetComponent<Puppet>();
            puppet.gameObject.transform.position = new Vector3(9, 1 * Mathf.Pow(-1, i), 0);
            Hashtable moveTable = new Hashtable();
            moveTable.Add("time", 0.8f);
            moveTable.Add("position", new Vector3(4.5f, 1 * Mathf.Pow(-1, i), 0));
            moveTable.Add("easeType", "easeOutCubic");
            iTween.MoveTo(puppet.gameObject, moveTable);
            puppets.Add(puppet);
        }
        invisibleCount = 0.8f;
    }

    public void StartRound()
    {
        for(int i = 0; i < puppets.Count; i++)
        {
            puppets[i].SetIsMove(true, i * 60, 5f, 100);
        }
    }


    public void GameStartDeal2()
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
            if (puppetterSprite.color.a == 1)
            {
                puppetterSprite.color = new Color(1, 1, 1, 0.2f);
            }
            else
            {
                puppetterSprite.color = new Color(1, 1, 1, 1f);
            }
            count++;
            yield return new WaitForSeconds(0.1f);
        }
        puppetterSprite.color = new Color(1, 1, 1, 0f);
        yield return new WaitForSeconds(0.3f);
        GameManager.instance.GameClearDeal();
    }

    private IEnumerator BossDieAnimationCoroutine()
    {
        for (int i = 0; i <= 2; i++)
        {
            for (int j = 0; j <= 1; j++)
            {
                Vector3 pos = puppetterTransform.position + new Vector3(Random.value * 1.4f - 0.7f, Random.value * 2.6f - 1.3f, 0);
                BulletManager.CreateBossDieAnimation(pos);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
