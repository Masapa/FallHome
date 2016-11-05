using UnityEngine;
using System.Collections;

public class fillcamera : MonoBehaviour {

    SpriteRenderer sr;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(1, 1, 1);
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;
        float worldscreen = Camera.main.orthographicSize * 2;
        float worldscreenwidth = worldscreen / Screen.height * Screen.width;
        transform.localScale = new Vector2(worldscreenwidth / width, worldscreen / height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
