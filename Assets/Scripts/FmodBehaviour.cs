using UnityEngine;
using System.Collections;


public class FmodBehaviour : MonoBehaviour {



	float distance;
	float initialDistance;

	Transform player;
	Transform earth;

	FMOD.Studio.EventInstance gameBGM;	
	FMOD.Studio.ParameterInstance pDistance;

	FMOD.Studio.EventInstance jetpack_Boost;

    FMOD.Studio.EventInstance planetCrash;

	/*
	[FMODUnity.EventRef]
	public string gameBGMSnapshot = "snapshot:/Game";
	FMOD.Studio.EventInstance gameBGMSnapshotEv;
	*/

	// Use this for initialization
	void Start () {
        if (GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player").transform;
            earth = GameObject.Find("Earth").transform;

            distance = Vector3.Distance(player.position, earth.position);
            initialDistance = Vector3.Distance(player.position, earth.position);

        }
		/* Ennen kuin mitään fmodeilua, pitää lausua loitsut */
		/*
		var FMOD_StudioSystem = FMODUnity.RuntimeManager.StudioSystem;
		FMOD.Studio.CPU_USAGE cpuUsage;
		studioSystem.getCPUUsage( out cpuUsage);
		*/
		Debug.Log ("Music is created here");
		gameBGM = FMODUnity.RuntimeManager.CreateInstance ("event:/GameBGM"); 
		gameBGM.getParameter ("Distance", out pDistance);
		gameBGM.start ();
		Debug.Log ("Music SHOULD be playing");

		pDistance.setValue (scaledDistance( initialDistance, distance));

		jetpack_Boost = FMODUnity.RuntimeManager.CreateInstance ("event:/Jetpack_Boost");
        planetCrash = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerDeath");

//		gameBGMSnapshotEv = FMODUnity.RuntimeManager.CreateInstance (gameBGMSnapshot);
//		gameBGMSnapshotEv.start ();


	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player").transform;
            earth = GameObject.Find("Earth").transform;
            distance = Vector3.Distance(player.position, earth.position);
            pDistance.setValue(scaledDistance(initialDistance, distance));
            Debug.Log("Distance: " + distance);

                
        }


	}
    public void PlayJetPack()
    {
        jetpack_Boost.start();
    }
    public void RockImpact()
    {
        Debug.Log("CRAASH!");
        planetCrash.start();
    }

	// Return 0 to 1.0 based based on initial distance and current distance. 
	// 0.5 on initial distance, 1.0 on 2x and over

	float scaledDistance( float init, float d ) {
		float a = d / init;
		if( a>1.0f ) { 
			return 1.0f;
		}
		else return a;
	}

	public void oneShotSound (string sfxName) {

	}
}
