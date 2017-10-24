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
	
	// クリック音
	public AudioClip SE_CLICK;

	// スコープ
	public RawImage[] Scopes;

	// 受付
	public Text Timer;
	private bool isStartCountDown = false;
	private float startCount = 10f;
	
	private GameDirector gameDirector;
	
	private IEnumerator StartBGM()
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.PlayOneShot(BGM);
		yield break;
	}

// Use this for initialization
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

		if (isStartCountDown && startCount >= 0f)
		{
			if (startCount > 0f)
			{
				startCount -= Time.deltaTime;
				if (startCount < 1f)
				{
					startCount = 0f;
					gameDirector.Action(GameActionEvent.EventType.TitleSceneEnd);
					isStartCountDown = false;
				}
				Timer.text = String.Format("{0}", (int)(startCount/60));
			}
		}
		
		// デバッグ用 マウス操作
		gameDirector.HoverScreen(Player.ColorType.Pink, Input.mousePosition);
		if (Input.GetMouseButtonDown(0))
		{
			gameDirector.ShotScreen(Player.ColorType.Pink);
		}
		
	}

	
	public IEnumerator OnClickButton()
	{
		yield return new WaitForSeconds(0.5f);
	}

	/**
	 * スクリーンのHover情報
	 */
	private void OnScreenPosition(Player.ColorType colorType, Vector3 point)
	{
		Scopes[(int) colorType].transform.position = point;
	}
	
	// 
	private void OnScreenShot(Player.ColorType colorType)
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.PlayOneShot(SE_CLICK);

		gameDirector.PlayerEntry(colorType);
		
		if (isStartCountDown == false)
		{
			isStartCountDown = true;
			StartCoroutine(OnClickButton());
		}
		
	}

	private void test()
	{
		// メインカメラからクリックしたポジションに向かってRayを撃つ。
		Ray ray = Camera.main.ScreenPointToRay(new Vector3());
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
				
		// シーンビューにRayを可視化するデバッグ（必要がなければ消してOK）
		Debug.DrawRay(ray.origin, ray.direction * 30.0f, Color.red, 100.0f);
			
		if (Physics.Raycast(ray, out hit, 30.0f))
		{
			GameObject selectedGameObject = hit.collider.gameObject;
			string hitTag = hit.collider.tag;
			Debug.Log(hitTag);

//				TapBehaviour target = selectedGameObject.GetComponent(typeof(TapBehaviour)) as TapBehaviour;
//				if (target != null)
//				{
//					target.TapDown(ref hit);
//				}
		}
	}
}
