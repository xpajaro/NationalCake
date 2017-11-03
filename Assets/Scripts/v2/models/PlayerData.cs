using System;

[Serializable]
public class PlayerData  {

	public int Revenue {get; set;}
	public string formattedRevenue{
		get { 
			return Utilities.FormatMoney (Revenue);
		}
	}
	public int WinCount {get; set;}
	public int GameCount {get; set;}
	public DateTime LastLogin { get; set;}

	public PlayerData(){ }

	public void SavePlayer(){
		LocalStorage.Instance.Save (this);
	}

	public PlayerData LoadPlayer(){
		PlayerData player =  LocalStorage.Instance.Load ();

		if (player == null) {
			player = new PlayerData ();
			LocalStorage.Instance.Save (player);
		}

		return player;
	}


}
