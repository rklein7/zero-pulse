using UnityEngine;

// ============================================================
// ZERO PULSE - SentinelLaser
// Projetil disparado pela Sentinela Voadora. Cai para baixo
// e mata Kael ao encostar. Autodestroi apos um tempo.
//
// Prefab: Collider2D (Is Trigger) + este script.
// ============================================================
public class SentinelLaser : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 7f;
    [SerializeField] private float lifeTime = 4f;
    [SerializeField] private string deathMessage = "ATINGIDO PELA SENTINELA";

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver(deathMessage);
        }
        // Some ao tocar o chao tambem
        Destroy(gameObject);
    }
}
