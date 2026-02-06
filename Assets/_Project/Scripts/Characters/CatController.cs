using UnityEngine;
using CatFootball.Data;

namespace CatFootball.Characters
{
    /// <summary>
    /// Main controller for cat characters
    /// Handles movement, actions, and ball interactions
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class CatController : MonoBehaviour
    {
        [Header("Character Data")]
        [SerializeField] private CatCharacterData characterData;

        [Header("Movement Settings")]
        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private float sprintMultiplier = 1.5f;

        [Header("Ball Interaction")]
        [SerializeField] private float ballControlRadius = 1f;
        [SerializeField] private Transform ballHoldPosition;

        [Header("Stamina")]
        [SerializeField] private float currentStamina = Constants.STAMINA_MAX;

        [Header("Debug")]
        [SerializeField] private bool showDebugGizmos = true;

        // Components
        private Rigidbody2D rb;
        private CircleCollider2D catCollider;
        private Animator animator;

        // State
        private Vector2 moveInput;
        private bool isSprinting;
        private bool hasBall;
        private GameObject ballObject;
        private AnimationState currentAnimState = AnimationState.Idle;

        // Cached calculations
        private float effectiveSpeed;
        private float staminaPercentage = 1f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            catCollider = GetComponent<CircleCollider2D>();
            animator = GetComponent<Animator>();

            ConfigurePhysics();
        }

        private void Start()
        {
            if (characterData != null)
            {
                InitializeFromData();
            }
            else
            {
                Debug.LogWarning($"CatController on {gameObject.name} has no character data assigned!");
            }
        }

        private void ConfigurePhysics()
        {
            rb.gravityScale = 0f; // Top-down view
            rb.drag = 2f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            catCollider.radius = 0.3f;
        }

        private void InitializeFromData()
        {
            // Calculate effective speed based on tailBalance stat
            baseSpeed = 3f + (characterData.tailBalance / 100f) * 5f; // 3-8 units/sec

            // Initialize stamina based on nineLives stat
            currentStamina = Constants.STAMINA_MAX * (characterData.nineLives / 100f);

            Debug.Log($"{characterData.characterName} initialized - Speed: {baseSpeed}, Stamina: {currentStamina}");
        }

        private void Update()
        {
            UpdateStamina();
            UpdateAnimation();
        }

        private void FixedUpdate()
        {
            if (IsStunned()) return;

            HandleMovement();
            HandleBallPossession();
        }

        /// <summary>
        /// Set movement input (called by input manager or AI)
        /// </summary>
        public void SetMoveInput(Vector2 input)
        {
            moveInput = input.normalized;
        }

        /// <summary>
        /// Set sprint state
        /// </summary>
        public void SetSprint(bool sprint)
        {
            if (sprint && currentStamina > 10f)
            {
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
            }
        }

        private void HandleMovement()
        {
            // Calculate effective speed based on stats and stamina
            effectiveSpeed = baseSpeed;

            if (isSprinting && currentStamina > 0f)
            {
                effectiveSpeed *= sprintMultiplier;
            }

            // Apply stamina modifier
            staminaPercentage = currentStamina / Constants.STAMINA_MAX;
            if (staminaPercentage < 0.4f)
            {
                effectiveSpeed *= 0.75f; // Slow when tired
            }

            // Apply movement
            Vector2 targetVelocity = moveInput * effectiveSpeed;
            rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * 10f);

            // Face movement direction
            if (moveInput.magnitude > 0.1f)
            {
                float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        private void UpdateStamina()
        {
            if (isSprinting && moveInput.magnitude > 0.1f)
            {
                // Drain stamina while sprinting
                float drainRate = Constants.STAMINA_SPRINT_DRAIN_RATE;

                // Lazy cats tire faster
                if (characterData != null)
                {
                    drainRate *= (1f + (characterData.laziness / 10f));
                }

                currentStamina -= drainRate * Time.deltaTime;
                currentStamina = Mathf.Max(0f, currentStamina);

                // Trigger catnap if completely exhausted
                if (currentStamina <= Constants.STAMINA_CATNAP_THRESHOLD)
                {
                    StartCatnap();
                }
            }
            else
            {
                // Regenerate stamina when not sprinting
                float regenRate = Constants.STAMINA_REGEN_RATE;

                // Nine Lives stat affects regen rate
                if (characterData != null)
                {
                    regenRate *= (characterData.nineLives / 100f);
                }

                currentStamina += regenRate * Time.deltaTime;
                currentStamina = Mathf.Min(Constants.STAMINA_MAX, currentStamina);
            }
        }

        private void HandleBallPossession()
        {
            if (!hasBall) return;

            // Keep ball at hold position while moving
            if (ballObject != null && ballHoldPosition != null)
            {
                ballObject.transform.position = ballHoldPosition.position;
            }
        }

        /// <summary>
        /// Kick the ball
        /// </summary>
        public void Kick(Vector2 direction, float powerMultiplier = 1f)
        {
            if (!hasBall || ballObject == null) return;

            var ballController = ballObject.GetComponent<Gameplay.BallController>();
            if (ballController != null)
            {
                // Calculate kick power based on pouncePower stat
                float kickPower = characterData != null
                    ? 10f + (characterData.pouncePower / 100f) * 10f
                    : 10f;

                kickPower *= powerMultiplier;

                ballController.Kick(direction, kickPower, gameObject);
                ReleaseBall();

                // Play kick animation
                PlayAnimation(AnimationState.Kick);

                Debug.Log($"{characterData?.characterName ?? name} kicked ball");
            }
        }

        /// <summary>
        /// Pass the ball to a target position
        /// </summary>
        public void Pass(Vector2 targetPosition)
        {
            if (!hasBall || ballObject == null) return;

            var ballController = ballObject.GetComponent<Gameplay.BallController>();
            if (ballController != null)
            {
                // Calculate pass accuracy based on whiskerPrecision stat
                float accuracy = characterData != null
                    ? characterData.whiskerPrecision / 100f
                    : 0.7f;

                ballController.Pass(targetPosition, accuracy, gameObject);
                ReleaseBall();

                // Play pass animation
                PlayAnimation(AnimationState.Pass);

                Debug.Log($"{characterData?.characterName ?? name} passed ball");
            }
        }

        /// <summary>
        /// Attempt to take possession of nearby ball
        /// </summary>
        public void TryTakeBall(GameObject ball)
        {
            if (hasBall) return;

            float distance = Vector2.Distance(transform.position, ball.transform.position);

            if (distance <= ballControlRadius)
            {
                var ballController = ball.GetComponent<Gameplay.BallController>();
                if (ballController != null && !ballController.IsPossessed())
                {
                    TakeBall(ball);
                }
            }
        }

        private void TakeBall(GameObject ball)
        {
            ballObject = ball;
            hasBall = true;

            var ballController = ball.GetComponent<Gameplay.BallController>();
            if (ballController != null)
            {
                ballController.SetPossessor(gameObject);
                ballController.Stop();
            }

            Debug.Log($"{characterData?.characterName ?? name} has the ball");
        }

        private void ReleaseBall()
        {
            if (ballObject != null)
            {
                var ballController = ballObject.GetComponent<Gameplay.BallController>();
                if (ballController != null)
                {
                    ballController.ClearPossessor();
                }
            }

            ballObject = null;
            hasBall = false;
        }

        private void StartCatnap()
        {
            // Cat is exhausted and takes a brief nap
            PlayAnimation(AnimationState.Catnap);

            // Restore some stamina
            currentStamina = Constants.STAMINA_CATNAP_RESTORE;

            // Release ball if held
            if (hasBall)
            {
                ReleaseBall();
            }

            Debug.Log($"{characterData?.characterName ?? name} is taking a catnap!");
        }

        private void UpdateAnimation()
        {
            if (animator == null) return;

            // Determine animation state based on movement and actions
            if (currentAnimState == AnimationState.Catnap)
            {
                // Catnap animation will reset itself after duration
                return;
            }

            if (moveInput.magnitude > 0.1f)
            {
                if (isSprinting)
                {
                    PlayAnimation(AnimationState.Sprint);
                }
                else
                {
                    PlayAnimation(AnimationState.Run);
                }
            }
            else
            {
                PlayAnimation(AnimationState.Idle);
            }

            // Set animation parameters
            animator.SetFloat("Speed", rb.velocity.magnitude);
            animator.SetBool("HasBall", hasBall);
            animator.SetFloat("StaminaPercentage", staminaPercentage);
        }

        private void PlayAnimation(AnimationState state)
        {
            if (currentAnimState == state) return;

            currentAnimState = state;

            if (animator != null)
            {
                animator.SetInteger("AnimState", (int)state);
            }
        }

        private bool IsStunned()
        {
            return currentAnimState == AnimationState.Catnap;
        }

        /// <summary>
        /// Get character data
        /// </summary>
        public CatCharacterData GetCharacterData()
        {
            return characterData;
        }

        /// <summary>
        /// Check if has ball
        /// </summary>
        public bool HasBall()
        {
            return hasBall;
        }

        /// <summary>
        /// Get stamina percentage (0-1)
        /// </summary>
        public float GetStaminaPercentage()
        {
            return currentStamina / Constants.STAMINA_MAX;
        }

        private void OnDrawGizmos()
        {
            if (!showDebugGizmos) return;

            // Draw ball control radius
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, ballControlRadius);

            // Draw velocity vector
            if (Application.isPlaying && rb != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + (Vector3)rb.velocity);
            }
        }
    }
}
