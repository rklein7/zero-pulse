using UnityEngine;

// ============================================================
// ZERO PULSE - EnergyPickup
// Celula de energia: reabastece o transmissor ao tocar.
// Requer Collider2D com "Is Trigger" MARCADO.
// O Player precisa ter a Tag "Player".
// ============================================================
public class EnergyPickup : MonoBehaviour
{
    [SerializeField] private float amount = 15f;
    [SerializeField] private AudioClip collectSfx; // som de coleta (opcional)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.AddEnergy(amount);

        if (collectSfx != null)
            AudioSource.PlayClipAtPoint(collectSfx, transform.position, 0.8f);

        Destroy(gameObject);
    }
}