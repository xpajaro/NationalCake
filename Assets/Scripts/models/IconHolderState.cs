using System;
using UnityEngine;

public class IconHolderState {
	public IconHolderState (GameObject _holder)
	{
		holder = _holder;
	}

	public IconHolderState (GameObject _holder, GameObject _icon)
	{
		holder = _holder;
		icon = _icon;
	}

	public GameObject holder{
		get; set;
	}

	public GameObject icon{
		get; set;
	}

}


