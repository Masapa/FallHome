﻿using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

    public Transform player1;
    public Transform player2;

    private const float DISTANCE_MARGIN = 1.0f;

    private Vector3 middlePoint;
    private float distanceFromMiddlePoint;
    private float distanceBetweenPlayers;
    private float cameraDistance;
    private float aspectRatio;
    private float fov;
    private float tanFov;

    void Start()
    {
        player1 = GameObject.Find("Player").GetComponent<Transform>();
        player2 = GameObject.Find("Earth").GetComponent<Transform>();
        aspectRatio = Screen.width / Screen.height;
        tanFov = Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView / 2.0f);
    }

    void Update()
    {
        // Position the camera in the center.
        Vector3 newCameraPos = Camera.main.transform.position;
        newCameraPos.x = middlePoint.x;
        Camera.main.transform.position = newCameraPos;

        // Find the middle point between players.
        Vector3 vectorBetweenPlayers = player2.position - player1.position;
        middlePoint = player1.position + 0.5f * vectorBetweenPlayers;

        // Calculate the new distance.
        distanceBetweenPlayers = vectorBetweenPlayers.magnitude;
        cameraDistance = (distanceBetweenPlayers / 2.0f / aspectRatio) / tanFov;

        // Set camera to new position.
        Vector3 dir = (Camera.main.transform.position - middlePoint).normalized;
        Camera.main.transform.position = middlePoint + dir * (cameraDistance + DISTANCE_MARGIN);
    }
}