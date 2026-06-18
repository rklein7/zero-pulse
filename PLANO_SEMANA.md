# Zero Pulse — Plano da Semana Extra (conteúdo)

> Documento vivo. Atualizado conforme a fundação é construída. Foco: **mais conteúdo** (zonas, inimigos, boss) com qualidade.

## Divisão de trabalho

- **Código / sistemas:** construído nos arquivos `.cs` (não exige Unity aberto). Já entregue na fundação abaixo.
- **Conteúdo / editor:** montar zonas, posicionar inimigos, virar prefabs, testar, balancear. **Trabalho da equipe no Unity.**

Os dois acontecem em paralelo. Sempre commitar antes de grandes mudanças.

---

## FUNDAÇÃO JÁ ENTREGUE (Dias 1–2)

### Sistema de inimigos (classe-base reaproveitável)

Novo padrão: `EnemyBase.cs` é a classe-mãe. Os inimigos herdam dela e já vêm com morte-ao-toque e autodestruição embutidas — **não precisa mais de `DeadlyOnTouch` nos inimigos novos**.

| Script | Inimigo (GDD) | Comportamento | Zonas sugeridas |
|---|---|---|---|
| `CrawlerEnemy` (existente) | Zumbi Rasteiro | Rasteja para a esquerda | 1–2 |
| `RunnerInfected` | Corredor Infectado | Persegue rápido, dá "bote" quando Kael se aproxima | 2–4 |
| `FlyingSentinel` | Sentinela Voadora | Flutua e dispara lasers para baixo | 3–5 |
| `SentinelLaser` | (projétil) | Cai e mata ao tocar | — |
| `KamikazeDrone` | Drone Kamikaze | Paira e mergulha quando Kael chega perto | 4–5 |

**Como criar o prefab de cada inimigo novo (padrão igual ao Crawler):**
1. Criar objeto com Sprite + `Collider2D` com **Is Trigger ✓**.
2. Adicionar o script do inimigo (ex.: `RunnerInfected`).
3. (Opcional) Spot Light 2D filha para o brilho de perigo.
4. Para a Sentinela: criar também o prefab do laser e arrastá-lo no campo `Laser Prefab`.
5. Arrastar para a pasta `Prefabs`.

### Sistema de zonas (`ZoneManager.cs`) — tudo numa cena só

Divide o level em zonas **por posição X**. Ao cruzar para uma zona nova: mostra um banner com o nome, muda a velocidade de corrida (dificuldade crescente) e troca a música.

**Setup (uma vez):**
1. Criar objeto vazio `ZoneManager` e adicionar o script.
2. Criar um texto TMP grande no Canvas para o banner (estilo do EndMessage), deixá-lo desativado, e arrastá-lo no campo `Zone Banner`.
3. Arrastar o `Player` e um `AudioSource` (pode ser o da Main Camera) nos campos.
4. Configurar o array `Zones` no Inspector — uma entrada por zona, **em ordem crescente de Start X**:
   - `Zone Name` (ex.: "Zona 2 — Favela Neon")
   - `Start X` (onde a zona começa no level)
   - `Run Speed` (sobe a cada zona: 7 → 8 → 9...)
   - `Music` (clip opcional)
   - `Subtitle` (texto menor, opcional)

### Consertos de robustez (aplicados)
- **Slide:** corrigido o acúmulo de offset que fazia Kael "afundar" no chão em runs longas; e ele não reaparece mais achatado após morrer no meio de um slide.
- **GameManager:** protegido contra duplicata acidental na cena.
- **PlayerController:** novo método `SetRunSpeed()` usado pelo ZoneManager.

---

## PRÓXIMOS PASSOS

### Equipe (no Unity, pode começar já)
- [ ] Testar: abrir o Unity, confirmar que o Console compila sem erros após a fundação.
- [ ] Criar os prefabs dos 3 inimigos novos (RunnerInfected, FlyingSentinel + SentinelLaser, KamikazeDrone) seguindo o padrão do Crawler.
- [ ] Montar a **Zona 2 (Favela Neon)** com os assets cyberpunk já baixados: estender o chão, novo parallax, posicionar RunnerInfected.
- [ ] Configurar o ZoneManager com as zonas existentes.

### Código (Duds + Claude, sob demanda)
- [ ] Sistema de checkpoint/vidas (evitar reiniciar tudo ao morrer) — **DESCARTADO** (decisão: morte = reinício mantém a tensão do GDD).
- [x] Boss da zona final: **NEXUS, 3 fases** — sistema entregue (`NexusBoss.cs` + `NexusWeakpoint.cs`).
- [ ] Menu inicial e tela de transição.

---

## BOSS FINAL — NEXUS (entregue, falta montar a arena)

Sistema em `NexusBoss.cs`. **Arquitetura degradável:** roda quantas fases você configurar (3 = completo, mas 2 ou 1 funcionam sem reescrever nada). As 2 primeiras fases reaproveitam inimigos que já existem.

### Design das 3 fases
1. **A Investida** (`LaserBarrage`): NEXUS bombardeia a arena. Spawna `SentinelLaser` em intervalos; Kael sobrevive desviando pela duração. Reusa o projétil da Sentinela.
2. **O Enxame** (`DroneSwarm`): ondas de `KamikazeDrone`. Kael lê o padrão e desvia com dash. Reusa o drone.
3. **Sobrecarga** (`Weakpoint`): pontos fracos (`NexusWeakpoint`) expõem; Kael precisa alcançá-los N vezes. Única mecânica nova.

### Setup da arena (no Unity)
1. No fim do level, criar a arena do boss (chão amplo, sem buracos — é combate, não corrida).
2. Criar objeto `NexusBoss` e adicionar o script. Definir `Trigger X` (onde Kael ativa o boss).
3. Arrastar o `Player`.
4. Criar 3–5 objetos vazios no alto da arena como `Spawn Points` (de onde caem lasers/drones) e arrastá-los no array.
5. Criar um texto TMP grande (`Boss Banner`) e um Slider (`Boss Health Bar`) no Canvas; arrastar nos campos.
6. Configurar o array `Phases` (1 a 3 entradas):
   - Fase 1: type `LaserBarrage`, duration ~12, spawnInterval ~1, `Spawn Prefab` = prefab do SentinelLaser.
   - Fase 2: type `DroneSwarm`, duration ~14, spawnInterval ~1.5, `Spawn Prefab` = prefab do KamikazeDrone.
   - Fase 3: type `Weakpoint`, weakpointHits = 3.
7. Para a fase 3: posicionar objetos `NexusWeakpoint` (Collider trigger + script) na arena, arrastando o NexusBoss no campo `boss` de cada um.

### Se o tempo apertar (degradar com segurança)
- Entregar com **2 fases** (Laser + Drone): tira a entrada da fase 3 do array. Funciona 100%.
- Entregar com **1 fase** (Laser): boss curto mas existente. Ainda é um clímax.
- Em todos os casos, vencer chama `GameManager.Win()` → tela de vitória.

---

---

## Regras da semana
1. **Commit antes de cada grande mudança.** O Git é a rede de segurança.
2. **Uma pessoa edita a cena (`SampleScene`) por vez** — é arquivo único, conflito de merge é doloroso. Scripts podem ser editados em paralelo.
3. **Construir na ordem de prioridade.** Se faltar tempo, cortamos a última coisa, nunca o miolo. Melhor 2 zonas polidas + boss do que 5 zonas meia-boca.
4. **Sem checkpoint = zonas curtas.** Como morrer reinicia tudo, o jogo inteiro deve ser jogável em 3–5 min. Cada zona curta e intensa.
5. Nenhuma feature nova depois do build final do penúltimo dia. Último dia é polish, documentação e ensaio.
