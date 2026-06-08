using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInputHandler))] // Ensures the input script is present
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 6f;                      // Movement speed
        [SerializeField][Range(10f, 100f)] private float acceleration = 60f; // Higher value means faster response
        [SerializeField][Range(10f, 100f)] private float deceleration = 40f; // Higher value means faster deceleration

        private Rigidbody2D rb;
        private PlayerInputHandler inputHandler;
        private Vector2 currentVelocity;

        private void Awake()
        {
            // Get the Rigidbody2D and InputHandler components from the player
            rb = GetComponent<Rigidbody2D>();
            inputHandler = GetComponent<PlayerInputHandler>();

            // Configure Rigidbody2D for a top-down game
            rb.gravityScale = 0f;                                            // No gravity downwards
            rb.freezeRotation = true;                                        // Prevents rotation when colliding with objects
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Avoid passing through walls
        }

        private void FixedUpdate()
        {
            var moveInput = inputHandler.MoveInput;
            var targetVelocity = moveInput * moveSpeed;

            // Handle movement with acceleration and deceleration (sqrMagnitude is faster mathematically than magnitude)
            var currentStep = (moveInput.sqrMagnitude > 0) ? acceleration : deceleration;

            // Smoothly change the current velocity towards the desired velocity
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, currentStep * Time.fixedDeltaTime);

            // Move the Rigidbody2D based on moveSpeed
            rb.linearVelocity = currentVelocity;
        }
    }
}
