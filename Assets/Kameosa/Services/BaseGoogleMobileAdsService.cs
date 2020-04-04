/*
using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

namespace Kameosa
{
    namespace Services
    {
        public abstract class BaseGoogleMobileAdsService : MonoBehaviour
        {
            private const float INTERSTITIAL_DEFAULT_SPORADIC_RATE = 0.2f;
            private const float INTERSTITIAL_SPORADIC_RATE_DELTA = 0.1f;

            private static AdsController instance;

            private BannerView banner;
            private InterstitialAd interstitial;
            private RewardBasedVideoAd rewardBasedVideo;

            private float interstitialSporadicRate = INTERSTITIAL_DEFAULT_SPORADIC_RATE;

            public static AdsController Instance
            {
                get
                {
                    return instance;
                }
            }

            private void Awake()
            {
                if (instance != null)
                {
                    Destroy(gameObject);
                }
                else
                {
                    instance = this;
                    DontDestroyOnLoad(gameObject);
                }
            }

            private void Start()
            {
                Initialize();

        #if UNITY_ANDROID
                string appId = "ca-app-pub-3940256099942544~3347511713";
        #elif UNITY_IPHONE
                string appId = "ca-app-pub-3940256099942544~1458002511";
        #else
                string appId = "unexpected_platform";
        #endif

                MobileAds.Initialize(appId);

                RequestInterstitial();
                //RequestRewardBasedVideo();
            }

            protected abstract void Initialize()
            {
            }

            public void ShowBanner(AdPosition adPosition = AdPosition.Top)
            {
                RequestBanner(adPosition);
            }

            public void HideBanner()
            {
                this.banner.Destroy();
            }

            public void ShowInterstitial()
            {
                if (this.interstitial == null || this.interstitial.IsLoaded())
                {
                    this.interstitial.Show();
                }
                else
                {
                    Debug.Log("Interstitial is not ready yet");
                    RequestInterstitial();
                }
            }

            public void ShowSporadicInterstitial()
            {
                float dice = UnityEngine.Random.Range(0f, 1f);

                if (dice < this.interstitialSporadicRate)
                {
                    ShowInterstitial();
                }
                else
                {
                    this.interstitialSporadicRate += INTERSTITIAL_SPORADIC_RATE_DELTA;
                }
            }

            public void HideInterstitial()
            {
                this.interstitial.Destroy();
            }

            public void ShowRewardBasedVideo()
            {
                if (this.rewardBasedVideo == null || this.rewardBasedVideo.IsLoaded())
                {
                    this.rewardBasedVideo.Show();
                }
                else
                {
                    Debug.Log("Reward based video ad is not ready yet");
                    RequestRewardBasedVideo();
                }
            }

            private AdRequest CreateAdRequest()
            {
                return new AdRequest.Builder()
                    .AddTestDevice(AdRequest.TestDeviceSimulator)
                    .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
                    .AddKeyword("game")
                    .SetGender(Gender.Male)
                    .SetBirthday(new DateTime(1985, 1, 1))
                    .TagForChildDirectedTreatment(false)
                    .AddExtra("color_bg", "9B30FF")
                    .Build();
            }

            private void RequestBanner(AdPosition adPosition = AdPosition.Top)
            {
        #if UNITY_EDITOR
                string adUnitId = "unused";
        #elif UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
                string adUnitId = "unexpected_platform";
        #endif
                if (this.banner != null)
                {
                    this.banner.Destroy();
                }

                this.banner = new BannerView(adUnitId, AdSize.SmartBanner, adPosition);
                RegisterBannerEventHandlers();
                this.banner.LoadAd(CreateAdRequest());
            }

            private void RequestInterstitial()
            {
        #if UNITY_EDITOR
                string adUnitId = "unused";
        #elif UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        #elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
                string adUnitId = "unexpected_platform";
        #endif
                if (this.interstitial != null)
                {
                    this.interstitial.Destroy();
                }

                this.interstitial = new InterstitialAd(adUnitId);
                RegisterInterstitialEventHandlers();
                this.interstitial.LoadAd(CreateAdRequest());
            }

            private void RequestRewardBasedVideo()
            {
        #if UNITY_EDITOR
                string adUnitId = "unused";
        #elif UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-3940256099942544/1712485313";
        #else
                string adUnitId = "unexpected_platform";
        #endif
                if (this.rewardBasedVideo == null)
                {
                    this.rewardBasedVideo = RewardBasedVideoAd.Instance;
                    RegisterRewardBasedVideoEventHandlers();
                }

                this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
            }

            #region Register and unregister event handlers

            private void RegisterBannerEventHandlers()
            {
                this.banner.OnAdLoaded += this.HandleBannerAdLoaded;
                this.banner.OnAdFailedToLoad += this.HandleBannerAdFailedToLoad;
                this.banner.OnAdOpening += this.HandleBannerAdOpening;
                this.banner.OnAdClosed += this.HandleBannerAdClosed;
                this.banner.OnAdLeavingApplication += this.HandleBannerAdLeavingApplication;
            }

            private void UnregisterBannerEventHandlers()
            {
                // Register for ad events.
                this.banner.OnAdLoaded -= this.HandleBannerAdLoaded;
                this.banner.OnAdFailedToLoad -= this.HandleBannerAdFailedToLoad;
                this.banner.OnAdOpening -= this.HandleBannerAdOpening;
                this.banner.OnAdClosed -= this.HandleBannerAdClosed;
                this.banner.OnAdLeavingApplication -= this.HandleBannerAdLeavingApplication;
            }

            private void RegisterInterstitialEventHandlers()
            {
                this.interstitial.OnAdLoaded += this.HandleInterstitialAdLoaded;
                this.interstitial.OnAdFailedToLoad += this.HandleInterstitialAdFailedToLoad;
                this.interstitial.OnAdOpening += this.HandleInterstitialAdOpening;
                this.interstitial.OnAdClosed += this.HandleInterstitialAdClosed;
                this.interstitial.OnAdLeavingApplication += this.HandleInterstitialAdLeavingApplication;
            }

            private void UnregisterInterstitialEventHandlers()
            {
                this.interstitial.OnAdLoaded += this.HandleInterstitialAdLoaded;
                this.interstitial.OnAdFailedToLoad += this.HandleInterstitialAdFailedToLoad;
                this.interstitial.OnAdOpening += this.HandleInterstitialAdOpening;
                this.interstitial.OnAdClosed += this.HandleInterstitialAdClosed;
                this.interstitial.OnAdLeavingApplication += this.HandleInterstitialAdLeavingApplication;
            }

            private void RegisterRewardBasedVideoEventHandlers()
            {
                this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoAdLoaded;
                this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoAdFailedToLoad;
                this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoAdOpening;
                this.rewardBasedVideo.OnAdStarted += this.HandleRewardBasedVideoAdStarted;
                this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoAdRewarded;
                this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoAdClosed;
                this.rewardBasedVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoAdLeavingApplication;
            }

            #endregion

            #region Banner callback handlers

            public void HandleBannerAdLoaded(object sender, EventArgs args)
            {
                Debug.Log("HandleBannerAdLoaded event received");
            }

            public void HandleBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
            {
                Debug.Log("HandleBannerAdFailedToLoad event received with message: " + args.Message);
                UnregisterBannerEventHandlers();
                RequestBanner();
            }

            public void HandleBannerAdOpening(object sender, EventArgs args)
            {
                Debug.Log("HandleBannerAdOpening event received");
            }

            public void HandleBannerAdClosed(object sender, EventArgs args)
            {
                Debug.Log("HandleBannerAdClosed event received");
                UnregisterBannerEventHandlers();
                RequestBanner();
            }

            public void HandleBannerAdLeavingApplication(object sender, EventArgs args)
            {
                Debug.Log("HandleBannerAdLeavingApplication event received");
                UnregisterBannerEventHandlers();
                RequestBanner();
            }

            #endregion

            #region Interstitial callback handlers

            public void HandleInterstitialAdLoaded(object sender, EventArgs args)
            {
                Debug.Log("HandleInterstitialAdLoaded event received");
            }

            public void HandleInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
            {
                Debug.Log("HandleInterstitialAdFailedToLoad event received with message: " + args.Message);
                UnregisterInterstitialEventHandlers();
                RequestInterstitial();
            }

            public void HandleInterstitialAdOpening(object sender, EventArgs args)
            {
                Debug.Log("HandleInterstitialAdOpening event received");
                this.interstitialSporadicRate = INTERSTITIAL_DEFAULT_SPORADIC_RATE;
            }

            public void HandleInterstitialAdClosed(object sender, EventArgs args)
            {
                Debug.Log("HandleInterstitialAdClosed event received");
                UnregisterInterstitialEventHandlers();
                RequestInterstitial();
            }

            public void HandleInterstitialAdLeavingApplication(object sender, EventArgs args)
            {
                Debug.Log("HandleInterstitialAdLeavingApplication event received");
                UnregisterInterstitialEventHandlers();
                RequestInterstitial();
            }

            #endregion

            #region RewardBasedVideoAd callback handlers

            public void HandleRewardBasedVideoAdLoaded(object sender, EventArgs args)
            {
                Debug.Log("HandleRewardBasedVideoAdLoaded event received");
            }

            public void HandleRewardBasedVideoAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
            {
                Debug.Log("HandleRewardBasedVideoAdFailedToLoad event received with message: " + args.Message);
            }

            public void HandleRewardBasedVideoAdOpening(object sender, EventArgs args)
            {
                Debug.Log("HandleRewardBasedVideoAdOpening event received");
            }

            public void HandleRewardBasedVideoAdStarted(object sender, EventArgs args)
            {
                Debug.Log("HandleRewardBasedVideoAdStarted event received");
            }

            public void HandleRewardBasedVideoAdClosed(object sender, EventArgs args)
            {
                Debug.Log("HandleRewardBasedVideoAdClosed event received");
            }

            public void HandleRewardBasedVideoAdRewarded(object sender, Reward args)
            {
                string type = args.Type;
                double amount = args.Amount;
                Debug.Log("HandleRewardBasedVideoAdRewarded event received for " + amount.ToString() + " " + type);
            }

            public void HandleRewardBasedVideoAdLeavingApplication(object sender, EventArgs args)
            {
                Debug.Log("HandleRewardBasedVideoAdLeavingApplication event received");
            }

            #endregion
        }
    }
}
*/