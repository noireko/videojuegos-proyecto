# PUNTO CERO
**UADE 2026 — Miércoles Turno Noche**

Degiorgis Camila · Fernandez Rolón Agustín · Migueles Mariana · Prepelitchi Iara

---

## Descripción

**Punto Cero** es un videojuego de acción y supervivencia con construcción modular desarrollado en Unity 6 (URP). El jugador controla a John, un sobreviviente en un mundo post-apocalíptico donde el oxígeno escasea, atravesando sectores tóxicos, construyendo estructuras defensivas y persiguiendo al antagonista Eryx para recuperar el Prototipo de Purificador Portátil.

**Género:** Acción y supervivencia con construcción modular  
**Perspectiva:** Top-down isométrica  
**Estética:** Pixel Art Hi-Bit Cinemático · Neo-Noir  
**Plataforma:** PC (Windows / macOS)  
**Motor:** Unity 6000.3.6f1 LTS · Universal Render Pipeline

---

## Mecánicas principales

**Sistema de oxígeno:** El tanque de O₂ se depleta de forma constante. La muerte por asfixia es el principal motor de tensión. El jugador gestiona la supervivencia recolectando cápsulas de carbón y construyendo Purificadores Parciales que generan zonas de aire limpio temporal.

**Construcción modular:** Sistema de build en tiempo real con preview de colocación (overlay transparente verde/rojo según validez de posición), verificación de materiales en inventario y descuento automático al confirmar la construcción.

**Ciclo día/noche:** Afecta la iluminación global de la escena (via `Global Light 2D` de URP) y el comportamiento de los enemigos. Se visualiza en el HUD mediante un indicador deslizante.

**Sistema de interacción (tecla E):** Interfaz unificada a través de `IInteractable` que gestiona recolección de recursos, activación de objetos narrativos, pickup de armas y construcciones.

**Combate:** Hacha improvisada como arma cuerpo a cuerpo. Sistema de daño sobre objetos que implementan `IDamageable`, con sprites de daño progresivo.

---

## Controles

| Acción | Input |
|---|---|
| Movimiento omnidireccional | WASD |
| Correr | Shift |
| Interactuar / Atacar | E |
| Abrir inventario | TAB |
| Abrir panel de construcción | X |
| Pausar | ESC |

---

## Estructura de escenas

| Escena | Descripción |
|---|---|
| `VJ-MIERCOLES` | Escena principal de gameplay — Sector 1: La Periferia |
| `Main-Menu` | Menú principal con opciones Nueva Partida / Continuar / Opciones / Salir |
| `Cinematica` | Cinemática de introducción al Sector 1 |

---

## Aportes individuales — Examen Final

---

### Degiorgis Camila — Sistema de Oxígeno + Game Over por Asfixia

#### A. Sistema de Oxígeno — Soporte vital y Cápsula de Carbón

**Descripción:** Sistema de supervivencia continuo que implementa la mecánica corazón del GDD. El tanque de O₂ se consume de forma constante por frame, con un multiplicador del 20% al correr. Si llega a cero, la partida termina. Las Cápsulas de Carbón distribuidas en el escenario otorgan recarga instantánea parcial. El estado se comunica en todo momento via barra de O₂ en el HUD.

**Scripts creados:**
- `OxygenSystem.cs` — componente central. Maneja el drenaje por frame, el multiplicador de consumo al correr (`SetSprinting`), la recarga por ítems (`AddOxygen`, `RefillFull`), la pérdida por daño (`LoseOxygen`), y expone `UnityEvent` (`OnOxygenDepleted`) para que la UI y el Game Over reaccionen sin acoplarse directamente al script.
- `CarbonCapsule.cs` — pickup de recolección con animación de flotación (`Mathf.Sin`), sonido y partículas al recolectarse.
- `OxygenBarUI.cs` — controla la barra de O₂ del HUD (`Image` tipo Filled) con cambio de color dinámico según porcentaje restante (óptimo / advertencia / crítico), siguiendo los umbrales de percepción definidos en el GDD.

**Modificaciones a scripts existentes:**
- Script de movimiento de John: llamada a `SetSprinting(true/false)` en la detección de Shift para que correr impacte el consumo.
- Script de vida/daño de John: llamada a `LoseOxygen()` al recibir daño, y lógica de Game Over al recibir `OnOxygenDepleted`.

**Assets generados:** sprite de la Cápsula de Carbón (pixel art), diseño de la barra de O₂ del HUD.

**Prefabs creados:** `CarbonCapsule_Pickup`

#### B. Game Over por Asfixia

**Descripción:** Sistema de fin de partida vinculado al Sistema de Oxígeno. Al agotarse el O₂, se activa un panel de Game Over, el juego se congela (`Time.timeScale = 0`) y se habilita un botón de reintentar que recarga el nivel. Cierra el ciclo de la mecánica de supervivencia que hasta el TP2 no tenía consecuencia real.

**Scripts creados:**
- `GameOverOnAsphyxia.cs` — escucha el evento `OnOxygenDepleted` de `OxygenSystem` y activa el panel de Game Over con el método `RetryLevel()`. La conexión se realizó íntegramente por `UnityEvent` desde el Inspector, sin modificar código ajeno.

**Assets generados:** panel de Game Over (UI), texto del mensaje de muerte por asfixia.

---

### Fernandez Rolón Agustín — Mecánica de Construcción

**Descripción:** Extensión del sistema de construcción existente (que solo soportaba la cama como punto de respawn) hacia un sistema completo con múltiples estructuras, verificación de materiales, preview de colocación con feedback visual y descuento automático de inventario.

**Scripts creados:**
- `BuildManager.cs` — script principal. Gestiona el preview transparente del objeto (overlay verde/rojo según `CanBuild`), verifica disponibilidad de materiales en `Inventory.cs` (`HasRequiredMaterials`), instancia la estructura y descuenta materiales (`TryBuild`). Cancelación con click derecho o Escape.
- `BuildingSlotUI.cs` — controla cada slot del panel de construcción. Actualiza color de texto y sprite del botón (verde/gris) según si el jugador tiene los materiales requeridos.

**Variables configurables en Inspector (`BuildManager`):**

| Variable | Descripción |
|---|---|
| `Build Items` | Lista de estructuras disponibles |
| `Grid Size` | Tamaño de la grilla de snapping |
| `Blocked Layer` | Layer que bloquea la colocación |
| `Check Size` | Área de verificación de colisión |

**Estructuras implementadas:**

| Estructura | Material | Cantidad |
|---|---|---|
| Cama (respawn) | Madera | 5 |
| Pared Horizontal | Madera | 7 |
| Pared Vertical | Madera | 7 |
| Pared Diagonal A | Madera | 7 |
| Pared Diagonal B | Madera | 7 |

**Flujo de uso:** `X` abre panel → selección de estructura → overlay sigue al mouse → click izquierdo para colocar / click derecho o Escape para cancelar.

**Dependencias:** `Inventory.cs` (verificación y descuento de materiales), prefabs de estructuras con `SpriteRenderer` + `Collider2D`, layer `Interactable` en estructuras interactuables.

**Assets generados:** sprites de paredes de madera en perspectiva isométrica (4 orientaciones).

---

### Migueles Mariana — Sistema de Narrativa Ambiental: Radio de Eryx



**Descripción:** Objeto interactuable que encadena tres sistemas: activación narrativa mediante cinemática, destrucción progresiva en 3 estados y drop de recursos al romperse.

**Flujo de interacción:**
1. Jugador se acerca → prompt `E · INTERACTUAR` (via `PlayerInteraction.cs`)
2. Al presionar E → se activa `VideoPanelUI` (Canvas con `VideoPlayer` en Render Texture) y se reproduce `RadioEryx.mp4`
3. Post-cinemática → `cinematicaYaVista = true`, `UsesChopAnimation` pasa a `true`
4. Golpes con hacha → `Interact()` decrementa `currentHP` y actualiza sprite via `damageSprites[]`
5. Al llegar a 0 → `Instantiate(componentePrefab)` con offset de posición, `Destroy(gameObject)`

**Scripts creados:**
- `RadioDeEryx.cs` — implementa `IInteractable`. Maneja el estado de la cinemática, el sistema de daño progresivo y el spawn del drop.

**Prefabs creados:** `RadioDeEryx`, `ComponenteElectronico`

**Assets generados (NanoBanana / Gemini):** sprites isométricos de la radio en 3 estados de daño, sprite del componente electrónico como ítem de suelo, cinemática `RadioEryx.mp4` (editada en CapCut).

**Archivos nuevos:**
```
Assets/Scripts/RadioDeEryx.cs
Assets/Prefabs/RadioDeEryx.prefab
Assets/Prefabs/ComponenteElectronico.prefab
Assets/Sprites/RadioDeEryx/Estado1.png
Assets/Sprites/RadioDeEryx/Estado2.png
Assets/Sprites/RadioDeEryx/Estado3.png
Assets/Sprites/RadioDeEryx/Componente.png
Assets/Cinematicas/RadioEryx.mp4
Assets/Resources/RadioTexture.renderTexture
```

---

### Prepelitchi Iara — Regeneración de vida + Ciclo Día/Noche

#### A. Suero Adrenalínico (regeneración de vida)

**Descripción:** Sistema de emergencia que spawnea automáticamente un vial de suero cuando la vida de John cae al 30%, requiriendo interacción explícita del jugador para consumirlo. La regeneración es gradual (via `Mathf.Lerp`) con resplandor aditivo pulsante en el sprite.

**Scripts creados:**
- `AdrenalineVialPickup.cs` — implementa `IInteractable`. Integrado al sistema de interacción existente del equipo. Controla la lógica de pickup y dispara `StartFastRegen()` en `PlayerHealth.cs`.
- `FloatingItemEffect.cs` — efecto de flotación vertical suave (`Mathf.Sin`) para señalizar el ítem al jugador.

**Modificaciones a scripts existentes (`PlayerHealth.cs`):**
- Desgaste de vida lineal en el tiempo (`perdidaVidaPorSegundo`, configurable desde Inspector)
- `StartFastRegen()` — Coroutine de curación gradual
- `RevisarSiHayQueGenerarVial()` / `GenerarVialCercaDeJohn()` — spawn por código con límite de usos y cooldown

**Prefabs creados:** `Vial_Pickup`, `FloatingTextPrefab`

**Assets generados:** sprite del vial (pixel art), ícono de texto flotante `+HP`

#### B. Ciclo Día/Noche

**Descripción:** Alternancia continua entre día y noche con transición suave basada en función coseno, afectando la intensidad del `Global Light 2D` de URP y reflejada en el HUD mediante un indicador deslizante.

**Scripts creados:**
- `CicloDiaNoche.cs` — modifica `Light2D.intensity` con `Mathf.Cos`. Expone `ValorCiclo` (0–1) como propiedad pública para ser consumida por otros sistemas.
- `CicloDiaNocheUI.cs` — mueve el indicador sol/luna sobre la barra de UI mediante `Mathf.Lerp` según `ValorCiclo`. Separado del script de lógica para respetar separación de responsabilidades.

**Assets generados:** barra de fondo (degradé sol/luna), sprite del indicador deslizante.

---

## Tecnologías y herramientas

| Herramienta | Uso |
|---|---|
| Unity 6000.3.6f1 LTS | Motor de juego |
| C# | Programación |
| Universal Render Pipeline (URP) | Pipeline de renderizado, `Global Light 2D` |
| GitHub Desktop | Control de versiones |
| NanoBanana (Gemini) | Generación de assets con IA |
| Claude (Anthropic) | Asistencia en desarrollo de código (scripts de Unity) |
| CapCut | Edición de cinemáticas |
| Adobe Illustrator / Photoshop | Diseño de interfaz y assets |

---

## Cómo abrir el proyecto

```bash
git clone https://github.com/noireko/videojuegos-proyecto.git
```

1. Instalá **Unity 6000.3.6f1 LTS** desde Unity Hub
2. Abrí la carpeta clonada desde Unity Hub → *Open project from disk*
3. La carpeta `Library/` se regenera automáticamente en la primera apertura (excluida por `.gitignore`)
4. Escena de entrada: `Assets/VJ-MIERCOLES.unity`

---

## Documentación

- [GDD actualizado](https://www.figma.com/proto/KN8gbMxpZ00Q0yB8ttCGLr/videojuegos?node-id=805-2343&m=draw&scaling=scale-down&content-scaling=fixed&page-id=58%3A2&t=sij9EAwOcCQHfogZ-1) 
- [Documentación de tareas — Notion](https://app.notion.com/p/PUNTO-CERO-366bd38e23f4800492cee0238138e276?source=copy_link)
