using System.Collections;
using System.Collections.Generic;
using Kameosa.Collections.IList;
using UnityEngine;

namespace Kameosa
{
    namespace Components
    {
        public class ScoreComponent : MonoBehaviour
        {
            #region Inspector Variables
            [SerializeField]
            private int score = 0;
            [Space(10)]

            [Header("Streak")]
            [SerializeField]
            private bool canStreak = false;
            [SerializeField]
            [Range(0.1f, 60f)]
            private float streakExpiryTimeInSeconds = 1f;
            #endregion

            #region Private Variables
            private float lastScoreTime;
            private int streakCount;
            #endregion

            #region Properties
            public int Score
            {
                get
                {
                    return this.score;
                }
            }

            public int StreakCount
            {
                get
                {
                    return this.streakCount;
                }
            }
            #endregion

            #region MonoBehaviour Functions
            private void Awake()
            {

            }
            #endregion

            #region Public Functions
            public void AddScore(int score)
            {
                if (this.canStreak)
                {
                    if (Time.time < this.lastScoreTime + this.streakExpiryTimeInSeconds)
                    {
                        this.streakCount++;
                    }
                    else
                    {
                        this.streakCount = 0;
                    }

                    this.lastScoreTime = Time.time;

                    //this.score += (int) Mathf.Pow(score, this.streakCount + 1);
                    this.score += score + this.streakCount;
                }
                else
                {
                    this.score += score;
                }
            }
            #endregion

            #region Private Functions 
            #endregion
        }
    }
}
