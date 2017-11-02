using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Screen2Director : MonoBehaviour {

	// 開始
	public Text startText;
	// 発見次へ
	public Text nextText;
	
	// 背景
	public GameObject Background;
	
	// BGM
	public AudioClip BGM;
	
	// 発見音
	public AudioClip SE_BossFindAll;
	public AudioClip SE_BossFindOne;

	// スポットライト
	public GameObject[] SpotLights;
	
	// ゲームハンドラー
	private GameDirector gameDirector;

    // ボス発見フラグ
	private bool isFindBoss;

	// 敵探索
	private int maskPadding = 50;
	private Vector2[] targets = new Vector2[]
	{
		new Vector2(), new Vector2(), new Vector2(),   
	};
    // プレイヤーごとのターゲット発見フラグ(Activeなプレイヤーがいればfalse)
    private bool[] isTargetsFind = new bool[] { true, true, true };
	
	// BGM開始
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
		if(!isFindBoss){
			startText.gameObject.SetActive(true);
			yield return new WaitForSeconds(5.0f);
			startText.gameObject.SetActive(false);
		}
		yield break;
	}
	
	/**
	 * モンスター発見
	 */
	private IEnumerator ShowFindMessage() {
		
		// 開始テキスト非表示
		if(startText.gameObject.activeSelf) startText.gameObject.SetActive(false);
		if(nextText.gameObject.activeSelf) yield break;
		
		// BGMストップ
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.Stop();
		yield return new WaitForSeconds(0.5f);
		// 発見音再生
		audioSource.PlayOneShot(SE_BossFindAll);
		yield return new WaitForSeconds(2.0f);
		// テキスト表示
		nextText.gameObject.SetActive(true);
		yield return new WaitForSeconds(2f);
		// メイン照明ON
		Background.gameObject.SetActive(false);
		yield return new WaitForSeconds(2f);
		// 遷移
		gameDirector.Action(GameActionEvent.EventType.SearchModeSceneEnd);
		
		yield break;
	}
	
	/**
	 * スクリーンのHover情報
	 */
	private void OnScreenPosition(Player.ColorType colorType, Vector3 position)
	{
		int color = (int) colorType;
		if (isTargetsFind[color]) return;

		// Light移動
		SpotLights[color].transform.position = position;
		
		
		
		
	}

    /**
	 * シュート
	 */
    private void OnScreenShot(Player.ColorType colorType, Vector3 position)
    {
        // 
        OnFindBoss();
    }

    // 
    private void OnFindBoss()
	{
		if (isFindBoss) return;
		isFindBoss = true;
		StartCoroutine(ShowFindMessage());
	}
	
	void Start ()
	{
		gameDirector = GameDirector.GetSheredInstance();

        // イベントハンドラー設定
        gameDirector.AddListenerScreenPositon(OnScreenPosition);
        gameDirector.AddListenerScreenShot(OnScreenShot);
		
		// ボス対象位置設定
		foreach (Player.ColorType color in Enum.GetValues(typeof(Player.ColorType)))
		{
			// 
			Player player = gameDirector.GetPlayer(color);
			if (player.IsEntry)
			{
				isTargetsFind[(int) color] = false;
			}
			int targetX = Random.Range(maskPadding, Screen.width - maskPadding);
			int targetY = Random.Range(maskPadding, Screen.height - maskPadding);

			targets[(int)player.Color] = new Vector2(targetX, targetY);
			Debug.Log(targets[(int)player.Color]);
		}

		// スタートメッセージ
		StartCoroutine(StartBGM());
		StartCoroutine(ShowStartMessage());
	}

	void Update()
	{
        // デバッグ用 マウス操作
        var mousePos = Input.mousePosition;
        mousePos.z = 0.1f;
        var point = Camera.main.ScreenToWorldPoint(mousePos);
        gameDirector.HoverScreen(Player.ColorType.Pink, point);
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OnFindBoss();
		}
		//		if (Input.GetMouseButtonDown(0))
		//		{
		//			gameDirector.ShotScreen(Player.ColorType.Pink, point);
		//		}	
	}
}