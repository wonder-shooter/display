using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Screen0Director : MonoBehaviour
{
	// BGM
	public AudioClip BGM;
	
	// シュート音
	public AudioClip SE_Shoot;

	// スコープ
	public RawImage[] Scopes;

	
	// タイトル
	public GameObject TitleArea;
	
	// カウントダウン
	public Text Timer;
	
	// エントリー締め切り
	private float startCount = 11f;
	
	// ゲームハンドラー
	private GameDirector gameDirector;
	
	// BGM スタート
	private IEnumerator StartBGM()
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.PlayOneShot(BGM);
		yield break;
	}

	void Start () {

		Timer.gameObject.SetActive(false);

		gameDirector = GameDirector.GetSheredInstance();
		gameDirector.GameReset();
			
		// イベントハンドラー設定
		gameDirector.AddListenerScreenPositon(OnScreenPosition);
		gameDirector.AddListenerScreenShot(OnScreenShot);
		
		StartCoroutine(StartBGM());
	}

	// Update is called once per frame
	void Update () {

		// エントリー表示中なら
		if (Timer.gameObject.activeSelf && startCount >= 0f)
		{
			// カウントダウン
			startCount -= Time.deltaTime;
			if (startCount < 0f)
			{
				// 0 なったらシーン移動
				startCount = 0f;
				gameDirector.Action(GameActionEvent.EventType.TitleSceneEnd);
			}
			Timer.text = String.Format("{0}", (int)(startCount));
		}
		
		// デバッグ用 マウス操作
		gameDirector.HoverScreen(Player.ColorType.Pink, Input.mousePosition);
		if (Input.GetKeyDown(KeyCode.Space))
		{
			gameDirector.ShotScreen(Player.ColorType.Pink);
		}
		
	}
	
	/**
	 * スクリーンのHover情報
	 */
	private void OnScreenPosition(Player.ColorType colorType, Vector3 point)
	{
		Scopes[(int) colorType].transform.position = point;
	}
	
	/**
	 * シュート
	 */
	private void OnScreenShot(Player.ColorType colorType)
	{
		// シュート音再生
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.PlayOneShot(SE_Shoot);

		// シュートしたプレイヤーのエントリー
		gameDirector.PlayerEntry(colorType);
		
		// カウントダウン開始していなければ
		if (Timer.gameObject.activeSelf == false)
		{
			// タイトル非表示
			TitleArea.SetActive(false);
			// タイマーON
			Timer.gameObject.SetActive(true);
		}
		
	}

}
