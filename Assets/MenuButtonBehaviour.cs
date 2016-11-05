using UnityEngine;
using System.Collections;

public class MenuButtonBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void StartGame()
    {
        //Debug.Log("painettu");
        GameObject.Find("SoundManager").GetComponent<FmodBehaviour>().gameBGM.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        Application.LoadLevel(7);
    }
}
