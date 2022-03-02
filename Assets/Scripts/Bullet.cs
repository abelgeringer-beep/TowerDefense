using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;
    public float speed = 20f;
    public GameObject impactEffect;
    public int damage = 50;
    public float explosionRadius = 0f;

    public void Seek(Transform target)
    {
        _target = target;
    }

    public void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = _target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.LookAt(_target);
    }

    private void HitTarget()
    {
        GameObject effectIns =  PhotonNetwork.IsConnected && PhotonNetwork.InRoom 
            ? PhotonNetwork.Instantiate(impactEffect.name, transform.position, transform.rotation)
            : Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(_target);
        }

        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        // gets all the objects within the radius of the position
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy")) // if the collider has a tag named Enemy, than damage it
            {
                Damage(collider.transform);
            }
        }
    }

    private void Damage(Transform enemy)
    {
        Enemy.Enemy e = enemy.GetComponent<Enemy.Enemy>();
        if (e != null)
        {
            e.TakeDamage(damage);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}