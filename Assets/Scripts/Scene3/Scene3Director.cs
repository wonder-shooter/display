using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Scene3Director : MonoBehaviour {

	public AudioClip[] BGMs;
	public AudioClip Whistle;

	// 銃音
	public AudioClip SE_Shot;
	
	public RawImage[] Scopes;
	
	public Text timer;
	public Text StartMessage;

	private GameDirector gameDirector;
	private float playSeconds = 181f;
	private bool canCount = false;

	// Use this for initialization
	void Start () {
		StartMessage.gameObject.SetActive(false);

		gameDirector = GameDirector.GetSheredInstance();		
		
		// イベントハンドラー設定
		gameDirector.AddListenerScreenPositon(OnScreenPosition);
		gameDirector.AddListenerScreenShot(OnScreenShot);
		
		StartCoroutine(PlayBGM());
		StartCoroutine(ShowStartMessage());
		StartCoroutine(WaitCountUp());
	}
	
	// Update is called once per frame
	void Update () {
		if (canCount && playSeconds >= 0f)
		{
			playSeconds -= Time.deltaTime;
			if (playSeconds < 1f)
			{
				playSeconds = 0f;
				gameDirector.Action(GameActionEvent.EventType.ChaserModeCountEnd);
				canCount = false;
				StartCoroutine(GameEnd());
			}
			UpdateCounter((int)playSeconds);
		}
		
		// デバッグ用 マウス操作
		gameDirector.HoverScreen(Player.ColorType.Pink, Input.mousePosition);
		if (Input.GetMouseButtonDown(0))
		{
			gameDirector.ShotScreen(Player.ColorType.Pink);
		}
	}

	private void OnScreenPosition(Player.ColorType colorType, Vector3 point)
	{
		Scopes[(int) colorType].transform.position = point;
	}

	private void OnScreenShot(Player.ColorType colorType)
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.PlayOneShot(SE_Shot);
	}
	
	//
	private void UpdateCounter(int t)
	{
		timer.text = String.Format("{0:D2}:{1:D2}", (int)(t/60), (int)(t%60));
	}

	private IEnumerator ShowStartMessage()
	{
		yield return new WaitForSeconds(1f);
		StartMessage.gameObject.SetActive(true);
		yield break;
	}

	//
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
	
	//
	private IEnumerator WaitCountUp()
	{		
		yield return new WaitForSeconds(7f);
		canCount = true;
		StartMessage.gameObject.SetActive(false);
		gameDirector.Action(GameActionEvent.EventType.ChaserModeCountStart);
		
		yield break;
	}

	//
	private IEnumerator GameEnd()
	{
		AudioSource audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.Stop();
		audioSource.PlayOneShot(Whistle);
		
		yield return new WaitForSeconds(1f);
		gameDirector.Action(GameActionEvent.EventType.ChaserModeSceneEnd);
		
		yield break;
	}
}
