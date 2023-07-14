using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace root
{
    public class PlayerHealth : MonoBehaviour
    {
        private IPlayer _player;
        [SerializeField] private Image health;
        
        [Inject]
        private void Construct(IPlayer player)
        {
            _player = player;
        }


        private void Update()
        {
            HealthSetup();
        }

        private void HealthSetup()
        {
            var playerHp = _player.GetHealth();
            health.fillAmount = playerHp/100;
        }
    }
}