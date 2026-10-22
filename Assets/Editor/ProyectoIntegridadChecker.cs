using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ProyectoIntegridadChecker
{
    [MenuItem("Prácticas/Comprobar Integridad de Escenas")]
    public static void Comprobar()
    {
        int erroresTotales = 0;
        var reporte = new List<string>();

        string[] escenas = {
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
            "Assets/Scenes/Arkanoid_Bloques_Destruibles.unity",
            "Assets/Scenes/Fisica_04_Equilibrio_ControlesClasicos.unity"
        };

        foreach (var ruta in escenas)
        {
            if (!File.Exists(ruta))
            {
                reporte.Add("ERROR: Falta archivo de escena: " + ruta);
                erroresTotales++;
                continue;
            }

            Scene escena = EditorSceneManager.OpenScene(ruta, OpenSceneMode.Single);
            GameObject[] raices = escena.GetRootGameObjects();
            int componentesNulos = 0;
            int objetosContados = 0;

            foreach (var raiz in raices)
            {
                Transform[] todos = raiz.GetComponentsInChildren<Transform>(true);
                foreach (var t in todos)
                {
                    objetosContados++;
                    Component[] componentes = t.GetComponents<Component>();
                    for (int i = 0; i < componentes.Length; i++)
                    {
                        if (componentes[i] == null)
                        {
                            componentesNulos++;
                            reporte.Add(string.Format("MISSING SCRIPT en escena '{0}', GameObject '{1}' (índice {2})", ruta, t.name, i));
                            erroresTotales++;
                        }
                    }
                }
            }

            reporte.Add(string.Format("OK: Escena '{0}' validada ({1} GameObjects, {2} componentes nulos).", Path.GetFileName(ruta), objetosContados, componentesNulos));
        }

        Directory.CreateDirectory("Documentacion");
        string salida = "# Reporte de Integridad de Escenas y Componentes\n\n" +
                        "Fecha de verificación: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n\n" +
                        string.Join("\n", reporte) + "\n\n" +
                        "Total errores encontrados: " + erroresTotales + "\n";
        File.WriteAllText("Documentacion/REPORTE_INTEGRIDAD_ESCENAS.md", salida);
        Debug.Log("[Integridad] Verificación terminada con " + erroresTotales + " errores.");

        if (Application.isBatchMode)
        {
            EditorApplication.Exit(erroresTotales == 0 ? 0 : 1);
        }
    }
}
