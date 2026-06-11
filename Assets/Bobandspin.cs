using UnityEngine;

// ============================================================
// ZERO PULSE - BobAndSpin
// Da "vida" a objetos estaticos: flutuacao suave + giro opcional.
// Use nas celulas de energia (bob) e no que mais quiser.
// O offset por posicao X dessincroniza as celulas entre si.
// ============================================================
public class BobAndSpin : MonoBehaviour
{
    [SerializeField] private float bobAmplitude = 0.15f; // altura da flutuacao
    [SerializeField] private float bobSpeed = 2.5f;      // velocidade da flutuacao
    [SerializeField] private float spinSpeed = 0f;       // graus/seg (0 = nao gira)

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float phase = startPos.x; // cada objeto bobina fora de sincronia
        transform.position = startPos
            + Vector3.up * (Mathf.Sin(Time.time * bobSpeed + phase) * bobAmplitude);

        if (spinSpeed != 0f)
            transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }
}