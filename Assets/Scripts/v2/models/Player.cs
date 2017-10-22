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

	public void SavePlayer(){
		LocalStorage.Instance.Save (this);
	}

	public Player LoadPlayer(){
		return LocalStorage.Instance.Load ();
	}

	public int CalculateScore(int opponentWincount){

		float ratio = Math.Max ((float)WinCount / opponentWincount, 1);
		float newScore = Math.Max(1 / ratio, 5) * 2;

		return (int)newScore;
	}
}
