using System;
using UnityEngine;

public class LagMessage
{
	public DateTime timeSent{ 
		get; set; 
	}

	public int roundtripDuration{ 
		get; set; 
	}

	public LagMessage (DateTime timeSent, int roundtripDuration)
	{
		this.timeSent = timeSent;
		this.roundtripDuration = roundtripDuration;
	}

	
}