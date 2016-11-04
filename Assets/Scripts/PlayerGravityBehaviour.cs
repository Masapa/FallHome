﻿using UnityEngine;
using System.Collections;

public class PlayerGravityBehaviour : MonoBehaviour {
    Rigidbody2D rb;
    public bool gravitation = false;
    Vector2 directiong;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	
	}
    public void SetGravity(Vector2 direction, float gravity)
    {
        directiong = direction * gravity;
        gravitation = true;
        Debug.Log(direction);
    }
 
	
	// Update is called once per frame
	void FixedUpdate () {
        if (gravitation)
        {
            rb.velocity += directiong;
        }
        //rb.AddForce(-Vector2.right * 2);
       // Debug.Log("Velocity: "+rb.velocity);
	}
}
