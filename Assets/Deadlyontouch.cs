using UnityEngine;

// ============================================================
// ZERO PULSE - DeadlyOnTouch
// Mata Kael ao encostar. Use em: obstaculos, canos baixos,
// inimigos e na KillZone (abismo).
// Requer Collider2D com "Is Trigger" MARCADO.
// ============================================================
public class DeadlyOnTouch : MonoBehaviour
{
    [SerializeField] private string deathMessage = "FIM DA CORRIDA";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance != null)
            GameManager.Instance.GameOver(deathMessage);
    }
}