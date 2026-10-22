using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoFisicoInput : MonoBehaviour
{
    public float fuerza = 8f;
    public float fuerzaImpulso = 4f;
    public Vector2 Movimiento { get; private set; }
    public int Impulsos { get; private set; }
    private Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();
    public void OnMove(InputValue value) => Movimiento = value.Get<Vector2>();
    void FixedUpdate() => rb.AddForce(Movimiento * fuerza);

    public void OnJump(InputValue value)
    {
        if (!value.isPressed) return;
        rb.AddForce(Vector2.up * fuerzaImpulso, ForceMode2D.Impulse);
        Impulsos++;
    }

    void OnDisable() => Movimiento = Vector2.zero;
}
