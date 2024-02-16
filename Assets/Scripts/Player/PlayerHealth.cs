using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace root
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private Image health;

        private float _playerHp;
        private bool oneTime = true;

        private void Awake()
        {
            _playerHp = GetComponent<Player>().GetHealth();
        }


        private void Update()
        {
            HealthSetup();
        }

        private void HealthSetup()
        {
            health.fillAmount = _playerHp/100;
        }
    }
}