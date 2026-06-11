using UnityEngine;

// ============================================================
// ZERO PULSE - FinishLine
// Checkpoint final do segmento: dispara a vitoria.
// Requer Collider2D com "Is Trigger" MARCADO.
// ============================================================
public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.Win();
    }
}