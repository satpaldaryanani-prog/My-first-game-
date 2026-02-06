using UnityEngine;
using UnityEngine.Events;
using CatFootball.Characters;

namespace CatFootball.Gameplay
{
    /// <summary>
    /// Handles player input and translates to game actions
    /// Supports multiple control schemes
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [Header("Control Settings")]
        [SerializeField] private ControlScheme controlScheme = ControlScheme.Classic;

        [Header("Controlled Character")]
        [SerializeField] private CatController controlledCharacter;

        [Header("Input Settings")]
        [SerializeField] private float doubleTapTime = 0.3f;
        [SerializeField] private float swipeThreshold = 50f;

        // Input state
        private Vector2 moveInput;
        private Vector2 touchStartPos;
        private float lastTapTime;
        private bool isSprinting;

        // Events
        public UnityEvent<Vector2> OnPassInput;
        public UnityEvent<Vector2> OnShootInput;
        public UnityEvent OnTackleInput;
        public UnityEvent OnSwitchPlayerInput;

        private void Update()
        {
            if (controlledCharacter == null) return;

            HandleInput();
        }

        private void HandleInput()
        {
            switch (controlScheme)
            {
                case ControlScheme.Classic:
                    HandleClassicInput();
                    break;
                case ControlScheme.Gesture:
                    HandleGestureInput();
                    break;
                case ControlScheme.Hybrid:
                    HandleHybridInput();
                    break;
            }
        }

        private void HandleClassicInput()
        {
            // Virtual joystick movement (will be connected to UI joystick)
            // For now, use keyboard for testing
#if UNITY_EDITOR || UNITY_STANDALONE
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            isSprinting = Input.GetKey(KeyCode.LeftShift);

            // Action buttons
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandlePassAction();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                HandleShootAction();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                HandleTackleAction();
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                HandleSwitchPlayer();
            }
#endif

            // Apply inputs to character
            ApplyMovementInput();
        }

        private void HandleGestureInput()
        {
            // Touch-based gesture controls
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchStartPos = touch.position;
                        break;

                    case TouchPhase.Moved:
                        // Drag to move character
                        Vector2 dragDelta = touch.position - touchStartPos;
                        moveInput = dragDelta.normalized;
                        break;

                    case TouchPhase.Ended:
                        // Swipe detection for actions
                        Vector2 swipeDelta = touch.position - touchStartPos;

                        if (swipeDelta.magnitude > swipeThreshold)
                        {
                            HandleSwipeAction(swipeDelta.normalized);
                        }
                        else
                        {
                            // Tap detection
                            HandleTap(touch.position);
                        }

                        moveInput = Vector2.zero;
                        break;
                }
            }
            else
            {
                moveInput = Vector2.zero;
            }

            ApplyMovementInput();
        }

        private void HandleHybridInput()
        {
            // Combination of joystick movement + gesture actions
            // Movement from virtual joystick (UI)
            // Actions from swipes/taps

            // For now, fall back to classic for testing
            HandleClassicInput();
        }

        private void ApplyMovementInput()
        {
            if (controlledCharacter != null)
            {
                controlledCharacter.SetMoveInput(moveInput);
                controlledCharacter.SetSprint(isSprinting);
            }
        }

        private void HandleSwipeAction(Vector2 swipeDirection)
        {
            // Swipe up = shoot
            if (swipeDirection.y > 0.7f)
            {
                HandleShootAction();
            }
            // Swipe down = tackle
            else if (swipeDirection.y < -0.7f)
            {
                HandleTackleAction();
            }
            // Swipe horizontal = pass
            else
            {
                HandlePassAction();
            }
        }

        private void HandleTap(Vector2 tapPosition)
        {
            float currentTime = Time.time;

            // Check for double tap (sprint toggle)
            if (currentTime - lastTapTime < doubleTapTime)
            {
                isSprinting = !isSprinting;
            }

            lastTapTime = currentTime;
        }

        private void HandlePassAction()
        {
            if (controlledCharacter == null || !controlledCharacter.HasBall()) return;

            // Calculate pass direction (forward from character)
            Vector2 passDirection = controlledCharacter.transform.up;

            // TODO: Calculate actual target based on teammate positions
            Vector2 targetPosition = (Vector2)controlledCharacter.transform.position + passDirection * 5f;

            controlledCharacter.Pass(targetPosition);
            OnPassInput?.Invoke(targetPosition);

            Debug.Log("Pass action triggered");
        }

        private void HandleShootAction()
        {
            if (controlledCharacter == null || !controlledCharacter.HasBall()) return;

            // Calculate shoot direction (toward goal)
            // TODO: Get goal position from MatchManager
            Vector2 shootDirection = controlledCharacter.transform.up;

            controlledCharacter.Kick(shootDirection, 1f);
            OnShootInput?.Invoke(shootDirection);

            Debug.Log("Shoot action triggered");
        }

        private void HandleTackleAction()
        {
            // TODO: Implement tackle mechanics
            OnTackleInput?.Invoke();

            Debug.Log("Tackle action triggered");
        }

        private void HandleSwitchPlayer()
        {
            // TODO: Implement player switching logic
            OnSwitchPlayerInput?.Invoke();

            Debug.Log("Switch player triggered");
        }

        /// <summary>
        /// Set the character to control
        /// </summary>
        public void SetControlledCharacter(CatController character)
        {
            controlledCharacter = character;
            Debug.Log($"Now controlling: {character.GetCharacterData()?.characterName ?? character.name}");
        }

        /// <summary>
        /// Set control scheme
        /// </summary>
        public void SetControlScheme(ControlScheme scheme)
        {
            controlScheme = scheme;
            PlayerPrefs.SetInt(Constants.PREF_CONTROL_SCHEME, (int)scheme);
            PlayerPrefs.Save();

            Debug.Log($"Control scheme changed to: {scheme}");
        }

        /// <summary>
        /// Set movement input directly (for virtual joystick)
        /// </summary>
        public void SetMovementInput(Vector2 input)
        {
            moveInput = input;
        }

        /// <summary>
        /// Set sprint state (for UI button)
        /// </summary>
        public void SetSprintState(bool sprint)
        {
            isSprinting = sprint;
        }

        /// <summary>
        /// Button callbacks for UI
        /// </summary>
        public void OnPassButtonPressed()
        {
            HandlePassAction();
        }

        public void OnShootButtonPressed()
        {
            HandleShootAction();
        }

        public void OnTackleButtonPressed()
        {
            HandleTackleAction();
        }

        public void OnSwitchButtonPressed()
        {
            HandleSwitchPlayer();
        }
    }
}
