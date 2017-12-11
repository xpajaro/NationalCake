using System;

public class Room
{
	public int ID {get; set;}
	public string Name {get; set;}
	public string Description {get; set;}
	public int Budget {get; set;}
	public int Recovery {get; set;}

	// public static Room demoRoom = new Room(0, "practice grounds", "learning the ropes", 0, 0);
	public static Room beginnerRoom = new Room(0, "local liasons", "developing grassroots infrastructure", 1, 1);
	public static Room intermediateRoom = new Room(1, "the wire", "funding small business enterprises", 5, 6);
	public static Room proRoom = new Room(2, "power play", "ramping up commodity production", 10, 15);

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