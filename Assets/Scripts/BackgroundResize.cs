using UnityEngine;
using System.Collections;

public class BackgroundResize : MonoBehaviour {
    void LateUpdate()
    {
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Screen.width / Screen.height;
        transform.localScale = new Vector3(width, height, 1);
    }
}
