using Pools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerShooting : MonoBehaviour
    {
        [Header("Shooting Settings")]
        [SerializeField] private GameObject bulletPrefab; // Bullet prefab
        [SerializeField] private float fireRate = 0.1f;   // Time between shots
        [SerializeField] private float bulletSpeed = 10f; // Bullet speed

        private PlayerInputHandler inputHandler;
        private Camera mainCamera;
        private float nextFireTime;

        private void Awake()
        {
            // Get the InputHandler component from the player
            inputHandler = GetComponent<PlayerInputHandler>();

            // Cache the main camera to avoid expensive use of Camera.main in Update
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // Bullet shooting logic
            HandleShooting();
        }

        private void HandleShooting()
        {
            // If enough time has passed, proceed with bullet shooting logic
            if (!inputHandler.IsShootPressed || Time.time < nextFireTime) return;
            
            // If the mainCamera is destroyed, recreate it here (e.g., scene change)
            if (!mainCamera)
            {
                mainCamera = Camera.main;
                
                // If the camera doesn't exist in the scene at all, the function returns
                if (!mainCamera) return;
            }
            
            

            // If the pointer exists, get the generic mouse position (works with Mouse and Touch/Pointer on multiple platforms)
            if (Pointer.current == null) return;
            var mouseWindowPos = Pointer.current.position.ReadValue();

            // Calculate the direction from the center of the screen (where the player is) to the cursor using the cached camera
            var mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseWindowPos);
            mouseWorldPosition.z = 0f;   // The z (depth) doesn't matter
            var shootDirection = ((Vector2)mouseWorldPosition - (Vector2)transform.position).normalized;

            // Bullet logic
            ShootBullet(shootDirection);
            nextFireTime = Time.time + fireRate;    // Reset the cooldown timer
        }

        private void ShootBullet(Vector2 direction)
        {
            if (!BulletPool.Instance) return;

            // Add an offset to the bullet spawn position to avoid player collisions
            var spawnPos = (Vector2)transform.position + direction * 0.3f;

            // Create the bullet at the current player position
            var bullet = BulletPool.Instance.GetBullet(spawnPos, Quaternion.identity);

            // Assign speed to the bullet (TryGetComponent optimizes memory and avoids garbage allocation)
            if (bullet.TryGetComponent(out Rigidbody2D bulletRb))
            {
                bulletRb.linearVelocity = direction * bulletSpeed;
            }
        }
    }
}
