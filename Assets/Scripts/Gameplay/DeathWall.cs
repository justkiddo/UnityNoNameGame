using System;
using System.Collections;
using System.Collections.Generic;
using root;
using UnityEngine;
using Zenject;

public class DeathWall : MonoBehaviour
{
    [SerializeField] private Player _player;



    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") )
        {
            _player.TakeDamage(10000);
        }
    }
}
