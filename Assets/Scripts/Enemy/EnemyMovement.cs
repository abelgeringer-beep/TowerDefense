using Photon.Pun;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyMovement : MonoBehaviour
    {
        private Transform _target;

        private int _wavePointIndex;

        private Enemy _enemy;

        private void Start()
        {
            _enemy = GetComponent<Enemy>();
            _target = WayPoints.Points[0];
        }

        private void Update()
        {
            var direction = _target.position - transform.position;
            transform.Translate(direction.normalized * (_enemy.speed * Time.deltaTime),  Space.World);

            if (Vector3.Distance(transform.position, _target.position) <= 0.4f)
            {
                GetNextWayPoint();
            }

            _enemy.speed = _enemy.startSpeed;
        }

        private void GetNextWayPoint()
        {
            if (_wavePointIndex >= WayPoints.Points.Length - 1)
            {
                EndPath();
                return;
            }

            _wavePointIndex++;
            _target = WayPoints.Points[_wavePointIndex];
        }

        private void EndPath()
        {
            PlayerStats.Lives--;
            WaveSpawner.EnemiesAlive--;
            PhotonNetwork.Destroy(gameObject);
        }
    }
}