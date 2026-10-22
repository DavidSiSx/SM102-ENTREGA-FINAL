#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEditor;

// Pruebas de integración con eventos de teclado que pasan por PlayerInput y Send Messages.
// Sólo existe en el editor; nunca forma parte de una compilación del juego.
public class PracticaInputPruebas : MonoBehaviour
{
    private readonly List<string> resultados = new List<string>();
    private readonly List<string> mediciones = new List<string>();
    private Keyboard teclado;
    private InputSettings.BackgroundBehavior fondoAnterior;
    private InputSettings.EditorInputBehaviorInPlayMode focoAnterior;
    private bool segundoPlanoAnterior;
    private int errores;
    private int logsCaida;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        teclado = Keyboard.current;
        fondoAnterior = InputSystem.settings.backgroundBehavior;
        focoAnterior = InputSystem.settings.editorInputBehaviorInPlayMode;
        segundoPlanoAnterior = Application.runInBackground;
        Application.runInBackground = true;
        InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
        InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
        Application.logMessageReceived += RegistrarLog;
        StartCoroutine(EjecutarProtegido());
    }

    void RegistrarLog(string mensaje, string pila, LogType tipo)
    {
        if (mensaje == "La pelota cayó al suelo.") logsCaida++;
        if (tipo == LogType.Error || tipo == LogType.Exception || tipo == LogType.Assert)
        {
            errores++;
            resultados.Add("ERROR EN EJECUCIÓN: " + mensaje);
        }
    }

    void Verificar(bool condicion, string nombre)
    {
        resultados.Add((condicion ? "PASS: " : "FAIL: ") + nombre);
        if (!condicion) errores++;
    }

    void Teclas(params Key[] teclas) => InputSystem.QueueStateEvent(teclado, new KeyboardState(teclas));
    IEnumerator Esperar(float segundos) { yield return new WaitForSeconds(segundos); }
    IEnumerator Abrir(string escena)
    {
        Teclas();
        yield return null;
        SceneManager.LoadScene(escena);
        yield return null;
        yield return null;
        yield return Esperar(0.08f);
    }

    IEnumerator EjecutarProtegido()
    {
        IEnumerator pruebas = Ejecutar();
        while (true)
        {
            object actual = null;
            bool siguiente = false;
            try { siguiente = pruebas.MoveNext(); if (siguiente) actual = pruebas.Current; }
            catch (Exception ex) { resultados.Add("FAIL: " + ex); errores++; }
            if (!siguiente) break;
            yield return actual;
        }
        Teclas();
        yield return null;
        InputSystem.settings.backgroundBehavior = fondoAnterior;
        InputSystem.settings.editorInputBehaviorInPlayMode = focoAnterior;
        Application.runInBackground = segundoPlanoAnterior;
        Application.logMessageReceived -= RegistrarLog;
        Directory.CreateDirectory("Documentacion");
        string informe = "# Verificación automática en Unity Play Mode\n\n" +
            "Editor: " + Application.unityVersion + " · Input System: 1.20.0\n\n" +
            "Método: eventos KeyboardState enviados a Input System; callbacks reales de PlayerInput, movimiento de fotogramas, simulación física y colisiones.\n\n" +
            string.Join("\n", resultados.ConvertAll(r => "- " + r)) + "\n\n" +
            "## Mediciones\n\n" + string.Join("\n\n", mediciones) + "\n\n" +
            "Resultado: " + (errores == 0 ? "TODAS LAS COMPROBACIONES CORRECTAS" : errores + " FALLOS") + "\n";
        File.WriteAllText("Documentacion/RESULTADOS_INPUT_SYSTEM.md", informe);
        File.WriteAllText("Temp/PracticaInputVerificador.result", errores == 0 ? "OK" : informe);
        Debug.Log("[Input System] Verificación terminada: " + errores + " fallos.");
        EditorApplication.isPlaying = false;
        if (Application.isBatchMode) EditorApplication.Exit(errores == 0 ? 0 : 1);
    }

    IEnumerator Ejecutar()
    {
        Verificar(teclado != null, "Teclado disponible");
        yield return Abrir("12_Ejercicio1_ConfigurarAcciones");
        // El receptor se agrega sólo durante la prueba; no forma parte de la escena entregada.
        var monitor = GameObject.Find("Jugador").AddComponent<PracticaInputMonitorPruebas>();
        Teclas(Key.W); yield return Esperar(0.12f);
        Verificar(monitor.Movimiento == Vector2.up, "Ejercicio 1: W produce Move (0,1)");
        Teclas(Key.RightArrow); yield return Esperar(0.12f);
        Verificar(monitor.Movimiento == Vector2.right, "Ejercicio 1: flecha derecha produce Move (1,0)");
        Teclas(Key.W, Key.D); yield return Esperar(0.12f);
        Verificar(Mathf.Abs(monitor.Movimiento.magnitude - 1f) < 0.01f, "Composite 2DVector normaliza la diagonal por defecto");
        Teclas(); yield return Esperar(0.1f);
        Verificar(monitor.Movimiento == Vector2.zero, "Soltar las teclas comunica Move (0,0)");
        Teclas(Key.Space); yield return Esperar(0.4f);
        Verificar(monitor.Saltos == 1, "Jump se ejecuta una vez al mantener espacio");
        Teclas(); yield return Esperar(0.08f);
        Teclas(Key.C); yield return Esperar(0.1f);
        Teclas(); yield return Esperar(0.08f);
        Teclas(Key.V); yield return Esperar(0.1f);
        Verificar(monitor.Cambios == 2, "C y binding personalizado V activan ChangeColor");
        Teclas(); yield return Esperar(0.08f);
        Teclas(Key.F); yield return Esperar(0.1f);
        Verificar(monitor.Cambios == 2, "Binding anterior F ya no activa ChangeColor sin cambiar scripts");
        Verificar(GameObject.Find("Jugador").transform.position == Vector3.zero, "Ejercicio 1 no mueve el objeto");

        yield return Abrir("13_Ejercicio2_MoverPersonaje_InputSystem");
        var jugador = GameObject.Find("Jugador");
        var movimiento = jugador.GetComponent<MovimientoInput>();
        Verificar(jugador.GetComponent<Rigidbody2D>() == null, "Ejercicio 2 sin Rigidbody2D");
        foreach (float velocidad in new[] { 2f, 5f, 10f })
        {
            movimiento.velocidad = velocidad;
            jugador.transform.position = Vector3.zero;
            float inicio = Time.time;
            Teclas(Key.D); yield return Esperar(0.4f);
            Teclas(); yield return null; yield return null;
            float duracion = Time.time - inicio;
            float distancia = jugador.transform.position.x;
            Verificar(Mathf.Abs(distancia / duracion - velocidad) < velocidad * 0.25f, "Transform: velocidad " + velocidad + " unidades/s");
            mediciones.Add(Formato("Transform velocidad {0}: {1:F3} unidades en {2:F3} s.", velocidad, distancia, duracion));
        }
        float xParado = jugador.transform.position.x;
        yield return Esperar(0.2f);
        Verificar(Mathf.Abs(jugador.transform.position.x - xParado) < 0.001f, "Transform se detiene al soltar");
        jugador.transform.position = new Vector3(-3f, 1f, 0f);
        jugador.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        Teclas(Key.UpArrow); yield return Esperar(0.2f);
        Teclas(); yield return null;
        Verificar(jugador.transform.position.y > 1.1f && Mathf.Abs(jugador.transform.position.x + 3f) < 0.01f, "Transform: flechas, escala y posición inicial modificadas");

        yield return Abrir("14_Ejercicio3_FuerzasImpulsoColor");
        jugador = GameObject.Find("JugadorFisico");
        var rb = jugador.GetComponent<Rigidbody2D>();
        var fisica = jugador.GetComponent<MovimientoFisicoInput>();
        Verificar(rb.bodyType == RigidbodyType2D.Dynamic && rb.gravityScale == 0, "Ejercicio 3 Dynamic y gravedad cero");
        Teclas(Key.D); yield return Esperar(0.5f);
        Teclas(); yield return Esperar(0.12f);
        float inercia = rb.linearVelocity.x;
        yield return Esperar(0.25f);
        Verificar(inercia > 1f && Mathf.Abs(rb.linearVelocity.x - inercia) < 0.15f, "AddForce acelera y conserva la velocidad sin entrada");
        mediciones.Add(Formato("Ejercicio 3: velocidad por inercia {0:F3} unidades/s.", inercia));
        Teclas(Key.LeftArrow); yield return Esperar(0.2f);
        Teclas(); yield return Esperar(0.05f);
        Verificar(rb.linearVelocity.x < inercia, "Flecha izquierda aplica fuerza opuesta");
        Teclas(Key.Space); yield return Esperar(0.4f);
        Verificar(fisica.Impulsos == 1 && rb.linearVelocity.y > 3.8f, "Un único impulso de magnitud 4 al mantener espacio");
        Teclas(); yield return Esperar(0.08f);
        Teclas(Key.C); yield return Esperar(0.1f);
        Verificar(jugador.GetComponent<SpriteRenderer>().color == Color.red, "C cambia a rojo");
        yield return Esperar(0.2f);
        Verificar(jugador.GetComponent<CambioColorInput>().Cambios == 1, "Mantener C no repite el cambio de color");
        Teclas(); yield return Esperar(0.08f);
        Teclas(Key.V); yield return Esperar(0.1f);
        Verificar(jugador.GetComponent<SpriteRenderer>().color == Color.white, "V vuelve a blanco");

        yield return Abrir("15_RetoFinal_MigrarNaveEquilibrio");
        var nave = GameObject.Find("Nave");
        var pelota = GameObject.Find("Pelota").GetComponent<PelotaInput>();
        yield return Esperar(0.6f);
        Verificar(pelota.GetComponent<Collider2D>().IsTouching(nave.GetComponent<Collider2D>()) && pelota.transform.position.y > nave.transform.position.y,
            "La pelota cae y se apoya sobre el collider de la nave");
        Teclas(Key.D); yield return Esperar(0.15f);
        Teclas(); yield return Esperar(0.05f);
        Verificar(nave.GetComponent<Rigidbody2D>().linearVelocity.x > 0.2f, "Reto: WASD mueve la nave por fuerzas");
        Teclas(Key.LeftArrow); yield return Esperar(0.1f);
        Teclas(); yield return Esperar(0.06f);
        Verificar(nave.GetComponent<NaveInput>().Movimiento == Vector2.zero, "Reto: las flechas y liberación llegan a la nave");
        Teclas(Key.C); yield return Esperar(0.1f);
        Verificar(nave.GetComponent<SpriteRenderer>().color == Color.red, "Reto: C cambia el color de la nave");
        Teclas(); yield return Esperar(0.05f);
        Teclas(Key.Space); yield return Esperar(0.1f);
        Verificar(pelota.Impulsos == 1 && pelota.GetComponent<Rigidbody2D>().linearVelocity.y > 1f, "Reto: la pelota recibe espacio compartiendo teclado con la nave");
        Teclas(); yield return Esperar(0.05f);
        Teclas(Key.V); yield return Esperar(0.08f);
        Verificar(nave.GetComponent<SpriteRenderer>().color == Color.white, "Reto: V activa el binding personalizado");
        Teclas(); yield return Esperar(0.05f);
        pelota.GetComponent<Rigidbody2D>().position = new Vector2(5f, -2f);
        pelota.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        yield return Esperar(1f);
        Verificar(pelota.Caidas > 0 && logsCaida > 0, "Colisión con Suelo emite exactamente el mensaje requerido en consola");

        for (int perfil = 0; perfil < 3; perfil++)
        {
            yield return Abrir("15_RetoFinal_MigrarNaveEquilibrio");
            nave = GameObject.Find("Nave");
            pelota = GameObject.Find("Pelota").GetComponent<PelotaInput>();
            rb = nave.GetComponent<Rigidbody2D>();
            nave.GetComponent<NaveInput>().fuerza = perfil == 2 ? 12f : 8f;
            rb.linearDamping = perfil == 0 ? 0f : perfil == 1 ? 2.5f : 4f;
            pelota.fuerzaImpulso = perfil == 2 ? 5f : 4f;
            pelota.GetComponent<Rigidbody2D>().gravityScale = perfil == 2 ? 1.5f : 1f;
            // Medición de la nave aislada: se mantiene la pelota fuera de contacto.
            pelota.GetComponent<Rigidbody2D>().simulated = false;
            Teclas(Key.D); yield return Esperar(0.5f);
            Teclas(); yield return Esperar(0.04f);
            float velocidadSoltar = rb.linearVelocity.x;
            float xSoltar = rb.position.x;
            yield return Esperar(0.5f);
            float residual = rb.linearVelocity.x;
            float deriva = rb.position.x - xSoltar;
            var bola = pelota.GetComponent<Rigidbody2D>();
            bola.position = Vector2.zero;
            bola.linearVelocity = Vector2.zero;
            bola.simulated = true;
            // Evitar contacto para comparar sólo el arco del impulso.
            rb.position = new Vector2(-6f, -1f);
            rb.linearVelocity = Vector2.zero;
            float inicioSalto = Time.time;
            Teclas(Key.Space); yield return Esperar(0.25f);
            Teclas(); yield return Esperar(0.08f);
            Verificar(pelota.Impulsos == 1, "Configuración " + (perfil + 1) + ": impulso funcional");
            float alturaMaxima = bola.position.y;
            while (bola.position.y > 0f && Time.time - inicioSalto < 2f)
            {
                yield return new WaitForFixedUpdate();
                alturaMaxima = Mathf.Max(alturaMaxima, bola.position.y);
            }
            float vuelo = Time.time - inicioSalto;
            Verificar(bola.position.y <= 0f && alturaMaxima > 0.3f, "Configuración " + (perfil + 1) + ": gravedad completa el arco del salto");
            mediciones.Add(Formato("Configuración {0}: fuerza={1}, damping={2}, gravedad pelota={3}, impulso={4}. Velocidad al soltar={5:F3}; tras 0.5 s={6:F3}; deriva={7:F3}; altura máxima del salto={8:F3}; tiempo hasta volver a y=0={9:F3} s.",
                perfil + 1, nave.GetComponent<NaveInput>().fuerza, rb.linearDamping, bola.gravityScale,
                pelota.fuerzaImpulso, velocidadSoltar, residual, deriva, alturaMaxima, vuelo));
            Verificar(perfil == 0 ? Mathf.Abs(residual - velocidadSoltar) < 0.2f : residual < velocidadSoltar * 0.5f,
                "Configuración " + (perfil + 1) + ": inercia/frenado esperado");
        }
    }

    static string Formato(string formato, params object[] valores) => string.Format(CultureInfo.InvariantCulture, formato, valores);
}

public class PracticaInputMonitorPruebas : MonoBehaviour
{
    public Vector2 Movimiento { get; private set; }
    public int Saltos { get; private set; }
    public int Cambios { get; private set; }
    public void OnMove(InputValue value) => Movimiento = value.Get<Vector2>();
    public void OnJump(InputValue value) { if (value.isPressed) Saltos++; }
    public void OnChangeColor(InputValue value) { if (value.isPressed) Cambios++; }
}
#endif
