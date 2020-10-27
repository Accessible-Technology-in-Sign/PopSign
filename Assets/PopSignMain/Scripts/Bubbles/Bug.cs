using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bug : MonoBehaviour
{
  Transform spiders;
  public Sprite[] textures;
  public int color;
  int score = 25;
  // Use this for initialization
  void Start()
  {
    spiders = GameObject.Find("Spiders").transform;
    ChangeColor(0);
    if (mainscript.Instance.ComboCount % 3 == 0 && mainscript.Instance.ComboCount > 0) ChangeColor(1);
    if (mainscript.Instance.ComboCount % 5 == 0 && mainscript.Instance.ComboCount > 0) ChangeColor(2);
  }


  public void MoveOut()
  {
    transform.parent = null;
    StartCoroutine(MoveOutCor());
  }

  IEnumerator MoveOutCor()
  {
    SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.bugDissapier);

    AnimationCurve curveY = new AnimationCurve(new Keyframe(0, transform.localPosition.y), new Keyframe(1, 10));
    curveY.AddKey(0.5f, transform.localPosition.y + 3);
    float startTime = Time.time;
    Vector3 startPos = transform.localPosition;
    float speed = 0.6f;
    float distCovered = 0;
    while (distCovered < 1)
    {
      distCovered = (Time.time - startTime) * speed;
      transform.localPosition = new Vector3(transform.localPosition.x, curveY.Evaluate(distCovered), 0);

      yield return new WaitForEndOfFrame();
    }
    transform.parent = null;
    Destroy(gameObject);
  }

  public void ChangeColor(int i)
  {
    color = i;
    GetComponent<SpriteRenderer>().sprite = textures[i];
  }

}
