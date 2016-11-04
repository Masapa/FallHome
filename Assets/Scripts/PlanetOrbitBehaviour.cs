using UnityEngine;
using System.Collections;

public class PlanetOrbitBehaviour : MonoBehaviour {
    Transform target;
    float distance;
    Rigidbody2D rb;
    public float speed = 5;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Earth").GetComponent<Transform>();
        
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 tmp = target.position - transform.position;
        transform.RotateAround(target.position, Vector3.forward, 10 * Time.deltaTime);

	}
}
