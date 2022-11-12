using System.Collections;
using Managers;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;
    public float speed = 20f;
    public GameObject impactEffect;
    public GameObject smokeTrail;
    public int damage = 50;
    public float explosionRadius;
    public bool isRocket;

    private float _range;
    private float _timeCounter;

    public void SetRange(float r)
    {
        _range = r;
    }
    
    public void Seek(Transform target)
    {
        _target = target;
    }

    public void Update()
    {
        if (_target == null && !isRocket)
        {
            PhotonNetwork.Destroy(gameObject);
            return;
        }
        
        if (_target == null && isRocket)
        {
            FindNewEnemy();
            CircleRocket();
            return;
        }
        
        float distanceThisFrame = speed * Time.deltaTime;
        Vector3 direction = _target.position - transform.position;
        
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.LookAt(_target);
        transform.Rotate(0, 90, 0);
    }

    private void CircleRocket()
    {
        transform.Translate(-0.1f, 0, 0);
        transform.Rotate(0, 1f, 0);
    }

    private void FindNewEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _range);
        foreach (Collider c in colliders)
            if (c.CompareTag("Enemy"))
                _target = c.transform;
        
    }

    private void HitTarget()
    {
        GameObject effectIns = PhotonNetwork.Instantiate(impactEffect.name, transform.position, transform.rotation);

        if (explosionRadius > 0f)
            Explode();
        
        else 
            Damage(_target);

        if(isRocket)
        {
            smokeTrail.transform.parent = null;
            smokeTrail.GetComponent<ParticleSystem>().Stop();
        }

        CoroutineManager.Instance.DestroyGameObject(effectIns, 5f);
        PhotonNetwork.Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Enemy"))
            {
                Damage(c.transform);
            }
        }
    }

    private void Damage(Transform enemy)
    {
        Enemy.Enemy e = enemy.GetComponent<Enemy.Enemy>();
        if (e != null)
            e.TakeDamage(damage);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}