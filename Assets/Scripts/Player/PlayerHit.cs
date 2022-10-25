using System;
using UnityEngine;

public class PlayerHit:MonoBehaviour
{
    [SerializeField]private Player _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player.OnTriggerEnter2D(collision);
    }
}
