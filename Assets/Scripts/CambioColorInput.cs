using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class CambioColorInput : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool colorAlternativo;
    public int Cambios { get; private set; }

    void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();

    public void OnChangeColor(InputValue value)
    {
        if (!value.isPressed) return;
        colorAlternativo = !colorAlternativo;
        spriteRenderer.color = colorAlternativo ? Color.red : Color.white;
        Cambios++;
    }
}
