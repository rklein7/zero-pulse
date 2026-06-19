using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// ============================================================
// ZERO PULSE - MenuManager
// Controla o menu principal e o painel de tutorial.
// Anexar a um GameObject vazio chamado "MenuManager" na cena Menu.
//
// Estrutura de UI esperada no Canvas:
//   Canvas
//   ├── PainelMenu       (tela inicial)
//   │   ├── TituloText   (TextMeshProUGUI)
//   │   ├── SubtituloText(TextMeshProUGUI)  <- opcional
//   │   ├── BotaoJogar   (Button)
//   │   └── BotaoTutorial(Button)
//   └── PainelTutorial   (tela de tutorial, desativada por padrão)
//       ├── TutorialText (TextMeshProUGUI)
//       └── BotaoIniciar (Button)
// ============================================================
public class MenuManager : MonoBehaviour
{
    [Header("Paineis")]
    [SerializeField] private GameObject painelMenu;
    [SerializeField] private GameObject painelTutorial;

    [Header("Botoes do Menu")]
    [SerializeField] private Button botaoJogar;
    [SerializeField] private Button botaoTutorial;

    [Header("Botao do Tutorial")]
    [SerializeField] private Button botaoIniciar;

    [Header("Cena para carregar")]
    [SerializeField] private string primeiraFase = "Fase1"; // nome da cena da Fase1

    private void Awake()
    {
        // Garante estado inicial correto
        if (painelMenu != null)     painelMenu.SetActive(true);
        if (painelTutorial != null) painelTutorial.SetActive(false);
    }

    private void Start()
    {
        // Conecta os botoes
        if (botaoJogar != null)    botaoJogar.onClick.AddListener(Jogar);
        if (botaoTutorial != null) botaoTutorial.onClick.AddListener(AbrirTutorial);
        if (botaoIniciar != null)  botaoIniciar.onClick.AddListener(Jogar);

        Time.timeScale = 1f; // garante que nao esta pausado por morte/vitoria anterior
    }

    public void Jogar()
    {
        SceneManager.LoadScene(primeiraFase);
    }

    public void AbrirTutorial()
    {
        if (painelMenu != null)     painelMenu.SetActive(false);
        if (painelTutorial != null) painelTutorial.SetActive(true);
    }

    public void VoltarAoMenu()
    {
        if (painelMenu != null)     painelMenu.SetActive(true);
        if (painelTutorial != null) painelTutorial.SetActive(false);
    }
}
