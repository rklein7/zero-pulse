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

Arte, fontes e áudio de terceiros utilizados (gratuitos / uso permitido, com os devidos créditos):

**Personagens e inimigos**
- *City Man Pixel Art Character Sprite Sheets* (Kael) — CraftPix — https://craftpix.net/freebies/city-man-pixel-art-character-sprite-sheets/
- *Urban Zombie Pixel Art Pack* (corredor infectado) — Free Game Assets / CraftPix — https://free-game-assets.itch.io/free-urban-zombie-sprite-sheet-pixel-art-pack
- *Free Drones Pack Pixel Art* (drone kamikaze / sentinela) — CraftPix — licença: https://craftpix.net/file-licenses/
- *Free 2D Character 16x16 — CR71435353* (NEXUS) — JGIIO — https://jgiio.itch.io/free-2d-character-16x16-cr71435353
- *Monsters Creatures Fantasy 2* — LuizMelo — https://luizmelo.itch.io  

**Cenários e efeitos**
- *Pixel Art Fire Asset Pack* (fogo) — Devkidd — https://devkidd.itch.io/pixel-fire-asset-pack
- *Bulkhead Walls* e *Parallax Industrial* (Distrito Industrial) — Luis Zuno (ansimuz) — https://ansimuz.itch.io  
- *FREE Parallax Forest Background HQ* (Floresta) 
- *Ephemeral* e *Futuristic City Parallax* (Cidade / Setor Zero) — 

**Fontes**
- *Press Start 2P* — CodeMan38 (Google Fonts — licença SIL OFL)
- *VT323* — Peter Hull (Google Fonts — licença SIL OFL)
- *Future Millennium* — dafont.com (incluída no pack de drones)

**Ferramentas**
- Unity 6 (URP 2D) · C# · Git/GitHub · parte da arte feita/editada em Aseprite

## Equipe

Eduardo Sichelero

Rafael Augusto Klein

Vitor Augusto Feil Quadros

---

*Projeto acadêmico.*
