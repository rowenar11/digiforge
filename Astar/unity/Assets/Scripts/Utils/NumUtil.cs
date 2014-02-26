using System;

public class NumUtil
{
	public static string ToHex(long number)
	{
		return number.ToString("X");
	}

	public static long FromHex(string hexNumber)
	{
		return Convert.ToInt64(hexNumber, 16);
	}
}
