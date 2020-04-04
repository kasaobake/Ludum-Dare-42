using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TitleController : MonoBehaviour 
{
    #region Inspector Variables
    [SerializeField]
    private string text;
    [Header("References")]
    [SerializeField]
    private Text foregroundText;
    [SerializeField]
    private Text backgroundText;
    #endregion

    #region Properties 
    public string Text
    {
        get
        {
            return this.text;
        }

        set
        {
            this.text = value;
            this.foregroundText.text = this.text;
            this.backgroundText.text = this.text;
        }
    }
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (this.foregroundText == null)
        {
            this.foregroundText = this.transform.Find("Foreground").GetComponent<Text>();
        }

        if (this.backgroundText == null)
        {
            this.backgroundText = this.transform.Find("Background").GetComponent<Text>();
        }
    }

    private void Start() 
    {
        this.foregroundText.text = this.text;
        this.backgroundText.text = this.text;
    }
    #endregion
}
