using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollButton : MonoBehaviour {

	private string Name;
	private bool selected;
	public Text ButtonText;
	public ScrollView ScrollView;
	public GameObject checkmark;

	public void Start()
	{
		selected = false;
		checkmark.SetActive(false);
	}

	public void SetName(string name)
	{
		Name = name;
		ButtonText.text = name;
	}
	public void ButtonClick()
	{
		ScrollView.ButtonClicked(Name);
	}

	public void ToggleSelection()
	{
		if(selected)
		{
			checkmark.SetActive(false);
		}
		else
		{
			checkmark.SetActive(true);
		}
		selected = !selected;
		ScrollView.UpdateSelection(Name, selected);
	}
}
