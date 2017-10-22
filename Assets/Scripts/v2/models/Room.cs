using System;

public class Room
{
	public int ID {get; set;}
	public string Name {get; set;}
	public string Description {get; set;}
	public string Budget {get; set;}
	public string Recovery {get; set;}

	private static Room beginnerRoom = new Room(0, "codename small fish", "investigate the local corridor", "$1 billion", "$2 billion");
	private static Room intermediateRoom = new Room(1, "codename private pirate", "audit the V.I. naira highway", "$5 billion", "$12 billion");
	private static Room proRoom = new Room(2, "codename cabo cabana", "recover stolen swiss accounts", "$10 billion", "$25 billion");

	private static Room[] rooms = {beginnerRoom, intermediateRoom, proRoom};

	public Room () { }
	
	public Room (int id, string name,  string description, string budget, string recovery) {
		ID = id;
		Name = name;
		Description = description;
		Budget = budget;
		Recovery = recovery;
	}

	public static Room[] GetAllRooms(){
		return rooms;
	}
}