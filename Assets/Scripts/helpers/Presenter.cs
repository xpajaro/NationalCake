using UnityEngine;
using System.Collections;

public class Presenter {

	public static void Attach (GameObject actor, SpriteRenderer renderer){
		ReturnToRig (renderer);
		StartCollisions (actor);
	}


	public static void Detach (GameObject actor, SpriteRenderer renderer){
		StopCollisions (actor);
		RemoveFromRig (renderer);
	}


	static void ReturnToRig (SpriteRenderer renderer){
		renderer.sortingLayerName = Constants.SORTING_LAYER_RIG_TOP;
	}


	static void RemoveFromRig (SpriteRenderer renderer){
		renderer.sortingLayerName = Constants.SORTING_LAYER_WATER_TOP;
	}


	public static void StopCollisions (GameObject actor){
		if (actor.name.Equals ("player")) {
			actor.layer = Constants.COLLISION_FREE_LAYER_ACTOR;
		
		} else if (actor.name.Equals ("enemy")) {
			actor.layer = Constants.COLLISION_FREE_LAYER_ENEMY;
		
		} else if (actor.name.Equals ("cake") || actor.name.Equals ("cakeEffigy")) {
			actor.layer = Constants.COLLISION_FREE_LAYER_CAKE;
		
		} else if (actor.name.Contains ("wine")) {
			actor.layer = Constants.COLLISION_FREE_LAYER_OTHERS;
		
		}
	}


	public static void StartCollisions (GameObject actor){

		if (actor.name.Equals ("cake") || actor.name.Equals ("cakeEffigy")) {
			actor.layer = Constants.COLLISION_CAKE;
		} else {
			actor.layer = Constants.GAME_LAYER;
		}
	}
}
