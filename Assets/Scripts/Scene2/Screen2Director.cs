using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Screen2Director : MonoBehaviour {

	public Text startText;
	public Text nextText;

	public AudioClip SE_BossFind;
	
	private GameDirector gameDirector;

	//
	private IEnumerator ShowStartMessage() {
		yield return new WaitForSeconds(0.5f);
		startText.gameObject.SetActive(true);
		yield return new WaitForSeconds(5.0f);
		startText.gameObject.SetActive(false);
	}
	
	private IEnumerator ShowFindMessage() {
		
		if(startText.gameObject.activeSelf) startText.gameObject.SetActive(false);
		
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.PlayOneShot(SE_BossFind);
		
		yield return new WaitForSeconds(2.0f);
		nextText.gameObject.SetActive(true);
		
		yield return new WaitForSeconds(5.0f);
		gameDirector.Action(GameActionEvent.EventType.SearchModeSceneEnd);
	}
	
	// 
	private void OnFindBoss()
	{
		StartCoroutine(ShowFindMessage());
	}
	
	void Start ()
	{
		gameDirector = GameDirector.GetSheredInstance(); 
		
		// スタートメッセージ
		StartCoroutine(ShowStartMessage());
	}

	private void Update()
	{
		// デバッグ用
		if (Input.GetMouseButtonDown(0))
		{
			OnFindBoss();
		}

	}
}
