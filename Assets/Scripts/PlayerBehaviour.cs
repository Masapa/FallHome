using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    public GameObject bloodSpatterPrefab;
    public GameObject explosionPrefab;

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

    private GameController gameController = null;

    bool levelSelection = false;

    // Use this for initialization
    void Start ()
    {
        if (GameObject.Find("LevelSelection"))
        {
            levelSelection = true;
        }
        body = GetComponent<Rigidbody2D>();
        if (body == null) {
            Debug.LogError("Rigidbody2D missing from player");
        }

        particles = GetComponentInChildren<ParticleSystem>();
        if (particles == null) {
            Debug.LogError("ParticleSystem missing from player");
        }

        GameObject gcObj = GameObject.Find("GameController");
        if (gcObj) {
            gameController = gcObj.GetComponent<GameController>();
        }

        if (gameController == null) {
            Debug.LogError("gameController missing from player");
        }

        if (bloodSpatterPrefab == null) {
            Debug.LogError("bloodSpatterPrefab missing from player");
        }

        if (explosionPrefab == null) {
            Debug.LogError("explosionPrefab missing from player");
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


    void OnCollisionStay2D(Collision2D other)
    {
        if(levelSelection)
        {
            if ((levelSelectionTime + 1.5f) <= Time.timeSinceLevelLoad)
            {
                GameObject.Find("SoundManager").GetComponent<FmodBehaviour>().gameBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                Application.LoadLevel(other.gameObject.GetComponent<LevelSelectionBehaviour>().SceneNumber);
            }
        }
    }
    float levelSelectionTime;
    bool levelSelectionFuck = false;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (levelSelection && !levelSelectionFuck) { levelSelectionTime = Time.timeSinceLevelLoad;levelSelectionFuck = true; }

        if (other.transform.tag != "Asteroid")
        {
            if (bloodSpatterPrefab != null)
            {
                Vector3 dir = (transform.position - other.transform.position).normalized;
                GameObject splatter = (GameObject)Instantiate(bloodSpatterPrefab, other.transform, false);
                splatter.transform.localPosition += dir * 1.2f;
            }

            if (explosionPrefab != null)
            {
                Vector3 offset = transform.position - other.transform.position;

                Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);
                rotation.z = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg - 45.0f;

                GameObject explosion = (GameObject)Instantiate(explosionPrefab, other.transform, false);
                explosion.transform.position += offset;
                explosion.transform.eulerAngles = rotation;
            }


            CameraBehaviour cb = Camera.main.gameObject.GetComponent<CameraBehaviour>();
            if (cb != null)
            {
                cb.Shake(1f, 20.0f);
            }

            Kill();
        }
    }

    public void Kill()
    {
        CameraBehaviour cb = Camera.main.gameObject.GetComponent<CameraBehaviour>();
        if (cb != null) {
            cb.Shake(1f, 20.0f);
        }

            // Set these to zero to hide the GUI elements
            numCharges = 0;
            thrustTimer = 0.0f;

            GameObject.Find("SoundManager").GetComponent<FmodBehaviour>().RockImpact();

        try {
        GameObject.Find("SoundManager").GetComponent<FmodBehaviour>().RockImpact();
        } catch (System.Exception e) {
        }


            // Send message to the GameController that we died
            gameController.StartLevelReset();

            explodeparts();

            // Quick hack, because we're already destroyed before the level reset message comes to use
            gameObject.SendMessage("OnLevelReset", null, SendMessageOptions.DontRequireReceiver);

            // Destroy ourself
            Destroy(gameObject);
    }


    void explodeparts()
    {
        for(int i = 0; i <= 5; i++)
        {
            GameObject tmp = Instantiate(Resources.Load("spacemanpalaset_" + i), transform.position, transform.rotation) as GameObject;
            Destroy(tmp, 30);
            tmp.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        }
    }

    private bool IsChargeAvailable()
    {
        return chargeTimer <= 0.0f && numCharges > 0;
    }

    private void ActivateJetpack(float direction)
    {
        if (!IsChargeAvailable()) {
            if (numCharges <= 0)
            {
                GameObject.Find("SoundManager").GetComponent<FmodBehaviour>().PlayJetPackEmpty();
            }
            return;
        }

        if (reloadTimer <= 0.0f) {
            reloadTimer = reloadTime;
        }

        CameraBehaviour cb = Camera.main.gameObject.GetComponent<CameraBehaviour>();
        if (cb != null) {
            cb.Shake(0.5f, 20.0f);
        }

        particles.Play();

        try {
        if (GameObject.Find("SoundManager")) {
            GameObject.Find("SoundManager").GetComponent<FmodBehaviour>().PlayJetPack();
        }
        } catch (System.Exception e) {
        }

        numCharges--;
        chargeTimer = chargeTime;
        thrustAngle = direction;
        steeringAngle = direction;
        thrustTimer = thrustTime;
    }
}
