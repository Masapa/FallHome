using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    public int numCharges = 3;
    public int maxCharges = 3;

    // How long thrust is applied
    public float thrustTime = 1.0f;

    // Time left in the jetpack thrust
    public float thrustTimer = 0.0f;

    // Amount of thrust power lost when reaching
    // The end of jetpack charge
    public float thrustFallof = 1.0f;

    // Angle to apply the thrust to
    public Vector2 thrustAngle;

    public float thrustForce = 3.0f;

    // Time it takes for the jetpack to recharge
    public float chargeTime = 2.0f;

    // Time left to recharge
    public float chargeTimer = 0.0f;

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
    }

    // Update is called once per frame
    void Update ()
    {
        Vector2 playerToCamera = Camera.main.ScreenToWorldPoint(
            Input.mousePosition) - transform.position;

        float playerToCameraAngle = Mathf.Atan2(playerToCamera.y,
            playerToCamera.x) * Mathf.Rad2Deg - 90.0f;

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = playerToCameraAngle - 90.0f;
        transform.eulerAngles = rotation;

        if (Input.GetMouseButtonDown(0)) {
            ActivateJetpack(playerToCamera);
        }

        if (thrustTimer < 0.0f && particles.isPlaying) {
            particles.Stop();
        }
    }

    void FixedUpdate()
    {
        if (chargeTimer > 0.0f) {
            chargeTimer -= Time.fixedDeltaTime;
        }

        if (thrustTimer > 0.0f) {
            float thrustLeft = thrustTimer / thrustTime;
            float force = Mathf.Pow(thrustForce * thrustLeft, 2);

            body.AddForce(thrustAngle * force);

            thrustTimer -= Time.fixedDeltaTime;
        }
    }

    private bool IsChargeAvailable()
    {
        return chargeTimer <= 0.0f && numCharges > 0;
    }

    private void ActivateJetpack(Vector2 direction)
    {
        if (!IsChargeAvailable()) {
            return;
        }

        particles.Play();

        numCharges--;
        chargeTimer = chargeTime;
        thrustAngle = direction.normalized;
        thrustTimer = thrustTime;
    }
}
