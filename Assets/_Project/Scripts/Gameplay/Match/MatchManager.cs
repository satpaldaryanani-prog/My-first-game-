using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace CatFootball.Gameplay
{
    /// <summary>
    /// Manages match flow, rules, and state
    /// </summary>
    public class MatchManager : MonoBehaviour
    {
        [Header("Match Settings")]
        [SerializeField] private float matchDuration = Constants.MATCH_DURATION_QUICK;
        [SerializeField] private GameMode gameMode = GameMode.QuickMatch;

        [Header("Teams")]
        [SerializeField] private List<GameObject> homeTeam = new List<GameObject>();
        [SerializeField] private List<GameObject> awayTeam = new List<GameObject>();

        [Header("Game Objects")]
        [SerializeField] private GameObject ball;
        [SerializeField] private Transform centerSpot;
        [SerializeField] private Transform homeGoal;
        [SerializeField] private Transform awayGoal;

        [Header("Match State")]
        [SerializeField] private int homeScore = 0;
        [SerializeField] private int awayScore = 0;
        [SerializeField] private float matchTime = 0f;
        [SerializeField] private bool matchInProgress = false;

        // Events
        public UnityEvent OnMatchStart;
        public UnityEvent<TeamSide> OnGoalScored;
        public UnityEvent<MatchResult> OnMatchEnd;
        public UnityEvent<float> OnTimeUpdate;

        private MatchState currentState = MatchState.PreMatch;

        private enum MatchState
        {
            PreMatch,
            Playing,
            Paused,
            PostMatch
        }

        private void Start()
        {
            PrepareMatch();
        }

        private void Update()
        {
            if (matchInProgress && currentState == MatchState.Playing)
            {
                UpdateMatchTime();
            }
        }

        private void PrepareMatch()
        {
            // Reset scores
            homeScore = 0;
            awayScore = 0;
            matchTime = 0f;

            // Position ball at center
            if (ball != null && centerSpot != null)
            {
                ball.transform.position = centerSpot.position;
            }

            // Position teams
            PositionTeamsForKickoff();

            Debug.Log("Match prepared - Ready to start");
        }

        /// <summary>
        /// Start the match
        /// </summary>
        public void StartMatch()
        {
            matchInProgress = true;
            currentState = MatchState.Playing;
            matchTime = 0f;

            OnMatchStart?.Invoke();

            Debug.Log($"Match started! Duration: {matchDuration}s, Mode: {gameMode}");
        }

        /// <summary>
        /// Pause the match
        /// </summary>
        public void PauseMatch()
        {
            if (currentState == MatchState.Playing)
            {
                currentState = MatchState.Paused;
                Time.timeScale = 0f;

                Debug.Log("Match paused");
            }
        }

        /// <summary>
        /// Resume the match
        /// </summary>
        public void ResumeMatch()
        {
            if (currentState == MatchState.Paused)
            {
                currentState = MatchState.Playing;
                Time.timeScale = 1f;

                Debug.Log("Match resumed");
            }
        }

        /// <summary>
        /// End the match
        /// </summary>
        public void EndMatch()
        {
            matchInProgress = false;
            currentState = MatchState.PostMatch;

            // Determine result
            MatchResult result;
            if (homeScore > awayScore)
            {
                result = MatchResult.Win;
            }
            else if (homeScore < awayScore)
            {
                result = MatchResult.Loss;
            }
            else
            {
                result = MatchResult.Draw;
            }

            OnMatchEnd?.Invoke(result);

            Debug.Log($"Match ended! Final score: {homeScore} - {awayScore}, Result: {result}");
        }

        private void UpdateMatchTime()
        {
            matchTime += Time.deltaTime;

            OnTimeUpdate?.Invoke(matchTime);

            // Check if match should end
            if (matchTime >= matchDuration)
            {
                EndMatch();
            }
        }

        /// <summary>
        /// Register a goal
        /// </summary>
        public void RegisterGoal(TeamSide scoringTeam)
        {
            if (scoringTeam == TeamSide.Home)
            {
                homeScore++;
                Debug.Log($"HOME GOAL! Score: {homeScore} - {awayScore}");
            }
            else
            {
                awayScore++;
                Debug.Log($"AWAY GOAL! Score: {homeScore} - {awayScore}");
            }

            OnGoalScored?.Invoke(scoringTeam);

            // Reset for kickoff
            ResetForKickoff();
        }

        private void ResetForKickoff()
        {
            // Pause briefly for celebration
            Invoke(nameof(PerformKickoffReset), 2f);
        }

        private void PerformKickoffReset()
        {
            // Position ball at center
            if (ball != null && centerSpot != null)
            {
                var ballController = ball.GetComponent<BallController>();
                if (ballController != null)
                {
                    ballController.SetPosition(centerSpot.position);
                }
            }

            // Reposition teams
            PositionTeamsForKickoff();

            Debug.Log("Kickoff reset complete");
        }

        private void PositionTeamsForKickoff()
        {
            // This would position players in their starting positions
            // For now, just a placeholder
            Debug.Log("Teams positioned for kickoff");
        }

        /// <summary>
        /// Get current match time remaining
        /// </summary>
        public float GetTimeRemaining()
        {
            return Mathf.Max(0f, matchDuration - matchTime);
        }

        /// <summary>
        /// Get current score
        /// </summary>
        public (int home, int away) GetScore()
        {
            return (homeScore, awayScore);
        }

        /// <summary>
        /// Check if match is in progress
        /// </summary>
        public bool IsMatchInProgress()
        {
            return matchInProgress && currentState == MatchState.Playing;
        }

        /// <summary>
        /// Get match duration
        /// </summary>
        public float GetMatchDuration()
        {
            return matchDuration;
        }
    }
}
