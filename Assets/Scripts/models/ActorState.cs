using System;
using UnityEngine;

public class ActorState
{
	public ActorState ()
	{
	}

	public int stateNumber{
		get; set;
	}

	public Vector3 playerPosition{
		get; set;
	}

	public Vector3 playerVelocity{
		get; set;
	}

	public bool playerFalling{
		get; set;
	}

	public Vector3 enemyPosition{
		get; set;
	}

	public Vector3 enemyVelocity{
		get; set;
	}

	public bool enemyFalling{
		get; set;
	}

	public Vector3 cakePosition{
		get; set;
	}

	public bool cakeFalling{
		get; set;
	}
}


