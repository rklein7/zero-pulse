using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// ============================================================
// ZERO PULSE - UIGradient
// Degrade vertical para qualquer elemento de UI (Image/Graphic)
// SEM precisar de imagem. Coloque num Image BRANCO esticado na
// tela toda (o fundo do menu) e defina as cores topo/baixo.
//
// COMO USAR:
//   1. No Canvas, crie um UI > Image, estique na tela toda,
//      deixe a cor (Color) BRANCA.
//   2. Add Component > UI Gradient (Zero Pulse).
//   3. Defina Top Color e Bottom Color.
// ============================================================
[AddComponentMenu("UI/Effects/UI Gradient (Zero Pulse)")]
public class UIGradient : BaseMeshEffect
{
    [SerializeField] private Color topColor = new Color(0.04f, 0.06f, 0.09f, 1f);    // quase preto azulado
    [SerializeField] private Color bottomColor = new Color(0.12f, 0.08f, 0.04f, 1f); // marrom/ambar escuro

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0) return;

        var verts = new List<UIVertex>();
        vh.GetUIVertexStream(verts);

        float minY = verts[0].position.y;
        float maxY = verts[0].position.y;
        for (int i = 1; i < verts.Count; i++)
        {
            float y = verts[i].position.y;
            if (y > maxY) maxY = y;
            else if (y < minY) minY = y;
        }

        float height = Mathf.Max(maxY - minY, 0.0001f);
        for (int i = 0; i < verts.Count; i++)
        {
            UIVertex v = verts[i];
            float t = (v.position.y - minY) / height;          // 0 embaixo, 1 no topo
            v.color = Color.Lerp(bottomColor, topColor, t);     // sobrescreve a cor do vertice
            verts[i] = v;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
    }
}
