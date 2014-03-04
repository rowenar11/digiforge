using System;
using UnityEngine;

public class EpochUtils
{
	public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	public static double getNowUTCSeconds()
	{
		return Math.Round((DateTime.UtcNow - UnixEpoch).TotalSeconds);
	}

	public static string secondsToString(double seconds)
	{
		TimeSpan t = TimeSpan.FromSeconds(seconds);
		return string.Format("{0:D2}h:{1:D2}m:{2:D2}s:", t.Hours, t.Minutes, t.Seconds);
	}

	public static string secondsToDateString(double seconds)
	{
		DateTime date = UnixEpoch;
		date = date.AddSeconds(seconds).ToLocalTime();
		
		//Debug.Log("ts="+seconds+"    Year="+date.Year+" Month="+date.Month+" Day="+date.Day+" Hour="+date.Hour+" Min="+date.Minute+" Sec="+date.Second);
		return zeroBased(date.Month) + "/" + zeroBased(date.Day) + "/" + twoDigitYear(zeroBased(date.Year)) + " " + zeroBased(date.Hour) + ":" + zeroBased(date.Minute) + ":" + zeroBased(date.Second);
	}

	private static string zeroBased(int num)
	{
		return (num <= 9) ? "0" + num : num.ToString();
	}

	private static string twoDigitYear(string year)
	{
		return year.Substring(2);
	}

	public static string timeSpanToFormattedDateString(TimeSpan timeSpan)
	{
		if(timeSpan.Days > 0)
		{
			return "" + timeSpan.Days + "d " + timeSpan.Hours + "h " + timeSpan.Minutes + "m";
		}

		if(timeSpan.Hours > 0)
		{
			return timeSpan.Hours + "h " + timeSpan.Minutes + "m";
		}

		if(timeSpan.Minutes > 0)
		{
			return timeSpan.Minutes + "m " + timeSpan.Seconds + "s";
		}

		return Mathf.Ceil(timeSpan.Seconds) + "s";
	}
}