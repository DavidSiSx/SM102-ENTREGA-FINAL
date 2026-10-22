using UnityEngine;

public class BloqueMovilTransform : MonoBehaviour
{
    public float velocidad = 2f;
    public float recorrido = 6f;

    private float desplazamiento;
    private float direccion = 1f;

    void FixedUpdate()
    {
        float paso = direccion * velocidad * Time.fixedDeltaTime;
        float nuevoDesplazamiento = Mathf.Clamp(
            desplazamiento + paso, -recorrido, recorrido);

        transform.Translate(
            Vector3.right * (nuevoDesplazamiento - desplazamiento),
            Space.World);

        desplazamiento = nuevoDesplazamiento;

        if (desplazamiento >= recorrido)
        {
            desplazamiento = recorrido;
            direccion = -1f;
        }
        else if (desplazamiento <= -recorrido)
        {
            desplazamiento = -recorrido;
            direccion = 1f;
        }

    }
}
