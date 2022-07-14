using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SwapButton : MonoBehaviour {

  private Image image;
  //Used to change color when new sign is pressed
  private Color purple = new Color(1f, 0f, 0.9f);
  private Color none = new Color(1f,1f,1f);
  private float duration = 2.5f;
  private float t = 0;
  private bool pressed = false;

	// Use this for initialization
	void Start ()
	{
    image = GetComponent<Image>();
	}

  void Update(){

    if(pressed && name =="SwapButton"){
      updateColor();
    }
  }

  void updateColor(){
    image.color = Color.Lerp(purple, none, t);
     t += Time.deltaTime/duration;
     if (t>1){
      t = 0;
      pressed = false;
     }
  }

  public void OnMouseDown()
  {
      if (name == "SwapButton")
      {
					mainscript.Instance.GetNewBall();
          pressed = true;
          t = 0;
      }
  }
}
