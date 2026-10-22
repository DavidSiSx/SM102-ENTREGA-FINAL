using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

// Dos PlayerInput en este reto cooperan con el MISMO teclado.
// PlayerInput normalmente reserva dispositivos para jugadores independientes.
[RequireComponent(typeof(PlayerInput))]
public class TecladoCompartidoInput : MonoBehaviour
{
    void Start()
    {
        PlayerInput player = GetComponent<PlayerInput>();
        if (Keyboard.current != null && player.user.valid)
            InputUser.PerformPairingWithDevice(Keyboard.current, player.user);
    }
}
