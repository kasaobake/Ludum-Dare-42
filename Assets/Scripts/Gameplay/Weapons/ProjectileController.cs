using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class ProjectileController : MonoBehaviour 
{
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private float velocity = 1f;

    [SerializeField]
    private float lifeTime = 3f;

    [SerializeField]
    private bool isDestroyOnContact = true;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Color trailColor = Color.red;

    private Vector2 direction;
    private float skinWidth = 0.1f;

    public float Velocity
    {
        get
        {
            return this.velocity;
        }

        set
        {
            this.velocity = value;
        }
    }

    public Vector2 Direction
    {
        get
        {
            return this.direction;
        }

        set
        {
            this.direction = value.normalized;
        }
    }

    private void Awake()
    {
        GetComponent<TrailRenderer>().material.SetColor("_TintColor", this.trailColor);
    }

    private void Start()
    {
        Destroy(this.gameObject, this.lifeTime);

        Collider[] initialCollision = Physics.OverlapSphere(this.transform.position, 0.1f, this.layerMask);

        foreach (Collider collider in initialCollision)
        {
            OnHitObject(collider, this.transform.position);
        }
    }

    private void Update() 
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * this.velocity);

        float distance = this.velocity * Time.deltaTime;
        transform.Translate(Vector3.forward * distance);
        CheckCollisions(distance);

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    IDamageable damageableObject = other.GetComponent<IDamageable>();

    //    if (damageableObject != null)
    //    {
    //        damageableObject.TakeDamage(this.damage);
    //    }

    //    if (this.isDestroyOnContact)
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

	void CheckCollisions(float distance)
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance + this.skinWidth, this.layerMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }

    void OnHitObject(Collider collider, Vector3 position)
    {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();

        if (damageableObject != null)
        {
            damageableObject.DamageableComponent.TakeDamage(this.damage, position, this.transform.forward);
        }

        if (this.isDestroyOnContact)
        {
            Destroy(this.gameObject);
        }
    }
}
