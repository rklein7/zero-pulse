using UnityEngine;

// ============================================================
// ZERO PULSE - CameraFollow
// Segue o Player com suavizacao e leve avanco para a direita
// (cria o "senso de urgencia" descrito no GDD, secao 3Cs).
// Anexar a Main Camera. A camera NAO pode ser filha do Player.
// ============================================================
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;                       // arraste o Player aqui
    [SerializeField] private Vector3 offset = new Vector3(3f, 1f, -10f); // avanco X, altura Y; Z = -10 SEMPRE
    [SerializeField] private float smoothTime = 0.2f;                // menor = camera mais "dura"

    private Vector3 velocity;

    // LateUpdate roda depois de todo movimento do frame:
    // garante que a camera ve a posicao final do Player.
    private void LateUpdate()
    {
        if (target == null || !target.gameObject.activeSelf) return;

        Vector3 desired = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }
}