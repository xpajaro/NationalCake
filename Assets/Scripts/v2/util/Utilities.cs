using System;


public class Utilities {

	private static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	public static long CurrentUnixDateTime () {
		return  UnixTimeFromDateTime(DateTime.Now) ;
	}

	public static long CurrentUnixDate () {
		return  UnixTimeFromDateTime(DateTime.Now.Date) ;
	}

	public static long ServerUnixDate () {
		return  UnixTimeFromDateTime(DateTime.UtcNow.Date) ;
	}

	public static long UnixTimeFromDateTime (DateTime dateTime) {
		return (long) (dateTime.ToUniversalTime() - UnixEpoch).TotalMilliseconds;
	}

	public static DateTime DateTimeFromUnixTimestamp(long millis) {
		return UnixEpoch.AddMilliseconds(millis);
	}

	public static string FormatMoney(int amount){
		return String.Format("$ {0} billion", amount);
	}

}


