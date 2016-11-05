using UnityEngine;
using System.Collections;

public class ExplosionBehaviour : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        if (!ps) {
            Debug.LogError("ParticleSystem missing from explosion");
        }
    }

    void Update ()
    {
        if (ps) {
            if (!ps.isPlaying) {
                Destroy(gameObject);
            }
        }
        else {
            Destroy(gameObject);
        }
    }
}
