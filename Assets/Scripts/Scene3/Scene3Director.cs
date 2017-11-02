using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using Random = UnityEngine.Random;

public class Scene3Director : MonoBehaviour {
    
	public AudioClip[] BGMs;
	public AudioClip Whistle;

	// 銃音
	public AudioClip SE_Shot;
	
	// スコープ
	public RawImage[] Scopes;
	// スコア
	public Text[] Scores;
	// スプラッシュ画像
	public Texture2D[] SplashImages;

	// 右上タイマー
	public Text timer;
	// スタートメッセージ
	public Text StartMessage;

    // インクが衝突するレイヤー
    public LayerMask InkedRayrMask;

    // ゲームハンドラー
    private GameDirector gameDirector;
	// プレイ時間
	private float playSeconds = 181f;
	// カウントアップ判定
	private bool canCount = false;

	/**
	 * Use this for initialization
	 */
	void Start () {
		// スタートメッセージ非表示
		StartMessage.gameObject.SetActive(false);
		
		// ゲームハンドラー
		gameDirector = GameDirector.GetSheredInstance();		
		
		// イベントハンドラー設定
		gameDirector.AddListenerScreenPositon(OnScreenPosition);
		gameDirector.AddListenerScreenShot(OnScreenShot);

		var players = gameDirector.GetActivePlayer();
		foreach (var player in players)
		{
			int score = player.Score;
			int index = (int) player.Color;
			Scores[index].text = String.Format("{0}", score);	
		}
		
		StartCoroutine(PlayBGM());
		StartCoroutine(ShowStartMessage());
        // 
        SteamVR.instance.hmd.ResetSeatedZeroPose();
        StartCoroutine(WaitCountUp());
	}
	
	/**
	 * Update is called once per frame
	 */
	void Update () {
		
		// counter更新
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


        // 着席モードでRキーで位置トラッキングをリセットする
        if (Input.GetKeyDown(KeyCode.R))
        {
            SteamVR.instance.hmd.ResetSeatedZeroPose();
        }


        // デバッグ用 マウス操作
        var mousePos = Input.mousePosition;
        mousePos.z = 0.1f;
        var point = Camera.main.ScreenToWorldPoint(mousePos);
        gameDirector.HoverScreen(Player.ColorType.Pink, point);
        if (Input.GetMouseButtonDown(0))
        {
            gameDirector.ShotScreen(Player.ColorType.Pink, point);

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(point));

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                InkSplashShaderBehavior script = hit.collider.gameObject.GetComponent<InkSplashShaderBehavior>();
                if (null != script && canCount)
                {
                    var colorType = Player.ColorType.Pink;
                    var index = (int)colorType;
                    script.PaintOn(hit.textureCoord, SplashImages[index]);
                    string hitTag = hit.collider.tag;

                    gameDirector.AddScore(colorType);

                    int score = gameDirector.GetScore(colorType);
                    Scores[index].text = String.Format("{0}", score);
                }
            }            
        }
    }

	/**
	 * スクリーンホバー
	 */
	private void OnScreenPosition(Player.ColorType colorType, Vector3 position)
	{
		Scopes[(int) colorType].transform.position = Camera.main.WorldToScreenPoint(position);
	}

	/**
	 * スクリーンシュート
	 */
	private void OnScreenShot(Player.ColorType colorType, Vector3 position)
	{
		var index = (int) colorType;
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.PlayOneShot(SE_Shot);

        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(position));
        //Ray ray = new Ray(position, Camera.main.transform.forward);
		RaycastHit hit = new RaycastHit();
            
		if (Physics.Raycast(ray, out hit, 20f, InkedRayrMask))
		{
			InkSplashShaderBehavior script = hit.collider.gameObject.GetComponent<InkSplashShaderBehavior>();
			if (null != script && canCount){
				
				script.PaintOn(hit.textureCoord, SplashImages[index]);
				string hitTag = hit.collider.tag;
				
				gameDirector.AddScore(colorType);
				
				int score = gameDirector.GetScore(colorType);
				Scores[index].text = String.Format("{0}", score);
			}
		}
		
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
