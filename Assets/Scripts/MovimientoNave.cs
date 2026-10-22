using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoNave : MonoBehaviour
{
    public float velocidad = 10f;
    public Rigidbody2D rigidbody2;

    private Vector2 inputMovimiento;

    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed) horizontal = -1f;
            if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed) horizontal = 1f;
            if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed) vertical = 1f;
            if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed) vertical = -1f;
        }

        if (horizontal == 0f) horizontal = Input.GetAxisRaw("Horizontal");
        if (vertical == 0f) vertical = Input.GetAxisRaw("Vertical");

        inputMovimiento = new Vector2(horizontal, vertical).normalized;
    }

    void FixedUpdate()
    {
        if (rigidbody2 != null)
        {
            rigidbody2.linearVelocity = inputMovimiento * velocidad;
        }
    }
}