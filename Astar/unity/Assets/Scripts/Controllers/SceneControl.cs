using UnityEngine;
using System.Collections;

public class SceneControl : MonoSingleton<SceneControl>
{
	public string currentScene;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void LoadWelcome()
	{
		LoadScene("Welcome");
	}

	public void LoadPayAttention()
	{
		LoadScene("PayAttention");
	}

	public void LoadPathFinding()
	{
		LoadScene("PathFindingScene1");
	}

	public void LoadRayCasting()
	{
		LoadScene("RayTrace");
	}

	public void LoadQuiz()
	{
		LoadScene("Quiz");
	}

	private void LoadScene(string sceneName)
	{
		currentScene = sceneName;
		Application.LoadLevelAsync(sceneName);
	}
}
