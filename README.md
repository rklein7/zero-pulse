# ZERO PULSE

> "Quando a civilização caiu, o que restou foi a corrida."

Protótipo jogável (vertical slice) do **Zero Pulse** — plataforma 2D / endless runner com progressão narrativa, ambientado em uma metrópole devastada por uma praga tecnológica. Desenvolvido para a disciplina de **Tecnologias Emergentes** (ATITUS Educação, 2026.1).

O jogador controla **Kael**, um mensageiro que carrega o último transmissor funcional do mundo. A mecânica central é o **Pulso de Transmissão**: a energia do transmissor drena continuamente — colete células de energia e alcance o ponto de transmissão antes que o sinal morra.

## Como jogar

| Tecla | Ação |
|---|---|
| `ESPAÇO` | Pulo (toque = curto, segurar = alto) |
| `S` / `↓` / `Ctrl esq.` | Slide (passa sob canos e obstáculos baixos) |
| `R` | Reiniciar após vitória ou derrota |

**Objetivo:** atravessar o segmento da Zona 1 (Cinzas do Metrô) e alcançar a antena âmbar com energia restante. Vermelho mata. Âmbar salva.

## Como rodar

1. **Unity 6.4** (6000.4.x) com o módulo Universal 2D (URP).
2. Clonar o repositório e abrir a pasta pelo Unity Hub (`Add project from disk`).
3. Abrir a cena `Assets/Scenes/SampleScene` e apertar Play — ou gerar uma build em `File > Build Profiles > Windows`.

## Arquitetura (scripts)

| Script | Responsabilidade |
|---|---|
| `PlayerController` | Corrida automática, pulo (coyote time, jump buffer, altura variável), slide |
| `GameManager` | Pulso de Transmissão (energia), morte, vitória, reinício, HUD |
| `CameraFollow` | Câmera com suavização e avanço |
| `EnergyPickup` | Células de energia coletáveis |
| `DeadlyOnTouch` | Morte ao toque (obstáculos, canos, inimigos, abismo) |
| `CrawlerEnemy` | Zumbi Rasteiro (matriz de inimigos do GDD) |
| `FinishLine` | Ponto de transmissão (vitória) |
| `ParallaxLayer` | Camadas de fundo com profundidade |
| `BobAndSpin` | Flutuação/giro de coletáveis |

Parâmetros de game feel e dificuldade são expostos no Inspector (`Run Speed`, `Jump Force`, `Drain Per Second` etc.).

## Créditos de assets

Arte e áudio de terceiros usados como placeholder de produção:

- [PREENCHER: pack de cenário — ex. autor do tileset industrial]
- [PREENCHER: pack do personagem — ex. Cyberpunk City 2]
- [PREENCHER: áudio]

## Equipe

[PREENCHER: nomes dos integrantes]

---

*Projeto acadêmico. Game Design Document completo disponível na entrega da disciplina (GDD Zero Pulse v1.1).*
