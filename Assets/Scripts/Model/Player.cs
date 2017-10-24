using System;
using System.Collections;
using System.Collections.Generic;

public class Player {
	public enum ColorType { Pink, Green, Purple }

	public Player(ColorType color)
	{
		this.Color = color;
		this.IsEntry = false;
	}

	public ColorType Color;
	public string TrackerID { get; set; }
	public bool IsEntry { get; set; }
	public string Name { get; private set; }
	public int Score { get; set; }

	public void CreateName(int index) {
		this.Name = String.Format("{0}#{1}", this.Color.ToString(), index);
	}
}
