using System;

public class Room
{
	public int ID {get; set;}
	public string Name {get; set;}
	public string Description {get; set;}
	public int Budget {get; set;}
	public int Recovery {get; set;}

	private static Room beginnerRoom = new Room(0, "codename small fish", "investigate the local corridor", 1, 2);
	private static Room intermediateRoom = new Room(1, "codename private pirate", "audit the V.I. naira highway", 5, 12);
	private static Room proRoom = new Room(2, "codename cabo cabana", "recover stolen swiss accounts", 10, 25);

	private static Room[] rooms = {beginnerRoom, intermediateRoom, proRoom};

	public Room () { }

	public Room (int id, string name,  string description, int budget, int recovery) {
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