using UnityEngine;
using System.Collections;

public class SwapButton : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{

	}

  public void OnMouseDown()
  {
      if (name == "SwapButton")
      {
					mainscript.Instance.GetNewBall();
      }
  }
}
