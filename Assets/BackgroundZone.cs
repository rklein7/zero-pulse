using UnityEngine;

// ============================================================
// ZERO PULSE - BackgroundZone
// Liga/desliga este Background com base na posicao X do player.
// Use em CADA contêiner de fundo (Background da Zona 1,
// Background_Z2, etc).
//
// O fundo so renderiza quando o jogador esta entre activeFromX
// e activeUntilX, com uma "margem" para a transicao ser suave.
//
// COMO USAR:
//   1. Anexe a cada GameObject de Background (o pai das camadas).
//   2. Configure os X de inicio e fim da zona daquele fundo.
//   3. Arraste o Player no campo "player".
//
// Resultado: ao mudar de zona, o fundo antigo desaparece e o
// novo aparece, sem cidade fantasma no fundo da floresta.
// ============================================================
public class BackgroundZone : MonoBehaviour
{
    [Header("Quando este fundo deve aparecer")]
    [SerializeField] private float activeFromX = 0f;
    [SerializeField] private float activeUntilX = 70f;
    [SerializeField] private float fadeMargin = 5f; // margem suave nas bordas

    [Header("Referencia")]
    [SerializeField] private Transform player;

    private SpriteRenderer[] renderers;

    private void Awake()
    {
        // Pega todos os SpriteRenderers das camadas filhas
        renderers = GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void Start()
    {
        // Se o player nao foi setado, tenta achar pela tag
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    private void LateUpdate()
    {
        if (player == null || renderers == null) return;

        float px = player.position.x;
        float alpha;

        if (px < activeFromX - fadeMargin || px > activeUntilX + fadeMargin)
        {
            alpha = 0f; // fora da zona: invisivel
        }
        else if (px < activeFromX)
        {
            // entrada suave (fade in)
            alpha = (px - (activeFromX - fadeMargin)) / fadeMargin;
        }
        else if (px > activeUntilX)
        {
            // saida suave (fade out)
            alpha = 1f - (px - activeUntilX) / fadeMargin;
        }
        else
        {
            alpha = 1f; // dentro da zona: totalmente visivel
        }

        alpha = Mathf.Clamp01(alpha);

        // Aplica em todos os renderers filhos preservando a cor
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] == null) continue;
            Color c = renderers[i].color;
            c.a = alpha;
            renderers[i].color = c;
        }
    }
}
