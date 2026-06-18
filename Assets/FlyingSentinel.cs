using UnityEngine;

// ============================================================
// ZERO PULSE - FlyingSentinel ("Sentinela Voadora", GDD secao 08)
// Zonas 3-5. Patrulha uma area no alto, flutuando, e dispara
// projeteis para baixo em intervalos. Robotica.
// Como superar (design): timing de movimento, usar cobertura.
//
// Prefab: Collider2D (Is Trigger) + este script.
// Precisa de um prefab de projetil (ver SentinelLaser) arrastado
// no campo "laserPrefab".
// ============================================================
public class FlyingSentinel : EnemyBase
{
    [Header("Sentinela Voadora")]
    [SerializeField] private float patrolSpeed = 1.5f;   // deslocamento horizontal lento
    [SerializeField] private float bobAmplitude = 0.4f;  // flutuacao vertical
    [SerializeField] private float bobSpeed = 2f;

    [Header("Disparo")]
    [SerializeField] private GameObject laserPrefab;     // projetil (opcional)
    [SerializeField] private float fireInterval = 2f;    // segundos entre disparos
    [SerializeField] private Transform firePoint;        // de onde sai o tiro (opcional)

    private Vector3 startPos;
    private float fireTimer;

    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
        fireTimer = fireInterval;
    }

    protected override void Move()
    {
        // Deriva lenta para a esquerda + flutuacao senoidal
        startPos += Vector3.left * patrolSpeed * Time.deltaTime;
        float bob = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        transform.position = startPos + Vector3.up * bob;

        // Disparo periodico
        if (laserPrefab == null) return;
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            fireTimer = fireInterval;
            Vector3 spawn = firePoint != null ? firePoint.position : transform.position;
            Instantiate(laserPrefab, spawn, Quaternion.identity);
        }
    }
}
