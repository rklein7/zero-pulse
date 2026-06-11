using UnityEngine;

// ============================================================
// ZERO PULSE - CrawlerEnemy ("Zumbi Rasteiro", GDD secao 08)
// Rasteja em direcao ao jogador. Combine com DeadlyOnTouch
// no mesmo objeto para matar ao encostar.
// ============================================================
public class CrawlerEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    private void Update()
    {
        // Anda para a esquerda (de encontro a Kael, que corre p/ direita)
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Limpeza: autodestroi quando ficar muito para tras da camera
        if (Camera.main != null &&
            transform.position.x < Camera.main.transform.position.x - 25f)
        {
            Destroy(gameObject);
        }
    }
}