using UnityEngine;

namespace CatFootball.Gameplay
{
    /// <summary>
    /// Controls ball physics and behavior
    /// This is critical for gameplay feel
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class BallController : MonoBehaviour
    {
        [Header("Physics Settings")]
        [SerializeField] private float maxSpeed = Constants.BALL_MAX_SPEED;
        [SerializeField] private float friction = Constants.BALL_FRICTION;
        [SerializeField] private float bounciness = Constants.BALL_BOUNCINESS;

        [Header("Visual Feedback")]
        [SerializeField] private TrailRenderer ballTrail;
        [SerializeField] private GameObject kickEffectPrefab;

        private Rigidbody2D rb;
        private CircleCollider2D ballCollider;
        private GameObject currentPossessor;
        private Vector2 lastVelocity;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            ballCollider = GetComponent<CircleCollider2D>();

            ConfigurePhysics();
        }

        private void Start()
        {
            if (ballTrail != null)
            {
                ballTrail.enabled = false;
            }
        }

        private void ConfigurePhysics()
        {
            // Configure Rigidbody2D
            rb.gravityScale = 0f; // Top-down view, no gravity
            rb.drag = friction;
            rb.angularDrag = 0.5f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            // Configure collider
            ballCollider.radius = 0.15f; // Adjust based on ball size

            // Create and configure physics material
            PhysicsMaterial2D ballMaterial = new PhysicsMaterial2D("BallMaterial")
            {
                bounciness = bounciness,
                friction = friction
            };
            ballCollider.sharedMaterial = ballMaterial;

            Debug.Log("BallController: Physics configured");
        }

        private void FixedUpdate()
        {
            // Clamp ball speed to maximum
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            // Store last velocity for collision response
            lastVelocity = rb.velocity;

            // Update trail based on speed
            UpdateTrail();
        }

        /// <summary>
        /// Kick the ball in a direction with power
        /// </summary>
        public void Kick(Vector2 direction, float power, GameObject kicker)
        {
            direction = direction.normalized;
            float kickForce = Mathf.Clamp(power, 0f, maxSpeed);

            rb.velocity = Vector2.zero; // Reset current velocity
            rb.AddForce(direction * kickForce, ForceMode2D.Impulse);

            // Visual feedback
            SpawnKickEffect(transform.position);

            // Audio feedback (will be implemented with AudioManager)
            // AudioManager.Instance.PlaySFX("KickBall");

            Debug.Log($"Ball kicked by {kicker.name} with power {power}");
        }

        /// <summary>
        /// Pass the ball with precision
        /// </summary>
        public void Pass(Vector2 targetPosition, float accuracy, GameObject passer)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

            // Add slight inaccuracy based on accuracy stat
            float inaccuracy = (1f - accuracy) * 0.2f;
            direction = AddInaccuracy(direction, inaccuracy);

            float passSpeed = maxSpeed * 0.7f; // Passes are slower than shots
            rb.velocity = direction * passSpeed;

            Debug.Log($"Ball passed by {passer.name} to {targetPosition}");
        }

        /// <summary>
        /// Apply spin to the ball (for curve shots)
        /// </summary>
        public void ApplySpin(float spinAmount)
        {
            rb.angularVelocity = spinAmount;

            // Add curve effect to velocity
            Vector2 perpendicular = Vector2.Perpendicular(rb.velocity.normalized);
            rb.AddForce(perpendicular * spinAmount * 0.1f, ForceMode2D.Impulse);
        }

        /// <summary>
        /// Stop the ball (for ball control)
        /// </summary>
        public void Stop()
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        /// <summary>
        /// Set ball possessor
        /// </summary>
        public void SetPossessor(GameObject possessor)
        {
            currentPossessor = possessor;
        }

        /// <summary>
        /// Get current possessor
        /// </summary>
        public GameObject GetPossessor()
        {
            return currentPossessor;
        }

        /// <summary>
        /// Clear possessor
        /// </summary>
        public void ClearPossessor()
        {
            currentPossessor = null;
        }

        /// <summary>
        /// Check if ball is possessed
        /// </summary>
        public bool IsPossessed()
        {
            return currentPossessor != null;
        }

        private void UpdateTrail()
        {
            if (ballTrail == null) return;

            // Enable trail when ball is moving fast
            float speedThreshold = maxSpeed * 0.4f;
            ballTrail.enabled = rb.velocity.magnitude > speedThreshold;

            // Adjust trail width based on speed
            if (ballTrail.enabled)
            {
                float normalizedSpeed = rb.velocity.magnitude / maxSpeed;
                ballTrail.widthMultiplier = normalizedSpeed * 0.2f;
            }
        }

        private void SpawnKickEffect(Vector3 position)
        {
            if (kickEffectPrefab != null)
            {
                GameObject effect = Instantiate(kickEffectPrefab, position, Quaternion.identity);
                Destroy(effect, 1f); // Clean up after 1 second
            }
        }

        private Vector2 AddInaccuracy(Vector2 direction, float inaccuracyAmount)
        {
            // Add random angle deviation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float randomDeviation = Random.Range(-inaccuracyAmount, inaccuracyAmount) * 180f;
            angle += randomDeviation;

            return new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Play bounce sound based on impact force
            float impactForce = collision.relativeVelocity.magnitude;

            if (impactForce > 5f)
            {
                // AudioManager.Instance.PlaySFX("BallBounce", impactForce / maxSpeed);
                Debug.Log($"Ball collided with {collision.gameObject.name}, impact: {impactForce}");
            }

            // Clear possessor on hard collision
            if (impactForce > 10f && currentPossessor != null)
            {
                ClearPossessor();
            }
        }

        /// <summary>
        /// Get ball current speed
        /// </summary>
        public float GetSpeed()
        {
            return rb.velocity.magnitude;
        }

        /// <summary>
        /// Get ball velocity
        /// </summary>
        public Vector2 GetVelocity()
        {
            return rb.velocity;
        }

        /// <summary>
        /// Set ball position (for resets, goals, etc.)
        /// </summary>
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            ClearPossessor();
        }

        private void OnDrawGizmos()
        {
            // Debug visualization
            if (Application.isPlaying && rb != null)
            {
                // Draw velocity vector
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, transform.position + (Vector3)rb.velocity);
            }
        }
    }
}
