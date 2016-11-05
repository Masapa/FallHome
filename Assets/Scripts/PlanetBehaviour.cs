using UnityEngine;
using System.Collections;

public class PlanetBehaviour : MonoBehaviour {
    public float gravitation = 1;
     float force;
    CircleCollider2D triggerArea;
    float radius;
    float playerRadius;
    Vector3 spawnPosition;

	// Use this for initialization
	void Start () {
        spawnPosition = transform.position;
        triggerArea = gameObject.GetComponent<CircleCollider2D>();
        playerRadius = GameObject.Find("Player").GetComponent<CircleCollider2D>().radius;
        radius = triggerArea.radius;
        force = (gameObject.GetComponentInChildren<CircleCollider2D>().radius) * gravitation;

        
	}
    void OnTriggerStay2D(Collider2D other)
    {
       
        
        
            float distanceNorm = Vector3.Distance(transform.position, other.transform.position) / radius;
        float gravity = force / Mathf.Pow(distanceNorm,2);
        Vector3 destination = (transform.position - other.transform.position).normalized;
        Vector2 destination2D = new Vector2(destination.x, destination.y);
        other.GetComponent<Rigidbody2D>().AddForce(destination2D * gravity);
        //Debug.Log(destination2D * gravity);
           // Debug.Log(gravity);
        
        
    }

    public void OnLevelReset()
    {
        transform.position = spawnPosition;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
