using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Enemy
{
public class WaveGenerator : MonoBehaviour
{
    private int _currentWaveCount;
    private float _countdown;
    
    public GameObject spawnPoint;

    [Header("Difficulty")]
    [Range(1,10)]
    public float delayBetweenWaves;

    [Header("Wave counter text")]
    public TextMeshProUGUI waveCounterText;
    public TextMeshProUGUI waveCountText;

    [Header("Enemy prefabs")]
    public GameObject standardEnemy;
    public GameObject fastEnemy;
    public GameObject toughEnemy;
    public GameObject boss;

    private IEnumerator SpawnWave()
    {
        if(_currentWaveCount % 10 != 0)
            for (int i = 0; i < _currentWaveCount * 1.5; i++)
            {
                PhotonNetwork.Instantiate(standardEnemy.name, spawnPoint.transform.position, spawnPoint.transform.rotation);
                yield return new WaitForSeconds(0.5f);
            }

        if(_currentWaveCount > 3 && _currentWaveCount % 10 != 0)
            for(int i = 0; i < _currentWaveCount * 0.8; i++)
            {
                PhotonNetwork.Instantiate(fastEnemy.name, spawnPoint.transform.position, spawnPoint.transform.rotation);
                yield return new WaitForSeconds(0.5f);
            }

        if(_currentWaveCount > 4 && _currentWaveCount % 10 != 0)
            for(int i = 0; i < _currentWaveCount * 0.7; i++)
            {
                PhotonNetwork.Instantiate(toughEnemy.name, spawnPoint.transform.position, spawnPoint.transform.rotation);
                yield return new WaitForSeconds(0.5f);
            }
        
        if(_currentWaveCount % 10 == 0)
            for(int i = 0; i < _currentWaveCount / 5; i++)
            {
                PhotonNetwork.Instantiate(boss.name, spawnPoint.transform.position, spawnPoint.transform.rotation);
                yield return new WaitForSeconds(0.5f);
            }
        
        _countdown = delayBetweenWaves;
        
        yield return new WaitForSeconds(delayBetweenWaves);
    }

    private int GetEnemiesAlive()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void Update()
    {
        if (GetEnemiesAlive() > 0 && _countdown >= 0f || spawnPoint == null) 
            return;

        waveCounterText.text = (++_currentWaveCount).ToString();
        StartCoroutine(SpawnWave());
        
        _countdown -= Time.deltaTime;
        _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);
        waveCountText.text = $"next wave in: {_countdown:00.0}";
    }

    private void Start()
    {
        _currentWaveCount = 0;
        _countdown = delayBetweenWaves;
        waveCounterText.text = _currentWaveCount.ToString();
    }
}
}