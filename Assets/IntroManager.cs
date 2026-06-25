using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

// ============================================================
// ZERO PULSE - IntroManager
// Mostra quadros de historia (slides) um a um antes da Fase1.
// Avanca no clique / Espaco / Enter. ESC pula tudo.
// Cada slide tem texto, e opcionalmente um titulo e uma imagem.
// Faz fade entre os quadros usando um CanvasGroup.
//
// COMO USAR (resumo): cena "Intro" com Canvas:
//   Canvas
//   ├── Fundo (Image escuro)
//   ├── Conteudo (CanvasGroup)   <- arraste no campo canvasGroup
//   │   ├── TituloText (TMP, opcional)
//   │   ├── ImagemUI   (Image, opcional)
//   │   └── TextoUI    (TMP)
//   ├── DicaText (TMP: "clique para continuar - ESC pula")
//   └── IntroManager (este script)
// ============================================================
public class IntroManager : MonoBehaviour
{
    [System.Serializable]
    public class Slide
    {
        public string titulo;                 // opcional
        [TextArea(2, 6)] public string texto; // narracao
        public Sprite imagem;                 // opcional
    }

    [Header("Slides da historia")]
    [SerializeField] private Slide[] slides;

    [Header("Referencias de UI")]
    [SerializeField] private TextMeshProUGUI textoUI;
    [SerializeField] private TextMeshProUGUI tituloUI;  // opcional
    [SerializeField] private Image imagemUI;            // opcional
    [SerializeField] private CanvasGroup canvasGroup;   // pro fade (obrigatorio p/ fade)

    [Header("Comportamento")]
    [SerializeField] private string proximaCena = "Fase1";
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float autoAvancar = 0f;    // 0 = so manual; >0 = segundos por slide
    [SerializeField] private float delayFinal = 1f;     // pausa (tela escura) apos o ultimo slide antes de carregar o jogo

    private int indice = -1;
    private bool transicionando;
    private float timer;

    private void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(ProximoSlide());
    }

    private void Update()
    {
        if (transicionando) return;

        if (Input.GetKeyDown(KeyCode.Escape)) { CarregarJogo(); return; } // pular

        bool avancar = Input.GetMouseButtonDown(0)
                    || Input.GetKeyDown(KeyCode.Space)
                    || Input.GetKeyDown(KeyCode.Return);

        if (autoAvancar > 0f)
        {
            timer += Time.deltaTime;
            if (timer >= autoAvancar) avancar = true;
        }

        if (avancar) StartCoroutine(ProximoSlide());
    }

    private IEnumerator ProximoSlide()
    {
        transicionando = true;
        timer = 0f;

        // fade out do slide atual (se houver)
        if (indice >= 0 && canvasGroup != null)
            yield return Fade(1f, 0f);

        indice++;
        if (slides == null || indice >= slides.Length)
        {
            // ultimo slide ja deu fade out acima: segura na tela escura antes de comecar
            yield return new WaitForSeconds(delayFinal);
            CarregarJogo();
            yield break;
        }

        // aplica o conteudo do slide
        Slide s = slides[indice];
        if (textoUI != null) textoUI.text = s.texto;

        if (tituloUI != null)
        {
            tituloUI.text = s.titulo;
            tituloUI.gameObject.SetActive(!string.IsNullOrEmpty(s.titulo));
        }
        if (imagemUI != null)
        {
            imagemUI.sprite = s.imagem;
            imagemUI.gameObject.SetActive(s.imagem != null);
        }

        // fade in
        if (canvasGroup != null) yield return Fade(0f, 1f);
        else if (canvasGroup != null) canvasGroup.alpha = 1f;

        transicionando = false;
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    private void CarregarJogo()
    {
        SceneManager.LoadScene(proximaCena);
    }
}
