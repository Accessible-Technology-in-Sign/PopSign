using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class creatorBall : MonoBehaviour
{
public static creatorBall Instance;
public GameObject ballPrefab;
public GameObject bugPrefab;
public GameObject boxPrefab;
public static int columns = 11;
public static int rows = 70;
public static List<Vector2> grid = new List<Vector2>();
int lastRow;
float offsetStep = 0.33f;
GameObject Meshes;
[HideInInspector]
public List<GameObject> squares = new List<GameObject>();
int[] map;
private int maxCols;
private VideoManager sharedVideoManager;
string[] colors = {"Orange", "Red", "Yellow", "Rainbow", "Blue", "Green", "Pink", "Violet", "Brown", "Gray"};
int[] frameInts = {7, 3, 1, 4, 6, 11, 8, 10, 5, 2, 9, 12};
int[] frameBugInts = {5, 3, 1, 4, 10, 10, 8, 7, 4, 2, 9, 6};

// Use this for initialization
void Start()
{
    Instance = this;
    sharedVideoManager = VideoManager.getVideoManager();
    boxPrefab.transform.localScale = new Vector3( 0.67f, 0.58f, 1 );
    Meshes = GameObject.Find( "-Ball" );
    LoadLevel();
    if( LevelData.mode == ModeGame.Vertical || LevelData.mode == ModeGame.Animals )
        MoveLevelUp();
    else
    {
        GameObject.Find( "TopBorder" ).transform.parent = null;
        GameObject.Find( "TopBorder" ).GetComponent<SpriteRenderer>().enabled = false;
        GameObject ob = GameObject.Find( "-Meshes" );
        ob.transform.position += Vector3.up * 2.5f;
        ob.transform.position -= Vector3.left * -.5f;
        GamePlay.Instance.GameStatus = GameState.PreTutorial;
    }
    createMesh(); 
    LoadMap( LevelData.map );
    Camera.main.GetComponent<mainscript>().connectNearBallsGlobal();
    StartCoroutine( getBallsForMesh() );


}

public void LoadLevel()
{
    mainscript.Instance.currentLevel = PlayerPrefs.GetInt("OpenLevel");
    if (mainscript.Instance.currentLevel == 0)
        mainscript.Instance.currentLevel = 1;
    LoadDataFromLocal(mainscript.Instance.currentLevel);

}

public bool LoadDataFromLocal(int currentLevel)
{
    //Read data from text file
    TextAsset mapText = Resources.Load("Levels/" + currentLevel) as TextAsset;
    if (mapText == null)
    {
        mapText = Resources.Load("Levels/" + currentLevel) as TextAsset;
    }
    ProcessGameDataFromString(mapText.text);
    return true;
}

void ProcessGameDataFromString(string mapText)
{
    string[] lines = mapText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
    LevelData.colorsDict.Clear();
    int mapLine = 0;
    int key = 0;
    foreach (string line in lines)
    {
        if (line.StartsWith("MODE "))
        {
            string modeString = line.Replace("MODE", string.Empty).Trim();
            LevelData.mode = (ModeGame)int.Parse(modeString);
        }
        else if (line.StartsWith("SIZE "))
        {
            string blocksString = line.Replace("SIZE", string.Empty).Trim();
            string[] sizes = blocksString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            maxCols = int.Parse(sizes[0]);
        }
        else if (line.StartsWith("LIMIT "))
        {
            string blocksString = line.Replace("LIMIT", string.Empty).Trim();
            string[] sizes = blocksString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            LevelData.LimitAmount = int.Parse(sizes[1]);

        }
        else if (line.StartsWith("COLOR LIMIT "))
        {
            string blocksString = line.Replace("COLOR LIMIT", string.Empty).Trim();
            LevelData.colors = int.Parse(blocksString);
        }
        else if (line.StartsWith("STARS "))
        {
            string blocksString = line.Replace("STARS", string.Empty).Trim();
            string[] blocksNumbers = blocksString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            LevelData.star1 = int.Parse(blocksNumbers[0]);
            LevelData.star2 = int.Parse(blocksNumbers[1]);
            LevelData.star3 = int.Parse(blocksNumbers[2]);
        }
        else
        { //Maps
         //Split lines again to get map numbers
            string[] st = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < st.Length; i++)
            {
                int value = int.Parse(st[i][0].ToString());
                if (!LevelData.colorsDict.ContainsValue((BallColor)value) && value > 0 && value < (int)BallColor.random)
                {
                    LevelData.colorsDict.Add(key, (BallColor)value);
                    key++;

                }

                LevelData.map[mapLine * maxCols + i] = int.Parse(st[i][0].ToString());
            }
            mapLine++;
        }
    }
    //random colors
    if (LevelData.colorsDict.Count == 0)
    {
        //add constant colors
        LevelData.colorsDict.Add(0, BallColor.yellow);
        LevelData.colorsDict.Add(1, BallColor.red);

        //add random colors
        List<BallColor> randomList = new List<BallColor>();
        randomList.Add(BallColor.blue);
        randomList.Add(BallColor.green);
        randomList.Add(BallColor.violet);
        for (int i = 0; i < LevelData.colors - 2; i++)
        {
            BallColor randCol = BallColor.yellow;
            while (LevelData.colorsDict.ContainsValue(randCol))
            {
                randCol = randomList[UnityEngine.Random.Range(0, randomList.Count)];
            }
            LevelData.colorsDict.Add(2 + i, randCol);

        }

    }

}

public void LoadMap( int[] pMap )
{
    map = pMap;
    int roww = 0;
    for( int i = 0; i < rows; i++ )
    {
        for( int j = 0; j < columns; j++ )
        {
            int mapValue = map[i * columns + j];
            if( mapValue > 0 )
            {
                roww = i;
                if (LevelData.mode == ModeGame.Rounded) roww = i +4;
                createBall( GetSquare(roww, j ).transform.position, (BallColor)mapValue, false, i );

            }
            else if( mapValue == 0 && LevelData.mode == ModeGame.Vertical && i == 0 )
            {
                Instantiate( Resources.Load( "Prefabs/TargetStar" ), GetSquare( i, j ).transform.position, Quaternion.identity );
            }
        }
    }
}

private void MoveLevelUp()
{
    StartCoroutine( MoveUpDownCor() );
}

IEnumerator MoveUpDownCor( bool inGameCheck = false )
{
    yield return new WaitForSeconds( 0.1f );
    if( !inGameCheck )
        GamePlay.Instance.GameStatus = GameState.BlockedGame;
    bool up = false;
    List<float> table = new List<float>();
    float lineY = -2.5f;
    Transform bubbles = GameObject.Find( "-Ball" ).transform;
    int i = 0;
    foreach( Transform item in bubbles )
    {
        if( !inGameCheck )
        {
            if( item.position.y < lineY )
            {
                table.Add( item.position.y );
            }
        }
        else if( !item.GetComponent<ball>().Destroyed )
        {
            if( inGameCheck)//item.position.y > lineY && mainscript.Instance.TopBorder.transform.position.y > 5.5f )
            {
                table.Add( item.position.y );
            }
            else if( item.position.y < lineY + 1f )
            {
                table.Add( item.position.y );
                up = true;
            }
        }
        i++;
    }


    if( table.Count > 0 )
    {
        if (!inGameCheck) {
            if( up ) AddMesh();

            float targetY = 0;
            table.Sort();
            if( !inGameCheck ) targetY = lineY - table[0] + 2.5f;
            else targetY = lineY - table[0] + 1.5f;
            GameObject Meshes = GameObject.Find( "-Meshes" );
            Vector3 targetPos = Meshes.transform.position + Vector3.up * targetY;
            float startTime = Time.time;
            Vector3 startPos = Meshes.transform.position;
            float speed = 0.5f;
            float distCovered = 0;
            while( distCovered < 1 )
            {
                speed += Time.deltaTime / 1.5f;
                distCovered = ( Time.time - startTime ) / speed;
                Meshes.transform.position = Vector3.Lerp( startPos, targetPos, distCovered );
                yield return new WaitForEndOfFrame();
                if( startPos.y > targetPos.y )
                {
                    if( mainscript.Instance.TopBorder.transform.position.y <= 5 && inGameCheck ) break;
                }
            }
        } else {
            if( up ) AddMesh();

            float targetY = 0;
            table.Sort();
            if( !inGameCheck ) targetY = lineY - table[0] + 2.5f;
            else targetY = lineY - table[0] + 1.5f;
            GameObject Meshes = GameObject.Find( "-Meshes" );
            Vector3 targetPos = Meshes.transform.position + Vector3.up * targetY;
            float startTime = Time.time;
            Vector3 startPos = Meshes.transform.position;
            float speed = 0.5f;
            float distCovered = 0;
            while( distCovered < 1 )
            {
                speed += Time.deltaTime / 1.5f;
                distCovered = ( Time.time - startTime ) / speed;
                Meshes.transform.position = Vector3.Lerp( startPos, targetPos, distCovered );
                yield return new WaitForEndOfFrame();
                if( startPos.y > targetPos.y && (PlayerPrefs.GetInt("OpenLevel") == 1 || PlayerPrefs.GetInt("OpenLevel") == 2 ))
                {
                    if( mainscript.Instance.TopBorder.transform.position.y <= 5) break;
                }
            }
        }
    }

    if( GamePlay.Instance.GameStatus == GameState.BlockedGame )
        GamePlay.Instance.GameStatus = GameState.PreTutorial;
    else if( GamePlay.Instance.GameStatus != GameState.GameOver && GamePlay.Instance.GameStatus != GameState.Win )
        GamePlay.Instance.GameStatus = GameState.Playing;


}

public void MoveLevelDown()
{
    StartCoroutine( MoveUpDownCor( true ) );
}

private bool BubbleBelowLine()
{
    throw new System.NotImplementedException();
}


IEnumerator getBallsForMesh()
{
    int ballLayerMask = 1 << 9;
    float searchRadius = 0.2f;
    GameObject[] meshes = GameObject.FindGameObjectsWithTag( "Mesh" );
    foreach( GameObject obj1 in meshes )
    {
        Collider2D[] fixedBalls = Physics2D.OverlapCircleAll( obj1.transform.position, searchRadius, ballLayerMask);
        foreach( Collider2D obj in fixedBalls )
        {
            obj1.GetComponent<Grid>().Busy = obj.gameObject;
        }
    }
    yield return new WaitForSeconds( 0.5f );
}

public void EnableGridColliders()
{
    foreach( GameObject item in squares )
    {
        item.GetComponent<BoxCollider2D>().enabled = true;
    }
}
public void DisableGridColliders()
{
    foreach( GameObject item in squares )
    {
        item.GetComponent<BoxCollider2D>().enabled = false;
    }
}

public void createRow( int j )
{
    float offset = 0;
    for( int i = 0; i < columns; i++ )
    {
        if( j % 2 == 0 ) offset = 0; else offset = offsetStep;
        Vector3 v = new Vector3( transform.position.x + i * boxPrefab.transform.localScale.x + offset, transform.position.y - j * boxPrefab.transform.localScale.y, transform.position.z );
        createBall( v );
    }
}

public GameObject createBall( Vector3 vec, BallColor color = BallColor.random, bool newball = false, int row = 1 )
{
    GameObject b = null;
    List<BallColor> colors = new List<BallColor>();

    for( int i = 1; i < System.Enum.GetValues( typeof( BallColor ) ).Length; i++ )
    {
        colors.Add( (BallColor)i );
    }

    if( color == BallColor.random )
        color = (BallColor)LevelData.colorsDict[UnityEngine.Random.Range( 0, LevelData.colorsDict.Count )];
    if( newball && mainscript.colorsDict.Count > 0 )
    {
        if( GamePlay.Instance.GameStatus == GameState.Playing )
        {
            mainscript.Instance.GetColorsInGame();
            color = (BallColor)mainscript.colorsDict[UnityEngine.Random.Range( 0, mainscript.colorsDict.Count )];
        }
        else
            color = (BallColor)LevelData.colorsDict[UnityEngine.Random.Range( 0, LevelData.colorsDict.Count )];

    }

    b = Instantiate( ballPrefab, transform.position, transform.rotation ) as GameObject;
    b.transform.position = new Vector3( vec.x, vec.y, ballPrefab.transform.position.z );
    // b.transform.Rotate( new Vector3( 0f, 180f, 0f ) );
    b.GetComponent<ColorBallScript>().SetColor( color );
    b.transform.parent = Meshes.transform;
    b.tag = "" + color;

    GameObject[] fixedBalls = GameObject.FindObjectsOfType( typeof( GameObject ) ) as GameObject[];
    b.name = b.name + fixedBalls.Length.ToString();
    if( newball )
    {
        b.gameObject.layer = 17;
        b.transform.parent = Camera.main.transform;
        Rigidbody2D rig = b.AddComponent<Rigidbody2D>();
        // b.collider2D.isTrigger = false;
        //   rig.fixedAngle = true;
        b.GetComponent<CircleCollider2D>().enabled = false;
        rig.gravityScale = 0;
        if (GamePlay.Instance.GameStatus == GameState.Playing) {
            b.GetComponent<Animation> ().Play ();
        }
    }
    else
    {
        b.GetComponent<ball>().enabled = false;
        if( LevelData.mode == ModeGame.Vertical && row == 0 )
            b.GetComponent<ball>().isTarget = true;
        b.GetComponent<BoxCollider2D>().offset = Vector2.zero;
        b.GetComponent<BoxCollider2D>().size = new Vector2( 0.5f, 0.5f );

        //POPSign add image to the bubbles
        GameObject imageObject = new GameObject();
        imageObject.transform.parent = b.transform;
        SpriteRenderer ballImage = imageObject.AddComponent<SpriteRenderer> ();
        if (b.GetComponent<ColorBallScript> ().mainColor != BallColor.star) {
            string imageName = sharedVideoManager.getVideoByColor (b.GetComponent<ColorBallScript> ().mainColor).imageName;
            ballImage.sprite = (Sprite)Resources.Load(imageName, typeof(Sprite));
            ballImage.sortingLayerName = "WordIconsLayer";
            ballImage.sortingOrder = 2;

            // Consider the image size
            ballImage.transform.localScale = new Vector3(0.2f, 0.2f, 0.0f);
            ballImage.transform.localPosition = new Vector3(0f, 0f, 5.0f);
        }

    }
    return b.gameObject;
}

public void createMesh()
{

    GameObject Meshes = GameObject.Find( "-Meshes" );
    float offset = 0;

    for( int j = 0; j < rows + 1; j++ )
    {
        for( int i = 0; i < columns; i++ )
        {
            if( j % 2 == 0 ) offset = 0; else offset = offsetStep;
            GameObject b = Instantiate( boxPrefab, transform.position, transform.rotation ) as GameObject;
            Vector3 v = new Vector3( transform.position.x + i * b.transform.localScale.x + offset, transform.position.y - j * b.transform.localScale.y, transform.position.z );
            b.transform.parent = Meshes.transform;
            b.transform.localPosition = v;
            GameObject[] fixedBalls = GameObject.FindGameObjectsWithTag( "Mesh" );
            b.name = b.name + fixedBalls.Length.ToString();
            b.GetComponent<Grid>().offset = offset;
            squares.Add( b );
            lastRow = j;
        }
    }
    creatorBall.Instance.DisableGridColliders();

}

public void AddMesh()
{
    GameObject Meshes = GameObject.Find( "-Meshes" );
    float offset = 0;
    int j = lastRow + 1;
    for( int i = 0; i < columns; i++ )
    {
        if( j % 2 == 0 ) offset = 0; else offset = offsetStep;
        GameObject b = Instantiate( boxPrefab, transform.position, transform.rotation ) as GameObject;
        Vector3 v = new Vector3( transform.position.x + i * b.transform.localScale.x + offset, transform.position.y - j * b.transform.localScale.y, transform.position.z );
        b.transform.parent = Meshes.transform;
        b.transform.position = v;
        GameObject[] fixedBalls = GameObject.FindGameObjectsWithTag( "Mesh" );
        b.name = b.name + fixedBalls.Length.ToString();
        b.GetComponent<Grid>().offset = offset;
        squares.Add( b );
    }
    lastRow = j;

}

public GameObject GetSquare( int row, int col )
{
    return squares[row * columns + col];
}


}
