using System;
using System.Collections;
using System.Collections.Generic;

public class Player {
	public enum ColorType { Pink, Green, Purple }

	public Player(ColorType color, int index)
	{
		this.Color = color;
		this.Name = String.Format("{0}#{1}", color.ToString(), index);
		this.IsEntry = false;
	}

	public ColorType Color;
	public string TrackerID { get; set; }
	public bool IsEntry { get; set; }
	public string Name { get; set; }
	public int Score { get; set; }
}
