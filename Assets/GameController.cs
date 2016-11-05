using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public int seed;

    public GameObject playerPrefab;

    private float levelResetTimer = 0.0f;

    // Time it takes for the level to reset, after calling StartLevelReset()
    public float levelResetTime = 2.0f;

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
        if (levelResetTimer > 0.0f) {
            levelResetTimer -= Time.fixedDeltaTime;
            if (levelResetTimer <= 0.0f) {
                ResetLevel();
            }
        }
    }

    // Resets the level, after a short period
    public void StartLevelReset()
    {
        levelResetTimer = levelResetTime;
    }

    // Reset the level, immedietly
    public void ResetLevel()
    {
        levelResetTimer = 0.0f;

        // Send reset message to all game objects, slow but works
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
        foreach(GameObject go in allObjects) {
            if (go.activeInHierarchy) {
                go.SendMessage("OnLevelReset", null, SendMessageOptions.DontRequireReceiver);
            }
        }

        // Remove any old copies of the player
        GameObject player = GameObject.Find("Player");
        if (player) {
            GameObject.Destroy(player);
        }

        // Re-instantiate the player
        if (playerPrefab) {
            player = Instantiate(playerPrefab);
            if (player) {
                player.transform.position = spawnPosition;

                // Reattach camera
                Camera.main.GetComponent<CameraBehaviour>().SetTarget(player.transform);
            }
        }
    }
}
