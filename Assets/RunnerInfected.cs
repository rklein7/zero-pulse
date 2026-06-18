using UnityEngine;

// ============================================================
// ZERO PULSE - RunnerInfected ("Corredor Infectado", GDD secao 08)
// Zonas 2-4. Persegue Kael em alta velocidade: corre para a
// esquerda mais rapido que o cenario, alcancando o jogador.
// Como superar (design): dash + colocar obstaculo entre eles.
//
// Prefab: Collider2D (Is Trigger) + este script. Sem DeadlyOnTouch.
// ============================================================
public class RunnerInfected : EnemyBase
{
    [Header("Corredor Infectado")]
    [SerializeField] private float chaseSpeed = 5f;   // velocidade rastejando p/ a esquerda
    [SerializeField] private float lungeBoost = 1.5f; // acelera quando o player se aproxima
    [SerializeField] private float lungeRange = 6f;   // distancia para iniciar o bote

    private Transform player;

    protected override void Start()
    {
        base.Start();
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    protected override void Move()
    {
        float speed = chaseSpeed;

        // Se Kael esta perto, da um "bote" acelerado (tensao do GDD)
        if (player != null)
        {
            float dist = Mathf.Abs(player.position.x - transform.position.x);
            if (dist < lungeRange) speed *= lungeBoost;
        }

        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
