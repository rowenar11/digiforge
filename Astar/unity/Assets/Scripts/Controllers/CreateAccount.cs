using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateAccount : MonoSingleton<CreateAccount>
{
	private bool _init = false;

	void Start()
	{
		init();
	}

	private bool created = false;

	private void init()
	{
		if (!_init)
		{
			_init = true;
			Debug.Log("");
			Debug.Log(" INIT()  :::::::::: ");
			Debug.Log("");

			NetworkControl.instance.init();

			GameObject.Find("CreateAccountBtn").GetComponent<BaseButton>().Subscribe(this, CreateAccountHdlr);
			//GameObject.Find("LoginBtn").GetComponent<BaseButton>().Subscribe(this, LoginHdlr);
			GameObject.Find("TestBtn").GetComponent<BaseButton>().Subscribe(this,TestHdlr);
		}
		else
		{
			Debug.LogWarning("NOPE");
		}
	}

	private void CreateAccountHdlr()
	{
		if(!created)
		{
			Debug.LogError("create account : " + _name + " : " + _email);
			NetworkControl.instance.createAccount(_name, _email, createAccountReturn);
			created = true;
			_showError = false;
		}
	}

	private  void createAccountReturn(object dataObj = null)
	{
		created = false;
		 
		Dictionary<string, object> loginReturn = (Dictionary<string, object>)dataObj;

		if(loginReturn.ContainsKey("success") && loginReturn.ContainsKey("data"))
		{
			Dictionary<string, object> data = loginReturn["data"] as Dictionary<string, object>;

			if(bool.Parse(loginReturn["success"].ToString()))
			{
				NetworkControl.instance.sessionId = int.Parse(data["sessionId"].ToString());
				NetworkControl.instance.userId = int.Parse(data["userId"].ToString());
				NetworkControl.instance.userName = _name;
				NetworkControl.instance.userEmail = _email;

				SceneControl.instance.LoadWelcome();
			}
			else
			{
				_showError = true;
				_errorMessage = data["msg"].ToString();
			}
		}
	}


	private void LoginHdlr()
	{
		if (!created)
		{
			NetworkControl.instance.login(_email, loginAccountReturn);
			created = true;
			_showError = false;
		}
	}

	private void TestHdlr()
	{
		if (!created)
		{
			NetworkControl.instance.test(testReturn);
			created = true;
			_showError = false;
		}
	}

	private void loginAccountReturn(object dataObj = null)
	{
		created = false;

		Dictionary<string, object> loginReturn = (Dictionary<string, object>)dataObj;

		if (loginReturn.ContainsKey("success") && loginReturn.ContainsKey("data"))
		{
			Dictionary<string, object> data = loginReturn["data"] as Dictionary<string, object>;

			if (bool.Parse(loginReturn["success"].ToString()))
			{
				NetworkControl.instance.sessionId = int.Parse(data["sessionId"].ToString());
				NetworkControl.instance.userId = int.Parse(data["userId"].ToString());
				NetworkControl.instance.userName = data["name"].ToString();
				NetworkControl.instance.userEmail = data["email"].ToString();

				SceneControl.instance.LoadWelcome();
			}
			else
			{
				_showError = true;
				_errorMessage = data["msg"].ToString();
			}
		}
	}

	private void testReturn(object dataObj = null)
	{
		created = false;

		_showError = true;
		_errorMessage = "OMG IT WORKED!";

	}

	private bool _showError = false;
	private string _errorMessage = "";

	public string _name = "";
	public string _email = "dave@davegeurts.com";
	void OnGUI()
	{
		GUIStyle myTextStyle = new GUIStyle(GUI.skin.textField);
		myTextStyle.fontSize = 40;

		GUIStyle myLabelStyle = new GUIStyle(GUI.skin.label);
		myLabelStyle.fontSize = 40;

		GUI.Label(new Rect(175, 450, 175, 60), "Name :", myLabelStyle);
		_name = GUI.TextField(new Rect(320, 450, 600, 60), _name, 25, myTextStyle);

		GUI.Label(new Rect(180, 550, 175, 60), "Email :", myLabelStyle);
		_email = GUI.TextField(new Rect(320, 550, 600, 60), _email, 25, myTextStyle);

		if(_showError)
		{
			GUIStyle myLabelStyleError = new GUIStyle(GUI.skin.label);
			myLabelStyleError.fontSize = 25;

			GUI.Label(new Rect(350, 400, 450, 60), _errorMessage, myLabelStyleError);
		}
	}
}
