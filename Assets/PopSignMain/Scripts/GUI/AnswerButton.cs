using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnswerButton : MonoBehaviour {

  public GameObject helpText;
  public Sprite normalImage;
  public Sprite selectedImage;
  bool active = false;

	public void ButtonClick()
	{
    // disable the answer banner
    if(active)
    {
      active = false;
      helpText.SetActive( false );
      gameObject.GetComponent<Image>().sprite = normalImage;
    }
    // enable the answer banner
    else
    {
      active = true;
      gameObject.GetComponent<Image>().sprite = selectedImage;
      helpText.SetActive( true );
    }
	}
}
