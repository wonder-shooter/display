using System.Collections;
using System.Collections.Generic;

public class GameDirector {

	static private GameDirector gameDirector;

	static public GameDirector GetSheredInstance () {
		if(gameDirector == null) gameDirector = new GameDirector();
		return gameDirector;
	}
	
	public void Start() {

	}

	public void NextSearch() {

	}

	public void Complate() {

	}
}
