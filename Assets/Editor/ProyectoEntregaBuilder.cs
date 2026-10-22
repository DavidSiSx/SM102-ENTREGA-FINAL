using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

public static class ProyectoEntregaBuilder
{
    [MenuItem("Prácticas/Completar Escenas Faltantes")]
    public static void GenerarTodo()
    {
        Sprite squareSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Square.png");
        Sprite circleSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Circle.png");

        GenerarScene02_FisicasBasicas(squareSprite, circleSprite);
        GenerarScene04_MovimientoBasico(squareSprite);
        GenerarScene07_MovimientoFlechas(squareSprite);
        GenerarScene08_MovimientoWASD(squareSprite);

        ConfigurarBuildSettings();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[Entrega] Todas las escenas y Build Settings configurados correctamente.");
    }

    private static Scene NuevaEscena()
    {
        string templatePath = "Assets/Settings/Scenes/URP2DSceneTemplate.unity";
        if (File.Exists(templatePath))
        {
            return EditorSceneManager.OpenScene(templatePath, OpenSceneMode.Single);
        }
        return EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    }

    private static void Limpiar(Scene scene)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name != "Main Camera" && root.name != "Global Light 2D")
            {
                Object.DestroyImmediate(root);
            }
        }
    }

    private static void GenerarScene02_FisicasBasicas(Sprite square, Sprite circle)
    {
        string ruta = "Assets/Scenes/02_Scene1_FisicasBasicas.unity";
        if (File.Exists(ruta)) return;

        Scene scene = NuevaEscena();
        Limpiar(scene);

        // Suelo estático con BoxCollider2D
        GameObject suelo = new GameObject("Suelo");
        suelo.transform.position = new Vector3(0f, -3.5f, 0f);
        suelo.transform.localScale = new Vector3(12f, 1f, 1f);
        var srSuelo = suelo.AddComponent<SpriteRenderer>();
        if (square != null) srSuelo.sprite = square;
        srSuelo.color = new Color(0.2f, 0.7f, 0.3f);
        suelo.AddComponent<BoxCollider2D>();

        // Pelota dinámica con CircleCollider2D y Rigidbody2D
        GameObject pelota = new GameObject("PelotaFisica");
        pelota.transform.position = new Vector3(-2f, 2f, 0f);
        pelota.transform.localScale = new Vector3(1f, 1f, 1f);
        var srPelota = pelota.AddComponent<SpriteRenderer>();
        if (circle != null) srPelota.sprite = circle;
        srPelota.color = new Color(0.9f, 0.3f, 0.2f);
        pelota.AddComponent<CircleCollider2D>();
        var rbPelota = pelota.AddComponent<Rigidbody2D>();
        rbPelota.bodyType = RigidbodyType2D.Dynamic;
        rbPelota.gravityScale = 1f;

        // Cubo dinámico con BoxCollider2D y Rigidbody2D
        GameObject cubo = new GameObject("CuboFisico");
        cubo.transform.position = new Vector3(2f, 3f, 0f);
        cubo.transform.localScale = new Vector3(1f, 1f, 1f);
        var srCubo = cubo.AddComponent<SpriteRenderer>();
        if (square != null) srCubo.sprite = square;
        srCubo.color = new Color(0.2f, 0.5f, 0.9f);
        cubo.AddComponent<BoxCollider2D>();
        var rbCubo = cubo.AddComponent<Rigidbody2D>();
        rbCubo.bodyType = RigidbodyType2D.Dynamic;
        rbCubo.gravityScale = 1f;

        EditorSceneManager.SaveScene(scene, ruta);
    }

    private static void GenerarScene04_MovimientoBasico(Sprite square)
    {
        string ruta = "Assets/Scenes/04_ScriptMovimiento_Basico.unity";

        Scene scene = NuevaEscena();
        Limpiar(scene);

        GameObject jugador = new GameObject("JugadorMovimientoBasico");
        jugador.transform.position = Vector3.zero;
        jugador.transform.localScale = Vector3.one;
        var sr = jugador.AddComponent<SpriteRenderer>();
        if (square != null) sr.sprite = square;
        sr.color = new Color(0.1f, 0.8f, 0.9f);
        jugador.AddComponent<BoxCollider2D>();
        var rb = jugador.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        var mov = jugador.AddComponent<Movimiento>();
        mov.fuerza = 500f;

        EditorSceneManager.SaveScene(scene, ruta);
    }

    private static void GenerarScene07_MovimientoFlechas(Sprite square)
    {
        string ruta = "Assets/Scenes/07_Clase4_Actividad1_MovimientoFlechas.unity";
        if (File.Exists(ruta)) return;

        Scene scene = NuevaEscena();
        Limpiar(scene);

        GameObject jugador = new GameObject("JugadorFlechas");
        jugador.transform.position = Vector3.zero;
        jugador.transform.localScale = Vector3.one;
        var sr = jugador.AddComponent<SpriteRenderer>();
        if (square != null) sr.sprite = square;
        sr.color = new Color(0.3f, 0.9f, 0.4f);
        var mov = jugador.AddComponent<MovimientoFlechas>();
        mov.velocidad = 5f;

        EditorSceneManager.SaveScene(scene, ruta);
    }

    private static void GenerarScene08_MovimientoWASD(Sprite square)
    {
        string ruta = "Assets/Scenes/08_Clase4_Actividad2_CambiarTeclas.unity";
        if (File.Exists(ruta)) return;

        Scene scene = NuevaEscena();
        Limpiar(scene);

        GameObject jugador = new GameObject("JugadorWASD");
        jugador.transform.position = Vector3.zero;
        jugador.transform.localScale = Vector3.one;
        var sr = jugador.AddComponent<SpriteRenderer>();
        if (square != null) sr.sprite = square;
        sr.color = new Color(0.9f, 0.7f, 0.1f);
        var mov = jugador.AddComponent<MovimientoWASD>();
        mov.velocidad = 5f;

        EditorSceneManager.SaveScene(scene, ruta);
    }

    public static void ConfigurarBuildSettings()
    {
        string[] todasLasEscenas = new string[]
        {
            "Assets/Scenes/01_SampleScene_ObjetosSimples.unity",
            "Assets/Scenes/02_Scene1_FisicasBasicas.unity",
            "Assets/Scenes/03_Plinko.unity",
            "Assets/Scenes/04_ScriptMovimiento_Basico.unity",
            "Assets/Scenes/05_Clase3_Actividad1.unity",
            "Assets/Scenes/06_Nave.unity",
            "Assets/Scenes/07_Clase4_Actividad1_MovimientoFlechas.unity",
            "Assets/Scenes/08_Clase4_Actividad2_CambiarTeclas.unity",
            "Assets/Scenes/09_Clase5_Ejercicio1_Transform.unity",
            "Assets/Scenes/10_Clase5_Ejercicio2_AddForce.unity",
            "Assets/Scenes/11_Clase5_Ejercicio3_LinearVelocity.unity",
            "Assets/Scenes/12_Ejercicio1_ConfigurarAcciones.unity",
            "Assets/Scenes/13_Ejercicio2_MoverPersonaje_InputSystem.unity",
            "Assets/Scenes/14_Ejercicio3_FuerzasImpulsoColor.unity",
            "Assets/Scenes/15_RetoFinal_MigrarNaveEquilibrio.unity",
            "Assets/Scenes/Fisica_04_Equilibrio_ControlesClasicos.unity",
            "Assets/Scenes/Arkanoid_Bloques_Destruibles.unity"
        };

        List<EditorBuildSettingsScene> buildScenes = new List<EditorBuildSettingsScene>();
        foreach (string r in todasLasEscenas)
        {
            if (File.Exists(r))
            {
                buildScenes.Add(new EditorBuildSettingsScene(r, true));
            }
        }
        EditorBuildSettings.scenes = buildScenes.ToArray();
    }
}
