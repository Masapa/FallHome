using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct PlayerReplaySnapshot
{
    public Vector2 position;
    public float rotation;
}

public class PlayerReplay
{
    public List<PlayerReplaySnapshot> snapshots;

    public PlayerReplay()
    {
        snapshots = new List<PlayerReplaySnapshot>();
    }

    public void AddSnapshot(Vector2 position, float rotation)
    {
        PlayerReplaySnapshot snapshot;
        snapshot.position = position;
        snapshot.rotation = rotation;

        snapshots.Add(snapshot);
    }

    public bool GetSnapshot(int index, ref PlayerReplaySnapshot snapshot)
    {
        if (index < snapshots.Count) {
            snapshot = snapshots[index];
            return true;
        }
        else {
            return false;
        }
    }
}

public class PlayerReplayRecordBehaviour : MonoBehaviour
{
    PlayerReplay replay;

    void Start()
    {
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        if (replay != null) {
            Vector2 position = transform.position;
            float rotation = transform.eulerAngles.z;

            // Adding sample every physics update...
            // Takes a lot of memory but gives us clean replay
            replay.AddSnapshot(position, rotation);
        }
    }

    public void SetReplay(PlayerReplay _replay)
    {
        replay = _replay;
    }
}
