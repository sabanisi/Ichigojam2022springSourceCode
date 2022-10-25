using UnityEngine;
using System.Collections;

public abstract class SubUnit : MonoBehaviour
{
    protected bool isAlive;
    [SerializeField] private SpriteRenderer sprite;


    void Start()
    {
        isAlive = true;
        GameManager.SetSubUnits(gameObject);
    }


    void Update()
    {
        if (GameManager.instance.IsGameOver()) return;
        if (!GameManager.instance.IsGameStart()) return;
        if (GameManager.instance.IsPause()) return;
        if (isAlive)
        {
            Action();
        }
    }

    protected abstract void Action();

    public void DestroyDeal()
    {
        isAlive = false;
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        for(int i=0;i<=4; i++)
        {
            float a = sprite.color.a;
            while (a >= 0.01f)
            {
                a -= 0.1f;
                sprite.color = new Color(1, 1, 1, a);
                yield return null;
            }
            while (a <= 0.99f)
            {
                a += 0.1f;
                sprite.color = new Color(1, 1, 1, a);
                yield return null;
            }
        }
        float b = sprite.color.a;
        while (b >= 0.01f)
        {
            b -= 0.1f;
            sprite.color = new Color(1, 1, 1, b);
            yield return null;
        }
        yield return null;
        Destroy(gameObject);
    }
}
