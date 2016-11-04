using UnityEngine;
using System.Collections;

public class BackgroundScrollingBehaviour : MonoBehaviour {

    public float speed = 0.05f;
    public Vector3 direction = new Vector3(0.1f,0f,0f);

    private Renderer rend;
    private Vector3 position;

    void Start()
    {
        rend = GetComponent<Renderer>();
        position = new Vector3(0f, 0f, 0f);
    }

    void Update()
    {
        position += direction;
        Vector2 offset = new Vector2(position.x * speed, position.y * speed);
        rend.materials[0].mainTextureOffset = offset;
    }
}
