using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PelotaInput : MonoBehaviour
{
    public float fuerzaImpulso = 4f;
    public int Impulsos { get; private set; }
    public int Caidas { get; private set; }
    private Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    public void OnJump(InputValue value)
    {
        if (!value.isPressed) return;
        rb.AddForce(Vector2.up * fuerzaImpulso, ForceMode2D.Impulse);
        Impulsos++;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name != "Suelo") return;
        Caidas++;
        Debug.Log("La pelota cayó al suelo.");
    }
}
