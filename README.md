# SM102 - Entregable Final de Prácticas y Ejercicios Unity

Repositorio oficial de entrega para el curso **SM102**. Contiene todas las actividades, prácticas y ejercicios resueltos, organizados, verificados y listos para su ejecución en Unity.

---

## 📌 Requisitos y Versión del Motor

- **Motor de Juego:** Unity `6000.0.64f1` (Unity 6 / `6000.6.4f1`)
- **Render Pipeline:** Universal Render Pipeline (URP 2D)
- **Input System:** Unity New Input System (`com.unity.inputsystem`) habilitado junto con el sistema clásico (Both) para compatibilidad total con todas las prácticas.

### Cómo abrir el proyecto:
1. Clonar este repositorio en su equipo local.
2. Abrir **Unity Hub**.
3. Seleccionar **Add** > **Add project from disk** y apuntar a la carpeta raíz del repositorio.
4. Asegurarse de tener instalada la versión **Unity 6000.0.64f1** (o compatible con Unity 6).
5. Abrir el proyecto y permitir que Unity importe los assets y compile los ensamblados.
6. Ir a `Assets/Scenes/` en la ventana *Project* y hacer doble clic en la escena que desee probar.

---

## 📋 Tabla de Correspondencia de las 15 Actividades

Todas las actividades requeridas están completamente cubiertas con escenas dedicadas, objetos en jerarquía, componentes funcionales y scripts limpios sin referencias rotas:

| # | Actividad Requerida | Escena en `Assets/Scenes/` | Script(s) Principal(es) | Comportamiento / Objetivo |
|---|---------------------|----------------------------|-------------------------|---------------------------|
| **1** | SampleScene — objetos simples | `01_SampleScene_ObjetosSimples.unity` | Jerarquía Transform | Estructura padre-hijo, organización de sprites y monedas. |
| **2** | Scene 1 — físicas básicas | `02_Scene1_FisicasBasicas.unity` | Físicas 2D (`Rigidbody2D`, `BoxCollider2D`) | Caída por gravedad, colisión con suelo estático. |
| **3** | Plinko | `03_Plinko.unity` | `PhysicsMaterial2D` de rebote | Tablero con clavijas (`Palo`), muros separadores y pelotas rebotando. |
| **4** | Script Movimiento — básico | `04_ScriptMovimiento_Basico.unity` | `Movimiento.cs` / `Movement.cs` | Objeto con componente `Movimiento` funcional usando `Rigidbody2D` y `AddForce` con flechas del teclado. |
| **5** | Clase 3, Actividad 1 | `05_Clase3_Actividad1.unity` | Plataformas y entidades 2D | Configuración de Player, Enemy y Platform con colliders y capas. |
| **6** | Nave | `06_Nave.unity` | `MovimientoNave.cs` | Nave dinámica (`Rigidbody2D` Dynamic), movimiento en 4 direcciones y colisiones. |
| **7** | Clase 4, Actividad 1 — movimiento con flechas | `07_Clase4_Actividad1_MovimientoFlechas.unity` | `MovimientoFlechas.cs` | Control estricto con flechas del teclado (`KeyCode.UpArrow`, etc.). |
| **8** | Clase 4, Actividad 2 — cambiar las teclas | `08_Clase4_Actividad2_CambiarTeclas.unity` | `MovimientoWASD.cs` | Migración de controles a teclas WASD (`KeyCode.W`, `A`, `S`, `D`). |
| **9** | Clase 5, Ejercicio 1 — movimiento directo con Transform | `09_Clase5_Ejercicio1_Transform.unity` | `MovimientoFisicoTransform.cs` | Desplazamiento directo vía `transform.Translate` con `GetAxisRaw`. |
| **10** | Clase 5, Ejercicio 2 — impulsos y fuerzas con Rigidbody2D | `10_Clase5_Ejercicio2_AddForce.unity` | `ImpulsoFisicoPelota.cs` | Aplicación de fuerzas `AddForce(..., ForceMode2D.Impulse)` al pulsar Espacio. |
| **11** | Clase 5, Ejercicio 3 — velocidad, gravedad y colisiones | `11_Clase5_Ejercicio3_LinearVelocity.unity` | `VelocidadInicialPelota.cs` | Velocidad inicial constante con `linearVelocity`, rebote y gravedad. |
| **12** | Ejercicio 1 — configurar las acciones de entrada | `12_Ejercicio1_ConfigurarAcciones.unity` | `PlayerControls.inputactions` | Configuración del Asset New Input System con Move, Jump y ChangeColor. |
| **13** | Ejercicio 2 — mover un personaje con el New Input System | `13_Ejercicio2_MoverPersonaje_InputSystem.unity` | `MovimientoInput.cs` | `PlayerInput` con comportamiento Send Messages llamando `OnMove`. |
| **14** | Ejercicio 3 — fuerzas, impulso y cambio de color | `14_Ejercicio3_FuerzasImpulsoColor.unity` | `MovimientoFisicoInput.cs`, `CambioColorInput.cs` | Movimiento con fuerzas en `FixedUpdate`, impulso vertical y cambio de color cíclico. |
| **15** | Reto final — migrar la nave de equilibrio | `15_RetoFinal_MigrarNaveEquilibrio.unity` | `NaveInput.cs`, `PelotaInput.cs`, `TecladoCompartidoInput.cs` | Nave y pelota equilibrada con New Input System, detección de caída y reinicio. |

### 🎮 Escenas Adicionales Incluidas
- `Arkanoid_Bloques_Destruibles.unity`: Juego de Arkanoid simple con barra de control, pelota rebotante y bloques destructibles al colisionar.
- `Fisica_04_Equilibrio_ControlesClasicos.unity`: Escena de referencia que conserva la implementación previa de la nave de equilibrio con el sistema de entrada clásico.

---

## 🕹️ Guía de Controles por Actividad

### Actividades 1 a 6 (Básicos y Físicas)
- **`01_SampleScene_ObjetosSimples`**: Escena estática para observar jerarquía y sprites.
- **`02_Scene1_FisicasBasicas`**: Pulsar Play. La esfera cae por gravedad y colisiona firmemente con el suelo.
- **`03_Plinko`**: Pulsar Play. Las pelotas caen a través de la matriz de clavijas rebotando de forma física y realista hacia los contenedores inferiores.
- **`04_ScriptMovimiento_Basico`**:
  - `↑` / `↓` / `←` / `→` (Flechas del teclado): Desplazan el objeto aplicando fuerzas físicas mediante `Movimiento.cs` (`rigidbody2.AddForce(Vector2... * fuerza * Time.deltaTime)`), manteniendo también disponible `Movement.cs`.
- **`05_Clase3_Actividad1`**: Entidades Player, Enemy y Platform con físicas y colliders bien ajustados.
- **`06_Nave`**:
  - Flechas o WASD: Controlan la nave (`MovimientoNave.cs`) desplazándola por el espacio con físicas dinámicas sin caer al vacío (`gravityScale = 0`).

### Actividades 7 y 8 (Clase 4 - Input.GetKey)
- **`07_Clase4_Actividad1_MovimientoFlechas`**:
  - `↑` (Arriba): Mueve hacia arriba.
  - `↓` (Abajo): Mueve hacia abajo.
  - `←` (Izquierda): Mueve hacia la izquierda.
  - `→` (Derecha): Mueve hacia la derecha.
  *(Utiliza estrictamente `Input.GetKey(KeyCode.UpArrow)`, etc.)*
- **`08_Clase4_Actividad2_CambiarTeclas`**:
  - `W`: Mueve hacia arriba.
  - `S`: Mueve hacia abajo.
  - `A`: Mueve hacia la izquierda.
  - `D`: Mueve hacia la derecha.
  *(Utiliza estrictamente `Input.GetKey(KeyCode.W)`, etc.)*

### Actividades 9 a 11 (Clase 5 - Físicas 2D)
- **`09_Clase5_Ejercicio1_Transform`**:
  - `A` / `D` o `←` / `→`: Movimiento horizontal directo modificando el `Transform`.
  - `W` / `S` o `↑` / `↓`: Movimiento vertical directo.
- **`10_Clase5_Ejercicio2_AddForce`**:
  - `Espacio`: Aplica un impulso vertical instantáneo (`AddForce` con `ForceMode2D.Impulse`).
- **`11_Clase5_Ejercicio3_LinearVelocity`**:
  - Pulsar Play: La pelota inicia con una velocidad predeterminada en `linearVelocity`, rebotando indefinidamente entre las paredes y el suelo con su material físico.

### Actividades 12 a 15 (New Input System)
- **`12_Ejercicio1_ConfigurarAcciones`**:
  - Contiene el asset `PlayerControls.inputactions` configurado con Action Map `Player`:
    - `Move`: Value (Vector2) con composiciones 2D para WASD y Teclas de Dirección.
    - `Jump`: Button (Espacio).
    - `ChangeColor`: Button (Tecla C y V).
  - En la escena, un monitor visual confirma la existencia y enlace del asset.
- **`13_Ejercicio2_MoverPersonaje_InputSystem`**:
  - `W` / `A` / `S` / `D` o Flechas: Mueve el personaje en los 4 ejes y diagonales mediante `OnMove(InputValue value)` y `transform.Translate`.
- **`14_Ejercicio3_FuerzasImpulsoColor`**:
  - `A` / `D` o `←` / `→`: Aplica fuerza horizontal continua al `Rigidbody2D` en `FixedUpdate`.
  - `Espacio`: Ejecuta `OnJump` aplicando impulso instantáneo hacia arriba.
  - `C`: Ejecuta `OnChangeColor` alternando cíclicamente la paleta de colores del sprite.
- **`15_RetoFinal_MigrarNaveEquilibrio`**:
  - `A` / `D` o `←` / `→`: Mueve la nave horizontalmente.
  - `J` / `L`: Mueve la pelota horizontalmente sobre la nave para mantener el equilibrio.
  - `Espacio`: Impulso de salto para la pelota.
  - Si la pelota cae de la plataforma y toca el suelo, el sistema detecta la caída y reinicia la posición.

---

## 🛠️ Resumen de Correcciones Realizadas

1. **Scripts vacíos restaurados:**
   - Se completó `Assets/Scripts/Movement.cs` que se encontraba en 0 bytes en el repositorio previo, implementando movimiento continuo con `Input.GetKey`.
2. **Separación de scripts en Clase 4:**
   - Se crearon `MovimientoFlechas.cs` y `MovimientoWASD.cs` como scripts independientes y explícitos para documentar y verificar de manera inequívoca las Actividades 1 y 2 de la Clase 4 sin sobrescribirse mutuamente.
3. **Corrección de Rigidbody2D en la Nave (`06_Nave.unity`):**
   - El objeto `Navecita` estaba erróneamente configurado como `Static` en el proyecto anterior. Se reconfiguró como `Dynamic`, con `gravityScale = 0` y `collisionDetectionMode = Continuous`, asociándole correctamente `MovimientoNave.cs`.
4. **Rescate de Prefabs y Sprites locales:**
   - Se incluyeron los prefabs de Plinko (`Bola.prefab`, `Murito divisor.prefab`, `Palo.prefab`) y los sprites que no estaban versionados en los commits antiguos de Git, manteniendo intactos sus archivos `.meta` y GUIDs originales.
5. **Configuración de Input System Dual:**
   - Se aseguró que `ProjectSettings/ProjectSettings.asset` mantenga `activeInputHandler: 2` (Both), garantizando que tanto los scripts clásicos con `Input.GetKey` como los nuevos scripts basados en New Input System funcionen sin conflicto alguno en el mismo proyecto.
6. **Nomenclatura uniforme:**
   - Todas las escenas fueron renombradas con prefijos numéricos ordenados (`01_...` a `15_...`) indicando con precisión la actividad a la que corresponden para facilitar la navegación y corrección del evaluador.

---

