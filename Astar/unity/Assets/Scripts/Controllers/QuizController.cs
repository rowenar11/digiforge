using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuizController : MonoBehaviour
{
	private bool _init = false;

	void Start()
	{
		init();
	}

	private void init()
	{
		if (!_init)
		{
			_init = true;

			Debug.LogError("OMG LETS GET THE QUIZ : "+NetworkControl.instance.currentSubState);
			NetworkControl.instance.getActiveQuiz(NetworkControl.instance.currentSubState, gotQuiz);
		}
		else
		{
			Debug.LogWarning("NOPE");
		}
	}


	private void gotQuiz(object dataObj)
	{
		Debug.LogWarning("WOOOOticles I got a fucking quiz");
		Dictionary<string, object> loginReturn = (Dictionary<string, object>)dataObj;

		if (loginReturn.ContainsKey("success") && loginReturn.ContainsKey("data"))
		{
			Dictionary<string, object> data = loginReturn["data"] as Dictionary<string, object>;

			foreach(KeyValuePair<string,object> quizValues in data["quiz"] as Dictionary<string,object>)
			{
				Debug.Log("QUIZ["+quizValues.Key+"]="+quizValues.Value);
				if (quizValues.Key == "questions")
				{
					foreach (KeyValuePair<string, object> question in quizValues.Value as Dictionary<string, object>)
					{
						Debug.Log("QUESTION[" + question.Key + "]=" + question.Value);
						if (quizValues.Key == "answers")
						{
							foreach (KeyValuePair<string, object> answer in quizValues.Value as Dictionary<string, object>)
							{
								Debug.Log("Answer[" + answer.Key + "]=" + answer.Value);
							}
						}
					}
				}
			}
		}
		else
		{
			Debug.LogError("SAD A");
		}
	}
}
