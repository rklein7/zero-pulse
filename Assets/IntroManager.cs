using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    [System.Serializable]
    public class Slide
    {
        public string titulo;
        [TextArea(2, 6)] public string texto;
        public Sprite imagem;

        [Header("Kael")]
        public bool mostrarKael = false;           // ativa/desativa por slide
        public Vector2 posicaoKael = Vector2.zero; // posição no Canvas
        public bool kaelEspelhado = false;         // vira o sprite (sai pela esquerda)
    }

    [Header("Slides da historia")]
    [SerializeField] private Slide[] slides;

    [Header("Referencias de UI")]
    [SerializeField] private TextMeshProUGUI textoUI;
    [SerializeField] private TextMeshProUGUI tituloUI;
    [SerializeField] private Image imagemUI;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Kael")]
    [SerializeField] private RectTransform kaelTransform; // arraste o objeto do Kael aqui
    [SerializeField] private float kaelMoveDuration = 0.4f;

    [Header("Comportamento")]
    [SerializeField] private string proximaCena = "Fase1";
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float autoAvancar = 0f;
    [SerializeField] private float delayFinal = 1f;

    private int indice = -1;
    private bool transicionando;
    private float timer;

    private void Start()
    {
        Time.timeScale = 1f;

        // esconde o Kael no início
        if (kaelTransform != null)
            kaelTransform.gameObject.SetActive(false);

        StartCoroutine(ProximoSlide());
    }

    private void Update()
    {
        if (transicionando) return;

        if (Input.GetKeyDown(KeyCode.Escape)) { CarregarJogo(); return; }

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

        if (indice >= 0 && canvasGroup != null)
            yield return Fade(1f, 0f);

        indice++;
        if (slides == null || indice >= slides.Length)
        {
            yield return new WaitForSeconds(delayFinal);
            CarregarJogo();
            yield break;
        }

        Slide s = slides[indice];

        // textos
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

        // Kael
        if (kaelTransform != null)
        {
            if (s.mostrarKael)
            {
                kaelTransform.gameObject.SetActive(true);

                // espelha o sprite se necessário
                Vector3 scale = kaelTransform.localScale;
                scale.x = s.kaelEspelhado ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
                kaelTransform.localScale = scale;

                // move suavemente para a posição do slide
                StartCoroutine(MoverKael(s.posicaoKael));
            }
            else
            {
                kaelTransform.gameObject.SetActive(false);
            }
        }

        if (canvasGroup != null) yield return Fade(0f, 1f);

        transicionando = false;
    }

    private IEnumerator MoverKael(Vector2 destino)
    {
        Vector2 origem = kaelTransform.anchoredPosition;
        float t = 0f;

        while (t < kaelMoveDuration)
        {
            t += Time.deltaTime;
            kaelTransform.anchoredPosition = Vector2.Lerp(origem, destino, t / kaelMoveDuration);
            yield return null;
        }

        kaelTransform.anchoredPosition = destino;
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