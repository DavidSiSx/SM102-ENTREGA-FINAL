using UnityEngine;

public class CambioColorNave : MonoBehaviour
{
    public KeyCode teclaCambio = KeyCode.C;
    public Color[] colores = new Color[]
    {
        Color.cyan,
        Color.yellow,
        Color.magenta,
        Color.green,
        Color.red,
        new Color(1f, 0.5f, 0f) // Naranja
    };

    private SpriteRenderer spriteRenderer;
    private int indiceColor = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(teclaCambio))
        {
            CambiarColor();
        }
    }

    public void CambiarColor()
    {
        if (spriteRenderer == null) return;

        if (colores != null && colores.Length > 0)
        {
            indiceColor = (indiceColor + 1) % colores.Length;
            spriteRenderer.color = colores[indiceColor];
        }
        else
        {
            spriteRenderer.color = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.8f, 1f);
        }
    }
}
