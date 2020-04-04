using System.Collections;
using System.Collections.Generic;
using Kameosa.Cartesian;
using Kameosa.Common;
using Kameosa.Components;
using Kameosa.Services;
using Kameosa.Tuples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kameosa
{
    namespace Managers
    {
        public class ResolutionManager : Singleton<ResolutionManager>
        {
            private const string RESOLUTION_INDEX = "RESOLUTION_INDEX";
            private const string IS_FULL_SCREEN = "IS_FULL_SCREEN";

            #region Inspector Variables
            [SerializeField]
            private bool isFullScreen = false;
            [SerializeField]
            private Coordinate aspectRatio = new Coordinate(16, 9);
            [SerializeField]
            private List<int> widths = new List<int>()
            {
                Kameosa.Constants.Resolution.Low["16:9"].X,
                Kameosa.Constants.Resolution.Mid["16:9"].X,
                Kameosa.Constants.Resolution.High["16:9"].X,
            };
            #endregion

            #region Private Variables
            private int resolutionIndex;
            private List<Coordinate> resolutions;
            #endregion

            #region Properties
            public bool IsFullScreen
            {
                get
                {
                    return this.isFullScreen;
                }

                set
                {
                    this.isFullScreen = value;
                    PlayerPrefsService.SetBool(IS_FULL_SCREEN, value);
                    SetFullScreen();
                }
            }

            public Coordinate AspectRatio
            {
                get
                {
                    return this.aspectRatio;
                }

                set
                {
                    this.aspectRatio = value;
                }
            }

            public int ResolutionIndex
            {
                get
                {
                    return this.resolutionIndex;
                }

                set
                {
                    this.resolutionIndex = value;
                    PlayerPrefsService.SetInt(RESOLUTION_INDEX, value);
                    SetScreenResolution();
                }
            }
            #endregion

            #region MonoBehaviour Functions
            protected override void Awake()
            {
                base.Awake();

                this.isFullScreen = PlayerPrefsService.GetBool(IS_FULL_SCREEN);
                this.resolutionIndex = PlayerPrefsService.GetInt(RESOLUTION_INDEX);
            }

            private void Start()
            {
                float aspectRatioDivided = (float)this.AspectRatio.X / this.AspectRatio.Y;

                this.resolutions = new List<Coordinate>();

                foreach (int width in this.widths)
                {
                    this.resolutions.Add(new Coordinate(width, (int) (width / aspectRatioDivided)));
                }
            }
            #endregion

            #region Public Functions
            #endregion

            #region Private Functions
            private void SetScreenResolution()
            {
                //Screen.SetResolution(this.resolutions[this.resolutionIndex].X, this.resolutions[this.resolutionIndex].Y, this.isFullScreen);
                Screen.SetResolution(this.resolutions[this.resolutionIndex].X, this.resolutions[this.resolutionIndex].Y, false);
            }

            private void SetFullScreen()
            {
                if (this.isFullScreen)
                {
                    Resolution[] allResolutions = Screen.resolutions;
                    Resolution maxResolution = allResolutions[allResolutions.Length - 1];
                    Screen.SetResolution(maxResolution.width, maxResolution.height, true);
                }
                else
                {
                    SetScreenResolution();
                }
            }
            #endregion
        }
    }
}