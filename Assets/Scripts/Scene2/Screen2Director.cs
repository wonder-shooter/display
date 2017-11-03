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

    // TargetPrefab
    public GameObject TargetPrefab;

    public GameObject[] TargetObject;

    // ゲームハンドラー
    private GameDirector gameDirector;

    // ボス発見フラグ
	private bool isFindBoss;

    // 敵探索
    private readonly int maskPadding = 50;
	private Vector2[] targets = new Vector2[]
	{
		new Vector2(), new Vector2(), new Vector2(),   
	};
    // プレイヤーごとのターゲット発見フラグ(Activeなプレイヤーがいればfalse)
    private bool[] isTargetsFind = new bool[] { true, true, true };

    // 
    private readonly int area = 50;


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

		yield return new WaitForSeconds(0.5f);

        foreach( GameObject target in TargetObject)
        {
            if (target.activeSelf)
            {
                SpScene2 hitscript = target.GetComponentInParent<SpScene2>();
                if(hitscript != null) { 
                    hitscript.HeadUp();
                }
            }
        }

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
		int index = (int) colorType;
		if (isTargetsFind[index]) return;

        // Light移動
        SpotLights[index].transform.position = position;

        var screenPoint = Camera.main.WorldToScreenPoint(position);
        if (IsAreaTarget(colorType, screenPoint)) {
            var pos = new Vector3(position.x, position.y, position.z);
            //TargetObject[index].transform.position = pos;
            //TargetObject[index].transform.Translate(Camera.main.transform.forward * 10);
            isTargetsFind[index] = true;

            gameDirector.TriggerViveTracker(colorType);

            bool findBoss = true;
            foreach(bool find in isTargetsFind)
            {
                if (!find)
                {
                    findBoss = false;
                    break;
                }
            }
            if (findBoss) {
                this.OnFindBoss();
            }
        }
    }

    private bool IsAreaTarget( Player.ColorType color, Vector2 current)
    {
        var index = (int)color;
        var target = this.targets[index];
        return (target.x - area < current.x && current.x < target.x + area
            && target.y - area < current.y && current.y < target.y + area);

    }

    /**
	 * シュート
	 */
    private void OnScreenShot(Player.ColorType colorType, Vector3 position)
    {
        // 
        // OnFindBoss();
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
        var tes = gameDirector.GetActivePlayer();
        // イベントハンドラー設定
        gameDirector.AddListenerScreenPositon(OnScreenPosition);
        gameDirector.AddListenerScreenShot(OnScreenShot);
        
        // ボス対象位置設定
        foreach (Player.ColorType color in Enum.GetValues(typeof(Player.ColorType)))
		{
            var index = (int)color;
			
			int targetX = Random.Range(maskPadding, Screen.width - maskPadding);
			int targetY = Random.Range(maskPadding, Screen.height - maskPadding);
            var targetPoint = new Vector2(targetX, targetY);
            targets[index] = targetPoint;

            if(TargetPrefab != null) {
                // ターゲット自動生成（不動）
                //TargetObject[index] = Instantiate(TargetPrefab, Camera.main.ScreenToWorldPoint(targetPoint), Quaternion.identity);
                //TargetObject[index].transform.LookAt(Camera.main.transform.position);
                //var distanse = TargetObject[index].transform.position - Camera.main.transform.position;
                //TargetObject[index].transform.Translate(Camera.main.transform.forward * 10);
            }

            // 
            Player player = gameDirector.GetPlayer(color);
            if (player.IsEntry)
            {
                isTargetsFind[index] = false;
                if (TargetObject[index] != null) { 
                    TargetObject[index].SetActive(true);
                    TargetObject[index].transform.LookAt(Camera.main.transform.position);
                }

            }
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