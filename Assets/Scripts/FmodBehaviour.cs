﻿using UnityEngine;
using System.Collections;


public class FmodBehaviour : MonoBehaviour {



	float distance;
	float initialDistance;

	Transform player;
	Transform earth;

	public FMOD.Studio.EventInstance gameBGM;	
	FMOD.Studio.ParameterInstance pDistance;

	FMOD.Studio.EventInstance jetpack_Boost;
    FMOD.Studio.EventInstance jetpack_Empty;
    public FMOD.Studio.EventInstance LevelSelection;
    public FMOD.Studio.EventInstance MenuSelect;
    FMOD.Studio.EventInstance planetCrash;
   public FMOD.Studio.EventInstance LevelComplete;
    /*
	[FMODUnity.EventRef]
	public string gameBGMSnapshot = "snapshot:/Game";
	FMOD.Studio.EventInstance gameBGMSnapshotEv;
	*/

    // Use this for initialization
    void Start () {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;

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
        if (Application.loadedLevel >= 1)
        {
            gameBGM = FMODUnity.RuntimeManager.CreateInstance("event:/GameBGM");
            gameBGM.getParameter("Distance", out pDistance);
        }
        else { gameBGM = FMODUnity.RuntimeManager.CreateInstance("event:/MenuBGM"); }
		gameBGM.start ();
		Debug.Log ("Music SHOULD be playing");
        LevelSelection = FMODUnity.RuntimeManager.CreateInstance("event:/StartScreenChoice");
        MenuSelect = FMODUnity.RuntimeManager.CreateInstance("event:/LevelSelect");
        LevelComplete = FMODUnity.RuntimeManager.CreateInstance("event:/LevelComplete");
        pDistance.setValue (scaledDistance( initialDistance, distance));

		jetpack_Boost = FMODUnity.RuntimeManager.CreateInstance ("event:/Jetpack_Boost");
        planetCrash = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerDeath");
        jetpack_Empty = FMODUnity.RuntimeManager.CreateInstance("event:/Jetpack_Empty");

//		gameBGMSnapshotEv = FMODUnity.RuntimeManager.CreateInstance (gameBGMSnapshot);
//		gameBGMSnapshotEv.start ();


	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
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
    public void PlayJetPackEmpty()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        jetpack_Empty.getPlaybackState(out state);
        if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            jetpack_Empty.start();
        }
        
    }
    public void RockImpact()
    {
        Debug.Log("CRAASH!");
        planetCrash.start();
    }

	// Return 0 to 1.0 based based on initial distance and current distance. 
	// 0.5 on initial distance, 1.0 on 2x and over

	float scaledDistance( float init, float d ) {
		float a = d / (init*2);
		if( a>1.0f ) { 
			return 1.0f;
		}
		else return a;
	}

	public void oneShotSound (string sfxName) {

	}
}
