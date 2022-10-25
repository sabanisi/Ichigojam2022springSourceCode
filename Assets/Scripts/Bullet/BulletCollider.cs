using UnityEngine;
using System.Collections;

public class BulletCollider : MonoBehaviour
{
    [SerializeField] private Bullet parent;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        parent.OnTriggerEnter2D(collision); 
    }
}
