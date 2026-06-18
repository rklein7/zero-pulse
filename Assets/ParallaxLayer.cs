using UnityEngine;

// ============================================================
// ZERO PULSE - ParallaxLayer
// Anexar a CADA camada de fundo (objeto com SpriteRenderer).
// parallaxFactor = quanto a camada ACOMPANHA a camera:
//   0.95 = ceu/fundo distante (quase grudado na tela)
//   0.7-0.8 = predios/arcos distantes
//   0.4-0.6 = pilares/elementos medios
//   0 = mesmo plano do gameplay (nao precisa do script)
// Dica: deixe cada sprite de fundo LARGO o suficiente
// (Draw Mode = Tiled, Width ~120) para cobrir o level inteiro.
// ============================================================
public class ParallaxLayer : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float parallaxFactor = 0.7f;

    private Transform cam;
    private Vector3 lastCamPos;

    private void Start()
    {
        if (Camera.main != null) cam = Camera.main.transform;
        if (cam != null) lastCamPos = cam.position;
    }

    // LateUpdate: depois da camera se mover no frame
private void LateUpdate()
    {
        if (cam == null) return;
        Vector3 delta = cam.position - lastCamPos;
        transform.position += new Vector3(delta.x * parallaxFactor, 0f, 0f);
        lastCamPos = cam.position;
    }
}