using UnityEngine;
using UnityEngine.SceneManagement;

public class PelotaArkanoid : MonoBehaviour
{
    public float velocidad = 6f;
    public Collider2D plataforma;
    public Collider2D suelo;

    private Rigidbody2D rb;
    private bool perdiste;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Acepta el collider de la nave, incluso si usa PolygonCollider2D.
        if (plataforma == null)
        {
            GameObject nave = GameObject.Find("Plataforma");
            if (nave != null)
                plataforma = nave.GetComponent<Collider2D>();
        }

        if (suelo == null)
        {
            GameObject bordeInferior = GameObject.Find("Base");
            if (bordeInferior != null)
                suelo = bordeInferior.GetComponent<Collider2D>();
        }
    }

    void Start()
    {
        AplicarDireccion(new Vector2(1f, 1f));
    }

    void FixedUpdate()
    {
        if (perdiste) return;

        if (rb.linearVelocity.sqrMagnitude > 0f)
        {
            AplicarDireccion(rb.linearVelocity);
        }
    }

    void OnCollisionEnter2D(Collision2D choque)
    {
        if (choque.collider == suelo)
        {
            Perder();
            return;
        }

      
        if (plataforma == null || choque.collider != plataforma)
            return;

        if (choque.GetContact(0).normal.y < 0.5f)
            return;


        float impacto = (rb.position.x - plataforma.bounds.center.x)
                        / plataforma.bounds.extents.x;

        impacto = Mathf.Clamp(impacto, -1f, 1f);

   
        float horizontal = impacto * 1.4f
                           + Random.Range(-0.1f, 0.1f);

        AplicarDireccion(new Vector2(horizontal, 1f));
    }

    void OnTriggerEnter2D(Collider2D choque)
    {
        if (choque == suelo)
            Perder();
    }

    void Perder()
    {
        if (perdiste) return;

        perdiste = true;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        SpriteRenderer imagen = GetComponent<SpriteRenderer>();
        if (imagen != null)
            imagen.enabled = false;

        Time.timeScale = 0f;
    }

    void OnGUI()
    {
        if (!perdiste) return;

        GUIStyle estilo = new GUIStyle(GUI.skin.label);
        estilo.fontSize = 64;
        estilo.fontStyle = FontStyle.Bold;
        estilo.alignment = TextAnchor.MiddleCenter;
        estilo.normal.textColor = Color.red;

        Rect panel = new Rect(
            (Screen.width - 400f) / 2f,
            (Screen.height - 200f) / 2f,
            400f, 200f);

        GUI.Box(panel, "");
        GUI.Label(new Rect(panel.x, panel.y, panel.width, 120f),
            "PERDISTE", estilo);

        GUIStyle estiloBoton = new GUIStyle(GUI.skin.button);
        estiloBoton.fontSize = 24;

        Rect boton = new Rect(panel.x + 60f, panel.y + 130f, 280f, 50f);
        if (GUI.Button(boton, "Volver a jugar", estiloBoton))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(gameObject.scene.name);
        }
    }

    void OnDisable()
    {

        if (perdiste)
            Time.timeScale = 1f;
    }

    void AplicarDireccion(Vector2 direccion)
    {
        direccion = direccion.normalized;


        if (Mathf.Abs(direccion.x) < 0.2f)
        {
            float signo = direccion.x == 0f
                ? (Random.value < 0.5f ? -1f : 1f)
                : Mathf.Sign(direccion.x);

            direccion.x = signo * 0.2f;
        }

 
        if (Mathf.Abs(direccion.y) < 0.25f)
        {
            direccion.y = direccion.y < 0f ? -0.25f : 0.25f;
        }

        rb.linearVelocity = direccion.normalized * velocidad;
    }
}
