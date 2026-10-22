using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Generación explícita, reproducible y limitada a las cuatro escenas de esta práctica.
[InitializeOnLoad]
public static class PracticaInputBuilder
{
    public static readonly string[] Rutas = {
        "Assets/Scenes/12_Ejercicio1_ConfigurarAcciones.unity",
        "Assets/Scenes/13_Ejercicio2_MoverPersonaje_InputSystem.unity",
        "Assets/Scenes/14_Ejercicio3_FuerzasImpulsoColor.unity",
        "Assets/Scenes/15_RetoFinal_MigrarNaveEquilibrio.unity"
    };
    const string Solicitud = "Temp/GenerarPracticaInput.request";

    static PracticaInputBuilder() { EditorApplication.update += ProcesarSolicitud; }

    static void ProcesarSolicitud()
    {
        if (!File.Exists(Solicitud) || EditorApplication.isCompiling || EditorApplication.isUpdating || EditorApplication.isPlayingOrWillChangePlaymode) return;
        File.Delete(Solicitud);
        try { Generar(); File.WriteAllText("Temp/PracticaInputBuilder.result", "OK"); }
        catch (Exception ex) { File.WriteAllText("Temp/PracticaInputBuilder.result", ex.ToString()); Debug.LogException(ex); }
    }

    [MenuItem("Prácticas/Input System/Crear las cuatro escenas")]
    public static void Generar()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        var actions = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/Settings/PlayerControls.inputactions");
        if (actions == null) throw new InvalidOperationException("Falta PlayerControls.inputactions");
        Sprite cuadrado = CargarSprite("Square");
        Sprite circulo = CargarSprite("Circle");
        for (int i = 0; i < Rutas.Length; i++)
        {
            Scene escena = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var camara = new GameObject("Main Camera");
            camara.tag = "MainCamera";
            camara.transform.position = new Vector3(0f, i == 3 ? -1.5f : 0f, -10f);
            var cam = camara.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = i == 3 ? 7f : 6f;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.035f, 0.055f, 0.10f);
            camara.AddComponent<AudioListener>();
            if (i < 3)
            {
                GameObject jugador = Objeto(i == 2 ? "JugadorFisico" : "Jugador", cuadrado, Vector3.zero, Vector3.one, i == 2 ? Color.white : new Color(0.1f, 0.85f, 1f));
                if (i == 1) jugador.AddComponent<MovimientoInput>();
                if (i == 2)
                {
                    jugador.AddComponent<BoxCollider2D>();
                    Cuerpo(jugador, 0f, 0f);
                    jugador.AddComponent<MovimientoFisicoInput>();
                    jugador.AddComponent<CambioColorInput>();
                }
                Entrada(jugador, actions);
            }
            else
            {
                var nave = Objeto("Nave", cuadrado, new Vector3(0, -1, 0), new Vector3(3, 0.5f, 1), Color.white);
                nave.AddComponent<BoxCollider2D>();
                Cuerpo(nave, 0f, 2.5f).constraints = RigidbodyConstraints2D.FreezeRotation;
                nave.AddComponent<NaveInput>();
                nave.AddComponent<CambioColorInput>();
                Entrada(nave, actions);
                nave.AddComponent<TecladoCompartidoInput>();
                var pelota = Objeto("Pelota", circulo, Vector3.zero, new Vector3(0.5f, 0.5f, 1), new Color(1f, 0.8f, 0.15f));
                pelota.AddComponent<CircleCollider2D>();
                Cuerpo(pelota, 1f, 0f);
                pelota.AddComponent<PelotaInput>();
                Entrada(pelota, actions);
                pelota.AddComponent<TecladoCompartidoInput>();
                var suelo = Objeto("Suelo", cuadrado, new Vector3(0, -4, 0), new Vector3(14, 1, 1), new Color(0.25f, 0.35f, 0.5f));
                suelo.AddComponent<BoxCollider2D>();
                suelo.isStatic = true;
            }
            EditorSceneManager.SaveScene(escena, Rutas[i]);
        }
        // La práctica inicia con el ejercicio 1 y conserva todas las escenas previas.
        var anteriores = EditorBuildSettings.scenes.Where(s => !Rutas.Contains(s.path));
        EditorBuildSettings.scenes = Rutas.Select(p => new EditorBuildSettingsScene(p, true)).Concat(anteriores).ToArray();
        AssetDatabase.SaveAssets();
        EditorSceneManager.OpenScene(Rutas[0]);
        Debug.Log("[Input System] Cuatro escenas independientes guardadas.");
    }

    static Sprite CargarSprite(string nombre)
    {
        string ruta = "Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/" + nombre + ".png";
        var sprite = AssetDatabase.LoadAllAssetsAtPath(ruta).OfType<Sprite>().FirstOrDefault();
        if (sprite == null) throw new InvalidOperationException("No se encontró el sprite " + ruta);
        return sprite;
    }

    static GameObject Objeto(string nombre, Sprite sprite, Vector3 posicion, Vector3 escala, Color color)
    {
        var go = new GameObject(nombre);
        go.transform.position = posicion;
        go.transform.localScale = escala;
        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = color;
        // Material sin luces: las prácticas se ven igual con el pipeline 2D del proyecto.
        renderer.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
        return go;
    }

    static Rigidbody2D Cuerpo(GameObject go, float gravedad, float damping)
    {
        var rb = go.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravedad;
        rb.linearDamping = damping;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        return rb;
    }

    static void Entrada(GameObject go, InputActionAsset actions)
    {
        var input = go.AddComponent<PlayerInput>();
        input.actions = actions;
        input.defaultActionMap = "Player";
        input.notificationBehavior = PlayerNotifications.SendMessages;
    }
}
