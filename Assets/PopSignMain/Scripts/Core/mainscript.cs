using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InitScriptName;


[RequireComponent(typeof(AudioSource))]
public class mainscript : MonoBehaviour {
public int currentLevel;
public static mainscript Instance;
GameObject ball;
GameObject PauseDialogLD;
GameObject OverDialogLD;
GameObject PauseDialogHD;
GameObject OverDialogHD;
GameObject UI_LD;
GameObject UI_HD;
GameObject PauseDialog;
GameObject OverDialog;
GameObject FadeLD;
GameObject FadeHD;
GameObject AppearLevel;
Target target;
Vector2 worldPos;
Vector2 startPos;
float startTime;
bool setTarget;
float mTouchOffsetX;
float mTouchOffsetY;
float xOffset;
float yOffset;
public int bounceCounter = 0;
GameObject[] fixedBalls;
public Vector2[][] meshArray;
int offset;
public GameObject flyingBall;
public GameObject newBall;
public static int stage = 1;
const int STAGE_1 = 0;
const int STAGE_2 = 300;
const int STAGE_3 = 750;
const int STAGE_4 = 1400;
const int STAGE_5 = 2850;
const int STAGE_6 = 4100;
const int STAGE_7 = 5500;
const int STAGE_8 = 6900;
const int STAGE_9 = 8500;
public int arraycounter = 0;
public ArrayList controlArray = new ArrayList();
bool destringAloneBall;
public bool droppingDown;
public float dropDownTime = 0f;
public bool isPaused;
public bool gameOver;
public bool arcadeMode;
public float bottomBorder;
public float topBorder;
public float leftBorder;
public float rightBorder;
public float gameOverBorder;
public float ArcadedropDownTime;
public bool hd;
public GameObject Fade;
public int highScore;
public AudioClip pops;
public AudioClip click;
public AudioClip levelBells;
float appearLevelTime;
public GameObject boxCatapult;
public GameObject BonusLiana;
public GameObject BonusScore;
public static bool ElectricBoost;
bool BonusLianaCounter;
bool gameOverShown;
public static bool StopControl;
public GameObject finger;
public GameObject BoostChanging;
public creatorBall creatorBall;
public GameObject GameOverBorderObject;
public GameObject TopBorder;
public Transform Balls;
public Hashtable animTable = new Hashtable();
public static Vector3 lastBall;
public static int doubleScore=1;
public int TotalTargets;
public int countOfPreparedToDestroy;
public int bugSounds;
public int potSounds;
public static Dictionary<int, BallColor> colorsDict = new Dictionary<int, BallColor>();
public GameObject[] starsObject;
public int stars = 0;
public GameObject perfect;
public GameObject[] boosts;
public GameObject[] locksBoosts;
public GameObject arrows;
public GameObject newBall2;
private int maxCols;
private int maxRows;
private int limit;
private int colorLimit;
private int MustPopCount = 11;
private int BallLayer = 9;

private int _ComboCount;

//this variable is set in response to the whiff variable in ball; it's true when the flying ball didn't hit at least 2 balls of its color
public bool BallWhiffed;
public int ComboCount

{
    get { return _ComboCount; }
    set
    {
        _ComboCount = value;
        if( value > 0 )
        {
            SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.combo[Mathf.Clamp(value-1, 0, 5)]);
            if( value >= 6 )
            {
                SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.combo[5]);
                //     FireEffect.SetActive( true );
                doubleScore = 2;
            }
        }
        else
        {
            //    FireEffect.SetActive( false );
            doubleScore = 1;
        }
    }
}

public int TargetCounter;
public int TargetCounter1
{
    get { return TargetCounter; }
    set {
        TargetCounter = value;
    }
}

private static int score;
public static int Score
{
    get { return mainscript.score; }
    set { mainscript.score = value; }
}

void Awake()
{
    if( InitScript.Instance == null )
        gameObject.AddComponent<InitScript>();
    currentLevel = PlayerPrefs.GetInt( "OpenLevel", 1 );
    stage = 1;
    mainscript.StopControl = false;
    animTable.Clear();
    creatorBall = GameObject.Find("Creator").GetComponent<creatorBall>();
    StartCoroutine(CheckColors());
}

IEnumerator CheckColors ()
{
    while(true)
    {
        GetColorsInGame();
        yield return new WaitForEndOfFrame();
        SetColorsForNewBall();
    }
}

IEnumerator ShowArrows()
{
    while( true )
    {
        yield return new WaitForSeconds( 30 );
        if( GamePlay.Instance.GameStatus == GameState.Playing )
        {
            arrows.SetActive( true );
        }
        yield return new WaitForSeconds( 3 );
        arrows.SetActive( false );
    }
}

void Start()
{
    Instance = this;
    score = 0;
    arcadeMode = false;
    GamePlay.Instance.GameStatus = GameState.BlockedGame;

    //Tutorial in level 1:
    // GameObject tutorial_text = GameObject.Find("arrowtextbox");
    // Debug.Log("Setting tutorial text on " + tutorial_text.name);
    // tutorial_text.SetActive(true);
}

// Update is called once per frame
void Update ()
{
    
    if(gameOver && !gameOverShown)
    {
        gameOverShown = true;
        GamePlay.Instance.GameStatus = GameState.Playing;
        score = 0;
            _ComboCount = 0;
            mainscript.score = 0;
     }

    // if there are balls to clear
    if( flyingBall != null && ( GamePlay.Instance.GameStatus == GameState.Playing || GamePlay.Instance.GameStatus == GameState.WaitForStar ))
    {
        // this line decides which balls to pop
        flyingBall.GetComponent<ball>().checkNearestColor();
        // get rid of the rigidbody on the flying ball (only necessary for collision, fixed balls don't have this)
        Destroy(flyingBall.GetComponent<Rigidbody>());
        // decrease the moves counter
        LevelData.LimitAmount--;
        
        int missCount = 1;
        if(stage >= 3) missCount = 2;
        if(stage >= 9) missCount = 1;
        
        //BallWhiffed will be used in clearDisconnectedBalls as a way to hopefully break out of the function early if there are no balls to pop
        if (!flyingBall.GetComponent<ball>().whiff) {
            BallWhiffed = false;
        } else {
            BallWhiffed = true;
        }
        StartCoroutine( clearDisconnectedBalls() );

        
        flyingBall = null;
        if(!arcadeMode)
        {
            if (bounceCounter >= missCount)
            {
                bounceCounter = 0;
                dropDownTime = Time.time + 0.5f;
                //Invoke("dropUp", 0.1f);
            }
            // commenting this out doesn't seem to do anything
            /*
            else
            {

                if (!destringAloneBall && !droppingDown)
                {
                    //connectNearBallsGlobal();
                    // destringAloneBall = true;
                    //StartCoroutine(clearDisconnectedBalls());
                    //  droppingDown = true;
                }

            }
            */
        }
        //      createBall();
    }

    if( arcadeMode && Time.time > ArcadedropDownTime && GamePlay.Instance.GameStatus == GameState.Playing )
    {
        bounceCounter = 0;
        ArcadedropDownTime = Time.time + 10f;
        dropDownTime = Time.time + 0.2f;
        dropDown();
    }

    if (Time.time > dropDownTime && dropDownTime != 0f)
    {
        dropDownTime = 0;
        StartCoroutine(getBallsForMesh());
    }

        // update game state
        // if (PlayerPrefs.GetInt("OpenLevel") == 7 || PlayerPrefs.GetInt("OpenLevel") == 11 || PlayerPrefs.GetInt("OpenLevel") == 21)
        // {
        //     MustPopCount = 4;
        // }
        // else if (PlayerPrefs.GetInt("OpenLevel") == 10 || PlayerPrefs.GetInt("OpenLevel") == 23)
        // {
        //     MustPopCount = 7;
        // }
        // else if (PlayerPrefs.GetInt("OpenLevel") == 12 || PlayerPrefs.GetInt("OpenLevel") == 14 || PlayerPrefs.GetInt("OpenLevel") == 17)
        // {
        //     MustPopCount = 9;
        // }
        // else if (PlayerPrefs.GetInt("OpenLevel") == 13)
        // {
        //     MustPopCount = 1;
        // }
        // else if (LevelData.mode == ModeGame.Vertical)
        // {
        //     MustPopCount = 11;
        // }
        // else
        // {
        //     MustPopCount = 11;
        // }
        //if( LevelData.mode == ModeGame.Vertical && TargetCounter == MustPopCount && GamePlay.Instance.GameStatus == GameState.Playing )
        //    GamePlay.Instance.GameStatus = GameState.Win;
        //else if( LevelData.mode == ModeGame.Rounded && TargetCounter >= 1 && GamePlay.Instance.GameStatus == GameState.WaitForStar )
        //    GamePlay.Instance.GameStatus = GameState.Win;
        //else if( LevelData.mode == ModeGame.Animals && TargetCounter >= TotalTargets && GamePlay.Instance.GameStatus == GameState.Playing )
        //    GamePlay.Instance.GameStatus = GameState.Win;
        //else if( LevelData.LimitAmount <= 0 && GamePlay.Instance.GameStatus == GameState.Playing && newBall == null )
        //    GamePlay.Instance.GameStatus = GameState.GameOver;

        // ProgressBarScript.Instance.UpdateDisplay( (float)score * 100f / ( (float)LevelData.star1 / ( ( LevelData.star1 * 100f / LevelData.star3 ) ) * 100f ) /100f );
        ProgressBarScript.Instance.UpdateDisplay( (float)score * 100f / ( (float)1000 / ( ( 1000 * 100f / 2000 ) ) * 100f ) / 100f );


    // update the number of stars the player has received
    bool gotAStar = score >= LevelData.star1;
    if ( score >= LevelData.star1)
    {
        stars = 1;
        starsObject[0].SetActive( true );
    }
    if( score >= LevelData.star2)
    {
        stars = 2;
        starsObject[1].SetActive( true );
    }
    if( score >= LevelData.star3)
    {
        stars = 3;
        starsObject[2].SetActive( true );
    }
    
    // bool gotAStar = score >= 1000;
    //     if ( score >= 500)
    // {
    //     stars = 1;
    //     starsObject[0].SetActive( true );
    // }
    // if( score >= 1500)
    // {
    //     stars = 2;
    //     starsObject[1].SetActive( true );
    // }
    // if( score >= 2000)
    // {
    //     stars = 3;
    //     starsObject[2].SetActive( true );
    // }

        if (LevelData.mode == ModeGame.Vertical && TargetCounter == MustPopCount && GamePlay.Instance.GameStatus == GameState.Playing)
        {
            GamePlay.Instance.GameStatus = GameState.Win;
            score = 0;
            _ComboCount = 0;
            mainscript.score = 0;
        }
        else if (LevelData.mode == ModeGame.Rounded && TargetCounter >= 1 && GamePlay.Instance.GameStatus == GameState.WaitForStar)
        {
            GamePlay.Instance.GameStatus = GameState.Win;
            score = 0;
            _ComboCount = 0;
            mainscript.score = 0;
        }
        else if (LevelData.mode == ModeGame.Animals && TargetCounter >= TotalTargets && GamePlay.Instance.GameStatus == GameState.Playing)
        {
            GamePlay.Instance.GameStatus = GameState.Win;
            score = 0;
            _ComboCount = 0;
            mainscript.score = 0;
        }
        else if (LevelData.LimitAmount <= 0 && !gotAStar && GamePlay.Instance.GameStatus == GameState.Playing && newBall == null)
        {
            GamePlay.Instance.GameStatus = GameState.Win;
            score = 0;
            _ComboCount = 0;
            mainscript.score = 0;
        }
        else if (LevelData.mode == ModeGame.Vertical && GamePlay.Instance.GameStatus == GameState.Playing && stars == 3)
        {
            GamePlay.Instance.GameStatus = GameState.Win;
            score = 0;
            _ComboCount = 0;
            mainscript.score = 0;
        }
        else if (LevelData.LimitAmount <= 0 && gotAStar && GamePlay.Instance.GameStatus == GameState.Playing && newBall == null) {
            GamePlay.Instance.GameStatus = GameState.Win;
            score = 0;
            _ComboCount = 0;
            mainscript.score = 0;
        }

        if (GamePlay.Instance.GameStatus == GameState.Win) {
            stars = 3;
            starsObject[0].SetActive( true );
            starsObject[1].SetActive( true );
            starsObject[2].SetActive( true );
            score = 0;
            mainscript.score = 0;
        }

    }

// Use OnApplicationPause instead of OnApplicationQuit for Android
void OnApplicationPause(bool pauseStatus){
    string playDates;
    string theDate = System.DateTime.Now.ToString ("yyyyMMdd");
    float timePlayed = Time.time;

    if (PlayerPrefs.HasKey (theDate)) {
        float timeAlreadyPlayed = PlayerPrefs.GetFloat (theDate);
        timePlayed += timeAlreadyPlayed;
    }

    PlayerPrefs.SetFloat (theDate, timePlayed);


    if (PlayerPrefs.HasKey ("PlayDates")) {
        playDates = PlayerPrefs.GetString ("PlayDates");

        if(!playDates.Contains(theDate))
            playDates += "," + theDate;

    } else {
        playDates = theDate;
    }

    PlayerPrefs.SetString("PlayDates", playDates);
}

IEnumerator getBallsForMesh()
{
    GameObject[] meshes = GameObject.FindGameObjectsWithTag("Mesh");
    int ballLayerMask = 1<<9;
    foreach(GameObject obj1 in meshes) {
        Collider2D[] fixedBalls = Physics2D.OverlapCircleAll(obj1.transform.position, 0.1f, ballLayerMask);
        foreach(Collider2D obj in fixedBalls) {
            obj1.GetComponent<Grid>().Busy = obj.gameObject;
            obj.GetComponent<bouncer>().offset = obj1.GetComponent<Grid>().offset;
        }
    }
    yield return new WaitForSeconds(0.2f);
}


public GameObject createCannonBall(Vector3 vector3)
{
    GameObject gm = GameObject.Find ("Creator");
    GameObject nextBall = gm.GetComponent<creatorBall>().createBall(vector3, BallColor.random, true);
    return nextBall;
}

public void connectNearBallsGlobal()
{
    ///connect near balls
    fixedBalls = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
    foreach(GameObject obj in fixedBalls) {
        if(obj.layer == BallLayer)
            obj.GetComponent<ball>().connectNearBalls();
    }
}

public void dropUp()
{
    if (!droppingDown)
    {
        creatorBall.AddMesh();
        droppingDown = true;
        GameObject Meshes = GameObject.Find("-Meshes");
        iTween.MoveAdd(Meshes, iTween.Hash("y", 0.5f, "time", 0.3, "easetype", iTween.EaseType.linear, "onComplete", "OnMoveFinished"));
    }
}

void OnMoveFinished()
{
    droppingDown = false;
}

public void dropDown()
{
    droppingDown = true;
    fixedBalls = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
    foreach(GameObject obj in fixedBalls)
    {
        if(obj.layer == BallLayer)
            obj.GetComponent<bouncer>().dropDown();
    }
    GameObject gm = GameObject.Find ("Creator");
    gm.GetComponent<creatorBall>().createRow(0);
}

public void explode(GameObject gameObject)
{
    //gameObject.GetComponent<Detonator>().Explode();
}

public IEnumerator clearDisconnectedBalls()
{
    mainscript.Instance.newBall2 = null;
    yield return new WaitForSeconds( Mathf.Clamp( (float)countOfPreparedToDestroy / 50, 0.6f, (float)countOfPreparedToDestroy / 50 ) );
    connectNearBallsGlobal();
    int willDestroy = 0;
    // destringAloneBall = true;
    Camera.main.GetComponent<mainscript>().arraycounter = 0;
    GameObject[] fixedBalls = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; // detect alone balls
    Camera.main.GetComponent<mainscript>().controlArray.Clear();
    foreach(GameObject obj in fixedBalls) {
        if(obj!=null) {
            if(obj.layer == BallLayer) {
                if(obj.GetComponent<ball>().nearBalls.Count>0) {
                    //      if(droppingDown) yield return new WaitForSeconds(1f);
                    yield return new WaitForEndOfFrame();
                    ArrayList b = new ArrayList();
                    obj.GetComponent<ball>().checkNearestBall(b);
                    if(b.Count >0 && BallWhiffed == false)
                    {
                        willDestroy++;
                        if ((PlayerPrefs.GetInt("OpenLevel") == 3 || PlayerPrefs.GetInt("OpenLevel") == 6 ||
                            PlayerPrefs.GetInt("OpenLevel") == 7 || PlayerPrefs.GetInt("OpenLevel") == 8 ||
                            PlayerPrefs.GetInt("OpenLevel") == 10 || PlayerPrefs.GetInt("OpenLevel") == 11 ||
                            PlayerPrefs.GetInt("OpenLevel") == 12 || PlayerPrefs.GetInt("OpenLevel") == 14 || 
                            PlayerPrefs.GetInt("OpenLevel") == 16 || PlayerPrefs.GetInt("OpenLevel") == 17 ||  
                            PlayerPrefs.GetInt("OpenLevel") == 18 ||  PlayerPrefs.GetInt("OpenLevel") == 19 ||  
                            PlayerPrefs.GetInt("OpenLevel") == 21 ||  PlayerPrefs.GetInt("OpenLevel") == 23) &&
                            (LevelData.mode != ModeGame.Vertical))
                        {
                            TargetCounter++;
                        }
                        //loop through array b, sum their total values (* some modifier if necessary) and add to point total
                        
                        destroy (b);
                    }
                }
            }
        }
    }
    // destringAloneBall = false;
    StartCoroutine(getBallsForMesh());
    droppingDown = false;

    if(LevelData.mode == ModeGame.Vertical)
        creatorBall.Instance.MoveLevelDown();
    else if( LevelData.mode == ModeGame.Animals )
        creatorBall.Instance.MoveLevelDown();
    else if( LevelData.mode == ModeGame.Rounded )
        CheckBallsBorderCross();

    yield return new WaitForSeconds( 0.0f );
    GetColorsInGame();
    mainscript.Instance.newBall = null;
    SetColorsForNewBall();
}

public void SetColorsForNewBall()
{
    GameObject ball = null;
    if( boxCatapult.GetComponent<Grid>().Busy != null && colorsDict.Count>0)
    {
        ball = boxCatapult.GetComponent<Grid>().Busy;
        BallColor color = ball.GetComponent<ColorBallScript>().mainColor;
        if( !colorsDict.ContainsValue( color ) )
        {
            ball.GetComponent<ColorBallScript>().SetColor( (BallColor)mainscript.colorsDict[Random.Range( 0, mainscript.colorsDict.Count )] );
        }
    }
}

public void GetColorsInGame()
{
    int i = 0;
    colorsDict.Clear();
    foreach( Transform item in Balls )
    {
        if( item.tag == "star" || item.tag == "empty" || item.tag == "Ball" ) continue;
        BallColor col = (BallColor)System.Enum.Parse( typeof( BallColor ), item.tag );
        if( !colorsDict.ContainsValue( col ) && (int)col <= (int) BallColor.random)
        {
            colorsDict.Add(i, col );
            i++;
        }
    }
}

public void CheckFreeStar()
{
    if( LevelData.mode != ModeGame.Rounded ) return;
    if(GamePlay.Instance.GameStatus == GameState.Playing)
        StartCoroutine( CheckFreeStarCor() );
}

IEnumerator CheckFreeStarCor()
{
    // yield return new WaitForSeconds( Mathf.Clamp( (float)countOfPreparedToDestroy / 100, 1.5f, (float)countOfPreparedToDestroy / 100 ) );
    GamePlay.Instance.GameStatus = GameState.WaitForStar;
    yield return new WaitForSeconds( 1.5f );
    bool finishGame = false;
    if( LevelData.mode == ModeGame.Rounded )
    {
        finishGame = true;

        GameObject balls = GameObject.Find( "-Ball" );
        foreach( Transform item in balls.transform )
        {
            if( item.tag != "Ball" && item.tag != "star" )
            {
                finishGame = false;
            }
        }
    }
    if( !finishGame )
    {
        GetColorsInGame();
        GamePlay.Instance.GameStatus = GameState.Playing;
    }
    else if( finishGame )
    {
        GamePlay.Instance.GameStatus = GameState.WaitForStar;

        GameObject star = GameObject.FindGameObjectWithTag( "star" );
        star.GetComponent<SpriteRenderer>().sortingLayerName = "UI layer";
        Vector3 targetPos = new Vector3( 2.3f, 6, 0 );
        mainscript.Instance.TargetCounter++;
        AnimationCurve curveX = new AnimationCurve( new Keyframe( 0, star.transform.position.x ), new Keyframe( 0.5f, targetPos.x ) );
        AnimationCurve curveY = new AnimationCurve( new Keyframe( 0, star.transform.position.y ), new Keyframe( 0.5f, targetPos.y ) );
        curveY.AddKey( 0.2f, star.transform.position.y - 1 );
        float startTime = Time.time;
        Vector3 startPos = star.transform.position;
        float distCovered = 0;
        while( distCovered < 0.6f )
        {
            distCovered = ( Time.time - startTime );
            star.transform.position = new Vector3( curveX.Evaluate( distCovered ), curveY.Evaluate( distCovered ), 0 );
            star.transform.Rotate( Vector3.back * 10 );
            yield return new WaitForEndOfFrame();
        }
            Destroy(star);
        }
}


void CheckBallsBorderCross()
{
    foreach( Transform item in Balls )
    {
        item.GetComponent<ball>().CheckBallCrossedBorder();
    }
}

public bool findInArray(ArrayList b, GameObject destObj)
{
    foreach(GameObject obj in b) {

        if(obj == destObj) return true;
    }
    return false;
}

// destroys all balls in the given list b
public void destroy( ArrayList b)
{
    Camera.main.GetComponent<mainscript>().bounceCounter = 0;
    //int scoreCounter = 0;
    //int rate = 0;
    bool hasTarget = false;
    foreach(GameObject obj in b) {
        if (obj.GetComponent<ball>().isTarget)
            hasTarget = true;
    }
    if (hasTarget)
    {
        return;
    }

    foreach (GameObject obj in b) {
        // if(obj.name.IndexOf("ball")==0) obj.layer = 0;
        if(!obj.GetComponent<ball>().Destroyed) {
            //if(scoreCounter > 3) {
            //    rate +=3;
            //    scoreCounter += rate;
            //}
            //scoreCounter++;
            // this is the function that causes the balls to fall away

            //figure out point functionality...

            obj.GetComponent<ball>().StartFall();
        }
    }
    // if this is a rounded level, check if the "star" (star) is free
    CheckFreeStar();
}

// this gets called when the "new sign" button is pressed
// it destroys the current ball, which indicates behind the scenes
// that a new ball should be created to replace it
public void GetNewBall()
{
    SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot( SoundBase.Instance.swish[0] );
    destroy(boxCatapult.GetComponent<Grid>().Busy);
    boxCatapult.GetComponent<Grid>().Busy = null;
}

// special flavor of destroy for the debugging function below
public void destroy( GameObject obj)
{
    if(obj.name.IndexOf("ball")==0) obj.layer = 0;
    Camera.main.GetComponent<mainscript>().bounceCounter = 0;
    //  obj.GetComponent<OTSprite>().collidable = false;
    //  Destroy(obj);
    obj.GetComponent<ball>().Destroyed = true;
    obj.GetComponent<ball>().growUp();
    Camera.main.GetComponent<mainscript>().explode(obj.gameObject);
}

// can be used as a debugging tool- hit "D" to destroy all the balls
public void destroyAllballs()
{
    foreach( Transform item in Balls )
    {
        if( item.tag != "star" )
        {
            destroy( item.gameObject );
        }
    }
    CheckFreeStar();
}
}
