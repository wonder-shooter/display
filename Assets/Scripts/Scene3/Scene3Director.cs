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

    // 敵ベース
    public GameObject[] TargetPrefabs;

    //
    public GameObject TargetBasePosition;

    /*  */

    // ゲームハンドラー
    private GameDirector gameDirector;
	// プレイ時間
	private float playSeconds = 181f;
	// カウントアップ判定
	private bool canCount = false;

    // 敵の数
    private readonly int instansNumber = 100;
    // 敵の場所振り幅
    private readonly float targetPosRam = 150f;
    private List<GameObject> targetInstance = new List<GameObject>();

    private readonly float scopeRange = 30f;

	/**
	 * Use this for initialization
	 */
	void Start () {

        // 
        StartCoroutine(TargetCreate());


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
		RaycastHit hit = new RaycastHit();            
		if (Physics.Raycast(ray, out hit, scopeRange, InkedRayrMask))
		{
            // 敵にヒット
			InkSplashShaderBehavior script = hit.collider.gameObject.GetComponent<InkSplashShaderBehavior>();
			if (null != script && canCount){
				
				script.PaintOn(hit.textureCoord, SplashImages[index]);
                SpScene3People hitscript = hit.collider.gameObject.GetComponentInParent<SpScene3People>();
                HitpointHandler hitpointHandler = hit.collider.gameObject.GetComponentInParent<HitpointHandler>();
                
                if (hitscript != null)
                {
                    if (hitpointHandler != null)
                    {
                        hitpointHandler.Hit();
                        if (hitpointHandler.isDie()) {
                            hitscript.DieOn();
                        }
                    }
                    else
                    {
                        hitscript.DieOn();
                    }
                }
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
    //
    private IEnumerator TargetCreate()
    {
        for(var i = 0; i < instansNumber; i++)
        {
            
            GameObject prefabs = TargetPrefabs[Random.Range(0, TargetPrefabs.Length)];
            if (TargetBasePosition != null)
            {
                float x = Random.Range(0f - targetPosRam, targetPosRam);
                float z = Random.Range(0f - targetPosRam, targetPosRam);

                var pos = new Vector3(x, 0, z);
                var tar = Instantiate(prefabs, TargetBasePosition.transform.position - new Vector3(x, 0, z), Quaternion.identity, TargetBasePosition.transform);
                targetInstance.Add(tar);
                Vector3 cameraP = Camera.main.transform.position;
                tar.transform.LookAt(new Vector3(cameraP.x, 0,  cameraP.z));
            }
            else
            {
                var tar = Instantiate(prefabs, Vector3.zero, Quaternion.identity);
            }
        }


        // camera lookat
        yield return new WaitForSeconds(5f);
        StartCoroutine(TargetLookAtCamera());

        yield break;
    }
    //
    private IEnumerator ShowStartMessage()
	{
		yield return new WaitForSeconds(1f);
		StartMessage.gameObject.SetActive(true);
		yield break;
	}


    private IEnumerator TargetLookAtCamera()
    {
        foreach (GameObject target in targetInstance) {
            Vector3 cameraP = Camera.main.transform.position;
            target.transform.LookAt(new Vector3(cameraP.x, 0, cameraP.z));
        }

        yield return new WaitForSeconds(5f);
        StartCoroutine(TargetLookAtCamera());
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
