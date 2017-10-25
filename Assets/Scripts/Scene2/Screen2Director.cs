using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Screen2Director : MonoBehaviour {

	public Text startText;
	public Text nextText;
	
	public Camera camera;
	public Light MainLight;
	
	public AudioClip BGM;
	
	public AudioClip SE_BossFindAll;
	public AudioClip SE_BossFindOne;

	public Light[] SpotLights; 
	
	private GameDirector gameDirector;

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
		startText.gameObject.SetActive(true);
		yield return new WaitForSeconds(5.0f);
		startText.gameObject.SetActive(false);
	}
	
	private IEnumerator ShowFindMessage() {
		
		if(startText.gameObject.activeSelf) startText.gameObject.SetActive(false);
		
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.Stop();
		yield return new WaitForSeconds(0.5f);

		audioSource.PlayOneShot(SE_BossFindAll);
		
		yield return new WaitForSeconds(2.0f);
		nextText.gameObject.SetActive(true);
		
		yield return new WaitForSeconds(2f);
		MainLight.gameObject.SetActive(true);
		
		yield return new WaitForSeconds(2f);
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
		Ray ray = camera.ScreenPointToRay(point);
		RaycastHit hit = new RaycastHit();
		
		if (Physics.Raycast(ray, out hit, 1000f))
		{
			GameObject selectedGameObject = hit.collider.gameObject;
			string hitTag = hit.collider.tag;
			
			light.transform.LookAt(selectedGameObject.transform);
			
		}
		
	}
	
	// 
	private void OnFindBoss()
	{
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
		if (Input.GetMouseButtonDown(0))
		{
			OnFindBoss();
		}

	}
}
