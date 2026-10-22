using UnityEngine;

public class DestruirNave : MonoBehaviour
{
    public GameObject pelota;

    private bool perdiste = false;

    void Awake()
    {
        Time.timeScale = 1f;
    }

    // Se ejecuta cuando el collider tiene "Is Trigger" activado (como está en tu Inspector)
    void OnTriggerEnter2D(Collider2D choque)
    {
        EvaluarImpacto(choque.gameObject);
    }

    // Se ejecuta si "Is Trigger" está desactivado (colisión física normal)
    void OnCollisionEnter2D(Collision2D choque)
    {
        EvaluarImpacto(choque.gameObject);
    }

    private void EvaluarImpacto(GameObject objetoChocado)
    {
        if (perdiste || objetoChocado != pelota)
            return;

        perdiste = true;
        Destroy(pelota);
        Time.timeScale = 0f;
    }

    void OnGUI()
    {
        if (!perdiste)
            return;

        GUIStyle estilo = new GUIStyle(GUI.skin.label);
        estilo.fontSize = 50;
        estilo.alignment = TextAnchor.MiddleCenter;
        estilo.normal.textColor = Color.white;

        GUI.Label(
            new Rect(0, 0, Screen.width, Screen.height),
            "PERDISTE",
            estilo
        );
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}