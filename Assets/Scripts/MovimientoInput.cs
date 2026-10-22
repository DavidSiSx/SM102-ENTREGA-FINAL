using UnityEngine;
using UnityEngine.InputSystem;

// Ejercicio 2: la entrada se almacena en el callback y se aplica cada fotograma.
public class MovimientoInput : MonoBehaviour
{
    public float velocidad = 5f;
    public Vector2 Movimiento { get; private set; }

    public void OnMove(InputValue value) => Movimiento = value.Get<Vector2>();

    void Update()
    {
        Vector3 direccion = new Vector3(Movimiento.x, Movimiento.y, 0f);
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    void OnDisable() => Movimiento = Vector2.zero;
}
