using System.Collections;
using Managers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public static string Name;
        public float startSpeed;

        [HideInInspector] public float speed;

        public float startHealth;
        private float _health;

        public int worth;

        public GameObject deathEffect;

        [Header("Unity Stuff")]
        public Image healthBar;

        private bool _isDead;

        void Start()
        {
            speed = startSpeed;
            _health = startHealth;
        }

        public void TakeDamage(float amount)
        {
            _health -= amount;

            healthBar.fillAmount = _health / startHealth;

            if (_health <= 0 && !_isDead)
            {
                Die();
            }
        }

        public void Slow(float pct)
        {
            speed = startSpeed * (1f - pct);
        }

        private void Die()
        {
            _isDead = true;

            PlayerStats.Money += worth;

            WaveSpawner.EnemiesAlive--;

            CoroutineManager.Instance.DestroyGameObject(deathEffect, 5f);
            
            PhotonNetwork.Destroy(gameObject);
        }
    }
}