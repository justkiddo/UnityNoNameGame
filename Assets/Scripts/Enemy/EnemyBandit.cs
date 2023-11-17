using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace root
{
    public class EnemyBandit : MonoBehaviour
    {
        private EnemyState _enemyState;
        private enum EnemyState
        {
            Patrolling,
            Following,
            Attacking,
            Returning
        }

        private void Awake()
        {
            _enemyState = EnemyState.Patrolling;
        }

        private void Update()
        {
            StateCheck();
            DistanceCheck();
        }

        private void DistanceCheck()
        {
            
        }

        private void StateCheck()
        {
            if (_enemyState == EnemyState.Patrolling)
            {
                
            }else if (_enemyState == EnemyState.Following)
            {
                
            }else if (_enemyState == EnemyState.Attacking)
            {
                
            }
            else if(_enemyState == EnemyState.Returning)
            {
                
            }
            else
            {
                
            }
            
        }
        
    }
}