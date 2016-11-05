﻿using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    public GameObject bloodSpatterPrefab;

    public int numCharges = 0;
    public int maxCharges = 3;

    // How long thrust is applied
    public float thrustTime = 1.0f;

    // Time left in the jetpack thrust
    public float thrustTimer = 0.0f;

    // Angle to apply the thrust to
    private float thrustAngle;

    // Angle to steer towards
    private float steeringAngle;

    // Max thrust force applied to player
    public float thrustForce = 3.0f;

    // Max steering angle during thrust
    public float thrustSteering = 15.0f;

    // Time it takes for the jetpack to recharge
    public float chargeTime = 2.0f;

    // Time left to recharge
    private float chargeTimer = 0.0f;

    public float reloadTimer = 0.0f;
    public float reloadTime = 4.0f;

    private Rigidbody2D body;
    private ParticleSystem particles;

    // Use this for initialization
    void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        if (body == null) {
            Debug.LogError("Rigidbody2D missing from player");
        }

        particles = GetComponent<ParticleSystem>();
        if (particles == null) {
            Debug.LogError("ParticleSystem missing from player");
        }

        if (bloodSpatterPrefab == null) {
            Debug.LogError("bloodSpatterPrefab missing from player");
        }


        numCharges = maxCharges;
    }

    // Update is called once per frame
    void Update ()
    {
        Vector2 playerToCamera = Camera.main.ScreenToWorldPoint(
            Input.mousePosition) - transform.position;

        float playerToCameraAngle = Mathf.Atan2(playerToCamera.y,
            playerToCamera.x) * Mathf.Rad2Deg;

        steeringAngle = playerToCameraAngle;

        if (thrustTimer <= 0.0f) {
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z = playerToCameraAngle - 180.0f;
            transform.eulerAngles = rotation;
        }

        if (Input.GetMouseButtonDown(0)) {
            ActivateJetpack(playerToCameraAngle);
        }

        if (thrustTimer <= 0.0f && !particles.isStopped) {
            particles.Stop();
        }
    }

    void FixedUpdate()
    {
        Debug.Log(body.velocity);
        if (chargeTimer > 0.0f) {
            chargeTimer -= Time.fixedDeltaTime;
        }

        float steerOffset = steeringAngle - thrustAngle;
        thrustAngle += Mathf.Clamp(
            steerOffset,
            Time.fixedDeltaTime * -thrustSteering,
            Time.fixedDeltaTime * thrustSteering
        );

        if (thrustTimer > 0.0f) {
            float thrustLeft = thrustTimer / thrustTime;
            float force = thrustForce * Mathf.Pow(thrustLeft, 2);

            Vector2 vector = new Vector2(
                Mathf.Cos(thrustAngle * Mathf.Deg2Rad),
                Mathf.Sin(thrustAngle * Mathf.Deg2Rad)
            );
            
            body.AddForce(vector * force);

            thrustTimer -= Time.fixedDeltaTime;
        }

        if (reloadTimer > 0.0f) {
            reloadTimer -= Time.fixedDeltaTime;
            if (reloadTimer <= 0.0f) {
                numCharges = maxCharges;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (bloodSpatterPrefab != null) {
            Vector3 dir = (transform.position - other.transform.position).normalized;
            GameObject splatter = (GameObject)Instantiate(bloodSpatterPrefab, other.transform, false);
            splatter.transform.localPosition += dir * 1.2f;
            
            CameraBehaviour cb = Camera.main.gameObject.GetComponent<CameraBehaviour>();
            if (cb != null) {
                cb.Shake(1f, 20.0f);
            }
        }
        explodeParts();
        Destroy(gameObject);

    }
    
    void explodeParts()
    {
        for(int i = 0; i < 5;i++)
        {
            
           GameObject tmp = Instantiate(Resources.Load("spacemanpalaset_" + i) as GameObject,transform.position + new Vector3(0,Random.Range(0.5f,1)),transform.rotation) as GameObject;
            Destroy(tmp, 30);
            tmp.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-10,10),Random.Range(-10,10));
        }
    }

    private bool IsChargeAvailable()
    {
        return chargeTimer <= 0.0f && numCharges > 0;
    }

    private void ActivateJetpack(float direction)
    {
        if (!IsChargeAvailable()) {
            return;
        }

        if (reloadTimer <= 0.0f) {
            reloadTimer = reloadTime;
        }

        CameraBehaviour cb = Camera.main.gameObject.GetComponent<CameraBehaviour>();
        if (cb != null) {
            cb.Shake(0.5f, 10.0f);
        }

        particles.Play();

        numCharges--;
        chargeTimer = chargeTime;
        thrustAngle = direction;
        steeringAngle = direction;
        thrustTimer = thrustTime;
    }
}
