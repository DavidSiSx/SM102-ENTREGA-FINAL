using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

[InitializeOnLoad]
public static class EscenasEjerciciosBuilder
{
    static EscenasEjerciciosBuilder()
    {
        EditorApplication.delayCall += CheckAndGenerateScenes;
    }

    [MenuItem("Prácticas/Generar Todas las Escenas")]
    public static void ForceGenerateAllScenes()
    {
        GenerarTodasLasEscenas(true);
    }

    private static void CheckAndGenerateScenes()
    {
        string s1 = "Assets/Scenes/09_Clase5_Ejercicio1_Transform.unity";
        string s2 = "Assets/Scenes/10_Clase5_Ejercicio2_AddForce.unity";
        string s3 = "Assets/Scenes/11_Clase5_Ejercicio3_LinearVelocity.unity";
        string s4 = "Assets/Scenes/Fisica_04_Equilibrio_ControlesClasicos.unity";

        if (!File.Exists(s1) || !File.Exists(s2) || !File.Exists(s3) || !File.Exists(s4))
        {
            GenerarTodasLasEscenas(false);
        }
    }

    public static void GenerarTodasLasEscenas(bool forzarReemplazo)
    {
        Debug.Log("[Builder] Iniciando generación de escenas y recursos...");

        // 1. Obtener Sprites
        Sprite squareSprite = GetSpriteByGuidOrPath("311925a002f4447b3a28927169b83ea6", "Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Square.png");
        Sprite circleSprite = GetSpriteByGuidOrPath("a86470a33a6bf42c4b3595704624658b", "Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Circle.png");

        // 2. Physics Material 2D para Ejercicio 3
        PhysicsMaterial2D matRebote = ObtenerOCrearPhysicsMaterial();

        // 3. Crear escenas
        GenerarEscena01(squareSprite, forzarReemplazo);
        GenerarEscena02(squareSprite, circleSprite, forzarReemplazo);
        GenerarEscena03(squareSprite, circleSprite, matRebote, forzarReemplazo);
        GenerarRetoFinal(squareSprite, circleSprite, forzarReemplazo);

        // 4. Actualizar Build Settings
        ActualizarBuildSettings();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[Builder] ¡Generación de escenas completada con éxito!");
    }

    private static Sprite GetSpriteByGuidOrPath(string guid, string defaultPath)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        if (!string.IsNullOrEmpty(path))
        {
            Sprite s = LoadSprite(path);
            if (s != null) return s;
        }

        if (!string.IsNullOrEmpty(defaultPath))
        {
            Sprite s = LoadSprite(defaultPath);
            if (s != null) return s;
        }

        return null;
    }

    private static Sprite LoadSprite(string assetPath)
    {
        Sprite sp = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        if (sp != null) return sp;

        Object[] all = AssetDatabase.LoadAllAssetsAtPath(assetPath);
        foreach (var obj in all)
        {
            if (obj is Sprite s) return s;
        }
        return null;
    }

    private static PhysicsMaterial2D ObtenerOCrearPhysicsMaterial()
    {
        string dir = "Assets/Physics";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            AssetDatabase.Refresh();
        }

        string matPath = "Assets/Physics/PelotaRebote.physicsMaterial2D";
        PhysicsMaterial2D mat = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(matPath);
        if (mat == null)
        {
            mat = new PhysicsMaterial2D("PelotaRebote")
            {
                bounciness = 0.75f,
                friction = 0.2f
            };
            AssetDatabase.CreateAsset(mat, matPath);
            AssetDatabase.SaveAssets();
        }
        return mat;
    }

    private static Scene AbrirPlantilla()
    {
        string templatePath = "Assets/Settings/Scenes/URP2DSceneTemplate.unity";
        if (File.Exists(templatePath))
        {
            return EditorSceneManager.OpenScene(templatePath, OpenSceneMode.Single);
        }
        return EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    }

    private static void LimpiarObjetosDeJuego(Scene scene)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name != "Main Camera" && root.name != "Global Light 2D")
            {
                Object.DestroyImmediate(root);
            }
        }
    }

    private static void GenerarEscena01(Sprite squareSprite, bool forzar)
    {
        string ruta = "Assets/Scenes/09_Clase5_Ejercicio1_Transform.unity";
        if (File.Exists(ruta) && !forzar) return;

        Scene scene = AbrirPlantilla();
        LimpiarObjetosDeJuego(scene);

        GameObject jugador = new GameObject("Jugador");
        jugador.transform.position = new Vector3(-5f, 0f, 0f);
        jugador.transform.localScale = new Vector3(1f, 1f, 1f);

        SpriteRenderer sr = jugador.AddComponent<SpriteRenderer>();
        if (squareSprite != null) sr.sprite = squareSprite;
        sr.color = new Color(0f, 0.85f, 1f, 1f); // Cian

        MovimientoTransform mt = jugador.AddComponent<MovimientoTransform>();
        mt.velocidad = 5f;

        EditorSceneManager.SaveScene(scene, ruta);
    }

    private static void GenerarEscena02(Sprite squareSprite, Sprite circleSprite, bool forzar)
    {
        string ruta = "Assets/Scenes/10_Clase5_Ejercicio2_AddForce.unity";
        if (File.Exists(ruta) && !forzar) return;

        Scene scene = AbrirPlantilla();
        LimpiarObjetosDeJuego(scene);

        // Suelo
        GameObject suelo = new GameObject("Suelo");
        suelo.transform.position = new Vector3(0f, -3f, 0f);
        suelo.transform.localScale = new Vector3(12f, 1f, 1f);

        SpriteRenderer srSuelo = suelo.AddComponent<SpriteRenderer>();
        if (squareSprite != null) srSuelo.sprite = squareSprite;
        srSuelo.color = new Color(0.2f, 0.8f, 0.35f, 1f); // Verde

        suelo.AddComponent<BoxCollider2D>();

        // Pelota
        GameObject pelota = new GameObject("Pelota");
        pelota.transform.position = new Vector3(0f, 0f, 0f);
        pelota.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        SpriteRenderer srPelota = pelota.AddComponent<SpriteRenderer>();
        if (circleSprite != null) srPelota.sprite = circleSprite;
        srPelota.color = new Color(0.95f, 0.25f, 0.25f, 1f); // Rojo

        pelota.AddComponent<CircleCollider2D>();

        Rigidbody2D rb = pelota.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;

        ImpulsoPelota ip = pelota.AddComponent<ImpulsoPelota>();
        ip.fuerza = 5f;

        EditorSceneManager.SaveScene(scene, ruta);
    }

    private static void GenerarEscena03(Sprite squareSprite, Sprite circleSprite, PhysicsMaterial2D mat, bool forzar)
    {
        string ruta = "Assets/Scenes/11_Clase5_Ejercicio3_LinearVelocity.unity";
        if (File.Exists(ruta) && !forzar) return;

        Scene scene = AbrirPlantilla();
        LimpiarObjetosDeJuego(scene);

        // Plataforma
        GameObject plataforma = new GameObject("Plataforma");
        plataforma.transform.position = new Vector3(0f, -2f, 0f);
        plataforma.transform.localScale = new Vector3(8f, 0.5f, 1f);

        SpriteRenderer srPlat = plataforma.AddComponent<SpriteRenderer>();
        if (squareSprite != null) srPlat.sprite = squareSprite;
        srPlat.color = new Color(0.25f, 0.55f, 0.9f, 1f); // Azul

        plataforma.AddComponent<BoxCollider2D>();

        // Pelota
        GameObject pelota = new GameObject("Pelota");
        pelota.transform.position = new Vector3(-3f, 1f, 0f);
        pelota.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        SpriteRenderer srPelota = pelota.AddComponent<SpriteRenderer>();
        if (circleSprite != null) srPelota.sprite = circleSprite;
        srPelota.color = new Color(1f, 0.6f, 0.1f, 1f); // Naranja

        CircleCollider2D cc = pelota.AddComponent<CircleCollider2D>();
        if (mat != null) cc.sharedMaterial = mat;

        Rigidbody2D rb = pelota.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;

        VelocidadPelota vp = pelota.AddComponent<VelocidadPelota>();
        vp.velocidadHorizontal = 4f;

        EditorSceneManager.SaveScene(scene, ruta);
    }

    private static void GenerarRetoFinal(Sprite squareSprite, Sprite circleSprite, bool forzar)
    {
        string ruta = "Assets/Scenes/Fisica_04_Equilibrio_ControlesClasicos.unity";
        if (File.Exists(ruta) && !forzar) return;

        Scene scene = AbrirPlantilla();
        LimpiarObjetosDeJuego(scene);

        // Nave
        GameObject nave = new GameObject("Nave");
        nave.transform.position = new Vector3(0f, -1f, 0f);
        nave.transform.localScale = new Vector3(3f, 0.5f, 1f);

        SpriteRenderer srNave = nave.AddComponent<SpriteRenderer>();
        if (squareSprite != null) srNave.sprite = squareSprite;
        srNave.color = new Color(0.2f, 0.7f, 1f, 1f); // Cian/Azul nave

        nave.AddComponent<BoxCollider2D>();

        Rigidbody2D rbNave = nave.AddComponent<Rigidbody2D>();
        rbNave.bodyType = RigidbodyType2D.Dynamic;
        rbNave.gravityScale = 0f;
        rbNave.constraints = RigidbodyConstraints2D.FreezeRotation;
        rbNave.linearDamping = 2.5f;

        NaveEquilibrio ne = nave.AddComponent<NaveEquilibrio>();
        ne.fuerza = 8f;

        CambioColorNave ccn = nave.AddComponent<CambioColorNave>();
        ccn.teclaCambio = KeyCode.C;

        // Pelota
        GameObject pelota = new GameObject("Pelota");
        pelota.transform.position = new Vector3(0f, 0f, 0f);
        pelota.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        SpriteRenderer srPelota = pelota.AddComponent<SpriteRenderer>();
        if (circleSprite != null) srPelota.sprite = circleSprite;
        srPelota.color = new Color(1f, 0.85f, 0.2f, 1f); // Amarillo pelota

        pelota.AddComponent<CircleCollider2D>();

        Rigidbody2D rbPelota = pelota.AddComponent<Rigidbody2D>();
        rbPelota.bodyType = RigidbodyType2D.Dynamic;
        rbPelota.gravityScale = 1f;

        ControlPelota cp = pelota.AddComponent<ControlPelota>();
        cp.velocidadHorizontal = 3f;
        cp.fuerzaImpulso = 4f;

        // Suelo
        GameObject suelo = new GameObject("Suelo");
        suelo.transform.position = new Vector3(0f, -4f, 0f);
        suelo.transform.localScale = new Vector3(14f, 1f, 1f);

        SpriteRenderer srSuelo = suelo.AddComponent<SpriteRenderer>();
        if (squareSprite != null) srSuelo.sprite = squareSprite;
        srSuelo.color = new Color(0.4f, 0.4f, 0.4f, 1f); // Gris suelo

        suelo.AddComponent<BoxCollider2D>();

        EditorSceneManager.SaveScene(scene, ruta);
    }

    private static void ActualizarBuildSettings()
    {
        string[] rutas = new string[]
        {
            "Assets/Scenes/09_Clase5_Ejercicio1_Transform.unity",
            "Assets/Scenes/10_Clase5_Ejercicio2_AddForce.unity",
            "Assets/Scenes/11_Clase5_Ejercicio3_LinearVelocity.unity",
            "Assets/Scenes/Fisica_04_Equilibrio_ControlesClasicos.unity"
        };

        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        HashSet<string> existentes = new HashSet<string>();
        foreach (var s in scenes)
        {
            existentes.Add(s.path);
        }

        bool modificado = false;
        foreach (var r in rutas)
        {
            if (!existentes.Contains(r))
            {
                scenes.Add(new EditorBuildSettingsScene(r, true));
                modificado = true;
            }
        }

        if (modificado)
        {
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
