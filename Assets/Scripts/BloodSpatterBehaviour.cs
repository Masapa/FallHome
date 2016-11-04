
using UnityEngine;
using System.Collections;

public class BloodSpatterBehaviour : MonoBehaviour
{
    public float lifetime = 15.0f;
    public float splatTime = 0.01f;
    public float splatSpeed = 0.2f;

    // Use this for initialization
    void Start ()
    {
    }

    // Update is called once per frame
    void Update ()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0.0f) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (splatTime > 0.0f) {
            splatTime -= Time.fixedDeltaTime;
            transform.localPosition = transform.localPosition * (1.0f - splatSpeed * Time.fixedDeltaTime);
        }
    }
}
