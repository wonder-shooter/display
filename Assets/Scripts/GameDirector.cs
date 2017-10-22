using System.Collections;
using System.Collections.Generic;

public class GameDirector {

	private static GameDirector gameDirector;
	
	public static GameDirector GetSheredInstance () {
		if (gameDirector == null) {
			gameDirector = new GameDirector();
		}
		return gameDirector;
	}
	
	public int Number { get; set; }
//	private GameData GameData = new GameData(); 
	
	public void Start() {

	}

	public void NextSearch() {

	}

	public void Complate() {

	}
}
