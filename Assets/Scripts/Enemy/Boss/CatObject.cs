using UnityEngine;
using System.Collections;

public class CatObject : MonoBehaviour
{
    [SerializeField]private Cats parent;
    [SerializeField] private Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parent.OnTriggerEnter2D(collision);
    }

    public void Update()
    {
        if (GameManager.instance.IsGameOver() || GameManager.instance.IsPause())
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }
    }
}
