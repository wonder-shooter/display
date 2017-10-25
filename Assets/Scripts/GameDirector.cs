using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	
	// イベント
	private GameActionEvent gameActionEvent = new GameActionEvent();
	private ScreenPositionEvent screenPositionEvent = new ScreenPositionEvent();
	private TrackerTriggerEvent	trackerTriggerEvent = new TrackerTriggerEvent();
	// コンフィグ管理
	private GameConfigHandler gameConfigHandler = new GameConfigHandler();
	private GameConfig config;

	// プレイヤー
	private Player[] players;
	// シュータモード回数
	private int gameCount = 0;
	// 点数の単位
	private readonly int pointUnit = 10;
	/**
	 * コンストラクタ
	 */
	private GameDirector()
	{
		config = gameConfigHandler.Load();

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
				SceneManager.LoadScene(++gameCount >= config.Count ? "Scene4" : "Scene2");  
				break;
			}
			case GameActionEvent.EventType.GameEnd:
			{
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

	// ゲーム遷移管理アクション
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
	 * スコア加算
	 */
	public void AddScore(Player.ColorType colorType)
	{
		Player player = players.Where(p => p.Color == colorType).SingleOrDefault();
		player.Score += pointUnit;
	}
	
	/**
	 * スコア取得
	 */
	public int GetScore(Player.ColorType colorType)
	{
		Player player = players.Where(p => p.Color == colorType).SingleOrDefault();
		return player.Score;
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
		
		foreach (var player in this.players) player.CreateName(this.config.Number);

	}
	
	// アクティブプレイヤーの数を返す
	public int GetActivePlayerCount()
	{
		if (players == null) return 0;
		return players.Where(player => player.IsEntry).Count();		
	}
	
	/**
	 * 
	 */
	public void GameEnd()
	{
		config.Number++;
		gameConfigHandler.Save(config);
	}
	
	// デバッグ用
	private void OnListenScreenPosition(Player.ColorType colorType, Vector3 positon)
	{
		Debug.Log(positon);
	}
}
