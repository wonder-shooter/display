using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Screen2Director : MonoBehaviour {

	// 開始
	public Text startText;
	// 発見次へ
	public Text nextText;
	
	//メインライト
	public Light MainLight;
	
	// BGM
	public AudioClip BGM;
	
	// 発見音
	public AudioClip SE_BossFindAll;
	public AudioClip SE_BossFindOne;

	// スポットライト
	public Light[] SpotLights; 
	
	// ゲームハンドラー
	private GameDirector gameDirector;

	private bool isFindBoss;

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
		if(!nextText.gameObject.activeSelf){
			startText.gameObject.SetActive(true);
			yield return new WaitForSeconds(5.0f);
			startText.gameObject.SetActive(false);
		}
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
		MainLight.gameObject.SetActive(true);
		yield return new WaitForSeconds(2f);
		// 遷移
		gameDirector.Action(GameActionEvent.EventType.SearchModeSceneEnd);
		
		yield break;
	}
	
	/**
	 * スクリーンのHover情報
	 */
	private void OnScreenPosition(Player.ColorType colorType, Vector3 point)
	{
		Light light = SpotLights[(int) colorType]; 

		// メインカメラから選択したポジションに向かってRayを撃つ。
		Ray ray = Camera.main.ScreenPointToRay(point);
		RaycastHit hit = new RaycastHit();
		
		if (Physics.Raycast(ray, out hit, 1000f))
		{
			GameObject selectedGameObject = hit.collider.gameObject;
			// 
			light.transform.LookAt(selectedGameObject.transform);
			
		}
		
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

		MainLight.gameObject.SetActive(false);
		
		// スタートメッセージ
		StartCoroutine(StartBGM());
		StartCoroutine(ShowStartMessage());
	}

	private void Update()
	{
		// デバッグ用 マウス操作
		gameDirector.HoverScreen(Player.ColorType.Pink, Input.mousePosition);
		if (Input.GetMouseButtonDown(0))
		{
			gameDirector.ShotScreen(Player.ColorType.Pink);
		}
		
		// デバッグ用
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OnFindBoss();
		}
		if (Input.GetMouseButtonDown(0))
		{
			OnFindBoss();
		}

	}
}
