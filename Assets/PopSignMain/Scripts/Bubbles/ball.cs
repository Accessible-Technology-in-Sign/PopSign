using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour
{
    public static int score = 0;
    public bool isTarget;
    public Vector3 target;
    Vector2 worldPos;
    
    public bool setTarget;
    public float startTime;
    public GameObject mesh;
    
    public bool findMesh;
    Vector3 dropTarget;
    public bool newBall;
    bool stoppedBall;
    private bool destroyed;
    public bool NotSorting;
    ArrayList fireballArray = new ArrayList();
    public ArrayList nearBalls = new ArrayList();
    GameObject Meshes;
    public int countNearBalls;
    float bottomBorder;
    float topBorder;
    float leftBorder;
    float rightBorder;
    bool isPaused;
    Vector3 meshPos;
    public bool falling;
    private bool fireBall;
    private static int fireworks;
    private int fireBallLimit = 10;
    private bool launched;
    private bool animStarted;
    private VideoManager sharedVideoManager;

    //this is true when the launched ball does not hit at least 2 balls of the same color as it
    public bool whiff;

    //Checks whether a ball at a particular location has been destroyed
    public bool Destroyed
    {
        get { return destroyed; }
        set
        {
            if (value)
            {
                // If it has been destroyed already, collision is disabled and it isn't rendered.
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;

            }
            destroyed = value;
        }
    }

    // Use this for initialization
    void Start() 
    {
        meshPos = new Vector3(-1000, -1000, -10);
        dropTarget = transform.position;
        Meshes = GameObject.Find("-Ball");
        launched = false;
        setTarget = false;
        newBall = true;

        //TODO: Why is it bottom border +20? Editing doesn't seem to change anything
        bottomBorder = Camera.main.GetComponent<mainscript>().bottomBorder;
        topBorder = Camera.main.GetComponent<mainscript>().topBorder;
        leftBorder = Camera.main.GetComponent<mainscript>().leftBorder;
        rightBorder = Camera.main.GetComponent<mainscript>().rightBorder;
        isPaused = Camera.main.GetComponent<mainscript>().isPaused;

        //POPSign using the gray bubble instead of colorful bubbles.
        GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<ColorBallScript>().sprites[6];
        sharedVideoManager = VideoManager.getVideoManager();
    }
    void Update() 
    {
        //Checks if current video is right video for ball
        //If ball has not been launched and target has not been set and no new ball is being swapped in and a current ball exists
        //and the game is currently in "play" mode or "wait for star" mode?
    
        if (launched && !gameObject.GetComponent<ball>().setTarget &&
            mainscript.Instance.newBall2 == null &&
            newBall && !Camera.main.GetComponent<mainscript>().gameOver &&
            (GamePlay.Instance.GameStatus == GameState.Playing ||
                GamePlay.Instance.GameStatus == GameState.WaitForStar)) {
    
            // GameObject tutorialText2 = GameObject.Find("rebound");
            // tutorialText2.SetActive(false);
        }

        if (!launched && !gameObject.GetComponent<ball>().setTarget &&
            mainscript.Instance.newBall2 == null &&
            newBall && !Camera.main.GetComponent<mainscript>().gameOver &&
            (GamePlay.Instance.GameStatus == GameState.Playing ||
                GamePlay.Instance.GameStatus == GameState.WaitForStar))
        {
            Video ballVideo = this.sharedVideoManager.getVideoByColor(gameObject.GetComponent<ColorBallScript>().mainColor);
            // If the current video doesn't exist or is not the video that matches the current ball, set it to the right video
            if (this.sharedVideoManager.curtVideo == null || this.sharedVideoManager.curtVideo.fileName != ballVideo.fileName)
            {
                this.sharedVideoManager.curtVideo = ballVideo;
                this.sharedVideoManager.shouldChangeVideo = true;
            }
        }

        // If user left clicks screen
        if (Input.GetMouseButtonUp(0))
        {
            GameObject ball = gameObject;
            //If the click has been released and the ball hasn't been launched yet
            //if (!ClickOnGUI(Input.mousePosition) && !launched &&
            //    !ball.GetComponent<ball>().setTarget && mainscript.Instance.newBall2 == null &&
            //    !Camera.main.GetComponent<mainscript>().gameOver &&
            //    (GamePlay.Instance.GameStatus == GameState.Playing ||
            //        GamePlay.Instance.GameStatus == GameState.WaitForStar))
            if (!launched && !ball.GetComponent<ball>().setTarget && mainscript.Instance.newBall2 == null &&
                !Camera.main.GetComponent<mainscript>().gameOver &&
                (GamePlay.Instance.GameStatus == GameState.Playing ||
                    GamePlay.Instance.GameStatus == GameState.WaitForStar))
            {
                //Get the position of the click
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPos = pos;
                //If the y position of the click is within 4 units of the bottom of the original lowest row of balls and you have control over the ball
                if (worldPos.y > -4f && worldPos.y < 4f && !mainscript.StopControl)
                {

                    //Once ball is launched, set color of ball to the color of its word.
                    int orginalColor = (int)ball.GetComponent<ColorBallScript>().mainColor;
                    GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<ColorBallScript>().sprites[orginalColor - 1];

                    //160-170 puts image of word on the launched ball
                    GameObject imageObject = new GameObject();
                    imageObject.transform.parent = ball.transform;

                    SpriteRenderer ballImage = imageObject.AddComponent<SpriteRenderer>();
                    // Consider the image size
                    ballImage.transform.localScale = new Vector3(0.2f, 0.2f, 0.0f);
                    ballImage.transform.localPosition = new Vector3(0f, 0f, 5.0f);
                    string imageName = this.sharedVideoManager.getVideoByColor(ball.GetComponent<ColorBallScript>().mainColor).imageName;
                    ballImage.sprite = (Sprite)Resources.Load(imageName, typeof(Sprite));
                    ballImage.sortingLayerName = "WordIconsLayer";
                    ballImage.sortingOrder = 2;

                    // If the ball is a fireball, disable collision.
                    if (!fireBall)
                    {
                        GetComponent<CircleCollider2D>().enabled = false;
                    }

                    // Launch the ball! Make the ball movement sound
                    target = worldPos;
                    setTarget = true;
                    startTime = Time.time;
                    dropTarget = transform.position;
                    mainscript.Instance.newBall = gameObject;
                    mainscript.Instance.newBall2 = gameObject;
                    GetComponent<Rigidbody2D>().AddForce(target - dropTarget, ForceMode2D.Force);
                    launched = true;
                    SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.swish[0]);

                    // Disappear the tutorial instructions
                    Camera.main.GetComponent<TutorialManager>().BallHit();
                    
                }
            }
        }
        //If the ball has not hit the target location and is still moving
        if (transform.position != target && setTarget && !stoppedBall && !isPaused && Camera.main.GetComponent<mainscript>().dropDownTime < Time.time)
        {
            float totalVelocity = Vector3.Magnitude(GetComponent<Rigidbody2D>().velocity);
            if (totalVelocity > 20)
            {
                float tooHard = totalVelocity / (20);
                GetComponent<Rigidbody2D>().velocity /= tooHard;

            }
            else if (totalVelocity < 15)
            {
                float tooSlowRate = totalVelocity / (15);
                if (tooSlowRate != 0)
                    GetComponent<Rigidbody2D>().velocity /= tooSlowRate;
            }

            if (GetComponent<Rigidbody2D>().velocity.y < 1.5f && GetComponent<Rigidbody2D>().velocity.y > 0)
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 1.7f);
        }


        if (setTarget)
            triggerEnter();

        // commenting this section out doesn't seem to do anything
        if ((transform.position.y <= -10 || transform.position.y >= 5) && fireBall && !Destroyed)
        {
            Debug.Log("transform.position.y: " + transform.position.y);
            mainscript.Instance.CheckFreeStar();
            setTarget = false;
            launched = false;
            DestroySingle(gameObject, 0.00001f);
            mainscript.Instance.flyingBall = gameObject;
        }
        if ((transform.position.y <= -10 || transform.position.y >= 5) && !Destroyed)
        {
            Debug.Log("transform.position.y: " + transform.position.y);
            setTarget = false;
            launched = false;
            DestroySingle(gameObject, 0.00001f);
            LevelData.LimitAmount--;
        }
    }

    //bool ClickOnGUI(Vector3 mousePos) // the problem
    //{
    //    UnityEngine.EventSystems.EventSystem ct = UnityEngine.EventSystems.EventSystem.current;

    //    if (ct.IsPointerOverGameObject())
    //        return true;
    //    else
    //        return false;
    //}


    void FixedUpdate() 
    {
        if (Camera.main.GetComponent<mainscript>().gameOver) return;

        if (stoppedBall)
        {
            transform.position = meshPos;
            stoppedBall = false;
            if (newBall)
            {
                newBall = false;
                // this is the layer for all the balls
                gameObject.layer = 9;
                Camera.main.GetComponent<mainscript>().flyingBall = gameObject;
                this.enabled = false;
            }
        }
    }

    public ArrayList union(ArrayList b, ArrayList b2) //this method puts the result of b union b2 in b2
    {
        foreach (GameObject obj in b)
        {
            if (!b2.Contains(obj))
            {
                b2.Add(obj);
            }
        }
        return b2;
    }

    public void checkNextNearestColor(ArrayList b)
    {
        Vector3 distEtalon = transform.localScale;
        int layerMask = 1 << LayerMask.NameToLayer("Ball");
        Collider2D[] meshes = Physics2D.OverlapCircleAll(transform.position, 1.0f, layerMask);
        foreach (Collider2D obj1 in meshes)
        {
            if (obj1.gameObject.tag == tag)
            {
                GameObject obj = obj1.gameObject;
                float distanceToBall = Vector3.Distance(transform.position, obj.transform.position);
                if (distanceToBall <= 1.0f)
                {
                    if (!b.Contains(obj))
                    {
                        b.Add(obj);
                        obj.GetComponent<bouncer>().checkNextNearestColor(b);
                    }
                }
            }
        }
    }

    // gets called every time a collision occurs
    // actually decides which balls to destroy
    public void checkNearestColor()
    {
        ArrayList ballsToClear = new ArrayList();
        // add flying ball to list of balls to clear
        ballsToClear.Add(gameObject);
        Vector3 distEtalon = transform.localScale;
        // find balls with the same color tag
        GameObject[] meshes = GameObject.FindGameObjectsWithTag(tag);

        // detect the same color balls
        foreach (GameObject obj in meshes)
        {
            float distanceToBall = Vector3.Distance(transform.position, obj.transform.position);
            if (distanceToBall <= 0.9f && distanceToBall > 0)
            {
                bool notIn = true;
                foreach (GameObject ball in ballsToClear)
                {
                    if (ball == obj)
                    {
                        notIn = false;
                    }
                }
                if (notIn)
                {
                    ballsToClear.Add(obj);
                    obj.GetComponent<bouncer>().checkNextNearestColor(ballsToClear);
                }
            }
        }

        mainscript.Instance.countOfPreparedToDestroy = ballsToClear.Count;

        // if we have a grouping of three or more bubbles, pop them!
        if (ballsToClear.Count >= 3)
        {   
            whiff = false;
            mainscript.Instance.ComboCount++;
            // score += ballsToClear.Count * 50;
            destroy(ballsToClear, 0.00001f);
            // mainscript.Score = score;
            //already should add this score???
            mainscript.Score += ballsToClear.Count * 50;
            mainscript.Instance.CheckFreeStar();
        } else {
            whiff = true;
        }

        if (ballsToClear.Count < 3)
        {
            Camera.main.GetComponent<mainscript>().bounceCounter++;
            mainscript.Instance.ComboCount = 0;
        }

        Camera.main.GetComponent<mainscript>().droppingDown = false;
        FindLight(gameObject);
    }

    // this function never gets called in this file, but it's called in Assets\PopSignMain\Scripts\Core\mainscript.cs(559,38)
    public void StartFall()
    {
        enabled = false;

        if (gameObject == null)
            return;
        if (mesh != null)
            mesh.GetComponent<Grid>().Busy = null;
        if (gameObject.GetComponent<Rigidbody2D>() == null)
            gameObject.AddComponent<Rigidbody2D>();

        // instantiate the little check icon if the cleared ball was on the top row
        // TODO: fix this so that it doesn't use the GameObject's position, use Grid or something instead
        // (currently they will sometimes display unevenly in the line)
        if (LevelData.mode == ModeGame.Vertical && isTarget)
            Instantiate(Resources.Load("Prefabs/TargetStar"), gameObject.transform.position, Quaternion.identity);
        else if (LevelData.mode == ModeGame.Animals && isTarget)
            StartCoroutine(FlyToTarget());

        setTarget = false;
        transform.SetParent(null);
        // layer 13 is called FallingBall
        gameObject.layer = 13;
        gameObject.tag = "Ball";

        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        // gameObject.GetComponent<Rigidbody2D>().fixedAngle = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity + new Vector2(Random.Range(-2, 2), 0);

        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
        gameObject.GetComponent<CircleCollider2D>().radius = 0.3f;

        GetComponent<ball>().falling = true;
        mainscript.Score += 50;
    }


    IEnumerator FlyToTarget()
    {
        Vector3 targetPos = new Vector3(2.3f, 6, 0);
        if (mainscript.Instance.TargetCounter1 < mainscript.Instance.TotalTargets)
            mainscript.Instance.TargetCounter1++;

        AnimationCurve curveX = new AnimationCurve(new Keyframe(0, transform.position.x), new Keyframe(0.5f, targetPos.x));
        AnimationCurve curveY = new AnimationCurve(new Keyframe(0, transform.position.y), new Keyframe(0.5f, targetPos.y));
        curveY.AddKey(0.2f, transform.position.y - 1);
        float startTime = Time.time;
        Vector3 startPos = transform.position;
        float distCovered = 0;
        while (distCovered < 0.6f)
        {
            distCovered = (Time.time - startTime);
            transform.position = new Vector3(curveX.Evaluate(distCovered), curveY.Evaluate(distCovered), 0);
            transform.Rotate(Vector3.back * 10);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    // the arraylist is updated to contain all the near balls, which is then passed to destroy()
    public bool checkNearestBall(ArrayList b)
    {
        if ((mainscript.Instance.TopBorder.transform.position.y - transform.position.y <= 0 && LevelData.mode != ModeGame.Rounded) || (LevelData.mode == ModeGame.Rounded && tag == "star"))
        {
            b.Clear();
            return true; // don't destroy
        }
        if (Camera.main.GetComponent<mainscript>().controlArray.Contains(gameObject))
        {
            b.Clear();
            return true; // don't destroy
        }
        b.Add(gameObject);
        foreach (GameObject obj in nearBalls)
        {
            if (obj != gameObject && obj != null)
            {
                if (obj.gameObject.layer == 9) // ball layer
                {
                    float distanceToBall = Vector3.Distance(transform.position, obj.transform.position);
                    if (distanceToBall <= 1.0f && distanceToBall > 0)
                    {
                        if (!b.Contains(obj.gameObject))
                        {
                            Camera.main.GetComponent<mainscript>().arraycounter++;
                            if (obj.GetComponent<ball>().checkNearestBall(b))
                                return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void connectNearBalls()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Ball");
        Collider2D[] fixedBalls = Physics2D.OverlapCircleAll(transform.position, 1.0f, layerMask);
        nearBalls.Clear();
        foreach (Collider2D obj in fixedBalls)
        {
            nearBalls.Add(obj.gameObject);
        }
        countNearBalls = nearBalls.Count;
    }

    // gets called once per ball launch
    IEnumerator pullToMesh(Transform otherBall = null)
    {
        GameObject busyMesh = null;
        float searchRadius = 0.2f;
        int meshLayerMask = 1 << 10;

        // while we still haven't figured out where in the mesh we're gonna put the flying ball
        while (findMesh)
        {
            // get all the fixed balls whose positions overlap with the center point of the flying ball
            Vector3 centerPoint = transform.position;
            Collider2D[] fixedBalls1 = Physics2D.OverlapCircleAll(centerPoint, 0.1f, meshLayerMask);

            foreach (Collider2D obj1 in fixedBalls1)
            {
                if (obj1.gameObject.GetComponent<Grid>() == null)
                {
                    DestroySingle(gameObject, 0.00001f);
                }
                // if the grid box is empty (busy does not contain a ball)
                else if (obj1.gameObject.GetComponent<Grid>().Busy == null)
                {
                    findMesh = false;
                    stoppedBall = true;
                    if (meshPos.y <= obj1.gameObject.transform.position.y)
                    {
                        meshPos = obj1.gameObject.transform.position;
                        busyMesh = obj1.gameObject;
                    }
                }
            }

            // if we still haven't found a place for the ball
            if (findMesh)
            {
                Collider2D[] fixedBalls = Physics2D.OverlapCircleAll(centerPoint, searchRadius, meshLayerMask);
                foreach (Collider2D obj in fixedBalls)
                {
                    if (obj.gameObject.GetComponent<Grid>() == null) DestroySingle(gameObject, 0.00001f);
                    else if (obj.gameObject.GetComponent<Grid>().Busy == null)
                    {
                        findMesh = false;
                        stoppedBall = true;
                        if (meshPos.y <= obj.gameObject.transform.position.y)
                        {
                            meshPos = obj.gameObject.transform.position;
                            busyMesh = obj.gameObject;
                        }
                    }
                }
            }

            // if we found a place in the mesh to stick the flying ball
            if (busyMesh != null)
            {
                busyMesh.GetComponent<Grid>().Busy = gameObject;
                gameObject.GetComponent<bouncer>().offset = busyMesh.GetComponent<Grid>().offset;
                /*
                throws an error that causes the star levels not to rotate
                if (LevelData.mode == ModeGame.Rounded)
                {
                    LockLevelRounded.Instance.Rotate(target, transform.position);
                }
                */
            }
            // add ball to mesh for fixed balls
            transform.parent = Meshes.transform;
            Destroy(GetComponent<Rigidbody2D>());
            yield return new WaitForFixedUpdate();
            dropTarget = transform.position;

            // if we still haven't found a place, increase our search radius
            if (findMesh) searchRadius += 0.2f;
            if (searchRadius > 0.6f) {
                DestroySingle(gameObject, 0.00001f);
                findMesh = false;
            }

            yield return new WaitForFixedUpdate();
        }

        // figure out which balls to pop?
        mainscript.Instance.connectNearBallsGlobal();

        // if we found a place for the ball, play the collision animation
        if (busyMesh != null)
        {
            Hashtable animTable = mainscript.Instance.animTable;
            animTable.Clear();
            PlayHitAnim(transform.position, animTable);
        }

        // turn off box colliders for each box in the grid now that our collision is done
        creatorBall.Instance.DisableGridColliders();

        yield return new WaitForSeconds(0.5f);
    }

    public void PlayHitAnim(Vector3 newBallPos, Hashtable animTable)
    {

        int layerMask = 1 << LayerMask.NameToLayer("Ball");
        Collider2D[] fixedBalls = Physics2D.OverlapCircleAll(transform.position, 0.5f, layerMask);
        float force = 0.15f;
        foreach (Collider2D obj in fixedBalls)
        {
            if (!animTable.ContainsKey(obj.gameObject) && obj.gameObject != gameObject && animTable.Count < 50)
                obj.GetComponent<ball>().PlayHitAnimCorStart(newBallPos, force, animTable);
        }
        if (fixedBalls.Length > 0 && !animTable.ContainsKey(gameObject))
            PlayHitAnimCorStart(fixedBalls[0].gameObject.transform.position, 0, animTable);
    }

    public void PlayHitAnimCorStart(Vector3 newBallPos, float force, Hashtable animTable)
    {
        if (!animStarted)
        {
            StartCoroutine(PlayHitAnimCor(newBallPos, force, animTable));
            PlayHitAnim(newBallPos, animTable);
        }
    }

    public IEnumerator PlayHitAnimCor(Vector3 newBallPos, float force, Hashtable animTable)
    {
        animStarted = true;
        animTable.Add(gameObject, gameObject);
        if (tag == "star") yield break;
        yield return new WaitForFixedUpdate();
        float dist = Vector3.Distance(transform.position, newBallPos);
        force = 1 / dist + force;
        newBallPos = transform.position - newBallPos;
        if (transform.parent == null)
        {
            animStarted = false;
            yield break;
        }
        newBallPos = Quaternion.AngleAxis(transform.parent.parent.rotation.eulerAngles.z, Vector3.back) * newBallPos;
        newBallPos = newBallPos.normalized;
        newBallPos = transform.localPosition + (newBallPos * force / 10);

        float startTime = Time.time;
        Vector3 startPos = transform.localPosition;
        float speed = force * 5;
        float distCovered = 0;
        while (distCovered < 1 && !float.IsNaN(newBallPos.x))
        {
            distCovered = (Time.time - startTime) * speed;
            if (this == null) yield break;
            if (falling)
            {
                yield break;
            }
            transform.localPosition = Vector3.Lerp(startPos, newBallPos, distCovered);
            yield return new WaitForEndOfFrame();
        }
        Vector3 lastPos = transform.localPosition;
        startTime = Time.time;
        distCovered = 0;
        while (distCovered < 1 && !float.IsNaN(newBallPos.x))
        {
            distCovered = (Time.time - startTime) * speed;
            if (this == null) yield break;
            if (falling)
            {
                yield break;
            }
            transform.localPosition = Vector3.Lerp(lastPos, startPos, distCovered);
            yield return new WaitForEndOfFrame();
        }
        transform.localPosition = startPos;
        animStarted = false;
    }

    public void FindLight(GameObject activatedByBall)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Ball");
        Collider2D[] fixedBalls = Physics2D.OverlapCircleAll(transform.position, 0.5f, layerMask);
        int i = 0;
        foreach (Collider2D obj in fixedBalls)
        {
            i++;
            if (i <= 10)
            {
                if ((obj.gameObject.tag == "light") && GamePlay.Instance.GameStatus == GameState.Playing)
                {
                    DestroySingle(obj.gameObject);
                    DestroySingle(activatedByBall);
                }
                else if ((obj.gameObject.tag == "cloud") && GamePlay.Instance.GameStatus == GameState.Playing)
                {
                    obj.GetComponent<ColorBallScript>().ChangeRandomColor();
                }

            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // stop
        if (other.gameObject.name.Contains("ball") && setTarget && name.IndexOf("bug") < 0)
        {
            if (!other.gameObject.GetComponent<ball>().enabled)
            {
                if ((other.gameObject.tag == "black_hole") && GamePlay.Instance.GameStatus == GameState.Playing)
                {
                    SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.black_hole);
                    DestroySingle(gameObject);
                }

                if (!fireBall)
                    StopBall(true, other.transform);
                else
                {
                    if (other.gameObject.tag.Contains("animal") || other.gameObject.tag.Contains("empty") || other.gameObject.tag.Contains("star")) return;
                    fireBallLimit--;
                    if (fireBallLimit > 0)
                        DestroySingle(other.gameObject, 0.000000000001f);
                    else
                    {
                        StopBall();
                        destroy(fireballArray, 0.000000000001f);
                    }
                }
            }
        }
        else if (other.gameObject.name.IndexOf("ball") == 0 && setTarget && name.IndexOf("bug") == 0)
        {
            if (other.gameObject.tag == gameObject.tag)
            {
                Destroy(other.gameObject);
            }
        }

        else if (other.gameObject.name == "TopBorder" && setTarget)
        {
            if (LevelData.mode == ModeGame.Vertical || LevelData.mode == ModeGame.Animals)
            {
                if (!findMesh)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    StopBall();

                    if (fireBall)
                    {
                        destroy(fireballArray, 0.000000000001f);
                    }
                }

            }
        }

    }

    void StopBall(bool pulltoMesh = true, Transform otherBall = null)
    {
        // mark that the ball has now officially been launched
        launched = true;
        // this line has to do with boosts and other frozen bubble features
        mainscript.lastBall = gameObject.transform.position;
        creatorBall.Instance.EnableGridColliders();
        target = Vector2.zero;
        setTarget = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        findMesh = true;
        GetComponent<BoxCollider2D>().offset = Vector2.zero;
        GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.5f);

        if (pulltoMesh)
            StartCoroutine(pullToMesh(otherBall));
    }
    public void CheckBallCrossedBorder()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, 1 << 14) != null || Physics2D.OverlapCircle(transform.position, 0.1f, 1 << 17) != null)
        {
            DestroySingle(gameObject, 0.00001f);
        }

    }

    void triggerEnter()
    {
        // If user clicks too close to the bottom of the screen, do not launch ball.
        if (transform.position.y < bottomBorder && target.y < -4f)
        {
            growUp();
            StopBall(false);
        }
        else
        {
            // If ball collides with leftBorder, make it bounce off by reversing its x velocity.
            if (transform.position.x <= leftBorder && target.x < 0)
            {
                target = new Vector2(target.x * -1, target.y);
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * -1, GetComponent<Rigidbody2D>().velocity.y);
            }
            // If ball collides with rightBorder, make it bounce off by reversing its x velocity.
            if (transform.position.x >= rightBorder && target.x > 0)
            {
                target = new Vector2(target.x * -1, target.y);
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * -1, GetComponent<Rigidbody2D>().velocity.y);
            }
            // If ball collides with topBorder, make it bounce off by reversing its y velocity.
            if (transform.position.y >= topBorder && target.y > 0 && LevelData.mode == ModeGame.Rounded)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y * -1);
            }

        }
    }

    public void destroy(ArrayList b, float speed = 0.1f)
    {
        //POPSign destroy the text on the bubble immediately
        foreach (GameObject obj in b)
        {
            foreach (Transform child in obj.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        StartCoroutine(DestroyCor(b, speed));
    }

    IEnumerator DestroyCor(ArrayList b, float speed = 0.1f)
    {
        ArrayList l = new ArrayList();
        foreach (GameObject obj in b)
        {
            l.Add(obj);
        }
        Camera.main.GetComponent<mainscript>().bounceCounter = 0;
        //int scoreCounter = 0;
        int soundPool = 0;
        foreach (GameObject obj in l)
        {
            if (obj == null) continue;
            if (obj.name.IndexOf("ball") == 0) obj.layer = 0;
            //if(obj2 != null)
            obj.GetComponent<ball>().growUp();
            soundPool++;
            GetComponent<Collider2D>().enabled = false;
            // if( scoreCounter > 3 )
            // {
            //   rate += 10;
            //   scoreCounter += rate;
            // }
            // scoreCounter += 10;
            if (b.Count > 10 && Random.Range(0, 10) > 5) mainscript.Instance.perfect.SetActive(true);
            obj.GetComponent<ball>().Destroyed = true;
            //		Destroy(obj);

            // Camera.main.GetComponent<mainscript>().explode( obj.gameObject );
            if (b.Count < 10 || soundPool % 20 == 0)
                yield return new WaitForSeconds(speed);

            //			Destroy(obj);
        }
        //  StartCoroutine( mainscript.Instance.clearDisconnectedBalls() );
    }

    void DestroySingle(GameObject obj, float speed = 0.1f)
    {
        Camera.main.GetComponent<mainscript>().bounceCounter = 0;
        int soundPool = 0;
        if (obj.name.IndexOf("ball") == 0) obj.layer = 0;
        obj.GetComponent<ball>().growUp();
        soundPool++;

        // if (scoreCounter > 3)
        // {
        //  rate += 10;
        //  scoreCounter += rate;
        // }
        //score += 10;
        obj.GetComponent<ball>().Destroyed = true;
    }

    public void SplashDestroy()
    {
        if (setTarget) mainscript.Instance.newBall2 = null;
        Destroy(gameObject);
    }

    public void growUp()
    {
        StartCoroutine(explode());
    }

    IEnumerator explode()
    {

        float startTime = Time.time;
        float endTime = Time.time + 0.1f;
        Vector3 tempPosition = transform.localScale;
        Vector3 targetPrepare = transform.localScale * 1.2f;

        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        while (!isPaused && endTime > Time.time)
        {
            //transform.position += targetPrepare * Time.deltaTime;
            transform.localScale = Vector3.Lerp(tempPosition, targetPrepare, (Time.time - startTime) * 10);
            //	transform.position = targetPrepare ;
            yield return new WaitForEndOfFrame();
        }
        //   yield return new WaitForSeconds(0.01f );
        GameObject prefab = Resources.Load("Particles/BubbleExplosion") as GameObject;
        GameObject explosion = (GameObject)Instantiate(prefab, gameObject.transform.position + Vector3.back * 20f, Quaternion.identity);
        if (mesh != null)
            explosion.transform.parent = mesh.transform;
        //  if( !isPaused )
        CheckNearCloud();
        if (LevelData.mode == ModeGame.Vertical && isTarget)
        {
            mainscript.Instance.TargetCounter++;
            Instantiate(Resources.Load("Prefabs/TargetStar"), gameObject.transform.position, Quaternion.identity);
        }
        else if (LevelData.mode == ModeGame.Animals && isTarget)
        {
            // Instantiate( Resources.Load( "Prefabs/TargetStar" ), gameObject.transform.position, Quaternion.identity );
        }
        Destroy(gameObject, 1);
    }

    void CheckNearCloud()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Ball");
        Collider2D[] meshes = Physics2D.OverlapCircleAll(transform.position, 1f, layerMask);
        foreach (Collider2D obj1 in meshes)
        {
            if (obj1.gameObject.tag == "cloud")
            {
                GameObject obj = obj1.gameObject;
                float distanceToBall = Vector3.Distance(transform.position, obj.transform.position);
                if (distanceToBall <= 1f)
                {
                    obj.GetComponent<ColorBallScript>().ChangeRandomColor();
                }
            }
        }
    }

    public void ShowFirework()
    {
        fireworks++;
        if (fireworks <= 2)
            SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.hit);
    }
}
