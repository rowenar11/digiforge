using System.Collections.Generic;
using System;
using System.Linq;

public class RandomUtil
{
	public static IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
	{
		Random rand = new Random();
		List<TValue> values = Enumerable.ToList(dict.Values);
		int size = dict.Count;
		while(true)
		{
			yield return values[rand.Next(size)];
		}
	}
}