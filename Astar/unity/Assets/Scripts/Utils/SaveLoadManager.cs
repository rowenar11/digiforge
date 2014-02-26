using UnityEngine;
using System.Collections;
using System.IO;

public static class SaveLoadManager
{
	public static void SaveFile(string path, string data)
	{
		WriteFile(path, data, false);
	}

	public static string[] LoadFile(string path)
	{
		//Debug.Log("*** path="+path);
		if(!FileExists(path)) return new string[0];
		string[] lines = File.ReadAllLines(path);
		return lines;
	}

	private static void WriteFile(string path, string data, bool append)
	{
		string dirPath = GetDirectoryPath(path);
		if(!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

		StreamWriter file = new StreamWriter(path, append);
		file.Write(data);
		file.Close();
	}

	public static bool FileExists(string path)
	{
		return File.Exists(path);
	}

	// Ex: C:\Users\Public\Test\blah.txt would return C:\Users\Public\Test\
	public static string GetDirectoryPath(string filePath)
	{
		if(string.IsNullOrEmpty(filePath)) return "";

		FileInfo fileInfo = new FileInfo(filePath);
		return (fileInfo != null) ? fileInfo.Directory.FullName : "";
	}

	public static string GetFileName(string filePath)
	{
		if(string.IsNullOrEmpty(filePath)) return "";
		
		FileInfo fileInfo = new FileInfo(filePath);
		return (fileInfo != null) ? fileInfo.Name : "";
	}
}
