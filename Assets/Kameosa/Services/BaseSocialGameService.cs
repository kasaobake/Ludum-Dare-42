#if KAMEOSA_SOCIAL
using System;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Kameosa
{
    namespace Services
    {
        public abstract class BaseSocialGameService : MonoBehaviour
        {
            private bool enableSavedGames = false;

            public event Action OnSignInSuccess;
            public event Action OnSignInFail;
            public event Action OnSignOut;
            public event Action<string> OnSaveGame;

            #region Properties

            public ILocalUser User
            {
                get
                {
                    return Social.localUser;
                }
            }

            public bool IsAuthenticated
            {
                get
                {
                    return PlayGamesPlatform.Instance.localUser.authenticated;
                }
            }

            public bool EnableSavedGames
            {
                get
                {
                    return this.enableSavedGames;
                }

                set
                {
                    this.enableSavedGames = value;
                }
            }

            #endregion

            private void Start()
            {
                Initialize();

                // Create client configuration
                PlayGamesClientConfiguration config = this.enableSavedGames 
                    ? new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build()
                    : new PlayGamesClientConfiguration.Builder().Build();

                Kameosa.Services.LogService.Info("games save enabled: " + config.EnableSavedGames);
                // Enable debugging output (recommended)
                PlayGamesPlatform.DebugLogEnabled = true;

                // Initialize and activate the platform
                PlayGamesPlatform.InitializeInstance(config);
                PlayGamesPlatform.Activate();

                //PlayGamesPlatform.Instance.Authenticate(OnSignIn, true);
            }

            protected abstract void Initialize();

            public void SignIn()
            {
                if (!PlayGamesPlatform.Instance.localUser.authenticated)
                {
                    // Sign in with Play Game Services, showing the consent dialog
                    // by setting the second parameter to isSilent=false.
                    PlayGamesPlatform.Instance.Authenticate(OnSignIn, false);
                }
                else
                {
                    // Sign out of play games
                    PlayGamesPlatform.Instance.SignOut();
                    ToastService.ShowToast("Signing out...");
                    LogService.Info("SignOut.", this);

                    if (OnSignOut != null)
                    {
                        OnSignOut();
                    }
                }
            }

            public void ShowAchievements()
            {
                if (IsAuthenticated)
                {
                    PlayGamesPlatform.Instance.ShowAchievementsUI();
                }
            }

            public void ReportScore(string leaderboard, int score)
            {
                if (IsAuthenticated)
                {
                    PlayGamesPlatform.Instance.ReportScore(score, leaderboard, OnReportScore);
                }
            }

            public void ReportProgress(string achievement, float progress = 100.0f)
            {
                if (IsAuthenticated)
                {
                    PlayGamesPlatform.Instance.ReportProgress(achievement, progress, OnReportProgress);
                }
            }

            public void IncrementAchievement(string achievement, int increment = 1)
            {
                if (IsAuthenticated)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(achievement, increment, OnIncrementAchievement);
                }
            }

            public void ShowLeaderboard()
            {
                if (IsAuthenticated)
                {
                    PlayGamesPlatform.Instance.ShowLeaderboardUI();
                }
            }

            protected virtual void LoadGame(string filename, Action<int> callback)
            {
                Action<SavedGameRequestStatus, byte[]> onReadBinaryData = delegate (SavedGameRequestStatus status, byte[] data) {
                    if (status == SavedGameRequestStatus.Success)
                    {
                        int score = 0;

                        try
                        {
                            string scoreString = System.Text.Encoding.UTF8.GetString(data);
                            score = Convert.ToInt32(scoreString);
                        }
                        catch (Exception e)
                        {
                            LogService.Exception("LoadGame: readBinaryData Exception: " + e, this);
                        }

                        LogService.Info("Score: " + score.ToString(), this);

                        callback(score);
                    }
                };

                Action<SavedGameRequestStatus, ISavedGameMetadata> onReadSavedGame = delegate (SavedGameRequestStatus status, ISavedGameMetadata savedGameMetadata) {
                    LogService.Info("onReadSavedGame: " + status.ToString(), this);

                    if (status == SavedGameRequestStatus.Success)
                    {
                        PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(savedGameMetadata, onReadBinaryData);
                    }
                };

                ReadSavedGame(filename, onReadSavedGame);
            }

            protected virtual void SaveGame(string filename, int score)
            {
                Action<SavedGameRequestStatus, ISavedGameMetadata> onReadSavedGame = delegate (SavedGameRequestStatus status, ISavedGameMetadata savedGameMetadata) {
                    LogService.Info("onReadSavedGame: " + status.ToString(), this);

                    if (status == SavedGameRequestStatus.Success)
                    {
                        string scoreString = Convert.ToString(score);
                        byte[] newData = System.Text.Encoding.UTF8.GetBytes(scoreString);

                        WriteSavedGame(savedGameMetadata, newData);
                    }
                };

                ReadSavedGame(filename, onReadSavedGame);
            }

            #region Private Functions

            private void ReadSavedGame(string filename, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
            {
                if (IsAuthenticated)
                {
                    ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                    savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, callback);
                }
            }

            private void WriteSavedGame(ISavedGameMetadata savedGameMetadata, byte[] savedData, Action<SavedGameRequestStatus, ISavedGameMetadata> callback = null)
            {
                if (IsAuthenticated)
                {
                    SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder()
                        .WithUpdatedPlayedTime(TimeSpan.FromMinutes(savedGameMetadata.TotalTimePlayed.Minutes + 1))
                        .WithUpdatedDescription("Saved at: " + System.DateTime.Now);

                    SavedGameMetadataUpdate savedGameMetadataUpdate = builder.Build();

                    ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

                    callback = callback == null ? OnWriteSavedGame : callback;
                    savedGameClient.CommitUpdate(savedGameMetadata, savedGameMetadataUpdate, savedData, callback);
                }
            }

            private void OnWriteSavedGame(SavedGameRequestStatus status, ISavedGameMetadata savedGameMetadata)
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    if (OnSaveGame != null)
                    {
                        OnSaveGame(savedGameMetadata.Filename);
                    }
                }

                LogService.Info("WriteSavedGame " + status.ToString(), this);
            }

            private void OnSignIn(bool success)
            {
                if (success)
                {
                    ToastService.ShowToast("Sign in success!");
                    LogService.Info("SignIn success.", this);

                    if (OnSignInSuccess != null)
                    {
                        OnSignInSuccess();
                    }
                }
                else
                {
                    ToastService.ShowToast("Sign in fail.");
                    LogService.Info("SignIn fail.", this);

                    if (OnSignInFail != null)
                    {
                        OnSignInFail();
                    }
                }
            }

            private void OnReportScore(bool success)
            {
                if (success)
                {
                    LogService.Info("ReportScore success.", this);
                }
                else
                {
                    LogService.Info("ReportScore fail.", this);
                }
            }

            private void OnReportProgress(bool success)
            {
                if (success)
                {
                    LogService.Info("ReportProgress success.", this);
                }
                else
                {
                    LogService.Info("ReportProgress fail.", this);
                }
            }

            private void OnIncrementAchievement(bool success)
            {
                if (success)
                {
                    LogService.Info("IncrementAchievement success.", this);
                }
                else
                {
                    LogService.Info("IncrementAchievement fail.", this);
                }
            }

            #endregion
        }
    }
}
#endif
