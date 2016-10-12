using System;
using UnityEngine;

public class ActorState
{
	public ActorState ()
	{
	}

	public int StateNumber{
		get; set;
	}

	public Vector3 PlayerPosition{
		get; set;
	}

	public Vector3 PlayerVelocity{
		get; set;
	}

	public Vector3 EnemyPosition{
		get; set;
	}

	public Vector3 EnemyVelocity{
		get; set;
	}

	public Vector3 CakePosition{
		get; set;
	}
}


