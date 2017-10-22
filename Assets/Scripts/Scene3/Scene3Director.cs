using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3Director : MonoBehaviour {

	public AudioClip[] BGMs;

    private IEnumerator PlayBGM()
    {
	    yield return new WaitForSeconds(0.5f);
	    // n, m-1まで
	    int r = Random.Range(0, BGMs.Length);
	    AudioSource audioSource = gameObject.GetComponent<AudioSource>();
	    audioSource.loop = true;
	    audioSource.clip = BGMs[r];
	    audioSource.Play();
	    
    	yield break;
    }

	// Use this for initialization
	void Start () {
		StartCoroutine(PlayBGM());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
