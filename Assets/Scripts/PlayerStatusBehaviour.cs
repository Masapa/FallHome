using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatusBehaviour : MonoBehaviour {

    public GameObject worldGuiPrefab;
    public GameObject screenGuiPrefab;

    public GameObject chargeIconPrefab;

    private RectTransform worldGui;
    private RectTransform screenGui;

    private Text thrustText;

    private PlayerBehaviour player;

    void Start ()
    {
        player = GetComponent<PlayerBehaviour>();
        if (player == null) {
            Debug.LogError("PlayerBehaviour is missing");
        }

        if (worldGuiPrefab == null) {
            Debug.LogError("worldGuiPrefab is missing");
        }

        if (screenGuiPrefab == null) {
            Debug.LogError("screenGuiPrefab is missing");
        }

        GameObject worldCanvas = GameObject.Find("WorldSpaceCanvas");
        if (worldCanvas == null) {
            Debug.LogError("WorldSpaceCanvas object missing from scene");
            return;
        }

        GameObject screenCanvas = GameObject.Find("ScreenSpaceCanvas");
        if (screenCanvas == null) {
            Debug.LogError("ScreenSpaceCanvas object is missing from scene");
            return;
        }

        GameObject worldGuiObj = Instantiate(worldGuiPrefab);
        worldGui = worldGuiObj.GetComponent<RectTransform>();
        worldGuiObj.transform.SetParent(worldCanvas.transform, false);
        thrustText = worldGuiObj.transform.Find("ThrustMeter").gameObject.GetComponent<Text>();

        GameObject screenGuiObj = Instantiate(screenGuiPrefab);
        screenGui = screenGuiObj.GetComponent<RectTransform>();
        screenGuiObj.transform.SetParent(worldCanvas.transform, false);
    }

    void Update()
    {
        UpdateWorldGui();
        UpdateScreenGui();
    }

    void UpdateScreenGui()
    {
        if (screenGui == null) {
            return;
        }

        if (screenGui.childCount != player.numCharges) {
            
            foreach (Transform child in screenGui.transform) {
                GameObject.Destroy(child.gameObject);
            }

            for (int i = 0; i < player.numCharges; i++) {
                GameObject chargeIcon = Instantiate(chargeIconPrefab);
                chargeIcon.transform.SetParent(screenGui, false);
                RectTransform t = chargeIcon.GetComponent<RectTransform>();

                Vector3 position = t.position;
                position.x -= t.rect.width * i;
                t.position = position;
            }
        }
    }

    void UpdateWorldGui()
    {
        if (worldGui == null) {
            return;
        }

        Vector2 pos = player.transform.position;
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos);

        worldGui.anchorMin = viewportPoint;
        worldGui.anchorMax = viewportPoint;

        if (player.thrustTimer <= 0.0f) {
            thrustText.text = "";
        }
        else {
            thrustText.text = player.thrustTimer.ToString("0.00");
        }
    }
}
