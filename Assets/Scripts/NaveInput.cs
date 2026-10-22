using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class NaveInput : MonoBehaviour
{
    public float fuerza = 8f;
    public Vector2 Movimiento { get; private set; }
    private Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();
    public void OnMove(InputValue value) => Movimiento = value.Get<Vector2>();
    void FixedUpdate() => rb.AddForce(Movimiento * fuerza);
    void OnDisable() => Movimiento = Vector2.zero;
}
