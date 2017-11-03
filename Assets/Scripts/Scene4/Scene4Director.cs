using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class Scene4Director : MonoBehaviour
{

	public AudioClip BGM;
	public AudioClip EndSE;
	
	// アクセサ
	public GameObject Areas;
	public Text[] Names;
	public Text[] Scores;
	
	private GameDirector gameDirector;

	private IEnumerator PlayBGM()
	{
		yield return new WaitForSeconds(1f);
		// n, m-1まで
		AudioSource audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.PlayOneShot(EndSE);
		
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(ShowRank());

		yield return new WaitForSeconds(2f);
		audioSource.loop = true;
		audioSource.clip = BGM;
		audioSource.Play();
		
		yield break;
	}

	private IEnumerator ShowRank()
	{
		// 表示
		Areas.gameObject.SetActive(true);

		// 名前更新
		foreach (var name in Names)
		{
			name.text = String.Format("#{0:D3}", gameDirector.GetNumberIndex());
		}
		
		var players = gameDirector.GetActivePlayer();
		foreach (var player in players)
		{
			int index = (int) player.Color;
			
			int score = player.Score;
			Scores[index].text = String.Format("{0:D4}", score);	
		}
		yield break;
	}

	/**
	 * 
	 */
	private IEnumerator PostScore()
	{	
		string Url = "http://wondershooter.net/score";

		Player pinkPlayer = gameDirector.GetPlayer(Player.ColorType.Pink);
		Player greenPlayer = gameDirector.GetPlayer(Player.ColorType.Green);
		Player purplePlayer = gameDirector.GetPlayer(Player.ColorType.Purple);
		
		//string JsonArrayBase = "[{\"name\":\"{0}\",\"score\":{1}},{\"name\":\"{2}\",\"score\":{3}},{\"name\":\"{4}\",\"score\":{5}}]";
		//string JsonArraystring = String.Format(JsonArrayBase, pinkPlayer.Name, pinkPlayer.Score, greenPlayer.Name, greenPlayer.Score,//
		//	purplePlayer.Name, purplePlayer.Score);
        string JsonArraystring = "[{\"name\":\"" + pinkPlayer.Name + "\",\"score\":" + pinkPlayer.Score + "},{\"name\":\"" + greenPlayer.Name + "\",\"score\":" + greenPlayer.Score + "},{\"name\":\"" + purplePlayer.Name + "\",\"score\":" + purplePlayer.Score + "}]";

        Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Content-Type", "application/json");
		byte[] body = System.Text.Encoding.UTF8.GetBytes(JsonArraystring);
		WWW result = new WWW(Url, body, headers);
		StartCoroutine("PostdataEnumerator", result);
		
		yield break;
	}

	IEnumerator PostdataEnumerator(WWW result)
	{
		yield return result;
		if (result.error != null)
		{
			Debug.Log("Data Submitted");
		}
		else
		{
			Debug.Log(result.error);
		}
	}
	
	/**
	 * Use this for initialization
	 */
	void Start () {
		// 非表示
		Areas.gameObject.SetActive(false);
		gameDirector = GameDirector.GetSheredInstance();
		
		StartCoroutine(PostScore());
		StartCoroutine(PlayBGM());
	}
	
	/**
	 * Update is called once per frame
	 */
	void Update () {

		if (Input.GetKeyDown(KeyCode.E))
		{
			gameDirector.GameEnd();
			gameDirector.Action(GameActionEvent.EventType.GameEnd);
		}
	}
}
