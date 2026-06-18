using UnityEngine;

// ============================================================
// ZERO PULSE - EnemyBase
// Classe-mae de todos os inimigos. Cuida do que e comum:
//   - matar Kael ao encostar (via trigger)
//   - autodestruir quando fica muito atras da camera (performance)
// Cada inimigo concreto herda desta classe e implementa Move().
//
// COMO USAR um inimigo novo:
//   1. Crie a classe herdando de EnemyBase e implemente Move().
//   2. No prefab: Collider2D com Is Trigger MARCADO + o script do inimigo.
//   3. NAO precisa mais do DeadlyOnTouch nos inimigos: a morte ja
//      esta embutida aqui (deathMessage configuravel no Inspector).
// ============================================================
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Inimigo (comum)")]
    [SerializeField] protected string deathMessage = "INFECTADO TE PEGOU";
    [SerializeField] private float despawnBehindCamera = 25f;

    private Transform cam;

    protected virtual void Start()
    {
        if (Camera.main != null) cam = Camera.main.transform;
    }

    // Cada inimigo define seu proprio movimento aqui.
    protected abstract void Move();

    private void Update()
    {
        Move();
        DespawnIfBehind();
    }

    private void DespawnIfBehind()
    {
        if (cam == null) return;
        if (transform.position.x < cam.position.x - despawnBehindCamera)
            Destroy(gameObject);
    }

    // Morte ao encostar em Kael - comum a todos os inimigos.
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance != null)
            GameManager.Instance.GameOver(deathMessage);
    }
}
