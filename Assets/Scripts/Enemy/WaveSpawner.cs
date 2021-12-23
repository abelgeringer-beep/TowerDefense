using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class WaveSpawner : MonoBehaviour
    {
        public static int EnemiesAlive;

        public Wave[] waves;

        public Transform spawnPoint;

        public float timeBetweenWaves = 5f;
        private float _countdown = 2f;
        public Text waveCount;

        public Text waveCountdownText;

        public GameMaster gameMaster;

        private int _waveIndex;

        private void Start()
        {
            _waveIndex = 0;
            EnemiesAlive = 0;
            Time.timeScale = 1f;
        }

        private void Update()
        {
            if (EnemiesAlive > 0)
            {
                return;
            }

            if (_waveIndex == waves.Length)
            {
                gameMaster.WinLevel();
                this.enabled = false;
            }

            if (_countdown <= 0f)
            {
                StartCoroutine(SpawnWave());
                _countdown = timeBetweenWaves;
                return;
            }

            _countdown -= Time.deltaTime;

            _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);

            waveCountdownText.text = $"{_countdown:00.0}";
        }

        private IEnumerator SpawnWave()
        {
            PlayerStats.Rounds++;
            waveCount.text = (_waveIndex + 1).ToString();

            var wave = waves[_waveIndex];

            EnemiesAlive = wave.count;

            var i = 0;
            for (; i < wave.count; i++)
            {
                SpawnEnemy(wave.enemy);
                yield return new WaitForSeconds(1f / wave.rate);
            }

            _waveIndex++;
        }

        private void SpawnEnemy(GameObject enemy)
        {
            Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        }
    }
}