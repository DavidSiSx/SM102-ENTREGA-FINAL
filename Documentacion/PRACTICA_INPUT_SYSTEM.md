# Práctica 6 — New Input System

Proyecto: SM102-CLASE · Materia: Desarrollo de Videojuegos.

Los nombres descriptivos actuales y su equivalencia con los nombres originales de la guía están en [NOMBRES_ESCENAS.md](NOMBRES_ESCENAS.md).

La guía indica Unity 6000.6.3f1. El proyecto está abierto y se ha trabajado con **Unity 6000.6.4f1**, con **Input System 1.20.0**, conservando la versión del editor del proyecto.

## Abrir y probar, paso a paso

1. Abre el proyecto en Unity 6000.6.4f1 y deja terminar la importación.
2. En `Window > Package Management > Package Manager`, comprueba `Input System 1.20.0`. Está declarado en `Packages/manifest.json`.
3. En `Edit > Project Settings > Player > Other Settings`, `Active Input Handling` está en **Both**. Así pueden ejecutarse también las escenas anteriores que usan `Input.GetKey` o `Input.GetAxisRaw`.
4. Abre una de las cuatro escenas siguientes desde `Assets/Scenes`. Cada ejercicio tiene su escena y sus objetos; las escenas de las clases anteriores se conservan.
5. Pulsa Play y haz clic en la vista Game para que reciba el teclado.
6. Detén Play y abre la escena del siguiente ejercicio desde Project. Cambia velocidad, escala, posición y valores físicos desde el Inspector, como indica la guía. Los cambios hechos durante Play se descartan al detenerlo.

| Ejercicio | Escena | Qué revisar |
| --- | --- | --- |
| 1. Configuración | `Input_01_Configuracion_SinMovimiento` | Abre PlayerControls en el editor de Input Actions y revisa el mapa Player y sus bindings. |
| 2. Movimiento | `Input_02_Movimiento_Transform` | `Jugador` se mueve directamente con `Transform.Translate`, sin Rigidbody2D. |
| 3. Física | `Input_03_Fuerzas_Impulso_Color` | `JugadorFisico` acelera por fuerzas, conserva inercia, recibe un impulso y alterna su color. |
| Reto final | `Input_04_Equilibrio_Nave_Pelota` | Nave y pelota con físicas, teclado compartido, cambio de color y detección de Suelo. |

Las cuatro escenas están incluidas al principio de Build Settings. No contienen paneles, menús ni interfaz de juego añadida.

## Ejercicio 1 — Configurar acciones

1. Se verificó el paquete instalado y se conservó `Both` para los ejercicios previos.
2. Se completó `Assets/Settings/PlayerControls.inputactions`.
3. El mapa se llama exactamente **Player**.
4. Se configuró **Move**, tipo `Value`, control `Vector2`, con dos composites `2DVector`: WASD y flechas.
5. Se configuró **Jump**, tipo `Button`, en espacio.
6. Se configuró **ChangeColor**, tipo `Button`, en C y un binding alternativo personalizado en V.
7. Se guardó el asset sin generar una clase C#: las notificaciones llegan mediante `PlayerInput` en `Send Messages`.
8. Se reservó una escena exclusiva para este ejercicio. El cuadrado no tiene controlador de movimiento; la configuración se revisa desde el editor de Input Actions.

| Acción | Tipo / Control | Binding |
| --- | --- | --- |
| Move | Value / Vector2 | W, A, S, D; flechas arriba, abajo, izquierda, derecha |
| Jump | Button / Button | Espacio |
| ChangeColor | Button / Button | C; V como alternativa personalizada |

**Personalización obligatoria:** el binding alternativo de color que estaba en **F** se reasignó a **V** únicamente en Input Actions. Los scripts no consultan ninguna de esas teclas. Se conservó C para cumplir también el control base de la guía. La verificación comprueba que C y V funcionan y que F ya no cambia el color.

### Respuestas de análisis

1. `Move` representa la intención de moverse. El código recibe una dirección sin depender de una tecla concreta.
2. La acción es lo que se solicita; el binding conecta esa solicitud con un control físico.
3. `Vector2` contiene dos componentes: horizontal X y vertical Y.
4. WASD y las flechas ofrecen dos formas de mover el mismo personaje sin duplicar el controlador.
5. Para gamepad se pueden agregar bindings, por ejemplo `<Gamepad>/leftStick` para Move, y un esquema de control si se necesita seleccionar dispositivos.

## Ejercicio 2 — Mover con Transform

1. Se creó `Input_02_Movimiento_Transform` con cámara y `Jugador` en `(0,0,0)`, escala `(1,1,1)`, de color cian.
2. Se añadió `MovimientoInput.cs`, con velocidad inicial 5.
3. Se añadió `PlayerInput`: `Actions = PlayerControls`, `Default Map = Player`, `Behavior = Send Messages`.
4. `OnMove(InputValue)` almacena el último vector recibido, incluido cero al soltar.
5. `Update()` llama a `Transform.Translate(direccion * velocidad * Time.deltaTime)`.
6. No se añadió Rigidbody2D porque el movimiento de este ejercicio es directo.
7. Se probaron velocidades **2, 5 y 10**, escala **1.5** y posición **(-3,1)**, cambiando las propiedades correspondientes. La escena se guarda en `(0,0,0)`, escala `(1,1,1)` y velocidad 5.
8. Se verificaron WASD, flechas, liberación de teclas y diagonal mediante eventos de Input System y pruebas de integración.

**Diagonal:** el composite `2DVector` usa de forma predeterminada el modo digital normalizado. Por eso W+D produce aproximadamente `(0.707, 0.707)`, con magnitud 1. En esta configuración no se recorre más distancia en diagonal. Si se cambiara el composite a `Digital` sin normalizar, `(1,1)` tendría magnitud aproximada 1.414; podría limitarse con `Vector2.ClampMagnitude(movimiento, 1f)` o usar el modo normalizado del composite. No conviene normalizar indiscriminadamente un stick analógico si se desea conservar su intensidad.

### Respuestas de análisis

1. `OnMove` lee el valor de Move y lo almacena para que Update lo utilice.
2. No hace falta `Input.GetKey`: los bindings ya convierten las teclas en una acción.
3. `PlayerInput` recibe la acción y envía el mensaje `OnMove` a los componentes del mismo objeto; `MovimientoInput` decide cómo moverlo.
4. Si la acción se llamara `Movement`, buscaría `OnMovement`. `OnMove` dejaría de recibir esa entrada.
5. Se pueden cambiar las teclas en el asset porque el controlador depende del nombre y del valor de la acción.

## Ejercicio 3 — Fuerza, impulso y color

1. Se creó `Input_03_Fuerzas_Impulso_Color` con `JugadorFisico` en `(0,0,0)`, escala `(1,1,1)` y color inicial blanco.
2. Se añadieron `BoxCollider2D` y `Rigidbody2D`, `Dynamic`, gravedad 0 y masa 1.
3. Se añadió `PlayerInput` con el mismo asset, mapa Player y Send Messages.
4. `MovimientoFisicoInput` aplica `AddForce(movimiento * fuerza)` en `FixedUpdate`, con fuerza inicial 8.
5. `OnJump` aplica un impulso vertical de 4 únicamente si `value.isPressed`.
6. `CambioColorInput` alterna entre rojo y blanco cuando recibe ChangeColor. El primer cambio deja el objeto rojo.
7. La fuerza puede modificarse desde el Inspector. Para recuperar la posición inicial, detén y vuelve a iniciar Play.

Las acciones Button no tienen interacciones de repetición configuradas. Mantener espacio produce un único impulso; liberar el botón no aplica otro. La misma comprobación se realiza para mantener C: no alterna el color repetidamente.

### Respuestas de análisis

1. Transform cambia la posición directamente; AddForce produce aceleración y respeta masa, inercia y contactos.
2. `FixedUpdate` coincide con los pasos de la simulación física y evita aplicar la fuerza según la cantidad de fotogramas dibujados.
3. `ForceMode2D.Impulse` aplica un impulso instantáneo: cambia la velocidad según el impulso y la masa del cuerpo.
4. El color es una propiedad del SpriteRenderer, independiente de la simulación del Rigidbody2D.
5. Separar scripts permite modificar la apariencia sin alterar el movimiento y reutilizar el cambio de color en la nave.

## Reto final — Nave de equilibrio

1. Se creó la escena independiente `Input_04_Equilibrio_Nave_Pelota`.
2. **Nave:** cuadrado en `(0,-1,0)`, escala `(3,0.5,1)`, BoxCollider2D, Rigidbody2D Dynamic, gravedad 0, masa 1, fuerza 8, damping lineal 2.5 y rotación bloqueada.
3. **Pelota:** círculo en `(0,0,0)`, escala `(0.5,0.5,1)`, CircleCollider2D, Rigidbody2D Dynamic, masa 1 y gravedad 1. Cae sobre la nave al iniciar.
4. **Suelo:** cuadrado estático en `(0,-4,0)`, escala `(14,1,1)`, BoxCollider2D, sin Rigidbody2D.
5. Se añadieron `NaveInput`, `CambioColorInput` y PlayerInput a la nave.
6. Se añadieron `PelotaInput` y PlayerInput a la pelota. Ambos PlayerInput usan PlayerControls, mapa Player y Send Messages.
7. Se añadió `TecladoCompartidoInput` a ambos: en Start empareja explícitamente el mismo teclado con los dos usuarios. Esto permite cumplir el diseño de la guía con dos PlayerInput sin que uno reserve el teclado y deje al otro sin entradas.
8. La nave responde a Move y ChangeColor; la pelota responde a Jump. Las notificaciones sin receptor se ignoran mediante el comportamiento propio de PlayerInput.
9. El contacto de la pelota con el objeto llamado exactamente `Suelo` ejecuta `OnCollisionEnter2D` y escribe **La pelota cayó al suelo.** en la consola.

La nave sigue siendo un cuerpo dinámico: aunque su propia Gravity Scale sea cero, recibe el peso de la pelota mediante los contactos y puede descender. Se puede compensar con movimiento vertical. La prueba de apoyo comprueba el contacto entre colliders y la posición de la pelota respecto a la nave, no una altura absoluta constante.

### Tres configuraciones reproducibles

Las tres configuraciones se reproducen cambiando los valores en el Inspector. La escena se guarda con la configuración **2, Equilibrada**.

| Perfil | Fuerza nave | Linear Damping nave | Impulso pelota | Gravity Scale pelota | Propósito |
| --- | ---: | ---: | ---: | ---: | --- |
| 1. Sin freno | 8 | 0 | 4 | 1 | Observar cómo continúa el movimiento después de soltar. |
| 2. Equilibrada | 8 | 2.5 | 4 | 1 | Frenar gradualmente y facilitar corregir la posición. |
| 3. Gravedad alta | 12 | 4 | 5 | 1.5 | Comparar frenado más fuerte, mayor fuerza y arco de salto más corto. |

Las mediciones y los resultados efectivamente obtenidos están en [RESULTADOS_INPUT_SYSTEM.md](RESULTADOS_INPUT_SYSTEM.md). El protocolo mantiene la pelota sin contacto durante la medición de frenado de la nave, aplica dirección derecha aproximadamente 0.5 segundos y mide la velocidad y la deriva durante otros 0.5 segundos sin entrada. Después compara un impulso vertical de la pelota sin contacto con la nave. La prueba de equilibrio y la colisión con Suelo se ejecutan aparte con las físicas activas.

**Resultados y elección:** en la configuración 1 la nave conservó su velocidad después de soltar y recorrió aproximadamente 2 unidades adicionales en medio segundo; fue la más difícil de detener. En la configuración 2 perdió cerca del 70 % de su velocidad en ese intervalo y la deriva se redujo a unas 0.6 unidades; por eso se eligió como valor guardado. En la configuración 3 el frenado fue todavía más fuerte, con una deriva cercana a 0.5 unidades. Su mayor gravedad exige corregir la posición de la nave con menos tiempo disponible durante el salto. Los valores exactos de velocidad, altura máxima y duración del salto quedan registrados en el informe de pruebas.

Para repetir la caída manualmente, mueve la nave a un lado para que la pelota toque el suelo. Abre `Window > General > Console` para ver el mensaje. Reinicia antes de comparar perfiles si quieres partir de la misma posición.

### Comparación de sistemas

El sistema clásico consulta controles concretos en el script. Con Input System, el script recibe intenciones como Move o Jump y el asset mantiene los bindings. Esto permite cambiar una tecla o agregar otro dispositivo sin reescribir la mecánica. También permite agrupar acciones en mapas y habilitar sólo las que corresponden al estado del juego. La física sigue dependiendo de Rigidbody2D y de las fuerzas; migrar las entradas no obliga a reemplazarla.

## Reflexión final

1. **Acción y binding:** la acción expresa lo que quiere hacer el jugador; el binding indica con qué control lo pide.
2. **Varias distribuciones:** una acción admite varios bindings y todos producen el mismo tipo de dato. WASD y flechas alimentan el mismo Move.
3. **PlayerInput:** conecta un asset de acciones y sus dispositivos con el objeto y envía las notificaciones configuradas.
4. **Move y OnMove:** con Send Messages, PlayerInput añade el prefijo On al nombre de la acción y busca ese método en el mismo objeto.
5. **Transform y fuerzas:** Transform responde inmediatamente y se detiene al dejar de moverlo. Las fuerzas aceleran el cuerpo; al soltar puede seguir por inercia y choca mediante los colliders.
6. **Problema de la migración:** tener dos PlayerInput exige compartir explícitamente el teclado en este reto. Se resolvió emparejándolo con los dos usuarios, manteniendo cada responsabilidad en su objeto.
7. **Agregar gamepad:** agregaría el stick izquierdo a Move y botones a Jump y ChangeColor desde Input Actions. Después revisaría los esquemas y el emparejamiento de dispositivos; los controladores ya reciben Vector2 y Button y pueden reutilizarse.
8. **Separar mapas por estado:** permite deshabilitar acciones de juego al abrir un menú y evitar, por ejemplo, que confirmar una opción también haga saltar a la pelota.

## Archivos y verificación

- Controladores en `Assets/Scripts`: MovimientoInput, MovimientoFisicoInput, CambioColorInput, NaveInput y PelotaInput.
- `TecladoCompartidoInput` permite que los dos PlayerInput del reto utilicen el mismo teclado.
- `Prácticas > Input System > Crear las cuatro escenas` permite reconstruir únicamente estas cuatro escenas. Sobrescribe sus contenidos, por lo que se debe usar sólo si se desea regenerarlas.
- `Prácticas > Input System > Verificar en Play Mode` ejecuta las pruebas de integración y escribe `Documentacion/RESULTADOS_INPUT_SYSTEM.md`. Durante esta prueba no se debe utilizar el teclado para jugar; el verificador inyecta eventos de prueba y recarga escenas. Al terminar restaura las opciones temporales de entrada y vuelve al ejercicio 1.
- La prueba usa los mismos componentes y callbacks de las escenas; no llama directamente a OnMove, OnJump ni OnChangeColor.
- Las escenas antiguas y sus scripts se mantienen para comparar los sistemas. No son dependencias de los ejercicios nuevos.


## Referencias de la guía

- [Input System en Unity 6.6](https://docs.unity.com/en-us/engine/6000.6/manual/packages-list/packages-all/pack-safe/com-unity-inputsystem)
- [Input Actions 1.20](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/manual/Actions.html)
- [PlayerInput 1.20](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/manual/PlayerInput.html)
