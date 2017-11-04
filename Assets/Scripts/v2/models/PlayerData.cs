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


}
