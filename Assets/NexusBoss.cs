using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

// ============================================================
// ZERO PULSE - NexusBoss
// Boss final (GDD secao 08): IA NEXUS com multiplas fases.
// "fases multiplas: corrida + puzzle + confronto final".
//
// ARQUITETURA DEGRADAVEL: o boss roda quantas fases voce
// configurar no array "phases". 3 fases = experiencia completa,
// mas funciona perfeitamente com 2 ou ate 1 se o tempo apertar.
// Cada fase reaproveita inimigos ja existentes (Sentinela, Drone).
//
// COMO USAR:
//   1. Anexe a um objeto "NexusBoss" no fim do level (a arena).
//   2. Configure o array "phases" no Inspector.
//   3. Arraste os spawn points e os prefabs de cada fase.
//   4. O boss inicia quando Kael cruza o "triggerX".
//   5. Ao vencer todas as fases, chama GameManager.Win().
// ============================================================
public class NexusBoss : MonoBehaviour
{
    public enum PhaseType { LaserBarrage, DroneSwarm, Weakpoint }

    [System.Serializable]
    public class Phase
    {
        public string phaseName = "Fase";
        public PhaseType type = PhaseType.LaserBarrage;
        public float duration = 12f;        // quanto a fase dura (Laser/Drone)
        public float spawnInterval = 1.2f;  // tempo entre spawns
        public GameObject spawnPrefab;      // laser, drone, etc. para esta fase
        public int weakpointHits = 3;       // so para a fase Weakpoint
    }

    [Header("Ativacao")]
    [SerializeField] private float triggerX = 200f;     // X em que o boss comeca
    [SerializeField] private PlayerController player;

    [Header("Arena")]
    [SerializeField] private Transform[] spawnPoints;   // pontos no alto da arena
    [SerializeField] private float arenaWidth = 16f;

    [Header("Fases (configure 1 a 3)")]
    [SerializeField] private Phase[] phases;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI bossBanner;   // anuncia cada fase
    [SerializeField] private Slider bossHealthBar;          // barra de "progresso" do boss

    [Header("Audio")]
    [SerializeField] private AudioClip phaseClearSfx;

    [Header("Fluxo de fase")]
    // Neste jogo o NEXUS e o FIM (true) -> chama Win() (tela de vitoria final).
    // Se algum dia o boss virar uma fase do MEIO, marque false -> ao vencer
    // ele AVANCA para a proxima fase via GameManager.LevelComplete().
    [SerializeField] private bool isFinalBoss = true;
    // Usado so quando isFinalBoss = false. Vazio = proxima cena da Scene List.
    [SerializeField] private string nextSceneName = "";

    private bool started;
    private int weakpointHitsRemaining;

    // A fase de pontos fracos esta ativa? (NexusWeakpoint consulta isso
    // para nao ser "gasto" durante as fases 1-2)
    public bool WeakpointPhaseActive => weakpointHitsRemaining > 0;

    private void Update()
    {
        if (started || player == null) return;
        if (player.transform.position.x >= triggerX)
        {
            started = true;
            // Boss = confronto: libera o movimento livre (A/D) na arena
            player.SetArenaMode(true);
            StartCoroutine(RunBoss());
        }
    }

    private IEnumerator RunBoss()
    {
        if (phases == null || phases.Length == 0)
        {
            // Sem fases configuradas: vitoria imediata (fail-safe)
            Victory();
            yield break;
        }

        for (int i = 0; i < phases.Length; i++)
        {
            yield return StartCoroutine(RunPhase(phases[i], i, phases.Length));
        }

        Victory();
    }

    private IEnumerator RunPhase(Phase phase, int index, int total)
    {
        AnnouncePhase(phase.phaseName, index, total);
        yield return new WaitForSeconds(1.5f); // respiro para ler o banner

        switch (phase.type)
        {
            case PhaseType.LaserBarrage:
            case PhaseType.DroneSwarm:
                yield return StartCoroutine(TimedSpawnPhase(phase));
                break;

            case PhaseType.Weakpoint:
                yield return StartCoroutine(WeakpointPhase(phase));
                break;
        }

        if (phaseClearSfx != null && player != null)
            AudioSource.PlayClipAtPoint(phaseClearSfx, player.transform.position, 0.9f);
    }

    // Fase por tempo: spawna inimigos/projeteis em intervalos.
    // Kael sobrevive ate o tempo acabar.
    private IEnumerator TimedSpawnPhase(Phase phase)
    {
        float t = 0f;
        float spawnT = 0f;
        while (t < phase.duration)
        {
            if (!IsPlayerAlive()) yield break; // morreu: a coroutine para

            t += Time.deltaTime;
            spawnT -= Time.deltaTime;
            UpdateHealthBar(t / phase.duration);

            if (spawnT <= 0f && phase.spawnPrefab != null)
            {
                spawnT = phase.spawnInterval;
                SpawnAtRandomPoint(phase.spawnPrefab);
            }
            yield return null;
        }
    }

    // Fase de pontos fracos: o jogador precisa "acertar" N vezes.
    // Aqui exposto como contador publico (HitWeakpoint) que um
    // gatilho na arena chama. Mantem simples e degradavel.
    private IEnumerator WeakpointPhase(Phase phase)
    {
        int totalHits = Mathf.Max(1, phase.weakpointHits);
        weakpointHitsRemaining = totalHits;
        while (weakpointHitsRemaining > 0)
        {
            if (!IsPlayerAlive()) yield break;
            UpdateHealthBar(1f - (float)weakpointHitsRemaining / totalHits);
            yield return null;
        }
    }

    // Chamado por um gatilho de ponto fraco na arena (Weakpoint trigger)
    public void HitWeakpoint()
    {
        if (weakpointHitsRemaining > 0) weakpointHitsRemaining--;
    }

    private void SpawnAtRandomPoint(GameObject prefab)
    {
        Vector3 pos;
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            pos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        }
        else if (player != null)
        {
            // Fallback: spawna acima do player, dentro da largura da arena
            float offsetX = Random.Range(-arenaWidth * 0.5f, arenaWidth * 0.5f);
            pos = player.transform.position + new Vector3(offsetX, 8f, 0f);
        }
        else return;

        Instantiate(prefab, pos, Quaternion.identity);
    }

    private void AnnouncePhase(string name, int index, int total)
    {
        if (bossBanner == null) return;
        bossBanner.text = "NEXUS\n<size=55%>" + name + "  (" + (index + 1) + "/" + total + ")</size>";
        bossBanner.gameObject.SetActive(true);
        CancelInvoke(nameof(HideBanner));
        Invoke(nameof(HideBanner), 2.5f);
    }

    private void HideBanner()
    {
        if (bossBanner != null) bossBanner.gameObject.SetActive(false);
    }

    private void UpdateHealthBar(float progress)
    {
        if (bossHealthBar != null) bossHealthBar.value = Mathf.Clamp01(progress);
    }

    private bool IsPlayerAlive()
    {
        return player != null && player.IsAlive;
    }

    private void Victory()
    {
        if (GameManager.Instance == null) return;

        // Devolve o controle normal ao player (sai do modo arena) antes de avancar.
        if (player != null) player.SetArenaMode(false);

        if (isFinalBoss)
            GameManager.Instance.Win();             // boss final: tela de vitoria
        else
            GameManager.Instance.LevelComplete(nextSceneName); // boss no meio: avanca de fase
    }
}
