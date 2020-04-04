using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerController : MonoBehaviour 
{
    [SerializeField]
    private PlayerController player;

    private void Awake()
    {
        if (this.player == null)
        {
            this.player = GameObject.Find("Player").GetComponent<PlayerController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageableObject = other.GetComponent<IDamageable>();

        if (damageableObject != null)
        {
            damageableObject.DamageableComponent.TakeDamage(999);
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
