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

    public Texture2D[] SplashImages;

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

		if(SteamVR.instance != null && SteamVR.instance.hmd != null)
        	SteamVR.instance.hmd.ResetSeatedZeroPose();

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
        var mousePos = Input.mousePosition;
        mousePos.z = 0.1f;
        var point = Camera.main.ScreenToWorldPoint(mousePos);
        gameDirector.HoverScreen(Player.ColorType.Pink, point);
		if (Input.GetKeyDown(KeyCode.Space))
		{
			gameDirector.ShotScreen(Player.ColorType.Pink, point);
		}

        if (Input.GetMouseButtonDown(0))
        {
            float distance = 100; // 飛ばす&表示するRayの長さ
            float duration = 3;   // 表示期間（秒）

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, duration, false);

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, distance))
            {
                GameObject hitObject = hit.collider.gameObject;
                // （以下略）
            }
        }
    }
	
	/**
	 * スクリーンのHover情報
	 */
	private void OnScreenPosition(Player.ColorType colorType, Vector3 position)
	{
        Scopes[(int) colorType].transform.position = Camera.main.WorldToScreenPoint(position);
	}
	
	/**
	 * シュート
	 */
	private void OnScreenShot(Player.ColorType colorType, Vector3 position)
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

        var index = (int)colorType;

        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(position));
        //Ray ray = new Ray(position, Camera.main.transform.forward);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            InkSplashShaderBehavior script = hit.collider.gameObject.GetComponent<InkSplashShaderBehavior>();
            if (null != script)
            {

                script.PaintOn(hit.textureCoord, SplashImages[index]);
                
            }
        }

    }

}
