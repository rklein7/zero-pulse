# Guia — Menu principal + nova ordem das fases

> Gerado em 23/06/2026. Os passos **[Claude]** são feitos por código (com o Unity FECHADO); os **[Você no Unity]** são gestos de editor.

---

## 0. Resumo da mudança

Ordem nova das fases (conteúdo → novo nome de cena):

| Conteúdo (tema) | Cena antiga | Cena nova | Posição |
|---|---|---|---|
| Distrito Industrial | Fase3 | **Fase1** | 1ª |
| Floresta | Fase2 | **Fase2** | 2ª |
| Setor Zero | Fase4 | **Fase3** | 3ª |
| Cidade decadente | Fase1 | **Fase4** | 4ª |
| Boss NEXUS | Fase5 | **Fase5** | 5ª (FINAL) |

Decisão: **o jogo termina no boss NEXUS** (Fase5). A cidade é a 4ª fase normal e avança para o boss. Isso mantém a arquitetura original de vitória (boss chama `Win()`).

Build List final (File > Build Profiles / Build Settings):

```
0  Menu        <- cena nova, você cria
1  Fase1       (industrial)
2  Fase2       (floresta)
3  Fase3       (setor zero)
4  Fase4       (cidade)
5  Fase5       (boss NEXUS) -> FIM
```

---

## 1. [Você no Unity] Renomear as cenas + reordenar a Build List

O rename é feito por você **dentro do Unity** (Project window), porque o ambiente do Claude não renomeia arquivos do projeto com segurança. O Unity faz isso certinho e atualiza GUIDs/referências sozinho.

Estado atual das cenas (nomes batem com o conteúdo): Fase1=cidade, Fase2=floresta, Fase3=industrial, Fase4=setor zero, Fase5=boss.

### 1.0 Limpeza rápida
Na pasta `Assets/Scenes` apague os arquivos de teste que sobraram: **`x.txt`, `y.txt`, `_teste_del.txt`** (clique direito → Delete). São inofensivos, só lixo.

### 1.1 Renomear (Project window → `Assets/Scenes`, F2 para renomear)
Faça **exatamente nesta ordem** (usa um nome temporário para evitar colisão):

1. `Fase1` (cidade) → renomeie para **`FaseTmp`**
2. `Fase3` (industrial) → renomeie para **`Fase1`**
3. `Fase4` (setor zero) → renomeie para **`Fase3`**
4. `FaseTmp` (cidade) → renomeie para **`Fase4`**

`Fase2` (floresta) e `Fase5` (boss) **não mudam**.

Resultado: Fase1=industrial, Fase2=floresta, Fase3=setor zero, Fase4=cidade, Fase5=boss.

### 1.2 Reordenar a Build List
Renomear **não** muda a ordem da Build List — ela continua na ordem antiga por GUID. Então abra `File > Build Profiles` (ou Build Settings) → Scene List e **arraste as entradas** até ficar exatamente:

```
Fase1  (industrial)
Fase2  (floresta)
Fase3  (setor zero)
Fase4  (cidade)
Fase5  (boss)
```

(O Menu entra no topo no passo 2.3.) Cada linha mostra o caminho `Assets/Scenes/FaseN.unity`, então é só ordenar por nome.

---

## 2. [Você no Unity] Criar a cena Menu

1. `File > New Scene` → template **Basic (URP)** ou 2D → `Ctrl+S` → salve como **`Menu.unity`** em `Assets/Scenes/`.
2. Garanta **Main Camera** e **EventSystem** na Hierarchy (o EventSystem vem junto ao criar um Canvas).
3. `GameObject > UI > Canvas`. No Canvas Scaler use **Scale With Screen Size** (Reference 1920x1080).

### 2.1 Estrutura de UI (o MenuManager espera exatamente isto)

```
Canvas
├── PainelMenu
│   ├── TituloText     (TextMeshPro)  -> "ZERO PULSE"
│   ├── SubtituloText  (TextMeshPro - opcional)
│   ├── BotaoJogar     (Button - TMP) -> "JOGAR"
│   └── BotaoTutorial  (Button - TMP) -> "COMO JOGAR"
└── PainelTutorial     (Panel) — DESATIVADO por padrão
    ├── TutorialText   (TextMeshPro)
    └── BotaoIniciar   (Button - TMP) -> "COMEÇAR"
```

Dicas: na 1ª vez o Unity pede para importar o **TMP Essentials** — aceite. Deixe **PainelTutorial desativado** (checkbox no topo do Inspector). Sugestão de tutorial: *"Kael corre sozinho. ESPAÇO = pular (segure p/ pular mais alto). S / ↓ / Ctrl = deslizar. Pegue células âmbar para não zerar o Pulso. R reinicia. No NEXUS, A/D liberam o movimento."*

### 2.2 Ligar o MenuManager

1. GameObject vazio chamado **`MenuManager`** → `Add Component > MenuManager`.
2. Arraste as referências: `Painel Menu`→PainelMenu, `Painel Tutorial`→PainelTutorial, `Botao Jogar`→BotaoJogar, `Botao Tutorial`→BotaoTutorial, `Botao Iniciar`→BotaoIniciar.
3. `Primeira Fase` → deixe **`Fase1`** (industrial na nova ordem).

> Os botões são ligados por código no `Start()` (AddListener). Você **não** precisa configurar o OnClick deles no Inspector — só arrastar as referências. (Configurar OnClick também duplicaria a ação.)

### 2.3 Menu no índice 0 da Build List

`File > Build Profiles` → Scene List → arraste `Menu.unity` e **mova para o topo**. Ordem final: Menu, Fase1, Fase2, Fase3, Fase4, Fase5.

---

## 3. [Você no Unity] Conferir a cidade (Fase4) — fase normal

A cidade (nova Fase4) é uma fase comum: a `FinishLine` dela deve ter **`Is Final Level` DESMARCADO** (avança para o boss). Nada a mudar se já estava assim — só confirme que a FinishLine está **ativa**.

---

## 4. [Você no Unity] Conferir o boss (Fase5) — final

1. Abra **Fase5.unity** (boss NEXUS).
2. Selecione **NexusBoss**. Em **Fluxo de fase**:
   - **`Is Final Boss`** → **MARCADO** ✓ (já vem true por padrão no script; confirme que está marcado no Inspector).
   - `Next Scene Name` → vazio.
3. A `FinishLine` desta cena continua **desativada** — quem encerra o jogo é o boss ao vencer (chama `Win()` → tela "TRANSMISSAO ENVIADA").

---

## 5. [Você no Unity] Testar o fluxo completo

1. Cena **Menu** → Play → **JOGAR** → carrega **Fase1 (industrial)**.
2. FinishLine de cada fase: Fase1 → Fase2 → Fase3 → Fase4 (cidade).
3. FinishLine da cidade → carrega **Fase5 (boss)**.
4. Vença o boss → **"TRANSMISSAO ENVIADA"** (vitória final). R reinicia.

Checklist se algo não avançar:
- As 6 cenas estão na Build List, na ordem certa? (Menu no índice 0)
- FinishLine das fases 1-4 **ativa** e com `Is Final Level` **desmarcado**?
- NexusBoss da Fase5 com `Is Final Boss` **marcado**?
