using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameDirector {

	private static GameDirector gameDirector;
	
	public static GameDirector GetSheredInstance () {
		if (gameDirector == null) {
			gameDirector = new GameDirector();
		}
		return gameDirector;
	}
	
	//
	private GameActionEvent gameActionEvent = new GameActionEvent();
	private ScreenPositionEvent screenPositionEvent = new ScreenPositionEvent();
	private TrackerTriggerEvent	trackerTriggerEvent = new TrackerTriggerEvent();
	
	private int MaxGameCount = 3;

	private int Number { get; set; }
	private int GameCount { get; set; }

	private Player[] players;
	
	private GameDirector()
	{
		// デバッグ用
//		this.AddListenerScreenPositon(this.OnListenScreenPosition);
	}

	/**
	 * ゲームシーンイベントアクション
	 */
	public void Action(GameActionEvent.EventType eventType)
	{
		switch (eventType)
		{
			case GameActionEvent.EventType.TitleSceneEnd:
			{
				SceneManager.LoadScene("Scene2");  
				break;
			}
			case GameActionEvent.EventType.SearchModeSceneEnd:
			{
				SceneManager.LoadScene("Scene3");  
				break;
			}
			case GameActionEvent.EventType.ChaserModeSceneEnd:
			{
				SceneManager.LoadScene(++GameCount > MaxGameCount ? "Scene4" : "Scene2");  
				break;
			}
			case GameActionEvent.EventType.GameEnd:
			{
				GameEnd();
				SceneManager.LoadScene("Scene0");  
				break;
			}
			default:
			{
				gameActionEvent.Invoke(eventType);
				break;
			}
		}
	}

	// 
	public void AddListenerGameAction(UnityAction<GameActionEvent.EventType> medhod)
	{
		gameActionEvent.AddListener(medhod);
	}

	/**
	 * スクリーンホバー
	 */
	public void AddListenerScreenPositon(UnityAction<Player.ColorType, Vector3> medhod)
	{
		screenPositionEvent.AddListener(medhod);
	}
	public void HoverScreen(Player.ColorType colorType, Vector3 point)
	{
		screenPositionEvent.Invoke(colorType, point);
	}
	
	/**
	* スクリーンショット
	*/
	public void AddListenerScreenShot(UnityAction<Player.ColorType> medhod)
	{
		trackerTriggerEvent.AddListener(medhod);
	}
	public void ShotScreen(Player.ColorType colorType)
	{
		trackerTriggerEvent.Invoke(colorType);
	}

	/**
	 * エントリー処理
	 */
	public void PlayerEntry(Player.ColorType colorType)
	{
		foreach (var player in this.players)
		{
			if (player.Color != colorType) return;
			
			player.IsEntry = true;
		}
	}
	
	/**
	 * 
	 */
	public void GameReset()
	{
		// プレイヤー作成
		this.players = new Player[]
		{
			new Player(Player.ColorType.Pink), 
			new Player(Player.ColorType.Green),
			new Player(Player.ColorType.Purple),
		};
		
		foreach (var player in this.players) player.CreateName(this.GameCount);

	}
	/**
	 * 
	 */
	private void GameEnd()
	{
		Number++;
	}
	
	// デバッグ用
	private void OnListenScreenPosition(Player.ColorType colorType, Vector3 positon)
	{
		Debug.Log(positon);
	}
}
