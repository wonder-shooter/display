using System;
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
	private ScreenTriggerEvent	screenTriggerEvent = new ScreenTriggerEvent();
    private TrackerViverationEvent trackerViverationEvent = new TrackerViverationEvent();
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
	public void HoverScreen(Player.ColorType colorType, Vector3 position)
	{
		screenPositionEvent.Invoke(colorType, position);
	}
    /*
     * TrackedControllerHover
     */
    public void HoverTracker(Tracker tracker)
    {
        screenPositionEvent.Invoke(this.ConvertColorType(tracker.Type), tracker.Position);
    }

    /**
	* スクリーンショット
	*/
    public void AddListenerScreenShot(UnityAction<Player.ColorType, Vector3> medhod)
	{
        screenTriggerEvent.AddListener(medhod);
	}
	public void ShotScreen(Player.ColorType colorType, Vector3 position)
	{
        screenTriggerEvent.Invoke(colorType, position);
	}
    /*
     * TrackedControllerトリガーショット
     */
    public void ShotTracker(Tracker tracker)
    {
        screenTriggerEvent.Invoke(this.ConvertColorType(tracker.Type), tracker.Position);

    }

    /**
     * tracker vive 
     */
    public void TriggerViveTracker(Player.ColorType colorType)
    {
        trackerViverationEvent.Invoke(ConvertDeviceType(colorType));

    }

    private Player.ColorType ConvertColorType(Tracker.DeviceType deviceType)
    {
        switch (deviceType)
        {
            case (Tracker.DeviceType.GreenTracker):
                return Player.ColorType.Green;
            //case (Tracker.DeviceType.PurpleTracker):
            //case (Tracker.DeviceType.LeftController):
            case (Tracker.DeviceType.RightController):
                return Player.ColorType.Purple;
            case (Tracker.DeviceType.PinkTracker):
            default:
                return Player.ColorType.Pink;
        }
    }

    private Tracker.DeviceType ConvertDeviceType(Player.ColorType colorType)
    {
        switch (colorType)
        {
            case (Player.ColorType.Pink):
                return Tracker.DeviceType.PinkTracker;
            //case (Tracker.DeviceType.PurpleTracker):
            //case (Tracker.DeviceType.LeftController):
            case (Player.ColorType.Green):
                return Tracker.DeviceType.GreenTracker;
            case (Player.ColorType.Purple):
            default:
                return Tracker.DeviceType.PurpleTracker;
        }
    }


    /**
	 * スクリーンホバー
	 */
    public void AddListenerViveration(UnityAction<Tracker.DeviceType> medhod)
    {
        trackerViverationEvent.AddListener(medhod);
    }

    /**
	 * エントリー処理
	 */
    public void PlayerEntry(Player.ColorType colorType)
	{
        var player = this.GetPlayer(colorType);
        player.IsEntry = true;
	}
	
	/**
	 * スコア加算
	 */
	public void AddScore(Player.ColorType colorType)
	{
		Player player = players.Where(p => p.Color == colorType).Single();
        if(player != null)
		    player.Score += pointUnit;
	}

    /**
	 * スコア取得
	 */
    public int GetScore(Player.ColorType colorType)
	{
		Player player = players.Where(p => p.Color == colorType).Single();
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
	
	/**
	 * 
	 */
	public Player GetPlayer(Player.ColorType type)
	{
		if (players == null)
		{
			players = new Player[0];
		}
		return players.Where(player => player.Color == type).FirstOrDefault();
	}
	
	// アクティブプレイヤーを返す
	public IEnumerable<Player> GetActivePlayer()
	{
		if (players == null) return new List<Player>();
		return players.Where(player => player.IsEntry);
	}
	
	// アクティブプレイヤーの数を返す
	public int GetActivePlayerCount()
	{
		if (players == null) return 0;
		return GetActivePlayer().Count();		
	}

	public int GetNumberIndex()
	{
		return this.config.Number;
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
