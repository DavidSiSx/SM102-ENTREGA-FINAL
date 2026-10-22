using UnityEngine;

public class ControlPelota : MonoBehaviour
{
    public float velocidadHorizontal = 3f;
    public float fuerzaImpulso = 4f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.linearVelocity = new Vector2(
            velocidadHorizontal,
            0f
        );
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(
                Vector2.up * fuerzaImpulso,
                ForceMode2D.Impulse
            );
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Suelo")
        {
            Debug.Log("La pelota cayó al suelo.");
        }
    }
}
