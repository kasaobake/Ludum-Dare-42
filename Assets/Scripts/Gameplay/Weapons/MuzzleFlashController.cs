using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashController : MonoBehaviour 
{
    [SerializeField]
    private GameObject flashWrapper;

    [SerializeField]
    private Sprite[] flashSprites;

    [SerializeField]
    private SpriteRenderer[] spriteRenderers;

    [SerializeField]
    private float flashTime;

    private void Awake()
    {
        if (this.flashWrapper == null)
        {
            this.flashWrapper = this.transform.Find("FlashWrapper").gameObject;
        }
    }

    private void Start()
    {
        Deactivate();
    }

    public void Activate()
    {
        this.flashWrapper.SetActive(true);

        int flashSpriteIndex = Random.Range(0, this.flashSprites.Length);

        for (int i = 0; i < this.spriteRenderers.Length; i++)
        {
            this.spriteRenderers[i].sprite = this.flashSprites[flashSpriteIndex];
        }

        Invoke("Deactivate", this.flashTime);
    }

    private void Deactivate()
    {
        this.flashWrapper.SetActive(false);
    }
}
