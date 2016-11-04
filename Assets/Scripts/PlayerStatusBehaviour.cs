using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatusBehaviour : MonoBehaviour {
    PlayerBehaviour player;
    public GameObject guiPrefab;

    RectTransform playerStatusTransform;

    Text chargesText;
    Text thrustText;

    void Start ()
    {
        player = GetComponent<PlayerBehaviour>();
        if (player == null) {
            Debug.LogError("PlayerBehaviour is missing");
        }

        if (guiPrefab == null) {
            Debug.LogError("guiPrefab is missing");
        }

        GameObject canvas = GameObject.Find("WorldSpaceCanvas");
        if (canvas == null) {
            Debug.LogError("WorldSpaceCanvas object missing");
            return;
        }

        GameObject gui = Instantiate(guiPrefab);
        playerStatusTransform = gui.GetComponent<RectTransform>();
        chargesText = gui.transform.Find("ThrustCharges").gameObject.GetComponent<Text>();
        thrustText = gui.transform.Find("ThrustMeter").gameObject.GetComponent<Text>();

        gui.transform.SetParent(canvas.transform, false);
    }

    void Update()
    {
        if (playerStatusTransform == null || chargesText == null || thrustText == null) {
            return;
        }

        Vector2 pos = player.transform.position;
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos);

        playerStatusTransform.anchorMin = viewportPoint;
        playerStatusTransform.anchorMax = viewportPoint;

        chargesText.text = player.numCharges.ToString();
        if (player.thrustTimer <= 0.0f) {
            thrustText.text = "";
        }
        else {
            thrustText.text = player.thrustTimer.ToString("0.00");
        }
    }
}
