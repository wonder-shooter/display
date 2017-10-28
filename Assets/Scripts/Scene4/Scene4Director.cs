﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene4Director : MonoBehaviour
{

	public AudioClip BGM;
	public AudioClip EndSE;
	
	// アクセサ
	public GameObject Areas;
	public Text[] Names;
	public Text[] Scores;
	
	private GameDirector gameDirector;

	private IEnumerator PlayBGM()
	{
		yield return new WaitForSeconds(1f);
		// n, m-1まで
		AudioSource audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.PlayOneShot(EndSE);
		
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(ShowRank());

		yield return new WaitForSeconds(2f);
		audioSource.loop = true;
		audioSource.clip = BGM;
		audioSource.Play();
		
		yield break;
	}

	private IEnumerator ShowRank()
	{
		// 表示
		Areas.gameObject.SetActive(true);

		// 名前更新
		foreach (var name in Names)
		{
			name.text = String.Format("#{0:D3}", gameDirector.GetNumberIndex());
		}
		
		var players = gameDirector.GetActivePlayer();
		foreach (var player in players)
		{
			int index = (int) player.Color;
			
			int score = player.Score;
			Scores[index].text = String.Format("{0:D4}", score);	
		}
		yield break;
	}

	/**
	 * Use this for initialization
	 */
	void Start () {
		// 非表示
		Areas.gameObject.SetActive(false);
		gameDirector = GameDirector.GetSheredInstance();
		
		StartCoroutine(PlayBGM());
	}
	
	/**
	 * Update is called once per frame
	 */
	void Update () {

		if (Input.GetKeyDown(KeyCode.E))
		{
			gameDirector.GameEnd();
			gameDirector.Action(GameActionEvent.EventType.GameEnd);
		}
	}
}
