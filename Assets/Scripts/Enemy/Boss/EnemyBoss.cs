using System;
using System.Collections;
using System.Collections.Generic;
using root;
using UnityEngine;
using Zenject;
using Random = System.Random;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] private List<Transform> points;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private BossTrigger _bossTrigger;

    private Vector3 _playerPos;
    private bool ftp = true;
    private float _startTime;
    private IPlayer _player;
    private float _speed = 2f;
    public bool startFight = false;
    

    [Inject]
    private void Construct(IPlayer player)
    {
        _player = player;
    }

    private void Awake()
    {
        _startTime = Time.time;
    }

    private void Update()
    {
        if (startFight)
        {
            Teleport();
        }
    }


    private void Shoot()
    {
        var direction = _player.GetCurrentPosition() - transform.position;
        var bullet = Instantiate(fireballPrefab, shootPoint.position, Quaternion.identity);
        var rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(direction.x,direction.y, direction.z) * _speed;
    }
    

    private void Teleport()
    {
        var tempTime = Time.time;
        if (ftp)
        {
            Random r = new Random();
            var n = r.Next(0, points.Capacity);
            transform.position = points[n].position;
            Shoot();
            ftp = false;
        }
        else
        {
            if (tempTime - _startTime > 3)
            {
                _startTime = Time.time;
                ftp = true;
            }
        }
    }
}
