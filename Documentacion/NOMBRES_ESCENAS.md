# Escenas del proyecto

Todas se encuentran en `Assets/Scenes`. Los nombres se cambiaron conservando los contenidos y los GUID de Unity.

## New Input System

| Nombre anterior | Nombre actual | Contenido |
| --- | --- | --- |
| Escena00_InputActions | Input_01_Configuracion_SinMovimiento | Configuración de PlayerControls y PlayerInput. No tiene controlador de movimiento; es normal que el cuadrado permanezca quieto. |
| Escena01_InputMovement | Input_02_Movimiento_Transform | Movimiento del jugador con WASD o flechas mediante Transform.Translate. |
| Escena02_InputPhysics | Input_03_Fuerzas_Impulso_Color | Movimiento por fuerzas, impulso con espacio y cambio de color con C o V. |
| Reto_Input_Equilibrio | Input_04_Equilibrio_Nave_Pelota | Nave y pelota con físicas, controles compartidos y detección de caída al suelo. |

## Prácticas de física anteriores

| Nombre anterior | Nombre actual | Contenido |
| --- | --- | --- |
| Escena01_Transform | Fisica_01_Movimiento_Transform | Movimiento directo del cuadrado con Transform. |
| Escena02_AddForce | Fisica_02_Impulso_Pelota | Pelota que recibe un impulso sobre un suelo. |
| Escena03_LinearVelocity | Fisica_03_Velocidad_Rebote_Pelota | Pelota con velocidad inicial y material de rebote sobre una plataforma. |
| Reto_Final_Equilibrio | Fisica_04_Equilibrio_ControlesClasicos | Nave de equilibrio con los controles de la práctica anterior. |

## Otras escenas

| Nombre anterior | Nombre actual | Contenido |
| --- | --- | --- |
| Escena08 | Arkanoid_Bloques_Destruibles | Pelota, plataforma, bloques destruibles y bloque móvil. |
| Jueguito | Nave_Movimiento_Colisiones | Movimiento de una nave y detección de colisión para perder. |
| Actividad | Tablero_Muros_Pelotas | Construcción del tablero con muros, palos y pelotas. |
| SampleScene | Objetos_Padre_Hijo_Monedas | Objetos Padre e Hijo y monedas. |
| Test | Prueba_Plataforma_Jugador_Enemigo | Escena de prueba con plataforma, jugador y enemigo. |

La plantilla `Assets/Settings/Scenes/URP2DSceneTemplate.unity` conserva su nombre porque es una plantilla del editor.
