using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Inicia pruebas reales en Play Mode; no modifica las escenas guardadas.
[InitializeOnLoad]
public static class PracticaInputVerificador
{
    const string Solicitud = "Temp/VerificarPracticaInput.request";
    const string Activa = "PracticaInputPruebas.Activa";
    static PracticaInputVerificador()
    {
        EditorApplication.update += Revisar;
        EditorApplication.playModeStateChanged += CambioModo;
    }

    static void Revisar()
    {
        if (!File.Exists(Solicitud) || EditorApplication.isCompiling || EditorApplication.isUpdating || EditorApplication.isPlayingOrWillChangePlaymode) return;
        File.Delete(Solicitud);
        Iniciar();
    }

    [MenuItem("Prácticas/Input System/Verificar en Play Mode")]
    public static void Iniciar()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        SessionState.SetBool(Activa, true);
        EditorSceneManager.OpenScene(PracticaInputBuilder.Rutas[0]);
        EditorApplication.isPlaying = true;
    }

    // Punto de entrada para una copia aislada de validación del mismo proyecto.
    public static void EjecutarLote()
    {
        PracticaInputBuilder.Generar();
        Iniciar();
    }

    static void CambioModo(PlayModeStateChange estado)
    {
        if (!SessionState.GetBool(Activa, false)) return;
        if (estado == PlayModeStateChange.EnteredPlayMode)
        {
            var go = new GameObject("Verificador temporal de Input System");
            go.AddComponent<PracticaInputPruebas>();
        }
        if (estado == PlayModeStateChange.EnteredEditMode)
        {
            SessionState.SetBool(Activa, false);
            EditorSceneManager.OpenScene(PracticaInputBuilder.Rutas[0]);
        }
    }
}
