using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private GameObject busy;

    public GameObject Busy
    {
        get { return busy; }
        set
        {
            if( value != null )
            {
                if( value.GetComponent<ball>() != null )
                {
                    if( !value.GetComponent<ball>().NotSorting  )
                    {

                //        value.GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt( 1 / ( transform.position.y + 10 ) * 100 );
                        value.GetComponent<ball>().mesh = gameObject;
                        if( value.tag == "star" ) value.GetComponent<SpriteRenderer>().sortingOrder = 100;
                    }
                }

            }

            busy = value;
        }
    }

    bool destroyed;
    public float offset;
    bool triggerball;
    public static bool waitForAnim;
    public GameObject boxCatapult;

    // Update is called once per frame
    void Update()
    {
        // if there is no ball in the box of the current game object
        if( busy == null )
        {
            // if the box is empty, create a new ball for it
            if( name == "boxCatapult" && !Grid.waitForAnim )
            {
                if( ( GamePlay.Instance.GameStatus == GameState.Playing || GamePlay.Instance.GameStatus == GameState.Win || GamePlay.Instance.GameStatus == GameState.WaitForStar ) && LevelData.LimitAmount > 0 )
                {
                    busy = Camera.main.GetComponent<mainscript>().createCannonBall( transform.position );
                    GameObject ball = boxCatapult.GetComponent<Grid>().busy;
                }
            }
        }

        // check if ball has been shot
        if( busy != null && !Grid.waitForAnim )
        {
            if( name == "boxCatapult" )
            {
                if( busy.GetComponent<ball>().setTarget )
                {
                    busy = null;
                }
            }
        }
    }

    public void BounceFrom( GameObject box )
    {
        GameObject ball = box.GetComponent<Grid>().busy;
        if( ball != null && busy != null )
        {
            busy.GetComponent<bouncer>().bounceTo( box.transform.position );
            box.GetComponent<Grid>().busy = busy;
            busy = ball;
        }
    }

    void setColorTag( GameObject ball )
    {
        if( ball.name.IndexOf( "Orange" ) > -1 )
        {
            ball.tag = "Fixed";
            //	tag = "Orange";
        }
        else if( ball.name.IndexOf( "Red" ) > -1 )
        {
            ball.tag = "Fixed";
            //	tag = "Red";
        }
        else if( ball.name.IndexOf( "Yellow" ) > -1 )
        {
            ball.tag = "Fixed";
            //	tag = "Yellow";
        }
    }

    void OnCollisionStay2D( Collision2D other )
    {
        if( other.gameObject.name.IndexOf( "ball" ) > -1 && busy == null )
        {
            busy = other.gameObject;
        }
    }

    void OnTriggerExit( Collider other )
    {
        //busy = null;
    }

    public void destroy()
    {
        tag = "Mesh";
        Destroy( busy );
        busy = null;
    }
}
