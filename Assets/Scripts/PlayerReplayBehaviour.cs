using UnityEngine;
using System.Collections;

public class PlayerReplayBehaviour : MonoBehaviour
{
    public int snapshotIndex;
    private PlayerReplay replay;

    void Start()
    {
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (replay == null) {
            return;
        }

        PlayerReplaySnapshot snapshot = new PlayerReplaySnapshot();

        if (replay.GetSnapshot(snapshotIndex, ref snapshot)) {
            Vector3 position = transform.position;
            Vector3 rotation = transform.eulerAngles;

            position = snapshot.position;
            rotation.z = snapshot.rotation;

            transform.eulerAngles = rotation;
            transform.position = position;

            snapshotIndex++;
        }
        else {
            EndReplay();
        }
    }

    public void SetReplay(PlayerReplay _replay)
    {
        Debug.Log("Starting replay " + _replay.snapshots.Count + "snapshots");

        replay = _replay;
        snapshotIndex = 0;
    }

    void EndReplay()
    {
        Destroy(this.gameObject);
    }
}
