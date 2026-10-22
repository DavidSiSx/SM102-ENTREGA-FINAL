using UnityEngine;

public class BloqueDestruible : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D choque)
    {
        // Comprueba que el objeto que chocó sea la pelota.
        PelotaArkanoid pelota =
            choque.gameObject.GetComponentInParent<PelotaArkanoid>();

        if (pelota != null)
        {
            Destroy(gameObject);
        }
    }
}