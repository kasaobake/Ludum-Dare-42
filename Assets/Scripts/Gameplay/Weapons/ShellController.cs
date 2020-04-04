using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShellController : MonoBehaviour 
{
    [SerializeField]
    private float velocityMin = 1f;

    [SerializeField]
    private float velocityMax = 1f;

    [SerializeField]
    private float lifeTime = 3f;

    [SerializeField]
    private float fadeTime = 2f;

    private void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        float velocity = Random.Range(this.velocityMin, this.velocityMax);
        rigidbody.AddForce(this.transform.right * velocity);
        // Adds a random rotation.
        rigidbody.AddTorque(Random.insideUnitSphere * velocity);

        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(this.lifeTime);

        float percent = 0;
        float fadeSpeed = 1 / this.fadeTime;

        Material material = GetComponent<Renderer>().material;
        Color initialColour = material.color;

        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            material.color = Color.Lerp(initialColour, Color.clear, percent);

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
