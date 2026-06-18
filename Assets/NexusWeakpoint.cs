using UnityEngine;

// ============================================================
// ZERO PULSE - NexusWeakpoint
// Ponto fraco do NEXUS (fase 3 do boss). Quando Kael encosta,
// registra um acerto no boss e se reposiciona/desativa.
//
// Prefab/objeto: Collider2D (Is Trigger) + este script.
// Arraste o NexusBoss da cena no campo "boss".
// Posicione varios deles na arena (ou reaproveite um que
// reaparece) para somar os "weakpointHits" da fase 3.
// ============================================================
public class NexusWeakpoint : MonoBehaviour
{
    [SerializeField] private NexusBoss boss;
    [SerializeField] private float respawnDelay = 0f; // 0 = some de vez

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        // So conta (e so e consumido) durante a fase de pontos fracos do boss
        if (boss == null || !boss.WeakpointPhaseActive) return;

        boss.HitWeakpoint();

        if (respawnDelay > 0f)
        {
            // Desativa e reativa depois (ponto fraco que pisca)
            gameObject.SetActive(false);
            Invoke(nameof(Reactivate), respawnDelay);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Reactivate()
    {
        gameObject.SetActive(true);
    }
}
