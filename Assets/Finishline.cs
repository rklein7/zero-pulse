using UnityEngine;

// ============================================================
// ZERO PULSE - FinishLine
// Fim de fase. Nas fases 1-4: carrega a proxima fase.
// Na ultima fase (Is Final Level marcado): vitoria final.
//
// Requer Collider2D com "Is Trigger" MARCADO.
// IMPORTANTE: todas as cenas (Fase1..Fase5) precisam estar na
// Scene List (File > Build Profiles), na ordem certa, senao o
// carregamento da proxima fase nao funciona.
// ============================================================
public class FinishLine : MonoBehaviour
{
    [Header("Fluxo de fases")]
    [SerializeField] private bool isFinalLevel = false;   // marcar SO na Fase 5
    [SerializeField] private string nextSceneName = "";   // vazio = proxima da Scene List

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance == null) return;

        if (isFinalLevel)
            GameManager.Instance.Win();
        else
            GameManager.Instance.LevelComplete(nextSceneName);
    }
}
