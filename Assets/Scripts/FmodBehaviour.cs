using UnityEngine;
using System.Collections;


public class FmodBehaviour : MonoBehaviour {



	float distance;
	Transform player;
	Transform earth;

	FMOD.Studio.EventInstance gameBGM;	
	[FMODUnity.EventRef]
	public string gameBGMSnapshot = "snapshot:/GameBGM";
	FMOD.Studio.EventInstance gameBGMSnapshotEv;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").transform;
		earth = GameObject.Find ("Earth").transform;

		distance = Vector3.Distance (player.position, earth.position);

		/* Ennen kuin mitään fmodeilua, pitää lausua loitsut */
		/*
		var studioSystem = FMODUnity.RuntimeManager.StudioSystem;
		FMOD.Studio.CPU_USAGE cpuUsage;
		studioSystem.getCPUUsage( out cpuUsage);
		*/

		gameBGM = FMODUnity.RuntimeManager.CreateInstance("event:/GameBGM"); 
		gameBGM.start ();


		gameBGMSnapshotEv = FMODUnity.RuntimeManager.CreateInstance (gameBGMSnapshot);
		gameBGMSnapshotEv.start ();
	
	}
	
	// Update is called once per frame
	void Update () {
		
		distance = Vector3.Distance (player.position, earth.position);	
		gameBGM.setParameterValue("Distance", (float)distance);

	}
}
