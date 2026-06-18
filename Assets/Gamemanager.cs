using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// ============================================================
// ZERO PULSE - GameManager
// Mecanica central do GDD: o Pulso de Transmissao.
// A energia drena com o tempo; zerou = game over.
// Tambem controla morte, vitoria e reinicio da fase.
// Anexar a um GameObject vazio chamado "GameManager".
// ============================================================
public class GameManager : MonoBehaviour
{
    // Singleton: outros scripts acessam via GameManager.Instance
    public static GameManager Instance { get; private set; }

    [Header("Transmissor (energia)")]
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float drainPerSecond = 6f;

    [Header("Referencias (arrastar no Inspector)")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Slider energyBar;          // HUD: barra de energia
    [SerializeField] private TextMeshProUGUI endMessage; // texto central, desativado por padrao

    [Header("Audio")]
    [SerializeField] private AudioClip deathSfx; // som de morte (opcional)
    [SerializeField] private AudioClip winSfx;   // som de vitoria (opcional)

    private float energy;
    private bool gameEnded;

    private void Awake()
    {
        // Protege contra dois GameManagers na cena (duplicata acidental)
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Ja existe um GameManager na cena. Destruindo a duplicata em " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        energy = maxEnergy;
        Time.timeScale = 1f; // garante que o jogo nao comeca congelado
    }

    private void Update()
    {
        if (gameEnded)
        {
            // R reinicia a fase apos vitoria ou derrota
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                Reload();
            }
            return;
        }

        // Drenagem continua do transmissor
        energy -= drainPerSecond * Time.deltaTime;
        if (energyBar != null) energyBar.value = energy / maxEnergy;

        if (energy <= 0f) GameOver("SINAL PERDIDO");
    }

    // Chamado pelas celulas de energia (EnergyPickup)
    public void AddEnergy(float amount)
    {
        energy = Mathf.Min(energy + amount, maxEnergy);
    }

    // Chamado por hazards, queda ou energia zerada
    public void GameOver(string reason)
    {
        if (gameEnded) return;
        gameEnded = true;
        if (player != null) player.Die();

        if (deathSfx != null && player != null)
            AudioSource.PlayClipAtPoint(deathSfx, player.transform.position, 0.8f);

        ShowMessage(reason + "\n<size=40%>reiniciando...</size>");
        Invoke(nameof(Reload), 1.4f);
    }

    // Chamado pela FinishLine na ULTIMA fase (vitoria final do jogo)
    public void Win()
    {
        if (gameEnded) return;
        gameEnded = true;
        DoWin();
    }

    private void DoWin()
    {
        if (winSfx != null && player != null)
            AudioSource.PlayClipAtPoint(winSfx, player.transform.position, 0.9f);

        ShowMessage("TRANSMISSAO ENVIADA\n<size=40%>aperte R para correr de novo</size>");
        Time.timeScale = 0f; // congela o jogo na tela de vitoria
    }

    // Chamado pela FinishLine nas fases 1-4: anuncia a conclusao
    // e carrega a proxima fase.
    public void LevelComplete(string nextSceneName)
    {
        if (gameEnded) return;
        gameEnded = true;
        pendingNextScene = nextSceneName;

        if (winSfx != null && player != null)
            AudioSource.PlayClipAtPoint(winSfx, player.transform.position, 0.9f);

        ShowMessage("ZONA CONCLUIDA\n<size=40%>carregando a proxima...</size>");
        Invoke(nameof(LoadNextLevel), 1.6f);
    }

    private string pendingNextScene;

    private void LoadNextLevel()
    {
        Time.timeScale = 1f;

        // Se a FinishLine especificou um nome de cena, usa ele
        if (!string.IsNullOrEmpty(pendingNextScene))
        {
            SceneManager.LoadScene(pendingNextScene);
            return;
        }

        // Senao, carrega a proxima da Scene List (ordem do Build Profiles)
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            DoWin(); // nao existe proxima fase: trata como vitoria final
    }

    private void ShowMessage(string msg)
    {
        if (endMessage == null) return;
        endMessage.text = msg;
        endMessage.gameObject.SetActive(true);
    }

    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}