using UnityEngine;
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;

public delegate void delegateNetworkData(object dataObj = null);
public delegate void delegateNetworkSendData(string command, Hashtable dataObj, delegateNetworkData callBack = null, delegateNetworkData failCallBack = null);

public class NetworkControl : MonoSingleton<NetworkControl>
{
	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

//	public int sessionId = -1;
//	public int userId=-1;
//	public string userName = "";
//	public string userEmail = "";

	public int sessionId = 1;
	public int userId = 1;

	public string userName = "Dave";
	public string userEmail = "dave@davegeurts.com";

	public int userTotalScore = 0;

	private int queryId = 0;
	private Hashtable queryCallBack;
	private Dictionary<WWW, delegateNetworkData> queryFailCallBack;
	public string gatewayURL = "https://www.auditrequests.com/digiforge/controller.php";

	private bool _initted = false;

	void Awake() { }

	private List<int> ignore = new List<int>(); 

	//Init sequence
	public void init()
	{
		if (!_initted)
		{
			_initted = true;

			queryCallBack = new Hashtable();
			queryFailCallBack = new Dictionary<WWW, delegateNetworkData>();
		}
	}

	public void sendData(string command, Hashtable dataObj, delegateNetworkData callBack = null, delegateNetworkData failCallBack = null)
	{
		WWWForm form = new WWWForm();
		queryId++;

		if (command != "getState")
		{
			UnityEngine.Debug.Log("**^^^^^ sending " + command + " to SERVER " + gatewayURL);
			ignore.Add(queryId);
		}

		if(callBack != null) queryCallBack[queryId] = callBack;

		dataObj["sessionId"] = sessionId;
		dataObj["userId"] = userId;

		form.AddField("secret", "ummmmmmmOk");
		form.AddField("json", Json.Serialize(dataObj));
		form.AddField("qid", queryId);
		form.AddField("command", command);
		WWW www = new WWW(gatewayURL, form);
		if (failCallBack != null) queryFailCallBack[www] = failCallBack;

		StartCoroutine(WaitForRequest(www));
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		if (www.error == null)
		{

			Hashtable returnData = new Hashtable();
			string[] wwwReturn = www.text.Split('&');
			foreach(string pair in wwwReturn)
			{
				string[] splitPair = pair.Split('=');
				if(splitPair.Length != 2)
				{
					UnityEngine.Debug.LogError("ERROR: Server response with invalid pair length: " + pair.Length);
					for(int i = 0; i < splitPair.Length; i++)
					{
						UnityEngine.Debug.LogError("*** splitPair[" + i + "]=" + splitPair[i]);
					}
					if(queryFailCallBack.ContainsKey(www) && queryFailCallBack[www] != null)
					{
						queryFailCallBack[www]();
						queryFailCallBack[www] = null;
					}
					yield break;
				}

				returnData[splitPair[0]] = splitPair[1];
				if(splitPair[0] == "qid") returnData[splitPair[0]] = Int32.Parse(returnData[splitPair[0]].ToString());
			}

			if(!ignore.Contains(int.Parse(returnData["qid"].ToString())))
			{
				//UnityEngine.Debug.Log("WWW>" + www.text);
			}
			else ignore.Remove(int.Parse(returnData["qid"].ToString()));

			if(queryCallBack[returnData["qid"]] != null)
			{
				object dataObj = null;
				if(returnData["json"] != null)
				{
					dataObj = Json.Deserialize(returnData["json"].ToString());
					//UnityEngine.Debug.LogWarning("DataObj::"+dataObj);
				}
				else dataObj = returnData;

				(queryCallBack[returnData["qid"]] as delegateNetworkData)(dataObj);

				queryCallBack[returnData["qid"]] = null;
				if(queryFailCallBack.ContainsKey(www) && queryFailCallBack[www] != null) queryFailCallBack[www] = null;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("ERROR WWWW");
		}
	}

	public void createAccount(string name, string email, delegateNetworkData callBack = null, delegateNetworkData failCallBack = null)
	{
		Hashtable loginObj = new Hashtable();
		loginObj["name"] = name;
		loginObj["email"] = email;
		sendData("createAccount", loginObj, callBack, failCallBack);
	}

	public void test(delegateNetworkData callBack = null,delegateNetworkData failCallBack = null)
	{
		Hashtable loginObj = new Hashtable();
		sendData("test",loginObj,callBack,failCallBack);
	}

	public void login(string email, delegateNetworkData callBack = null, delegateNetworkData failCallBack = null)
	{
		Hashtable loginObj = new Hashtable();
		loginObj["email"] = email;
		sendData("login", loginObj, callBack, failCallBack);
	}

	public void getState(delegateNetworkData callBack = null, delegateNetworkData failCallBack = null)
	{
		Hashtable loginObj = new Hashtable();
		sendData("getState", loginObj, callBack, failCallBack);
	}

	public void recordScore(int value, string type, delegateNetworkData callBack = null, delegateNetworkData failCallBack = null)
	{
		Hashtable loginObj = new Hashtable();
		loginObj["value"] = value;
		loginObj["type"] = type;
		sendData("recordScore", loginObj, callBack, failCallBack);
	}

	public void getTotalScore(delegateNetworkData callBack = null, delegateNetworkData failCallBack = null)
	{
		Hashtable loginObj = new Hashtable();
		sendData("getTotalScore", loginObj, callBack, failCallBack);
	}

	public void getLeaderBoard(delegateNetworkData callBack = null, delegateNetworkData failCallBack = null)
	{
		Hashtable loginObj = new Hashtable();
		sendData("getLeaderBoard", loginObj, callBack, failCallBack);
	}

	public void getActiveQuiz(int quizId, delegateNetworkData callBack = null, delegateNetworkData failCallBack = null)
	{
		Hashtable loginObj = new Hashtable();
		loginObj["quizId"] = quizId;
		sendData("getActiveQuiz", loginObj, callBack, failCallBack);
	}

	public string currentState = "";
	public int currentSubState = 0;
	public bool pollingActive = false;

	private float timeToWait=2f;

	public void StartPolling()
	{
		pollingActive = true;
		NextPoll();
	}

	public void NextPoll()
	{
		StartCoroutine(waitForPoll());
	}

	IEnumerator waitForPoll()
	{
		yield return new WaitForSeconds(timeToWait);
		timeToWait = 1f;

		getState(getStateReturn);
	}

	private void getStateReturn(object dataObj = null)
	{
		Dictionary<string, object> loginReturn = (Dictionary<string, object>)dataObj;

		if(loginReturn.ContainsKey("success") && loginReturn.ContainsKey("data"))
		{
			Dictionary<string, object> data = loginReturn["data"] as Dictionary<string, object>;
			if(bool.Parse(loginReturn["success"].ToString()))
			{
				if (currentState != data["state"].ToString())
				{
					gotNewState(data["state"].ToString(), int.Parse(data["subState"].ToString()));
					timeToWait = 1f;
				}
			}
		}

		NextPoll();
	}

	private void gotNewState(string newState, int subState=0)
	{
		switch(newState)
		{
			case "welcome":
				if (SceneControl.instance.currentScene != "Welcome")
				{
					SceneControl.instance.LoadWelcome();
					currentState = newState;
				}
				break;

			case "payAttention":
				if (SceneControl.instance.currentScene != "PayAttention")
				{
					SceneControl.instance.LoadPayAttention();
					currentState = newState;
				}
				break;

			case "pathFinding":
				if (SceneControl.instance.currentScene != "PathFindingScene1")
				{
					SceneControl.instance.LoadPathFinding();
					currentState = newState;
				}
				break;

			case "raycasting":
				if (SceneControl.instance.currentScene != "RayTrace")
				{
					SceneControl.instance.LoadRayCasting();
					currentState = newState;
				}
				break;

			case "quiz":
				if (SceneControl.instance.currentScene != "Quiz")
				{
					SceneControl.instance.LoadQuiz();
					currentState = newState;
					currentSubState = subState;
				}
				break;
		}
	}


}