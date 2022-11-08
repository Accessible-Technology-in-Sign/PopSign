using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    private int currentLevel;

    // new stuff:
    public GameObject step1;
    // public Transform hand1;
    public GameObject step2;
    // public Transform hand2;


    // private Vector3 _originalScale;
    // private Vector3 _scaleTo;

    private int numberOfTries = 0; // counter for number of times bubble is shot

    void Awake()
    {
        currentLevel = PlayerPrefs.GetInt( "OpenLevel", 1 );

        // _originalScale = hand1.localScale;
        // _scaleTo = _originalScale * 0.1f;

        step1.SetActive(false);
        step2.SetActive(false);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(currentLevel == 1) { 
        
            step1.SetActive(true);
            step2.SetActive(false);
            // OnScale(hand1);
        }
    }

    // private void OnScale(Transform hand)
    // {
    //     hand.DOScale(_scaleTo, 2.0f)
    //         .SetEase(Ease.InOutSine)
    //         .OnComplete(() =>
    //         {
    //             hand.DOScale(_originalScale, 2.0f)
    //                 .SetEase(Ease.OutBounce)
    //                 .OnComplete(() => OnScale(hand));
    //         });
    // }

    // Update is called once per frame
    void Update()
    {
    }

    public void BallHit(){
        if (currentLevel == 1) {
            numberOfTries++;
            if(numberOfTries == 1){

                step1.SetActive(false);
                step2.SetActive(false);

                StartCoroutine(Step2Animation());
            } else if (numberOfTries == 2){
                step1.SetActive(false);
                step2.SetActive(false);
            }
        } 
    }

    private IEnumerator Step2Animation(){
        yield return new WaitForSeconds(1f);
        
        step1.SetActive(false);
        step2.SetActive(true);
        // OnScale(hand2);
    }
}
