using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
    public int seed;

    public GameObject playerPrefab;
    public GameObject replayPrefab;

    public GameObject levelCompletePrefab;
    public GameObject levelCompleteParticles;

    private float levelResetTimer = 0.0f;

    private bool isLevelFinished;

    // Time it takes for the level to reset, after calling StartLevelReset()
    public float levelResetTime = 2.0f;

    // Where to spawn the player after dying.
    public Vector3 spawnPosition;

    public List<PlayerReplay> replays;
    public PlayerReplay recordingReplay;

	// Use this for initialization
	void Start ()
    {
        replays = new List<PlayerReplay>();
        isLevelFinished = false;

        GameObject player = GameObject.Find("Player");
        if (player) {
            recordingReplay = new PlayerReplay();
            PlayerReplayRecordBehaviour prrb = player.AddComponent<PlayerReplayRecordBehaviour>();
            prrb.SetReplay(recordingReplay);
            spawnPosition = player.transform.position;
        }
        else {
            Debug.LogError("No Player object in scene");
        }

        if (replayPrefab == null) {
            Debug.LogError("replayPrefab is missing from GameController");
        }

        if (playerPrefab == null) {
            Debug.LogError("playerPrefab is missing from GameController");
        }

        if (levelCompletePrefab == null) {
            Debug.LogError("levelCompletePrefab is missing from GameController");
        }

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            GameObject.Find("SoundManager").GetComponent<FmodBehaviour>().gameBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            Application.LoadLevel(1);
        }

        if (isLevelFinished) {
            return;
        }

        if (Input.GetKeyDown("r")) {
            // Ignore replay when reseting
            recordingReplay = null;
            ResetLevel();
        }
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

    void OnLevelComplete()
    {
        isLevelFinished = true;
        
        if (levelCompletePrefab) {
            GameObject tmp = (GameObject)Instantiate(
                levelCompletePrefab, GameObject.Find("ScreenSpaceCanvas").transform, false);

            tmp.name = "LevelCompleteGui";

            string score = "";
            GameObject sm = GameObject.Find("ScoreManager");
            if (sm) {
                score = sm.GetComponent<ScoreManager>().GetScore().ToString("0");
            }

            GameObject.Find("ScreenSpaceCanvas/LevelCompleteGui/LevelCompleteScore").GetComponent<Text>().text =
                "Score: " + score;
        }

        if (levelCompleteParticles)
        {
            Instantiate(levelCompleteParticles, GameObject.FindGameObjectsWithTag("Earth")[0].transform, false);
        }
    }

    // Resets the level, after a short period
    public void StartLevelReset()
    {
        levelResetTimer = levelResetTime;

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
        foreach(GameObject go in allObjects) {
            if (go.activeInHierarchy) {
                go.SendMessage("OnPlayerDeath", null, SendMessageOptions.DontRequireReceiver);
            }
        }
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

        GameObject player;

        // Remove any old copies of the player, should be destroyed already but
        // better safe than sorry
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) {
            GameObject.Destroy(p);
        }

        // Store the replay
        if (recordingReplay != null) {
            replays.Add(recordingReplay);
            recordingReplay = null;
        }

        // Re-instantiate the player
        if (playerPrefab) {
            player = Instantiate(playerPrefab);
            player.transform.position = spawnPosition;

            // Reattach camera
            Camera.main.GetComponent<CameraBehaviour>().SetTarget(player.transform);

            PlayerReplayRecordBehaviour prrb = player.AddComponent<PlayerReplayRecordBehaviour>();
            recordingReplay = new PlayerReplay();
            prrb.SetReplay(recordingReplay);
        }

        SpawnGhosts();
    }

    public void SpawnGhosts()
    {
        if (replayPrefab) {
            // Create replay ghosts for each player
            foreach (PlayerReplay replay in replays) {
                GameObject replayPlayer = (GameObject)Instantiate(replayPrefab);
                replayPlayer.transform.position = spawnPosition;

                PlayerReplayBehaviour pr = replayPlayer.AddComponent<PlayerReplayBehaviour>();
                pr.SetReplay(replay);
            }
        }
    }
}
