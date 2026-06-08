using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
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
            // Enable the control map
            controls.Enable();

            // Use events to read movement input
            controls.Player.Move.performed += OnMovePerformed;
            controls.Player.Move.canceled += OnMoveCanceled;

            // Use events to read shooting input
            controls.Player.Shoot.performed += OnShootPerformed;
            controls.Player.Shoot.canceled += OnShootCanceled;
        }

        private void OnDisable()
        {
            // Unsubscribe from events to prevent memory leaks
            controls.Player.Move.performed -= OnMovePerformed;
            controls.Player.Move.canceled -= OnMoveCanceled;

            controls.Player.Shoot.performed -= OnShootPerformed;
            controls.Player.Shoot.canceled -= OnShootCanceled;

            // Disable controls when the object is not active
            controls?.Disable();
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();

            // Vector2.ClampMagnitude is more efficient and cleaner than manual magnitude control (limits diagonal values to max 1)
            MoveInput = Vector2.ClampMagnitude(MoveInput, 1f);
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            MoveInput = Vector2.zero;
        }

        private void OnShootPerformed(InputAction.CallbackContext context)
        {
            IsShootPressed = true; // The shoot button is currently pressed
        }

        private void OnShootCanceled(InputAction.CallbackContext context)
        {
            IsShootPressed = false; // The shoot button has been released
        }
    }
}
