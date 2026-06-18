using UnityEngine;
using TMPro;

// ============================================================
// ZERO PULSE - ZoneManager
// Sistema de zonas em UMA cena so. Divide o level em zonas por
// posicao X. Ao entrar numa zona nova:
//   - mostra o nome da zona na tela (banner)
//   - aumenta a velocidade de corrida (dificuldade crescente, GDD)
//   - troca a musica (opcional)
//
// COMO USAR:
//   1. Anexe a um GameObject vazio "ZoneManager".
//   2. Configure as zonas no Inspector (startX, nome, velocidade...).
//   3. Arraste o Player, o texto de banner (TMP) e o AudioSource.
//   As zonas devem estar em ordem crescente de startX.
// ============================================================
public class ZoneManager : MonoBehaviour
{
    [System.Serializable]
    public class Zone
    {
        public string zoneName = "Zona";
        public float startX = 0f;            // a partir de que X esta zona comeca
        public float runSpeed = 7f;          // velocidade de corrida nesta zona
        public AudioClip music;              // trilha desta zona (opcional)
        [TextArea] public string subtitle;   // texto menor sob o nome (opcional)
    }

    [Header("Referencias")]
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI zoneBanner; // texto grande, ativado nas transicoes
    [SerializeField] private AudioSource musicSource;    // AudioSource da musica de fundo

    [Header("Zonas (em ordem de startX)")]
    [SerializeField] private Zone[] zones;

    [Header("Banner")]
    [SerializeField] private float bannerDuration = 2.5f;

    private int currentZone = -1;
    private float bannerTimer;

    private void Start()
    {
        if (zoneBanner != null) zoneBanner.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (player == null || zones == null || zones.Length == 0) return;

        // Descobre em qual zona o player esta (maior startX que ele ja passou)
        float px = player.transform.position.x;
        int zoneNow = currentZone;
        for (int i = 0; i < zones.Length; i++)
        {
            if (px >= zones[i].startX) zoneNow = i;
        }

        if (zoneNow != currentZone && zoneNow >= 0)
        {
            EnterZone(zoneNow);
        }

        // Esconde o banner depois de um tempo
        if (zoneBanner != null && zoneBanner.gameObject.activeSelf)
        {
            bannerTimer -= Time.deltaTime;
            if (bannerTimer <= 0f) zoneBanner.gameObject.SetActive(false);
        }
    }

    private void EnterZone(int index)
    {
        currentZone = index;
        Zone z = zones[index];

        // Velocidade de corrida da zona
        if (player != null) player.SetRunSpeed(z.runSpeed);

        // Banner com o nome da zona
        if (zoneBanner != null)
        {
            string txt = z.zoneName;
            if (!string.IsNullOrEmpty(z.subtitle))
                txt += "\n<size=45%>" + z.subtitle + "</size>";
            zoneBanner.text = txt;
            zoneBanner.gameObject.SetActive(true);
            bannerTimer = bannerDuration;
        }

        // Troca de musica (so se for diferente da atual)
        if (musicSource != null && z.music != null && musicSource.clip != z.music)
        {
            musicSource.clip = z.music;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
