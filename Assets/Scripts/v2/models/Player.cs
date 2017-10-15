using System;

[Serializable]
public class Player  {

	public string Id {get; set;}
	public string Name {get; set;}
	public Score currentScore { get; set;}
	public Score highScore { get; set;}
	public int WinCount {get; set;}
	public int GameCount {get; set;}
	public DateTime LastLogin { get; set;}

	public Player(){
		currentScore = new Score ();
		highScore = new Score ();
	}

}
