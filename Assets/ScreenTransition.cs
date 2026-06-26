using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// ============================================================
// ZERO PULSE - ScreenTransition
// Tela cheia (imagem) entre fases / final do jogo.
// Mostra a tela e, no clique (ou tempo), carrega a proxima cena.
// Por padrao vai para a PROXIMA cena da Build List (buildIndex+1),
// entao basta ordenar a Build List corretamente.
//
// COMO USAR: cena com Canvas + Image (tela cheia) + este script.
//   - proximaCena vazio  -> proxima da Build List
//   - na tela FINAL, ponha proximaCena = "Menu" (ou deixe vazio:
//     sendo a ultima cena, ele volta sozinho ao Menu, indice 0).
// ============================================================
public class ScreenTransition : MonoBehaviour
{
    [Header("Destino")]
    [SerializeField] private string proximaCena = "";   // vazio = proxima cena da Build List

    [Header("Tempo")]
    [SerializeField] private float tempoMinimo = 0.6f;   // segura antes de aceitar clique (evita pular sem querer)
    [SerializeField] private float autoAvancar = 0f;     // 0 = so no clique; >0 = avanca sozinho apos N segundos

    [Header("Fade in (opcional)")]
    [SerializeField] private CanvasGroup canvasGroup;    // arraste um CanvasGroup pra ter fade de entrada
    [SerializeField] private float fadeIn = 0.4f;

    private float t;
    private bool saindo;

    private void Start()
    {
        Time.timeScale = 1f;
        if (canvasGroup != null) StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float f = 0f;
        canvasGroup.alpha = 0f;
        while (f < fadeIn)
        {
            f += Time.deltaTime;
            canvasGroup.alpha = f / fadeIn;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private void Update()
    {
        if (saindo) return;

        t += Time.deltaTime;
        if (t < tempoMinimo) return;

        bool avancar = Input.GetMouseButtonDown(0)
                    || Input.GetKeyDown(KeyCode.Space)
                    || Input.GetKeyDown(KeyCode.Return);

        if (autoAvancar > 0f && t >= autoAvancar) avancar = true;

        if (avancar) Avancar();
    }

    private void Avancar()
    {
        saindo = true;

        if (!string.IsNullOrEmpty(proximaCena))
        {
            SceneManager.LoadScene(proximaCena);
            return;
        }

        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            SceneManager.LoadScene(0); // ultima cena -> volta ao Menu
    }
}
