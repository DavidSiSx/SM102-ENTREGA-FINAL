using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoArkanoid : MonoBehaviour
{
    public float fuerza = 60f;

    private Rigidbody2D rb;
    private float movimiento;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movimiento = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {

        rb.AddForce(Vector2.right * movimiento * fuerza, ForceMode2D.Force);
    }
}
