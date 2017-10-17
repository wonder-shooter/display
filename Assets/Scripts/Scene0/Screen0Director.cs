using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Screen0Director : MonoBehaviour {

	public AudioSource AudioSource;
    public AudioClip SE;

	// Use this for initialization
	void Start () {
		// this.GetComponent<Button>().onClick.AddListener(() => this.AudioSource.Play());
		AudioSource[] sources = GetComponents<AudioSource>();
		Debug.Log(sources.Length);

		//ピンク イエロー グリーン
	}

	
	// Update is called once per frame
	void Update () {
	}

	public void OnClickButton()
	{
		Debug.Log("onClick");
		AudioSource.PlayOneShot(AudioSource.clip);
		SceneManager.LoadScene("Scene2");

	}
}
