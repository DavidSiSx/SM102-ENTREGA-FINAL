# Práctica de Unity: Físicas y Movimiento
**Nombre:** David Alejandro Sierra Sosa
**Materia:** SM102  

---

### ¿De qué trató la práctica?
En esta clase probamos las diferentes formas que tiene Unity para mover objetos: moverlos directo por código con `Transform`, empujarlos con fuerzas físicas (`AddForce`), o fijarles una velocidad directa (`linearVelocity`). 

Al final armamos un minijuego donde controlamos una nave para mantener una pelota en equilibrio encima, evitando que caiga al suelo.

---

### Resumen de lo que hice en cada escena

1. **Escena 1 (`Fisica_01_Movimiento_Transform`):**
   Hice un cuadrado que se mueve de izquierda a derecha con las flechas del teclado. Usé `transform.Translate`. Aquí el objeto no tiene físicas ni peso, solo cambia de posición en la pantalla.

2. **Escena 2 (`Fisica_02_Impulso_Pelota`):**
   Puse un suelo y una pelota con `Rigidbody2D`. Cuando presiono la barra espaciadora, la pelota recibe un impulso hacia arriba (`AddForce`) y la gravedad la hace caer de vuelta al suelo.

3. **Escena 3 (`Fisica_03_Velocidad_Rebote_Pelota`):**
   Aquí la pelota arranca con una velocidad horizontal fija (`linearVelocity`). Conforme avanza, la gravedad la va jalando hacia abajo hasta que choca con una plataforma. Le puse un material de física para que rebotara al pegar.

4. **Reto Final (`Fisica_04_Equilibrio_ControlesClasicos`):**
   Junté todo: una nave que muevo con las flechas, una pelota arriba que cae por gravedad y que puedo hacer saltar con espacio, y el suelo abajo. Si se me cae la pelota y toca el suelo, sale un aviso en la consola de Unity diciendo *"La pelota cayó al suelo."*. Además, le agregué que al presionar la tecla **C**, la nave cambie de color.

---

### Preguntas de análisis de los ejercicios

#### Ejercicio 1 (Transform.Translate)
* **¿Qué pasa cuando duplicas la velocidad?**  
  El cuadrado se mueve el doble de rápido. Si antes tardaba 2 segundos en cruzar la pantalla, ahora tarda 1 segundo.
* **¿Cambiar la escala modifica la velocidad?**  
  No, la velocidad sigue siendo exactamente la misma. Lo único que cambia es qué tan grande o chico se ve el sprite.
* **¿Por qué se mueve sin tener Rigidbody2D?**  
  Porque el código de `Translate` solo le cambia las coordenadas X y Y directamente al objeto, no necesita del motor de física para funcionar.
* **¿Qué pasaría si intentas hacer una pelota que rebota con esto?**  
  Se vería muy falso y no funcionaría bien. La pelota atravesaría el piso porque no detecta colisiones reales y no caería sola porque no tiene gravedad. Tendría que programarle todo el rebote y la caída a mano.

#### Ejercicio 2 (AddForce e Impulso)
* **¿Qué pasa al aumentar la fuerza del impulso?**  
  La pelota sale disparada mucho más alto y tarda más tiempo en volver a caer al suelo.
* **¿Qué cambia cuando Gravity Scale es 0?**  
  Que la pelota sube y nunca regresa. Como no hay gravedad que la jale hacia abajo, sigue flotando hacia arriba infinitamente.
* **¿Por qué cae más rápido cuando subes la gravedad?**  
  Porque la fuerza que la atrae hacia abajo es más fuerte, así que acelera más rápido en la caída.
* **¿Qué diferencia hay entre una fuerza continua y un impulso?**  
  El impulso es un golpe único instantáneo (como pegarle a una pelota con un bate). La fuerza continua es como empujar un carrito todo el tiempo mientras dejas la tecla presionada.
* **¿Para qué sirve el Rigidbody2D?**  
  Es lo que le da "cuerpo físico" al objeto: hace que tenga peso, que le afecte la gravedad, que pueda chocar y que responda a fuerzas.

#### Ejercicio 3 (linearVelocity)
* **¿Por qué la pelota empieza moviéndose hacia el lado?**  
  Porque en el `Start` del script le dije que arrancara con velocidad en el eje X (`linearVelocity = new Vector2(velocidad, 0)`).
* **¿Por qué termina cayendo si la velocidad vertical inicial era 0?**  
  Porque aunque arrancó sin velocidad hacia abajo, la gravedad empieza a empujarla hacia abajo desde el primer segundo.
* **¿Qué diferencia hay entre linearVelocity y AddForce?**  
  Con `linearVelocity` le obligas al objeto a ir a esa velocidad exacta al instante, sin importar si es pesado o ligero. Con `AddForce` lo empujas, así que si el objeto es muy pesado va a acelerar más lento.
* **¿Qué pasa si quitas la gravedad?**  
  La pelota viaja en línea recta horizontal por siempre sin caer nunca a la plataforma.
* **¿Qué papel tienen los colliders?**  
  Son los que marcan el contorno sólido. Si no estuvieran, la pelota pasaría a través de la plataforma como si fuera un fantasma.
* **¿Por qué el material físico cambia el rebote?**  
  Porque tiene la propiedad `Bounciness` (elasticidad). Si se la subes, la pelota bota con fuerza; si se la dejas en 0, cae como una piedra y no bota nada.

---

### Mis pruebas con el Reto Final (Tabla de experimentos)

| Prueba | Fuerza de Nave | Gravedad de Pelota | Lo que me pasó en la prueba |
| :---: | :---: | :---: | :--- |
| **1** | **4** | **1** | **La nave era muy lenta:** Apenas me movía con las flechas, la pelota ya se había ido hacia un lado y cayó al suelo porque no alcancé a ponerme debajo. |
| **2** | **8** | **1** | **Se pudo jugar bien:** La nave respondía rápido y pude seguir la pelota para que no se cayera, incluso cuando la hacía saltar con espacio. |
| **3** | **8** | **2** | **Casi imposible:** Como la pelota tenía el doble de gravedad, caía rapidísimo. Si la hacía saltar, bajaba casi de golpe y no alcanzaba a reaccionar a tiempo. |

---

### Reflexión final (Con mis propias palabras)

1. **¿En qué se diferencia `Transform.Translate()` de `Rigidbody2D.AddForce()`?**  
   `Transform.Translate` mueve el objeto a la fuerza cambiando su posición en la pantalla (sin físicas ni peso). `AddForce` lo empuja usando el motor de física, respetando su masa, la gravedad y chocando con los demás objetos.

2. **¿Qué diferencia hay entre aplicar un impulso y asignar `linearVelocity`?**  
   El impulso le da un empujón extra a la velocidad que ya llevaba el objeto (como patear un balón en movimiento). En cambio, `linearVelocity` borra cualquier velocidad que traía y le pone una fija en seco.

3. **¿Qué papel desempeña la gravedad?**  
   Jala todo hacia abajo todo el tiempo. Es lo que hace que los saltos tengan una curva natural y que las cosas vuelvan a caer al piso.

4. **¿Por qué necesitamos colliders para detectar contactos físicos?**  
   Porque el Rigidbody solo calcula el peso y la velocidad, pero no sabe qué forma tiene el objeto. El collider es como su cuerpo sólido; sin él, dos objetos se atravesarían.

5. **¿Qué propiedad modificaste para mejorar el equilibrio?**  
   Le subí el **Linear Damping** a la nave (en `2.5`). Antes la nave patinaba como si estuviera en hielo cuando soltaba las teclas; con el damping se frena sola al soltar las flechas y es mucho más fácil de controlar.

6. **¿Qué problema encontraste durante las pruebas y cómo lo resolviste?**  
   Al principio la nave se resbalaba demasiado por la inercia y la pelota se caía muy fácil. Lo solucioné subiendo el Linear Damping en el Inspector y subiéndole un poco la fuerza a la nave para que reaccionara más rápido.

7. **Si tuvieras que construir un proyectil, ¿qué técnica elegirías y por qué?**  
   Si fuera una flecha o una granada, usaría un **impulso con Rigidbody2D (`AddForce`)** para que haga una parábola realista al caer. Pero si fuera un láser de nave espacial, le pondría **velocidad fija con `linearVelocity`** o `Translate` para que vaya en línea recta perfecta.

8. **¿Qué cambiarías si quisieras una nave que se desplazara de manera más controlada y sin inercia?**  
   En vez de usar `AddForce` (que hace que tarde en arrancar y en frenar), le pondría la velocidad directo en el script: `rb.linearVelocity = direccion * velocidad;`. Así, en cuanto suelto la tecla, la velocidad se vuelve 0 y la nave se clava en seco sin resbalar nada.
