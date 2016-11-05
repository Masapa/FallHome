using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public int seed;

    public GameObject playerPrefab;

    private float playerRespawnTimer = 0.0f;

    // Time it takes for the player to respawn after he/she has died
    public float playerRespawnTime = 2.0f;

    // Where to spawn the player after dying.
    public Vector3 spawnPosition;


	// Use this for initialization
	void Start ()
    {
        GameObject player = GameObject.Find("Player");
        if (player) {
            spawnPosition = player.transform.position;
        }
        else {
            Debug.LogError("No Player object in scene");
        }

        if (playerPrefab == null) {
            Debug.LogError("playerPrefab is missing from GameController");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    void FixedUpdate()
    {
        if (playerRespawnTimer > 0.0f) {
            playerRespawnTimer -= Time.fixedDeltaTime;
            if (playerRespawnTimer <= 0.0f) {
                RespawnPlayer();
            }
        }
    }

    void RespawnPlayer()
    {
        // Remove any old copies of the player
        GameObject player = GameObject.Find("Player");
        if (player) {
            GameObject.Destroy(player);
        }

        if (playerPrefab) {
            player = Instantiate(playerPrefab);
            if (player) {
                player.transform.position = spawnPosition;

                // Reattach camera
                Camera.main.GetComponent<CameraBehaviour>().SetTarget(player.transform);
            }
        }
    }

    void OnLevelReset()
    {
        playerRespawnTimer = playerRespawnTime;
    }
}
