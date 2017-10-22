using System;
using System.Collections;
using System.Collections.Generic;

public class Player {
	public enum ColorType { Yello, Green, Pink }

	public Player(ColorType color)
	{
		this.Color = color;
	}

	public ColorType Color;
	public string TrackerID { get; set; }
	public string Name { get; set; }
	public int Score { get; set; }
}
