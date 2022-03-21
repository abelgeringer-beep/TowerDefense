using Photon.Pun;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Enemy.Enemy _targetEnemy;
    private float _fireCountdown;

    [Header("General")] public float range = 20f;
    public LineRenderer shootingDistance;

    [Header("Use Bullets")] public GameObject bulletPrefab;
    public float fireRate = 1f;

    [Header("Use Leaser")] public bool useLeaser;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;
    public int damageOverTime = 30;
    public float slowPercentage = .5f;

    [Header("Unity Setup Fields")] public Transform target;
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;

    public Transform firePoint;

    public void Start()
    {
        if(shootingDistance)
            shootingDistance.enabled = false;
        
        DrawShootingDistance();
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    public void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            _targetEnemy = target.GetComponent<Enemy.Enemy>();
        }
        else
        {
            target = null;
        }
    }

    private void LockOnTarget()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void Leaser()
    {
        _targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        _targetEnemy.Slow(slowPercentage);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 direction = firePoint.position - target.position;
        impactEffect.transform.position = target.position + direction.normalized;
        impactEffect.transform.rotation = Quaternion.LookRotation(direction);
    }

    private void Update()
    {
        if (target == null)
        {
            if (useLeaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }

            return;
        }

        LockOnTarget();

        if (useLeaser)
        {
            Leaser();
        }
        else
        {
            if (_fireCountdown <= 0f)
            {
                Shoot();
                _fireCountdown = 1f / fireRate;
            }

            _fireCountdown -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        GameObject bulletGo = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGo.GetComponent<Bullet>();

        if (bullet == null) return;
        
        if (bullet.isRocket)
            bullet.SetRange(range);
        
        bullet.Seek(target);
    }

    private void OnMouseEnter()
    {
        shootingDistance.enabled = true;
    }

    private void OnMouseExit()
    {
        shootingDistance.enabled = false;
    }

    private void DrawShootingDistance()
    {
        const short steps = 150;
        shootingDistance.positionCount = steps;

        for (short i = 0; i < steps; i++)
        {
            float circumferenceProgress = (float) i / steps;
            float currentRadiant = circumferenceProgress * 2 * Mathf.PI;

            Vector3 currentPosition = new Vector3(
                Mathf.Cos(currentRadiant) * range,
                0,
                Mathf.Sin(currentRadiant) * range
            );
            currentPosition.y += 0.5f;
            shootingDistance.SetPosition(i, currentPosition + transform.position);
        }
    }
}