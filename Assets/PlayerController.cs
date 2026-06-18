using UnityEngine;

// ============================================================
// ZERO PULSE - PlayerController v2
// Corrida automatica + pulo (coyote time, jump buffer,
// altura variavel) + slide. Anexar ao objeto Player.
// IMPORTANTE: no Rigidbody2D do Player, defina Gravity Scale = 3.5
// ============================================================
public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    [Header("Game feel")]
    [SerializeField] private float coyoteTime = 0.12f;  // pode pular ate 0.12s apos sair da borda
    [SerializeField] private float jumpBuffer = 0.12f;  // aperto de pulo "vale" por 0.12s antes de pousar

    [Header("Checagem de chao")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSfx; // arraste o som de pulo aqui (opcional)

    private Rigidbody2D rb;
    private Vector3 normalScale;     // escala original (para sair do slide)
    private float coyoteCounter;
    private float bufferCounter;
    private bool jumpCut;            // soltou o pulo durante a subida?
    private bool isGrounded;
    private bool isSliding;
    private bool slideOffsetApplied; // controla o ajuste de posicao do slide (evita acumulo)
    private bool arenaMode;          // boss: movimento livre (A/D) em vez de corrida automatica
    private float horizontalInput;   // leitura do eixo no Update, uso no FixedUpdate

    public bool IsAlive { get; private set; } = true;

    // Chamado pelo NexusBoss: liga/desliga o movimento livre da arena
    public void SetArenaMode(bool active)
    {
        arenaMode = active;
    }

    // Chamado pelo ZoneManager para mudar a velocidade por zona
    public void SetRunSpeed(float speed)
    {
        runSpeed = speed;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        normalScale = transform.localScale;
    }

    // ---------- INPUT: sempre no Update ----------
    private void Update()
    {
        if (!IsAlive) return;

        // Eixo horizontal (so usado no modo arena do boss)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Jump buffer: guarda o aperto por alguns ms
        if (Input.GetButtonDown("Jump")) bufferCounter = jumpBuffer;
        else bufferCounter -= Time.deltaTime;

        // Pulo variavel: soltar o botao cedo corta a subida
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
            jumpCut = true;

        // Slide: segurar S / seta baixo / Ctrl esquerdo
        bool slideKey = Input.GetKey(KeyCode.S)
                     || Input.GetKey(KeyCode.DownArrow)
                     || Input.GetKey(KeyCode.LeftControl);

        if (slideKey && isGrounded && !isSliding) StartSlide();
        else if (!slideKey && isSliding) StopSlide();
    }

    // ---------- FISICA: sempre no FixedUpdate ----------
    private void FixedUpdate()
    {
        if (!IsAlive) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // Coyote: cheio no chao, esvaziando no ar
        coyoteCounter = isGrounded ? coyoteTime : coyoteCounter - Time.fixedDeltaTime;

        // Corrida automatica (ou movimento livre na arena do boss)
        float xVel = arenaMode ? horizontalInput * runSpeed : runSpeed;
        rb.linearVelocity = new Vector2(xVel, rb.linearVelocity.y);

        // Pulo: precisa de buffer ativo + coyote ativo
        if (bufferCounter > 0f && coyoteCounter > 0f && !isSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            bufferCounter = 0f;
            coyoteCounter = 0f;

            if (jumpSfx != null)
                AudioSource.PlayClipAtPoint(jumpSfx, transform.position, 0.7f);
        }

        // Corte do pulo (altura variavel)
        if (jumpCut)
        {
            if (rb.linearVelocity.y > 0f)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            jumpCut = false;
        }
    }

    private void StartSlide()
    {
        if (isSliding) return;
        isSliding = true;
        // Achata o personagem (collider acompanha a escala)
        transform.localScale = new Vector3(normalScale.x * 1.1f, normalScale.y * 0.5f, 1f);
        // Desce o centro para manter os pes no chao (uma unica vez)
        rb.position += Vector2.down * (normalScale.y * 0.25f);
        slideOffsetApplied = true;
    }

    private void StopSlide()
    {
        if (!isSliding) return;
        isSliding = false;
        // Desfaz EXATAMENTE o mesmo offset aplicado (evita afundar com o tempo)
        if (slideOffsetApplied)
        {
            rb.position += Vector2.up * (normalScale.y * 0.25f);
            slideOffsetApplied = false;
        }
        transform.localScale = normalScale;
    }

    // Chamado pelo GameManager quando Kael morre
    public void Die()
    {
        if (!IsAlive) return;
        IsAlive = false;
        // Garante que o player nao reaparece achatado num respawn
        transform.localScale = normalScale;
        isSliding = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;           // desliga a fisica
        gameObject.SetActive(false);    // placeholder de animacao de morte
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}