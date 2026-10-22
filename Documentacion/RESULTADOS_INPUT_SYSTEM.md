# Verificación automática en Unity Play Mode

Editor: 6000.6.4f1 · Input System: 1.20.0

Método: eventos KeyboardState enviados a Input System; callbacks reales de PlayerInput, movimiento de fotogramas, simulación física y colisiones.

- PASS: Teclado disponible
- PASS: Ejercicio 1: W produce Move (0,1)
- PASS: Ejercicio 1: flecha derecha produce Move (1,0)
- PASS: Composite 2DVector normaliza la diagonal por defecto
- PASS: Soltar las teclas comunica Move (0,0)
- PASS: Jump se ejecuta una vez al mantener espacio
- PASS: C y binding personalizado V activan ChangeColor
- PASS: Binding anterior F ya no activa ChangeColor sin cambiar scripts
- PASS: Ejercicio 1 no mueve el objeto
- PASS: Ejercicio 2 sin Rigidbody2D
- PASS: Transform: velocidad 2 unidades/s
- PASS: Transform: velocidad 5 unidades/s
- PASS: Transform: velocidad 10 unidades/s
- PASS: Transform se detiene al soltar
- PASS: Transform: flechas, escala y posición inicial modificadas
- PASS: Ejercicio 3 Dynamic y gravedad cero
- PASS: AddForce acelera y conserva la velocidad sin entrada
- PASS: Flecha izquierda aplica fuerza opuesta
- PASS: Un único impulso de magnitud 4 al mantener espacio
- PASS: C cambia a rojo
- PASS: Mantener C no repite el cambio de color
- PASS: V vuelve a blanco
- PASS: La pelota cae y se apoya sobre el collider de la nave
- PASS: Reto: WASD mueve la nave por fuerzas
- PASS: Reto: las flechas y liberación llegan a la nave
- PASS: Reto: C cambia el color de la nave
- PASS: Reto: la pelota recibe espacio compartiendo teclado con la nave
- PASS: Reto: V activa el binding personalizado
- PASS: Colisión con Suelo emite exactamente el mensaje requerido en consola
- PASS: Configuración 1: impulso funcional
- PASS: Configuración 1: gravedad completa el arco del salto
- PASS: Configuración 1: inercia/frenado esperado
- PASS: Configuración 2: impulso funcional
- PASS: Configuración 2: gravedad completa el arco del salto
- PASS: Configuración 2: inercia/frenado esperado
- PASS: Configuración 3: impulso funcional
- PASS: Configuración 3: gravedad completa el arco del salto
- PASS: Configuración 3: inercia/frenado esperado

## Mediciones

Transform velocidad 2: 0.833 unidades en 0.450 s.

Transform velocidad 5: 2.000 unidades en 0.433 s.

Transform velocidad 10: 4.170 unidades en 0.450 s.

Ejercicio 3: velocidad por inercia 4.000 unidades/s.

Configuración 1: fuerza=8, damping=0, gravedad pelota=1, impulso=4. Velocidad al soltar=4.160; tras 0.5 s=4.160; deriva=2.080; altura máxima del salto=0.646; tiempo hasta volver a y=0=0.763 s.

Configuración 2: fuerza=8, damping=2.5, gravedad pelota=1, impulso=4. Velocidad al soltar=2.148; tras 0.5 s=0.634; deriva=0.605; altura máxima del salto=0.653; tiempo hasta volver a y=0=0.766 s.

Configuración 3: fuerza=12, damping=4, gravedad pelota=1.5, impulso=5. Velocidad al soltar=2.224; tras 0.5 s=0.325; deriva=0.475; altura máxima del salto=0.637; tiempo hasta volver a y=0=0.631 s.

Resultado: TODAS LAS COMPROBACIONES CORRECTAS
