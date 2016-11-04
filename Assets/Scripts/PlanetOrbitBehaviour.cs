using UnityEngine;
using System.Collections;
public enum Direction { UP,DOWN}
public class PlanetOrbitBehaviour : MonoBehaviour {
    public Direction direction = Direction.UP;
    Transform target;
    float distance;
    Rigidbody2D rb;
    public float speed = 5;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();

            if (gameObject.GetComponentInParent<Transform>() != null)
            {
                target = transform.parent.transform;
            }
        
        
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (target != null)
        {
            Vector3 tmp;
            if (direction == Direction.UP)
            {
                tmp = Vector3.forward;
            }
            else
            {
                tmp = -Vector3.forward;
            }
            transform.RotateAround(target.position, tmp, speed * Time.deltaTime);
        }

	}
}
