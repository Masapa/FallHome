using UnityEngine;
using System.Collections;

public class ArrowBehaviour : MonoBehaviour {

    Transform target;
    Transform player;

	// Use this for initialization
	void Start () {
        target = GameObject.Find("Earth").transform;
        player = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find("Player"))
        {
            player = GameObject.Find("Player").transform;
            Vector3 diff = target.position - player.position;
            diff.Normalize();
            float rotz = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotz - 180);
            transform.position = player.position + (-transform.right * 7);
        }

     
	}
}
