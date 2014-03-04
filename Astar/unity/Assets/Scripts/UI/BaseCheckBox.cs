using UnityEngine;
using System.Collections;

public class BaseCheckBox : BaseButton 
{
	public Sprite check;
	public Sprite checkOvr;

	private bool _isChecked = false;
	public bool isChecked
	{
		get
		{
			return _isChecked;
		}

		set
		{
			_isChecked = value;

		}
	}

	protected override void OnMouseOvr()
	{
		if (_isChecked) _renderer.sprite = checkOvr;
		else _renderer.sprite = ovr;
	}

	protected override void OnMouseOff()
	{
		if(_isChecked) _renderer.sprite = check;
		else _renderer.sprite = up;
	}

	protected override void OnTap(Vector2 fingerPos)
	{
		_isChecked = !_isChecked;

		if(_isChecked) _renderer.sprite = checkOvr;

		base.OnTap(fingerPos);
	}
}
