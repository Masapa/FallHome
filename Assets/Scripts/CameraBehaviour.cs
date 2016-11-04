﻿using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {
     
    Vector3 position;
    Transform target;

    public int samplesPerSecond = 20;

    public Vector2 [] shakeSamples;

    public float shakeTime;
    public float shakeTimer;

    public float amplitude = 0.5f;

    void Start()
    {
        position = transform.position;

        if (target == null) {
            target = GameObject.Find("Player").transform;
        }

        shakeTime = 0.0f;
        shakeTimer = 0.0f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            Shake(0.5f, 20);
        }

        if (target) {
            position = target.position;
        }

        if (shakeTime > 0.0f) {
            shakeTime -= Time.deltaTime;
        }

        Vector2 shake = GetShake();

        transform.position = new Vector3(
            position.x + shake.x,
            position.y + shake.y,
            transform.position.z
        );
    }

    private Vector2 GetShake()
    {
        float s = (shakeTimer - shakeTime) / (1.0f / samplesPerSecond);
        int sampleIndex = (int) s;


        if (shakeTimer > 0.0f && sampleIndex + 1 < shakeSamples.Length) {
            float decay = shakeTime / shakeTimer;

            Vector2 s0 = shakeSamples[sampleIndex];
            Vector2 s1 = shakeSamples[sampleIndex + 1];

            return (Vector2.Lerp(s0, s1, s - sampleIndex) * decay) * amplitude;
        }
        else {
            return new Vector2(0.0f, 0.0f);
        }
    }

    private void CalculateShakeSamples(int numSamples)
    {
        shakeSamples = new Vector2 [numSamples];
        for (int i = 0; i < shakeSamples.Length; i++) {
            shakeSamples[i] = new Vector2(
                Random.value,
                Random.value
            );
        }
    }

    void Shake(float duration, float frequency)
    {
        // Already shaking for longer duration than the new shake?
        if (duration < shakeTimer) {
            return;
        }

        shakeTimer = duration;
        shakeTime = duration;

        CalculateShakeSamples((int)(duration / (1.0f / samplesPerSecond)));
    }
}
