using System.Collections;
using System.Collections.Generic;

public class TrackerData {
	public enum Color { Yello, Green, Pink };
	public string ID {
		get; set;
	}
	public float X {
		get; set;
	}
	public float Y {
		get; set;
	}
	public float Z {
		get; set;
	}
	public float Yaw {
		get; set;
	}
	public float Pitch {
		get; set;
	}
	public float Roll {
		get; set;
	}
	public int Degree {
		get; set;
	}
}
