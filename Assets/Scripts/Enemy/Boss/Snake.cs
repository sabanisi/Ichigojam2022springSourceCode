using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Boss
{
    [SerializeField] private GameObject snake;
    [SerializeField] private Vector3 attackPos1;
    [SerializeField] private Vector3 attackPos2;
    [SerializeField] private Vector3 attackPos3;
    private Transform snakeTF;
    private SpriteRenderer snakeSprite;

    protected override void Start()
    {
        base.Start();
        snakeTF = snake.transform;
        snakeSprite = snake.GetComponent<SpriteRenderer>();

        maxActionNumber = 6;
        actions.Add(new SnakeAction2(this, attackPos1, attackPos2));
        actions.Add(new SnakeAction4(this, attackPos3));
        actions.Add(new SnakeAction3(this, attackPos1));
        actions.Add(new SnakeAction1(this, attackPos1));
        actions.Add(new SnakeAction5(this, attackPos1, attackPos2));
        actions.Add(new SnakeAction6(this, attackPos1, attackPos2));
    }

    public override void GameStartDeal()
    {
        Hashtable moveTable = new Hashtable();
        moveTable.Add("time", 1.2f);
        moveTable.Add("position", new Vector3(6,0, 0));
        moveTable.Add("oncomplete", "EnterDeal");
        moveTable.Add("oncompletetarget", gameObject);
        moveTable.Add("easeType", "easeOutSine");
        iTween.MoveTo(gameObject, moveTable);
    }

    public void EnterDeal()
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
            if (snakeSprite.color.a == 1)
            {
                snakeSprite.color = new Color(1, 1, 1, 0.2f);
            }
            else
            {
                snakeSprite.color = new Color(1, 1, 1, 1f);
            }
            count++;
            yield return new WaitForSeconds(0.1f);
        }
        snakeSprite.color = new Color(1, 1, 1, 0f);
        yield return new WaitForSeconds(0.3f);
        GameManager.instance.GameClearDeal();
    }

    private IEnumerator BossDieAnimationCoroutine()
    {
        for (int i = 0; i <= 2; i++)
        {
            for (int j = 0; j <= 2; j++)
            {
                Vector3 pos = snakeTF.position + new Vector3(Random.value * 5.2f - 2.6f, Random.value * 5.0f - 2.5f, 0);
                BulletManager.CreateBossDieAnimation(pos);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
