using UnityEngine;

// ============================================================
// ZERO PULSE - KamikazeDrone ("Drone Kamikaze", GDD secao 08)
// Zonas 4-5. Fica parado/pairando ate Kael se aproximar, entao
// mergulha na direcao dele para explodir. Robotico.
// Como superar (design): dash lateral no ultimo momento.
//
// Prefab: Collider2D (Is Trigger) + este script.
// ============================================================
public class KamikazeDrone : EnemyBase
{
    [Header("Drone Kamikaze")]
    [SerializeField] private float triggerRange = 5f;   // distancia para ativar o mergulho
    [SerializeField] private float diveSpeed = 9f;      // velocidade do mergulho
    [SerializeField] private float hoverBob = 0.2f;     // flutuacao enquanto espera

    private Transform player;
    private Vector3 startPos;
    private bool diving;
    private Vector3 diveDir;

    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    protected override void Move()
    {
        if (player == null) return;

        if (!diving)
        {
            // Pairando, flutuando de leve
            float bob = Mathf.Sin(Time.time * 3f) * hoverBob;
            transform.position = startPos + Vector3.up * bob;

            // Kael chegou perto? inicia o mergulho travando a direcao
            float dist = Vector2.Distance(player.position, transform.position);
            if (dist < triggerRange)
            {
                diving = true;
                diveDir = (player.position - transform.position).normalized;
            }
        }
        else
        {
            // Mergulha em linha reta na direcao travada
            transform.position += diveDir * diveSpeed * Time.deltaTime;
        }
    }
}
