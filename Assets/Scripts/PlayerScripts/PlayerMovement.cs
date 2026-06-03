using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputHandler))] // Assicura che ci sia lo script di input
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 6f;                      // Velocità di movimento
    [SerializeField][Range(10f, 100f)] private float acceleration = 60f; // Più è alto, più risponde velocemente
    [SerializeField][Range(10f, 100f)] private float deceleration = 40f; // Più è alto, più rallenta velocemente

    private Rigidbody2D rb;
    private PlayerInputHandler inputHandler;
    private Vector2 currentVelocity;

    private void Awake()
    {
        // Recupera il componente Rigidbody2D e InputHandler dal Player
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<PlayerInputHandler>();

        // Config del Rigidbody2D per un gioco top-down
        rb.gravityScale = 0f;                                            // Niente gravità verso il basso
        rb.freezeRotation = true;                                        // Impedisce al personaggio di ruotare su se stesso quando urta un oggetto
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Per evitare di passare attraverso i muri
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = inputHandler.MoveInput;
        Vector2 targetVelocity = moveInput * moveSpeed;

        // Gestisce il movimento con accelerazione e decelerazione (sqrMagnitude è più veloce matematicamente di .magnitude)
        float currentStep = (moveInput.sqrMagnitude > 0) ? acceleration : deceleration;

        // Cambia in modo fluido la velocita attuale verso quella desiderata
        currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, currentStep * Time.fixedDeltaTime);

        // Muove il Rigidbody2D basandosi sulla moveSpeed
        rb.linearVelocity = currentVelocity;
    }
}
