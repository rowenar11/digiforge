using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class User
{
	public string name;
	public string email;
	public string totalScore;

	public User(string _name, string _email, string _totalScore)
	{
		name = _name;
		email = _email;
		totalScore = _totalScore;
	}
}

public class Quiz
{
	public List<Question> questions;

	public Quiz()
	{
		questions = new List<Question>();
	}
}

public class Question
{
	public List<Answer> answers;
	public Question()
	{
		answers = new List<Answer>();
	}

	public int id;
	public string question;
	public bool isImage;
	public int sort;
	public int value;
}

public class Answer
{
	public int id;
	public int questionId;
	public string answer;
	public int sort;
	public bool isCorrect;
}

public class QuizController : MonoBehaviour
{
	private bool _init = false;
	private int questionNumber = 0;
	private bool _quizMode = false;
	private bool _resultsMode = false;

	public Quiz quizObj;

	private Vector2 area;

	public Texture2D layoutBg;
	public Texture answerTexture;

	public int quizScore=0;
	public Dictionary<int,bool> quizAnswers;
	public Dictionary<int,string> quizAnswersAnswer;

	public Sprite quiz1_4;
	public Sprite quiz1_11;
	public Sprite quiz1_14;
	public Sprite quiz2_3;
	public Sprite quiz2_4;
	public Sprite quiz2_9;
	public Sprite quiz2_17;
	public Dictionary<int,Dictionary<int,Sprite>> imageQuestionsImages;

	private SpriteRenderer spriteRenderer;

	private bool _waitMode = false;
	private bool _forceOver = false;

	private bool showLeaderBoard = false;

	private List<User> leaderBoard; 

	void Start()
	{
		init();
	}

	private void init()
	{
		if (!_init)
		{
			imageQuestionsImages = new Dictionary<int,Dictionary<int,Sprite>>();
			imageQuestionsImages[1] = new Dictionary<int,Sprite>();
			imageQuestionsImages[1][4] = quiz1_4;
			imageQuestionsImages[1][11] = quiz1_11;
			imageQuestionsImages[1][14] = quiz1_14;
			imageQuestionsImages[2] = new Dictionary<int,Sprite>();
			imageQuestionsImages[2][3] = quiz2_3;
			imageQuestionsImages[2][4] = quiz2_4;
			imageQuestionsImages[2][9] = quiz2_9;
			imageQuestionsImages[2][17] = quiz2_17;

			initGui();
			NetworkControl.instance.init();

			_init = true;

			quizAnswers = new Dictionary<int,bool>();
			quizAnswersAnswer = new Dictionary<int,string>();

			spriteRenderer = GameObject.Find("QuizImageQuestion").GetComponent<SpriteRenderer>();

			float h = 2 * Camera.main.orthographicSize;
			float w = h * Camera.main.aspect;

			area = new Vector2(w,h);
			Debug.Log(" AREA : " + area);

			Debug.LogWarning(" SCREEN : "+Screen.width+"x"+Screen.height);

			Debug.LogError("OMG LETS GET THE QUIZ : "+NetworkControl.instance.currentSubState);
			if(NetworkControl.instance.currentSubState < 4)
			{
				NetworkControl.instance.getActiveQuiz(NetworkControl.instance.currentSubState, gotQuiz);
			}
			else
			{
				NetworkControl.instance.getLeaderBoard(gotLeaderBoard);
			}
		}
		else
		{
			Debug.LogWarning("NOPE");
		}
	}

	private void gotLeaderBoard(object dataObj)
	{
		Dictionary<string, object> gotLeaderBoard = (Dictionary<string, object>)dataObj;

		leaderBoard = new List<User>();

		if (gotLeaderBoard.ContainsKey("success") && gotLeaderBoard.ContainsKey("data"))
		{
			quizObj = new Quiz();

			Dictionary<string, object> data = gotLeaderBoard["data"] as Dictionary<string, object>;
			Debug.Log("" + gotLeaderBoard["data"]);
			foreach (object userObj in data["leaderboard"]  as List<object>)
			{
				User newUser = new User("","","");
				foreach (KeyValuePair<string, object> user in userObj as Dictionary<string, object>)
				{
					Debug.LogError(" [" + user.Key + "] " + user.Value);
					switch(user.Key)
					{
						case "name": newUser.name = user.Value.ToString(); break;
						case "email": newUser.email = user.Value.ToString(); break;
						case "totalScore": newUser.totalScore = user.Value.ToString(); break;
							
					}
				}
				leaderBoard.Add(newUser);
			}
			showLeaderBoard = true;
		}
	}

	private void gotQuiz(object dataObj)
	{
		Dictionary<string, object> loginReturn = (Dictionary<string, object>)dataObj;

		if(loginReturn.ContainsKey("success") && loginReturn.ContainsKey("data"))
		{
			quizObj = new Quiz();

			Dictionary<string, object> data = loginReturn["data"] as Dictionary<string, object>;

			foreach(KeyValuePair<string,object> quizValues in data["quiz"] as Dictionary<string,object>)
			{
//				Debug.Log("QUIZ["+quizValues.Key+"]="+quizValues.Value);
				if(quizValues.Key == "questions")
				{
					foreach(object questionObj in quizValues.Value as List<object>)
					{
						Question parseQuestion = new Question();
						foreach(KeyValuePair<string,object> question in questionObj as Dictionary<string,object>)
						{
							switch(question.Key)
							{
								case "id": parseQuestion.id = int.Parse(question.Value.ToString()); break;
								case "question": parseQuestion.question = WWW.UnEscapeURL(question.Value.ToString()); break;
								case "isImage": parseQuestion.isImage = ((question.Value.ToString() == "1") ? true : false); break;
								case "sort": parseQuestion.sort = int.Parse(question.Value.ToString()); break;
								case "value": parseQuestion.value = int.Parse(question.Value.ToString()); break;
							}

//							Debug.Log("QUESTION[" + question.Key + "]=" + question.Value);
							if(question.Key == "answers")
							{
								foreach(object answerObj in question.Value as List<object>)
								{
									Answer parseAnswer = new Answer();
									foreach(KeyValuePair<string,object> answer in answerObj as Dictionary<string,object>)
									{
										switch(answer.Key)
										{
											case "id": parseAnswer.id = int.Parse(answer.Value.ToString()); break;
											case "questionId": parseAnswer.questionId = int.Parse(answer.Value.ToString()); break;
											case "answer": parseAnswer.answer = WWW.UnEscapeURL(answer.Value.ToString()); break;
											case "sort": parseAnswer.sort = int.Parse(answer.Value.ToString()); break;
											case "isCorrect": parseAnswer.isCorrect = ((answer.Value.ToString()=="1") ? true : false); break;
										};
									}

									parseQuestion.answers.Add(parseAnswer);
								}
							}
						}
						quizObj.questions.Add(parseQuestion);
					}
				}
			}

			Debug.Log("OMG WE HAVE A QUIZ : "+quizObj.questions.Count+" : ");
			questionNumber = 0;
			_secondsLeft = "120";
			CountDownTimer.instance.startTimer(120, tick);
//			_secondsLeft = "10";
//			CountDownTimer.instance.startTimer(10, tick);
			showNextQuestion();
		}
		else
		{
			Debug.LogError("SAD A");
		}
	}

	private bool questionIsImage=false;
	private void showNextQuestion()
	{
		if (_forceOver) return;

		_quizMode = true;

		Question q = quizObj.questions[questionNumber];
		
		if(!q.isImage)
		{
			questionIsImage = false;
			_question = q.question;
			spriteRenderer.sprite = null;
		}
		else
		{
			questionIsImage = true;
			_question = q.question;
			spriteRenderer.sprite = imageQuestionsImages[NetworkControl.instance.currentSubState][(questionNumber+1)];
		}

		_answer1 = _answer2 = _answer3 = _answer4 = "";

		int xIt = 1;
		foreach(Answer answer in q.answers)
		{
			if(answer.isCorrect && NetworkControl.instance.userEmail == "dave@davegeurts.com") answer.answer += "-CORRECT";

			switch(xIt)
			{
				case 1: _answer1 = answer.answer; break;
				case 2: _answer2 = answer.answer; break;
				case 3: _answer3 = answer.answer; break;
				case 4: _answer4 = answer.answer; break;
			}

			xIt++;
		}
	}

	private void handleAnswer(int answerIdx)
	{
		if (_forceOver) return;

		spriteRenderer.sprite = null;
		_quizMode = false;
		Answer a = quizObj.questions[questionNumber].answers[answerIdx];
		if(a.isCorrect)
		{
			quizScore += quizObj.questions[questionNumber].value;
			NetworkControl.instance.userTotalScore += quizObj.questions[questionNumber].value;
			quizAnswers.Add(questionNumber,true);
		}
		else
		{
			quizAnswers.Add(questionNumber,false);
		}
		quizAnswersAnswer.Add(questionNumber,a.answer);

		NetworkControl.instance.recordScore( ((!a.isCorrect)?0:quizObj.questions[questionNumber].value),"quiz_" + quizObj.questions[questionNumber].id,answerSent);
	}

	private void answerSent(object dataObj)
	{
		if (_forceOver) return;

		questionNumber++;

		if(questionNumber < quizObj.questions.Count)
		{
			showNextQuestion();
		}
		else
		{
			_quizMode = false;
			_waitMode = true;

		}
	}

	private Vector2 topLeft;
	private Vector2 topRight;
	private Vector2 bottomLeft;
	private Vector2 bottomRight;

	private string _question="";
	private string _answer1 = "";
	private string _answer2 = "";
	private string _answer3 = "";
	private string _answer4 = "";

	private void initGui()
	{
		topLeft = translatePercent(new Vector2(5,18));
		topRight = translatePercent(new Vector2(95,18));
		bottomLeft = translatePercent(new Vector2(18,95));
		bottomRight = translatePercent(new Vector2(95,95));
	}

	private string _secondsLeft;
	public Vector2 scrollPosition = Vector2.zero;
	public Vector2 scrollPosition2 = Vector2.zero;

	void OnGUI()
	{
		GUI.enabled = true;

		GUIStyle myTextLargeStyle = new GUIStyle(GUI.skin.label);
		myTextLargeStyle.fontSize = 50;
		myTextLargeStyle.stretchWidth = true;
		myTextLargeStyle.normal.textColor = Color.black;
		myTextLargeStyle.alignment = TextAnchor.MiddleCenter;

		GUIStyle myTextStyle = new GUIStyle(GUI.skin.label);
		myTextStyle.fontSize = 30;
		myTextStyle.stretchWidth = true;
		myTextStyle.normal.textColor = Color.black;

		GUIStyle smallerTextStyle = new GUIStyle(GUI.skin.label);
		smallerTextStyle.fontSize = 25;
		smallerTextStyle.normal.textColor = Color.black;

		GUI.Label(new Rect(900, 11, 150, 150), _secondsLeft, myTextLargeStyle);

		if (_quizMode)
		{
			GUI.BeginGroup(new Rect(topLeft.x,topLeft.y,topRight.x - topLeft.x,bottomRight.y - topRight.y));
			GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
					GUILayout.BeginVertical();
						GUILayout.Label("Question :",myTextStyle,GUILayout.Width(topRight.x - topLeft.x));
						GUILayout.Label(_question,myTextStyle,GUILayout.Width(topRight.x - topLeft.x));
						if(questionIsImage) GUILayout.Space(175);
					GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				GUILayout.Space(10);

				GUILayout.BeginVertical();
					GUI.enabled = ((_answer1=="") ? false : true);
					GUILayout.Label(_answer1,smallerTextStyle,GUILayout.Width(topRight.x - topLeft.x));
						if(GUILayout.Button("ANSWER 1")) handleAnswer(0);
					GUI.enabled = true;
					GUILayout.EndVertical();

				GUILayout.Space(10);
				GUILayout.BeginVertical();
					GUI.enabled = ((_answer2 == "") ? false : true);
					GUILayout.Label(_answer2,smallerTextStyle,GUILayout.Width(topRight.x - topLeft.x));
						if(GUILayout.Button("ANSWER 2")) handleAnswer(1);
					GUI.enabled = true;
					GUILayout.EndVertical();

				GUILayout.Space(10);
				GUILayout.BeginVertical();
					GUI.enabled = ((_answer3 == "") ? false : true);
					GUILayout.Label(_answer3,smallerTextStyle,GUILayout.Width(topRight.x - topLeft.x));
						if(GUILayout.Button("ANSWER 3")) handleAnswer(2);
					GUI.enabled = true;
					GUILayout.EndVertical();

				GUILayout.Space(10);
				GUILayout.BeginVertical();
					GUI.enabled = ((_answer4 == "") ? false : true);
					GUILayout.Label(_answer4,smallerTextStyle,GUILayout.Width(topRight.x - topLeft.x));
						if(GUILayout.Button("ANSWER 4")) handleAnswer(3);
					GUI.enabled = true;
					GUILayout.EndVertical();

			GUILayout.EndVertical();
			GUI.EndGroup();
			GUI.enabled = false;
		}
		else if(_waitMode)
		{
			GUI.BeginGroup(new Rect(topLeft.x, topLeft.y, topRight.x - topLeft.x, bottomRight.y - topRight.y));
			GUILayout.BeginVertical();
			GUILayout.Label("WAIT FOR RESULTS...", myTextStyle, GUILayout.Width(topRight.x - topLeft.x));
			GUILayout.EndVertical();
			GUI.EndGroup();
			GUI.enabled = false;
		}
		else if(_resultsMode)
		{
			GUI.Label(new Rect(520, 11, 350, 150), "Total Score:"+NetworkControl.instance.userTotalScore, myTextLargeStyle);

			GUIStyle myTextStyle2 = new GUIStyle(GUI.skin.label);
			myTextStyle2.fontSize = 40;
			myTextStyle2.stretchWidth = true;
			myTextStyle2.normal.textColor = Color.black;

			GUIStyle smallerTextStyle2 = new GUIStyle(GUI.skin.label);
			smallerTextStyle2.fontSize = 25;
			smallerTextStyle2.normal.textColor = Color.black;

			GUIStyle smallTextCorrectStyle = new GUIStyle(GUI.skin.label);
			smallTextCorrectStyle.fontSize = 16;
			smallTextCorrectStyle.normal.textColor = Color.blue;

			GUIStyle smallTextWrongStyle = new GUIStyle(GUI.skin.label);
			smallTextWrongStyle.fontSize = 16;
			smallTextWrongStyle.normal.textColor = Color.red;

			GUI.BeginGroup(new Rect(topLeft.x,topLeft.y,topRight.x - topLeft.x,bottomRight.y - topRight.y));
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.Label("YOUR SCORE This quiz : +" + quizScore, myTextStyle2, GUILayout.Width(topRight.x - topLeft.x));
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.Space(10);

			scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(700), GUILayout.Height(500));
			int zIt = 1;
			foreach(KeyValuePair<int,bool> qa in quizAnswers)
			{
				GUILayout.Space(20);
				GUILayout.BeginVertical();
				GUILayout.Label("Question " + zIt + ") " + ((qa.Value) ? "CORRECT" : "WRONG") + " " + quizObj.questions[qa.Key].question,((qa.Value) ? smallTextCorrectStyle : smallTextWrongStyle),GUILayout.Width(topRight.x - topLeft.x));
				GUILayout.Label("Your Answer = " + quizAnswersAnswer[qa.Key],((qa.Value) ? smallTextCorrectStyle : smallTextWrongStyle),GUILayout.Width(topRight.x - topLeft.x));
				GUILayout.EndVertical();
				zIt++;
			}
			GUILayout.EndScrollView();

			GUILayout.EndVertical();
			GUI.EndGroup();
			GUI.enabled = false;

			
		}
		else if (showLeaderBoard)
		{
			GUI.Label(new Rect(520, 11, 350, 150), "LEADER BOARD", myTextLargeStyle);

			GUIStyle myTextStyle2 = new GUIStyle(GUI.skin.label);
			myTextStyle2.fontSize = 40;
			myTextStyle2.stretchWidth = true;
			myTextStyle2.normal.textColor = Color.black;

			GUIStyle smallerTextStyle2 = new GUIStyle(GUI.skin.label);
			smallerTextStyle2.fontSize = 25;
			smallerTextStyle2.normal.textColor = Color.black;

			GUIStyle smallTextCorrectStyle = new GUIStyle(GUI.skin.label);
			smallTextCorrectStyle.fontSize = 16;
			smallTextCorrectStyle.normal.textColor = Color.blue;

			GUIStyle smallTextWrongStyle = new GUIStyle(GUI.skin.label);
			smallTextWrongStyle.fontSize = 16;
			smallTextWrongStyle.normal.textColor = Color.red;

			GUI.BeginGroup(new Rect(topLeft.x, topLeft.y, topRight.x - topLeft.x, bottomRight.y - topRight.y));

			scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2, false, true, GUILayout.Width(700), GUILayout.Height(500));
			int zIt = 1;
			foreach (User user in leaderBoard)
			{
				GUILayout.Space(20);
				GUILayout.BeginVertical();
				GUILayout.Label("PLACE:" + zIt + " , Score:" + user.totalScore + ", " + user.name + " " + user.email + " " + user.totalScore, smallTextCorrectStyle, GUILayout.Width(topRight.x - topLeft.x));
				GUILayout.EndVertical();
				zIt++;
			}
			GUILayout.EndScrollView();

			GUI.EndGroup();
			GUI.enabled = false;
		}
		else
		{
			
		}
	}

	private Vector2 translatePercent(Vector2 vect)
	{
		Vector2 per = new Vector2(0,0);
		per.x = ((vect.x/100) * Screen.width);
		per.y = ((vect.y / 100) * Screen.height);
		return per;
	}

	private void tick(int secondsLeft)
	{
		if(!showLeaderBoard)
		{
			_secondsLeft = secondsLeft.ToString();

			if(secondsLeft <= 0)
			{
				_forceOver = true;
				questionIsImage = false;
				if(spriteRenderer != null) spriteRenderer.sprite = null;

				_quizMode = false;
				_resultsMode = true;
				_waitMode = false;
			}
		}
	}
}
