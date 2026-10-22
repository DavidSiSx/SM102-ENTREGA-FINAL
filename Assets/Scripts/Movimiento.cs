using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float fuerza = 10f;

    public Rigidbody2D rigidbody2;

    private void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody2.AddForce(Vector2.left * fuerza * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody2.AddForce(Vector2.right * fuerza * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody2.AddForce(Vector2.up * fuerza * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rigidbody2.AddForce(Vector2.down * fuerza * Time.deltaTime);     
        }
    }
}
