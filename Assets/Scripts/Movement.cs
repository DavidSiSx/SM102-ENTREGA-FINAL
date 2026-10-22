using UnityEngine;

public class Movement : MonoBehaviour
{
    public float velocidad = 5f;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * velocidad * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * velocidad * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * velocidad * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * velocidad * Time.deltaTime);
        }
    }
}
