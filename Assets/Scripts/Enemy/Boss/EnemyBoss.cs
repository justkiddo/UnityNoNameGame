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
    private Vector3 _playerPos;
    private bool ftp = true;
    private float _startTime;
    private IPlayer _player;
    private float _speed = 6f;


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
        _playerPos = _player.GetCurrentPosition();
       Teleport();
    }


    private void Shoot()
    {
        var bullet = Instantiate(fireballPrefab, shootPoint.position, Quaternion.identity);
        
         var rb = bullet.GetComponent<Rigidbody2D>();
         rb.velocity = new Vector2(_playerPos.x,_playerPos.y) * (Time.deltaTime * _speed);

        //bullet.transform.position = Vector3.MoveTowards(shootPoint.position, direction, Time.deltaTime);
    }
    

    private void Teleport()
    {
        var tempTime = Time.time;
        if (ftp)
        {
            Random r = new Random();
            var n = r.Next(0, points.Capacity);
            transform.position = points[n].position;
            
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
