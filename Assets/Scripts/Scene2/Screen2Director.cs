using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Screen2Director : MonoBehaviour {

	public Text startText;
	public Text nextText;

	public AudioClip BGM;
	
	public AudioClip SE_BossFindAll;
	public AudioClip SE_BossFindOne;
	
	private GameDirector gameDirector;

	private IEnumerator StartBGM()
	{
		// BGM 再生
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.clip = BGM;
		audioSource.Play();
		
		yield break;
	}

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
		audioSource.Stop();
		yield return new WaitForSeconds(0.5f);

		audioSource.PlayOneShot(SE_BossFindAll);
		
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
		
		Debug.Log(gameDirector.GetActivePlayerCount());
		
		// スタートメッセージ
		StartCoroutine(StartBGM());
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
