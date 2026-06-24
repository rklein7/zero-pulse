# CLAUDE.md — Contexto do projeto ZERO PULSE

> **Propósito deste arquivo:** dar contexto completo a uma nova conversa com o Claude.
> Ao iniciar um novo chat, peça: *"Leia o CLAUDE.md do projeto zero-pulse e vamos continuar"*.
> O Claude tem acesso a esta pasta via MCP filesystem (`C:\Users\Eduardo Sichelero\Desktop\projeto_quinta\zero-pulse`).
> **Manter atualizado:** ao final de sessões importantes, o Claude deve atualizar a seção "Estado atual" e o "Log de decisões".

**Última atualização:** 15/06/2026

---

## 1. O que é o projeto

**Zero Pulse** — plataforma 2D / endless runner com progressão narrativa. Projeto final da disciplina **Tecnologias Emergentes** (ATITUS Educação, 2026.1, Prof. M.Sc. Fernando P. Pinheiro). Baseado em um GDD v1.0 completo (será atualizado para v1.1 na entrega).

- **Premissa:** Kael, um mensageiro, carrega o último transmissor funcional por uma metrópole devastada. Mecânica central: **Pulso de Transmissão** — energia drena continuamente; coletar células âmbar reabastece; zerou = derrota.
- **Prazo:** prorrogado em 1 semana (entrega ~19/06/2026). Foco escolhido: **mais conteúdo** (5 fases + boss).
- **Equipe:** Duds (Eduardo) + 1-2 colegas. Duds faz quase tudo no Unity; Claude faz código e documentos.

## 2. Stack e ambiente

- **Unity 6.4** (6000.4.10f1), template Universal 2D (URP), C#, Input Manager clássico (aviso amarelo de deprecation no Console é esperado e inofensivo).
- Git inicializado, `.gitignore` correto. Regra: **commit antes de qualquer mudança grande**.
- Duds é iniciante em Unity (começou há dias) mas evolui rápido; é dev experiente em Python/web. Explicar gestos de editor passo a passo quando necessário.

## 3. Arquitetura — 5 fases (cenas separadas)

Cada fase é uma cena em `Assets/Scenes/`: **Fase1 … Fase5**, todas registradas na Scene List (File > Build Profiles) na ordem 0-4. Cada cena é autocontida (Player, Main Camera, GameManager, Canvas, EventSystem, Global Light 2D, Global Volume, KillZone).

- **Morte** = recarrega a fase atual (GameManager.Reload por buildIndex).
- **FinishLine** nas fases 1-4 chama `GameManager.LevelComplete()` → banner "ZONA CONCLUIDA" → carrega a próxima cena. Na Fase5, FinishLine fica DESATIVADA — quem encerra o jogo é o boss NEXUS (chama `Win()`).
- **Regra de equipe:** uma pessoa por cena (evita conflito de Git em arquivo de cena).

### Estado por fase
| Fase | Tema | Estado |
|---|---|---|
| Fase1 | Cidade decadente (fundo Ephemeral, parallax 4 camadas) | ✅ Completa e jogável |
| Fase2 | Floresta (pack Parallax Forest HQ) | ✅ Montada e jogável (revisar população de inimigos) |
| Fase3 | Distrito Industrial (pack bulkhead-walls) | 🔶 Em montagem — fundo parcial; faltam piso decorado, andaimes (cols), luz ~0.6. Alternativa rápida aprovada: reusar Ephemeral com Global Light esverdeada |
| Fase4 | Setor Zero | ⬜ Estratégia decidida: **palette swap** — reusar cidade Ephemeral com Global Light vermelha (`8B3A3A`, ~0.45). **ATENÇÃO:** a `Fase4.unity` atual é uma CÓPIA da cena da floresta/boss (141 KB idêntico à Fase5, FinishLine já desativada). Não é o palette swap — é placeholder. Plano: deletar a cena, duplicar a **Fase1** (cidade Ephemeral pronta), renomear p/ Fase4, trocar Global Light p/ vermelho e povoar |
| Fase5 | Torre / boss NEXUS | ⬜ Sistema 100% codificado; falta montar a arena no editor (roteiro pronto, ver seção 6) |

## 4. Scripts (todos em `Assets/`, raiz)

**Nota:** vários arquivos têm nome em minúscula (`Gamemanager.cs` etc.) diferente da classe (`GameManager`). Funciona no Unity 6.4 — NÃO renomear antes da entrega (item de limpeza pós-entrega).

| Arquivo | Classe | Função |
|---|---|---|
| PlayerController.cs | PlayerController | Corrida automática, pulo (coyote/buffer/altura variável), slide, `SetRunSpeed()`, **`SetArenaMode(bool)`** (movimento livre A/D na arena do boss), `Die()`, SFX de pulo |
| Gamemanager.cs | GameManager | Singleton. Energia (drain), HUD, `GameOver()`, `Win()` (final), `LevelComplete()` (carrega próxima fase), R = reiniciar, SFX morte/vitória |
| Energypickup.cs | EnergyPickup | Célula âmbar; `amount` + SFX |
| Deadlyontouch.cs | DeadlyOnTouch | Morte ao toque (obstáculos, canos, KillZone). Inimigos NOVOS não usam (herdam de EnemyBase) |
| Finishline.cs | FinishLine | Fim de fase. Campos: `isFinalLevel` (marcar só onde for o fim do jogo), `nextSceneName` (vazio = próxima da Scene List) |
| Camerafollow.cs | CameraFollow | Suavização + avanço (offset 3,1,-10) |
| Crawlerenemy.cs | CrawlerEnemy | Zumbi Rasteiro (legado, NÃO herda de EnemyBase, usa DeadlyOnTouch no prefab) |
| EnemyBase.cs | EnemyBase | Classe-mãe: morte ao toque embutida + despawn atrás da câmera. Inimigos novos herdam dela |
| RunnerInfected.cs | RunnerInfected | Corredor: persegue + "bote" quando perto |
| FlyingSentinel.cs | FlyingSentinel | Sentinela voadora: flutua + dispara `laserPrefab` em intervalos |
| SentinelLaser.cs | SentinelLaser | Projétil: cai, mata, autodestrói |
| KamikazeDrone.cs | KamikazeDrone | Paira; mergulha em linha reta quando Kael chega a `triggerRange` |
| NexusBoss.cs | NexusBoss | Boss de fases configuráveis (1-3): LaserBarrage / DroneSwarm / Weakpoint. Ativa arena mode no player. Vitória → `Win()` |
| NexusWeakpoint.cs | NexusWeakpoint | Ponto fraco da fase 3 do boss (só "consome" se `WeakpointPhaseActive`) |
| ParallaxLayer.cs | ParallaxLayer | Camada de fundo; factor 0 (mundo) a 1 (gruda na tela) |
| BobAndSpin.cs | BobAndSpin | Flutuação/giro de coletáveis |
| ZoneManager.cs / BackgroundZone.cs | — | Legado da arquitetura antiga (zonas numa cena). NÃO usados nas fases; manter (ZoneManager serve p/ banner de nome de fase se quiserem) |

## 5. Prefabs (`Assets/Prefabs/`)

Crawler, EnergyCell, FinishLine, Obstacle, Pipe, RunnerInfected, KamikazeDrone, FlyingSentinel, SentinelLaser. Padrão: Sprite + Collider2D **Is Trigger ✓** + script (+ Spot Light 2D opcional). Player tem Tag **Player** (obrigatória — tudo identifica por ela).

## 6. Arena do NEXUS (Fase5) — roteiro de montagem pendente

1. Limpar floresta da cena; desativar FinishLine.
2. `ArenaGround` (80,-3) scale (20,1) Layer Ground; 2 paredes sólidas (70,0) e (90.5,0) scale (1,6) Layer Ground SEM trigger.
3. Objeto `NexusBoss` + script: TriggerX 72, arrastar Player.
4. 4 spawn points filhos em (74,3)(79,3)(84,3)(88,3) → array.
5. UI: duplicar EndMessage→`BossBanner` (desativado); duplicar EnergyBar→`BossHealthBar` (top-right, fill vermelho).
6. Phases (3): A INVESTIDA/LaserBarrage/12s/1s/SentinelLaser · O ENXAME/DroneSwarm/14s/1.5s/KamikazeDrone · SOBRECARGA/Weakpoint/3 hits.
7. 3 `Weakpoint` âmbar (75,-2)(81,0.5)(88,-2), trigger + NexusWeakpoint + ref ao boss.
8. Teste: player provisório em x60. A/D liberados ao cruzar TriggerX.

## 7. Convenções e pegadinhas conhecidas

- Pixel art: Filter Mode **Point**, Compression **None**; sprites usados em Draw Mode Tiled exigem **Mesh Type Full Rect**.
- Dimensionar arte por **Pixels Per Unit** (bulkhead = PPU 40), nunca por Scale não-uniforme (distorce pixels). Scale dos fundos = (1,1,1).
- Fundo: Order in Layer sempre **negativo**; Z sempre 0. Tiled p/ texturas contínuas, Simple+cópias p/ elementos discretos (canos, pilares).
- Camada de piso decorado alinhada ao gameplay **NÃO leva ParallaxLayer**.
- Visual do player: objeto `Visual` filho (sprite animado KaelRun); SpriteRenderer do Player root desligado; nunca mexer no collider/scale do root.
- Gravity Scale do Player = 3.5.
- "Uma cena por vez": abrir por duplo clique; nunca arrastar cena pra Hierarchy (abre aditivo).
- Console: aviso amarelo de Input Manager = ignorar.

## 8. Log de decisões importantes

1. Arquitetura mudou de "zonas numa cena" → **5 fases em cenas separadas** (pedido do professor/equipe).
2. **Sem checkpoint dentro da fase** — morte reinicia a fase (mantém tensão; fases curtas: jogo todo em 3-5 min).
3. Boss = **NEXUS, 3 fases degradáveis** (pode entregar com 2 ou 1 sem reescrever).
4. Arte: Fase1 trocou metrô bulkhead → cidade **Ephemeral**; bulkhead realocado para Fase3; Fase4 = palette swap da cidade.
5. Não usar projeto/template pronto de terceiros (risco acadêmico + retrabalho); assets de arte/áudio de terceiros OK com crédito.
6. **Correções de código (15/06/2026, sessão Fase4):** revisão completa dos scripts. Corrigidos: divisão por zero na barra do boss quando `weakpointHits=0` (NexusBoss); possível NullReference em `EnergyPickup` e `DeadlyOnTouch` quando a cena não tem GameManager (agora checam null como os demais); NullReference em `ParallaxLayer` se `Camera.main` não existir por 1 frame. Cenas Unity (.unity) NÃO foram editadas — montagem de fase é trabalho de editor (editar YAML de cena via texto corromperia a cena).
7. **Reordenação de fases + menu (23/06/2026):** nova ordem — Fase1=Distrito Industrial (antiga Fase3), Fase2=Floresta (mantém), Fase3=Setor Zero (antiga Fase4), Fase4=Cidade (antiga Fase1), Fase5=Boss NEXUS (mantém, segue sendo o FINAL). O rename dos `.unity` é feito pelo Duds dentro do Unity (Project window) + reordenar a Build List — o ambiente do Claude (mount) NÃO renomeia/deleta arquivos do projeto de forma confiável (só edita conteúdo de arquivo). `NexusBoss.cs` ganhou `isFinalBoss` (default **true**) e `nextSceneName`: ao vencer chama `Win()` se for final, ou `LevelComplete()` se for boss do meio (devolve o player ao modo normal antes). `MenuManager.cs` já existe; falta montar a cena `Menu` (índice 0 da Build List). Passo a passo completo em `GUIA_MENU_E_REORDEM.md`.

## 9. Pendências (ordem de prioridade)

1. Fase5: montar arena do NEXUS (seção 6) e testar as 3 fases do boss.
2. Fase3: terminar (piso + cols + luz 0.6) OU aplicar fallback Ephemeral esverdeado.
3. Fase4: palette swap vermelho + povoar (drones + corredores).
4. Povoar fases com inimigos (princípio: cada inimigo estreia sozinho; combina depois; vencer cada fase na 2ª-3ª tentativa).
5. Áudio: música ambiente por fase (AudioSource na câmera) + clipes nos campos SFX (Player/EnergyCell/GameManager).
6. Menu inicial (cena 0 antes da Fase1 — Claude faz o código quando chegar a hora) + ajustar Scene List.
7. Build final Windows + vídeo de gameplay + GDD v1.1 (seção "Escopo do Protótipo" já redigida em docx; atualizar tabela de zonas/temas) + README créditos/equipe.

## 10. Outros arquivos de apoio

- `PLANO_SEMANA.md` — plano da semana (parcialmente DESATUALIZADO: anterior à migração p/ 5 fases; este CLAUDE.md prevalece).
- `README.md` — pronto; faltam preencher créditos de assets e nomes da equipe.
- Docs entregues fora do repo: GDD seção "Escopo do Protótipo" (docx), plano de finalização (docx).
