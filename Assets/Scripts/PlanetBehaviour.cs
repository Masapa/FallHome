using UnityEngine;
using System.Collections;

public class PlanetBehaviour : MonoBehaviour {
    public const float gravitation = 1;
     float force;
    CircleCollider2D triggerArea;
    float radius;
    float playerRadius;

	// Use this for initialization
	void Start () {
        triggerArea = gameObject.GetComponent<CircleCollider2D>();
        playerRadius = GameObject.Find("Player").GetComponent<CircleCollider2D>().radius;
        radius = triggerArea.radius;
        force = (playerRadius * gameObject.GetComponentInChildren<CircleCollider2D>().radius) * gravitation;
        
	}
    void OnTriggerStay2D(Collider2D other)
    {
       
        
        
            float distanceNorm = Vector3.Distance(transform.position, other.transform.position) / radius;
            float gravity = force / Mathf.Pow(distanceNorm, 2);
        Vector3 destination = transform.position - other.transform.position;
        Vector2 destination2D = new Vector2(destination.x, destination.y);
        other.GetComponent<PlayerGravityBehaviour>().SetGravity(destination2D, gravity);
           // Debug.Log(gravity);
        
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<PlayerGravityBehaviour>().gravitation = false;
    }


	
	// Update is called once per frame
	void Update () {
	
	}
}
