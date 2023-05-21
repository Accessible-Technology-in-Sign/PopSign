using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollButton : MonoBehaviour {

	private string Name;
	private bool selected;
	public Text ButtonText;
	public ScrollView ScrollView;
	public GameObject checkmark;
    public GameObject CheckBox;

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
        if (CustomizeLevelManager.Instance == null)
        {
            return;
        }
		int size = CustomizeLevelManager.Instance.selectedWord.Count;
		if (size >= 5 & !selected) {
			return;
		}
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
