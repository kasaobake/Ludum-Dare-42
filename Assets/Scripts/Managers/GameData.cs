using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    private const bool DEFAULT_IS_AUTO_SAVE = true;
    private const int DEFAULT_AUTO_SAVE_INTERVAL = 15;

    #region Private Variables
    private int highScore;
    private int autoSaveInterval;
	private bool isAutoSave;
	#endregion

    #region Properties
    public int HighScore
    {
        get
        {
            return this.highScore;
        }

        set
        {
            this.highScore = Mathf.Max(this.highScore, value);
        }
    }

    public bool IsAutoSave
    {
        get
        {
            return this.isAutoSave;
        }

        set
        {
            this.isAutoSave = value;
        }
    }

    public int AutoSaveInterval 
    {
        get
        {
            return this.autoSaveInterval;
        }

        set
        {
            this.autoSaveInterval = value;
        }
    }
    #endregion

    public GameData()
    {
        this.isAutoSave = DEFAULT_IS_AUTO_SAVE;
        this.autoSaveInterval = DEFAULT_AUTO_SAVE_INTERVAL;
    }
}