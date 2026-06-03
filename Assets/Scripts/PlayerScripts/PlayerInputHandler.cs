using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls controls;

    public Vector2 MoveInput { get; private set; }
    public bool IsShootPressed { get; private set; }

    void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Attiva la mappa dei controlli
        controls.Enable();

        // Usa gli eventi per leggere gli input del Movimento
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;

        // Usa gli eventi per leggere gli input dello Sparo
        controls.Player.Shoot.performed += OnShootPerformed;
        controls.Player.Shoot.canceled += OnShootCanceled;
    }

    private void OnDisable()
    {
        // Rimuove la sottoscrizione agli eventi per evitare memory leak
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;

        controls.Player.Shoot.performed -= OnShootPerformed;
        controls.Player.Shoot.canceled -= OnShootCanceled;

        // Disattiva i controlli quando l'oggetto non è attivo
        controls.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();

        // Vector2.ClampMagnitude è più efficiente e pulito del controllo manuale sulla magnitude (limita i valori diagonali a max 1)
        MoveInput = Vector2.ClampMagnitude(MoveInput, 1f);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        MoveInput = Vector2.zero;
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        IsShootPressed = true; // Il tasto di sparo è attualmente premuto
    }

    private void OnShootCanceled(InputAction.CallbackContext context)
    {
        IsShootPressed = false; // Il tasto di sparo è stato rilasciato
    }
}
