using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Screen0Director : MonoBehaviour
{

	public AudioClip BGM;
	public AudioClip SE_CLICK;

	private IEnumerator StartBGM()
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.PlayOneShot(BGM);
		yield break;
	}

// Use this for initialization
	void Start () {

		//ピンク イエロー グリーン
		StartCoroutine(StartBGM());
	}

	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			OnSelct(Input.mousePosition);
		}
	}

	public IEnumerator OnClickButton()
	{
		AudioSource audioSource = GetComponent<AudioSource>();
//		audioSource.Stop();
		audioSource.PlayOneShot(SE_CLICK);
		yield return new WaitForSeconds(0.5f);
		
		SceneManager.LoadScene("Scene2");
	}

	public void OnSelct(Vector3 point)
	{	
		Debug.Log(point);
		StartCoroutine(OnClickButton());
	}

	private void test()
	{
		// メインカメラからクリックしたポジションに向かってRayを撃つ。
		Ray ray = Camera.main.ScreenPointToRay(new Vector3());
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
