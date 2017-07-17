using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deserialization {
	public static string ITEM_KEY = "item";
	public static string POSITION_KEY = "pos";
	public static string HITCOUNT_KEY = "hitCount";

	public static Vector2 GetImpulse (byte[] dataFields){
		Vector2 impulse = new Vector2 ();
		//Debug.Log ("Get Impulse ");
		impulse.x = System.BitConverter.ToSingle(dataFields, 2);
		impulse.y = System.BitConverter.ToSingle(dataFields, 6);

		//Debug.Log ("Get Impulse done");
		return impulse;
	}

	public static Dictionary<string, object> GetItemDrop (byte[] dataFields){
		//Debug.Log ("Get item drop ");
		Dictionary<string, object> itemDrop = new Dictionary<string, object>();

		int item = System.BitConverter.ToInt32(dataFields, 2);
		itemDrop.Add (ITEM_KEY, item);

		Vector2 pos = new Vector2 ();
		pos.x = System.BitConverter.ToSingle(dataFields, 6);
		pos.y = System.BitConverter.ToSingle(dataFields, 10);
		itemDrop.Add (POSITION_KEY, pos);

		//Debug.Log ("Get item drop done");
		return itemDrop;
	}

	public static Dictionary<string, object> GetItemUse (byte[] dataFields){
		//Debug.Log ("Get item use ");
		Dictionary<string, object> itemUsed = new Dictionary<string, object>();

		int item = System.BitConverter.ToInt32(dataFields, 2);
		itemUsed.Add (ITEM_KEY, item);

		Vector2 pos = new Vector2 ();
		pos.x = System.BitConverter.ToSingle(dataFields, 6);
		pos.y = System.BitConverter.ToSingle(dataFields, 10);
		itemUsed.Add (POSITION_KEY, pos);

		//Debug.Log ("Get item use done");
		return itemUsed;
	}


	public static void UpdateGameState (byte[] dataFields){
		//Debug.Log ("Update game state ");
		GameState.gameEnded = System.BitConverter.ToBoolean (dataFields, 2);
		GameState.gameWon = !System.BitConverter.ToBoolean (dataFields, 3);

		//Debug.Log ("Update Game state");
	}


	public static Dictionary<string, object> GetBarrelHit (byte[] dataFields){
		//Debug.Log ("Get barrel hit ");
		Dictionary<string, object> itemUsed = new Dictionary<string, object>();

		Vector2 pos = new Vector2 ();
		pos.x = System.BitConverter.ToSingle(dataFields, 2);
		pos.y = System.BitConverter.ToSingle(dataFields, 6);
		itemUsed.Add (POSITION_KEY, pos);

		int item = System.BitConverter.ToInt32(dataFields, 10);
		itemUsed.Add (HITCOUNT_KEY, item);

		//Debug.Log ("Get item use done");
		return itemUsed;
	}


	public static ActorState GetActorState (byte[] dataFields){
		ActorState state = new ActorState ();
		//Debug.Log ("get state");
		Vector2 val = new Vector2 ();

		state.stateNumber = System.BitConverter.ToInt32(dataFields, 2);

		val.x = System.BitConverter.ToSingle(dataFields, 6);
		val.y = System.BitConverter.ToSingle(dataFields, 10);
		state.playerPosition = new Vector2(val.x, val.y); 

		val.x = System.BitConverter.ToSingle(dataFields, 14);
		val.y = System.BitConverter.ToSingle(dataFields, 18);
		state.playerVelocity = new Vector2(val.x, val.y);

		val.x = System.BitConverter.ToSingle(dataFields, 22);
		val.y = System.BitConverter.ToSingle(dataFields, 26);
		state.enemyPosition = new Vector2(val.x, val.y); 

		val.x = System.BitConverter.ToSingle(dataFields, 30);
		val.y = System.BitConverter.ToSingle(dataFields, 34);
		state.enemyVelocity = new Vector2(val.x, val.y);

		val.x = System.BitConverter.ToSingle(dataFields, 38);
		val.y = System.BitConverter.ToSingle(dataFields, 42);
		state.cakePosition = new Vector2(val.x, val.y);

		state.playerFalling = System.BitConverter.ToBoolean(dataFields, 46);
		state.enemyFalling = System.BitConverter.ToBoolean(dataFields, 47);
		state.cakeFalling = System.BitConverter.ToBoolean(dataFields, 48);

//		Debug.Log ("player deets " + state.PlayerPosition.ToString("G4") + " / " +  state.PlayerVelocity.ToString("G4") + " \n " +
//			"enemy deets " + state.EnemyPosition.ToString("G4") + " / " +  state.EnemyVelocity.ToString("G4") + " \n " +
//			"cake deets " + state.CakePosition.ToString("G4") +" \n " +
//			"falling deets " + state.PlayerFalling + " / " + state.EnemyFalling + " / " + state.CakeFalling);
//
		//Debug.Log ("get state done");
		return state;
	}

	public static char GetGamestamp(byte[] data){
		//Debug.Log ("get gamestamp done");
		return (char) data[3];;
	}


	public static char GetMessageType(byte[] data){
		//Debug.Log ("get message type done");
		return (char) data[1];;
	}




}
