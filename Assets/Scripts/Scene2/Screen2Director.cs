﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Screen2Director : MonoBehaviour {

	public Text startText;
	public Text nextText;
	
	private IEnumerator ShowStartMessage() {
		yield return new WaitForSeconds(0.5f);
		startText.gameObject.SetActive(true);
		yield return new WaitForSeconds(5.0f);
		startText.gameObject.SetActive(false);
	}
	
	private IEnumerator ShowFindMessage() {
		yield return new WaitForSeconds(2.0f);
		nextText.gameObject.SetActive(true);
		yield return new WaitForSeconds(5.0f);
		SceneManager.LoadScene("Scene3");

	}
	
	//
	private void OnFindBoss()
	{
		StartCoroutine(ShowFindMessage());
	}
	
	void Start () {
		// スタートメッセージ
		StartCoroutine(ShowStartMessage());
	}
}
