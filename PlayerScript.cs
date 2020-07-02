using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerScript : MonoBehaviour
{
    public GameScript gameScript;
    public GameObject grid, holdObject, ghostTetrominoObject, nextObject, lose, labelBackground, player, gameUI, pauseMenu, pauseLabelBackground;
    bool initialMove, playing, hold, softDropping, lost, restart, paused, pauseSelection, hardDropped, menuMove, initialMenuMove;
    int choice, nextChoice, holdChoice, level, linesCleared;
    public float gridTop, gridBottom, gridLeft, gridRight, xStart, yStart, timeTillMove, timeTillDrop, dropTimer, timeTillRespawn, menuTimer, menuTimerCount;
    public AudioSource click, clear;
    public MainMenuScript ui;
    uint points;
    public TextMeshProUGUI pointsText, linesClearedText, levelText, yes, mainMenu, yesPause, mainMenuPause;
    float horizontal, vertical;
    // Use this for initialization
    private void Awake()
    {
        BoxCollider2D bounds = grid.GetComponent<BoxCollider2D>();
        float top = bounds.size.y / 2f;
        float right = bounds.size.x / 2f;
        gridTop = grid.transform.position.y + top;
        gridBottom = grid.transform.position.y - top;
        gridLeft = grid.transform.position.x - right;
        gridRight = grid.transform.position.x + right;
        Destroy(bounds);
        //yStart = 0.855f;
        yStart = 0.765f;
        dropTimer = 0.0f;
        horizontal = 0.0f;
        vertical = 0.0f;
        timeTillRespawn = 0.6f;
        menuTimerCount = 0.3f;
    }
    private void Start()
    {
        gameScript = GameObject.FindGameObjectWithTag("Game").GetComponent<GameScript>();
    }
    // Update is called once per frame
    void Update()
    {
        if (lost && (Input.GetButtonDown("KeyReturn") || Input.GetButtonDown("ControllerSubmit")))
        {
            if (ui.soundOn) ui.beepSource.Play();
            for (int i = grid.transform.childCount - 1; i >= 0; --i)
            {
                Transform child = grid.transform.GetChild(i);
                Destroy(child.gameObject);
                child.SetParent(null);
            }
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                Transform child = transform.GetChild(i);
                Destroy(child.gameObject);
                child.SetParent(null);
            }
            holdObject.GetComponent<SpriteRenderer>().sprite = null;
            lose.SetActive(false);
            labelBackground.transform.localPosition = new Vector2(-106.0f, labelBackground.transform.localPosition.y);
            yes.color = Color.black;
            mainMenu.color = Color.white;
            if (!restart)
            {
                ui.gameObject.SetActive(true);
                player.SetActive(false);
                gameUI.SetActive(false);
            }
            else Begin();
            return;
        }
        if (lost)
        {
            if (!menuMove && (Input.GetButton("KeyRight") || Input.GetButton("KeyLeft") || Input.GetAxis("ControllerHorizontal") != 0))
            {
                menuTimer += Time.deltaTime;
                if (menuTimer >= menuTimerCount) menuMove = true;
                if (!initialMenuMove) initialMenuMove = true;
                else return;
            }
            else if(!Input.GetButton("KeyRight") && !Input.GetButton("KeyLeft") && Input.GetAxis("ControllerHorizontal") == 0)
            {
                menuMove = false;
                initialMenuMove = false;
                menuTimer = 0.0f;
                return;
            }
            if (ui.soundOn) ui.beepSource.Play();
            if (restart)
            {
                restart = false;
                labelBackground.transform.localPosition = new Vector2(97.0f, labelBackground.transform.localPosition.y);
                yes.color = Color.white;
                mainMenu.color = Color.black;
            }
            else
            {
                restart = true;
                labelBackground.transform.localPosition = new Vector2(-106.0f, labelBackground.transform.localPosition.y);
                yes.color = Color.black;
                mainMenu.color = Color.white;
            }
        }
        if (Input.GetButtonDown("KeyPause") || Input.GetButtonDown("ControllerPause"))
        {
            playing = !playing;
            paused = !paused;
            pauseSelection = false;
            if (paused) pauseMenu.SetActive(true);
            else pauseMenu.SetActive(false);
            ui.beepSource.Play();
            pauseLabelBackground.transform.localPosition = new Vector2(-106.0f, pauseLabelBackground.transform.localPosition.y);
            yesPause.color = Color.black;
            mainMenuPause.color = Color.white;
        }
        //main menu
        if (paused && pauseSelection && (Input.GetButtonDown("KeyReturn") || Input.GetButtonDown("ControllerSubmit")))
        {
            for (int i = grid.transform.childCount - 1; i >= 0; --i)
            {
                Transform child = grid.transform.GetChild(i);
                Destroy(child.gameObject);
                child.SetParent(null);
            }
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                Transform child = transform.GetChild(i);
                Destroy(child.gameObject);
                child.SetParent(null);
            }
            pauseSelection = false;
            holdObject.GetComponent<SpriteRenderer>().sprite = null;
            pauseMenu.SetActive(false);
            ui.gameObject.SetActive(true);
            player.SetActive(false);
            gameUI.SetActive(false);
            pauseLabelBackground.transform.localPosition = new Vector2(-106.0f, pauseLabelBackground.transform.localPosition.y);
            yesPause.color = Color.black;
            mainMenuPause.color = Color.white;
        }
        //continue
        if (paused && !pauseSelection && (Input.GetButtonDown("KeyReturn") || Input.GetButtonDown("ControllerSubmit")))
        {
            playing = true;
            paused = false;
            pauseMenu.SetActive(false);
            pauseSelection = false;
            pauseLabelBackground.transform.localPosition = new Vector2(-106.0f, pauseLabelBackground.transform.localPosition.y);
            yesPause.color = Color.black;
            mainMenuPause.color = Color.white;
            return;
        }
        if (paused)
        {
            if (!menuMove && (Input.GetButton("KeyRight") || Input.GetButton("KeyLeft") || Input.GetAxis("ControllerHorizontal") != 0))
            {
                menuTimer += Time.deltaTime;
                if (menuTimer >= menuTimerCount) menuMove = true;
                if (!initialMenuMove) initialMenuMove = true;
                else return;
            }
            else if (!Input.GetButton("KeyRight") && !Input.GetButton("KeyLeft") && Input.GetAxis("ControllerHorizontal") == 0)
            {
                menuMove = false;
                initialMenuMove = false;
                menuTimer = 0.0f;
                return;
            }
            ui.beepSource.Play();
            if (!pauseSelection)
            {
                pauseSelection = true;
                pauseLabelBackground.transform.localPosition = new Vector2(97.0f, pauseLabelBackground.transform.localPosition.y);
                yesPause.color = Color.white;
                mainMenuPause.color = Color.black;
            }
            else
            {
                pauseSelection = false;
                pauseLabelBackground.transform.localPosition = new Vector2(-106.0f, pauseLabelBackground.transform.localPosition.y);
                yesPause.color = Color.black;
                mainMenuPause.color = Color.white;
            }
        }
        
        if (!playing) return;
        bool emptyBelow = CheckEmptyBelow();
        bool emptyLeft = CheckEmptyLeft();
        bool emptyRight = CheckEmptyRight();
		if(((Input.GetButton("KeyLeft") && !Input.GetButton("KeyRight")) || Input.GetAxis("ControllerHorizontal") < 0) && emptyLeft)
        {
            if (!initialMove)
            {
                initialMove = true;
                MovePlayerLeft();
                if (ui.soundOn) click.Play();
            }
            else if (timeTillMove >= 0) timeTillMove -= Time.deltaTime;
            else
            {
                MovePlayerLeft();
                timeTillMove = gameScript.fastMove;
                if (ui.soundOn) click.Play();
            }
        }
        if(Input.GetButtonUp("KeyLeft") || (Input.GetAxis("ControllerHorizontal") == 0 && horizontal != 0))
        {
            timeTillMove = gameScript.move;
            initialMove = false;
        }
        if (((Input.GetButton("KeyRight") && !Input.GetButton("KeyLeft")) || (Input.GetAxis("ControllerHorizontal") > 0)) && emptyRight)
        {
            if (!initialMove)
            {
                initialMove = true;
                MovePlayerRight();
                if (ui.soundOn) click.Play();
            }
            else if (timeTillMove >= 0) timeTillMove -= Time.deltaTime;
            else
            {
                MovePlayerRight();
                timeTillMove = gameScript.fastMove;
                if (ui.soundOn) click.Play();
            }
        }
        if (Input.GetButtonUp("KeyRight") || (Input.GetAxis("ControllerHorizontal") == 0 && horizontal != 0))
        {
            timeTillMove = gameScript.move;
            initialMove = false;
        }
        if (Input.GetButtonDown("KeyUp") || Input.GetButtonDown("ControllerBack")) RotatePlayer(-90.0f);
        if(Input.GetButtonDown("KeyZ") || Input.GetButtonDown("ControllerSubmit")) RotatePlayer(90.0f);
        if((Input.GetButton("KeyDown") || Input.GetAxis("ControllerVertical") < 0) && emptyBelow && !softDropping)
        {
            dropTimer /= gameScript.softDropRatio;
            timeTillDrop = dropTimer;
            softDropping = true;
        }
        if (((Input.GetButtonUp("KeyDown") || (Input.GetAxis("ControllerVertical") == 0 && vertical != 0)) 
            && softDropping) || (softDropping && !emptyBelow))
        {
            dropTimer *= gameScript.softDropRatio;
            timeTillDrop = dropTimer;
            softDropping = false;
        }
        if (ui.soundOn && softDropping && !click.isPlaying) click.Play();
        if (!hardDropped && (Input.GetButtonDown("KeyReturn") || Input.GetAxis("ControllerVertical") > 0))
        {
            HardDrop();
            hardDropped = true;
        }
        if (hardDropped && (Input.GetButtonUp("KeyReturn") || Input.GetAxis("ControllerVertical") == 0)) hardDropped = false;
        if ((Input.GetButtonDown("KeyC") || Input.GetButtonDown("ControllerHold")) && !hold) Hold();
        if(timeTillDrop > 0) timeTillDrop -= Time.deltaTime;
        else
        {
            if (emptyBelow) DropPlayer();
            else
            {
                timeTillRespawn -= Time.deltaTime;
                if (timeTillRespawn <= 0) Respawn();
            }
        }
        pointsText.text = points.ToString();
        if (Input.GetAxis("ControllerHorizontal") != 0) horizontal = Input.GetAxis("ControllerHorizontal");
        if (Input.GetAxis("ControllerVertical") != 0) vertical = Input.GetAxis("ControllerVertical");
    }
    public void Begin()
    {
        timeTillMove = gameScript.move;
        timeTillDrop = dropTimer;
        playing = true;
        choice = -1;
        nextChoice = -1;
        holdChoice = -1;
        initialMove = false;
        softDropping = false;
        paused = false;
        points = 0;
        level = ui.level;
        levelText.text = level.ToString();
        lost = false;
        restart = true;
        pauseSelection = false;
        hardDropped = false;
        menuMove = false;
        initialMenuMove = false;
        menuTimer = 0.0f;
        linesCleared = 0;
        linesClearedText.text = linesCleared.ToString();
        pauseLabelBackground.transform.localPosition = new Vector2(-106.0f, -182.0f);
        mainMenuPause.color = Color.white;
        yesPause.color = Color.black;
        pointsText.text = points.ToString();
        dropTimer = 0.6f - (gameScript.speedUp * (ui.level - 1));
        xStart = grid.transform.position.x - (gameScript.shiftVal / 2.0f);
        labelBackground.transform.localPosition = new Vector2(-106.0f, labelBackground.transform.localPosition.y);
        SpawnTetromino();
    }
    public bool CheckEmptyAbove()
    {
        foreach (Transform child in transform)
        {
            if ((child.position.y + gameScript.shiftVal) > gridTop) return false;
            RaycastHit2D hit = Physics2D.Raycast(child.position, Vector2.up, gameScript.shiftVal, gameScript.gridPieceLayerMask);
            if (hit.collider != null) return false;
        }
        return true;
    }
    public bool CheckEmptyBelow()
    {
        foreach (Transform child in transform)
        {
            if ((child.position.y - gameScript.shiftVal) < gridBottom) return false;
            //make sure to only check for tetrominos
            RaycastHit2D hit = Physics2D.Raycast(child.position, Vector2.down, gameScript.shiftVal, gameScript.gridPieceLayerMask);
            if (hit.collider != null) return false;
        }
        return true;
    }
    public bool CheckEmptyLeft()
    {
        foreach (Transform child in transform)
        {
            if ((child.position.x - gameScript.shiftVal) < gridLeft) return false;
            //make sure to only check for tetrominos
            RaycastHit2D hit = Physics2D.Raycast(child.position, Vector2.left, gameScript.shiftVal, gameScript.gridPieceLayerMask);
            if (hit.collider != null) return false;
        }
        return true;
    }
    public bool CheckEmptyRight()
    {
        foreach (Transform child in transform)
        {
            if ((child.position.x + gameScript.shiftVal) > gridRight) return false;
            //make sure to only check for tetrominos
            RaycastHit2D hit = Physics2D.Raycast(child.position, Vector2.right, gameScript.shiftVal, gameScript.gridPieceLayerMask);
            if (hit.collider != null) return false;
        }
        return true;
    }
    public void DropPlayer()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - gameScript.shiftVal);
        timeTillDrop = dropTimer;
        if (softDropping) ++points;
    }
    public void MovePlayerLeft()
    {
        transform.position = new Vector2(transform.position.x - gameScript.shiftVal, transform.position.y);
        PlaceGhostTetromino();
    }
    public void MovePlayerRight()
    {
        transform.position = new Vector2(transform.position.x + gameScript.shiftVal, transform.position.y);
        PlaceGhostTetromino();
    }
    void SpawnTetromino()
    {
        HandleChoice();
        if (choice != (int)Tetromino.LINE) transform.position = new Vector2(xStart, yStart);
        else transform.position = new Vector2(xStart + gameScript.shiftVal, yStart);
        transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        float[] positions = GetPiecePositions();
        for (int i = 0; i < positions.Length; i += 2)
        {
            GameObject child = Instantiate(gameScript.piecePrefab, new Vector2(positions[i], positions[i + 1]), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), transform);
            child.GetComponent<SpriteRenderer>().sprite = gameScript.pieces[choice];
            ghostTetrominoObject.transform.GetChild(i / 2).transform.localPosition = child.transform.localPosition;
        }
        PlaceGhostTetromino();
        Collider2D[] results = new Collider2D[5];
        int overlap = 0;
        for(int i = 0; i < transform.childCount; ++i)
        {
            if(overlap == 3)
            {
                Lose();
                return;
            }
            Transform child = transform.GetChild(i);
            BoxCollider2D collider = child.GetComponent<BoxCollider2D>();
            while (Physics2D.OverlapCollider(collider, gameScript.rotationFilter, results) != 0)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + gameScript.shiftVal);
                ++overlap;
                i = 0;
            }
        }
    }
    void ConvertPlayer()
    {
        //converts player children into tetromino layer, does not put child them to grid yet
        for (int i = (transform.childCount - 1); i >= 0; --i)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.layer = gameScript.gridPieceLayer;
            child.gameObject.tag = "GridPiece";
        }
    }
    void HandleLines()
    {
        float shiftVal = gameScript.shiftVal;
        float start = gridLeft + (shiftVal / 2.0f);
        LayerMask gridPieceLayerMask = gameScript.gridPieceLayerMask;
        Transform gridTransform = grid.transform;
        int lines = 0;
        for (int i = (transform.childCount - 1); i >= 0; --i)
        {
            float val = -1000.0f;
            if (i >= transform.childCount) continue;
            Transform child = transform.GetChild(i);
            Vector2 left = new Vector2(child.position.x - shiftVal, child.position.y);
            Vector2 right = new Vector2(child.position.x + shiftVal, child.position.y);
            RaycastHit2D[] leftHits = Physics2D.RaycastAll(left, Vector2.left, shiftVal * 9, gridPieceLayerMask);
            RaycastHit2D[] rightHits = Physics2D.RaycastAll(right, Vector2.right, shiftVal * 9, gridPieceLayerMask);
            if ((leftHits.Length + rightHits.Length) == 9)
            {
                ++lines;
                ++linesCleared;
                linesClearedText.text = linesCleared.ToString();
                if (ui.soundOn) clear.Play();
                val = child.position.y;
                for (int j = 0; j < leftHits.Length; ++j)
                {
                    Destroy(leftHits[j].collider.gameObject);
                    leftHits[j].collider.transform.SetParent(null);
                }
                for (int j = 0; j < rightHits.Length; ++j)
                {
                    Destroy(rightHits[j].collider.gameObject);
                    rightHits[j].collider.transform.SetParent(null);
                }
                Destroy(child.gameObject);
                child.SetParent(null);
                if(linesCleared % gameScript.levelUp == 0 && level < gameScript.maxLevel)
                {
                    ++level;
                    levelText.text = level.ToString();
                    dropTimer -= gameScript.speedUp;
                }
            }
            else child.SetParent(gridTransform);
            if (val == -1000.0f) continue;
            List<RaycastHit2D[]> hitList = new List<RaycastHit2D[]>();
            for (int j = 0; j < 10; ++j)
            {
                Vector2 origin = new Vector2(start + (shiftVal * j), val);
                hitList.Add(Physics2D.RaycastAll(origin, Vector2.up, shiftVal * 20, gridPieceLayerMask));
            }
            for (int j = 0; j < hitList.Count; ++j)
            {
                for (int k = 0; k < hitList[j].Length; ++k)
                {
                    hitList[j][k].collider.transform.position = new Vector2(hitList[j][k].collider.transform.position.x,
                        hitList[j][k].collider.transform.position.y - shiftVal);
                }
            }
        }
        if (lines == 1) points += (uint)(40 * level);
        else if (lines == 2) points += (uint)(100 * level);
        else if (lines == 3) points += (uint)(300 * level);
        else if (lines == 4) points += (uint)(1200 * level);
    }
    public void RotatePlayer(float val)
    {
        if (choice == (int)Tetromino.BOX) return;
        if (ui.soundOn) click.Play();
        Vector2 position = transform.position;
        Quaternion rotation = transform.rotation;
        transform.Rotate(0.0f, 0.0f, val);
        foreach (Transform child in transform)
        {
            if (child.position.y < gridBottom)
            {
                while (child.position.y < gridBottom)
                {
                    transform.position = new Vector2(transform.position.x,
                        transform.position.y + gameScript.shiftVal);
                }
            }
            if (child.position.y > gridTop)
            {
                while (child.position.y > gridTop)
                {
                    transform.position = new Vector2(transform.position.x,
                        transform.position.y - gameScript.shiftVal);
                }
            }
            if (child.position.x < gridLeft)
            {
                while (child.position.x < gridLeft)
                {
                    transform.position = new Vector2(transform.position.x + gameScript.shiftVal,
                        transform.position.y);
                }
            }
            if (child.position.x > gridRight)
            {
                while (child.position.x > gridRight)
                {
                    transform.position = new Vector2(transform.position.x - gameScript.shiftVal,
                        transform.position.y);
                }
            }
        }
        bool collided = false;
        Collider2D[] results = new Collider2D[5];
        foreach(Transform child in transform)
        {
            BoxCollider2D collider = child.GetComponent<BoxCollider2D>();
            if (Physics2D.OverlapCollider(collider, gameScript.rotationFilter, results) != 0)
            {
                collided = true;
                break;
            }
        }
        if (collided)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
        else PlaceGhostTetromino();
    }
    public void HardDrop()
    {
        if (ui.soundOn) click.Play();
        float newVal = 0;
        int count = transform.childCount;
        int numDrops = 0;
        while (true)
        {
            bool doBreak = false;
            for (int i = 0; i < count; ++i)
            {
                Transform child = transform.GetChild(i);
                float y = child.position.y - newVal;
                if (y < gridBottom)
                {
                    newVal -= gameScript.shiftVal;
                    doBreak = true;
                    break;
                }
                Vector2 origin = new Vector2(child.position.x, y);
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, gameScript.shiftVal, gameScript.gridPieceLayerMask);
                if (hit.collider != null)
                {
                    bool doContinue = false;
                    for (int j = 0; j < count; ++j)
                    {
                        if (i == j) continue;
                        float y2 = transform.GetChild(j).position.y;
                        if (y2 < gridBottom)
                        {
                            doContinue = true;
                            break;
                        }
                    }
                    if (doContinue) continue;
                    doBreak = true;
                    break;
                }
            }
            if (doBreak) break;
            newVal += gameScript.shiftVal;
            ++numDrops;
        }
        transform.position = new Vector2(transform.position.x, transform.position.y - newVal);
        foreach (Transform child in transform)
        {
            if (child.position.y < gridBottom)
                transform.position = new Vector2(transform.position.x, transform.position.y + gameScript.shiftVal);
        }
        points += (uint) numDrops * 2;
        Respawn();
    }
    public void Respawn()
    {
        if (CheckForLoss())
        {
            Lose();
            return;
        }
        timeTillRespawn = 0.6f;
        hold = false;
        ConvertPlayer();
        HandleLines();
        timeTillDrop = dropTimer;
        SpawnTetromino();
    }
    public void Lose()
    {
        playing = false;
        lost = true;
        lose.SetActive(true);
    }
    public bool CheckForLoss()
    {
        foreach (Transform child in transform)
        {
            if (child.position.y < gridTop) return false;
        }
        return true;
    }
    public void Hold()
    {
        hold = true;
        if (holdChoice == -1)
        {
            holdChoice = choice;
            HandleChoice();
        }
        else
        {
            int temp = choice;
            choice = holdChoice;
            holdChoice = temp;
        }
        if (choice != (int)Tetromino.LINE) transform.position = new Vector2(xStart, yStart);
        else transform.position = new Vector2(xStart + gameScript.shiftVal, yStart);
        transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        float[] positions = GetPiecePositions();
        for (int i = 0; i < positions.Length; i += 2)
        {
            Transform child = transform.GetChild(i / 2);
            child.transform.position = new Vector2(positions[i], positions[i + 1]);
            child.GetComponent<SpriteRenderer>().sprite = gameScript.pieces[choice];
            ghostTetrominoObject.transform.GetChild(i / 2).transform.localPosition = child.transform.localPosition;
        }
        PlaceGhostTetromino();
        holdObject.GetComponent<SpriteRenderer>().sprite = gameScript.tetrominos[holdChoice];
    }
    public void PlaceGhostTetromino()
    {
        ghostTetrominoObject.transform.position = transform.position;
        ghostTetrominoObject.transform.rotation = transform.rotation;
        float newVal = 0;
        int count = ghostTetrominoObject.transform.childCount;
        while (true)
        {
            bool doBreak = false;
            for (int i = 0; i < count; ++i)
            {
                Transform child = ghostTetrominoObject.transform.GetChild(i);
                float y = child.position.y - newVal;
                if (y < gridBottom)
                {
                    newVal -= gameScript.shiftVal;
                    doBreak = true;
                    break;
                }
                Vector2 origin = new Vector2(child.position.x, y);
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, gameScript.shiftVal, gameScript.gridPieceLayerMask);
                if (hit.collider != null)
                {
                    bool doContinue = false;
                    for (int j = 0; j < count; ++j)
                    {
                        if (i == j) continue;
                        float y2 = ghostTetrominoObject.transform.GetChild(j).position.y;
                        if (y2 < gridBottom)
                        {
                            doContinue = true;
                            break;
                        }
                    }
                    if (doContinue) continue;
                    doBreak = true;
                    break;
                }
            }
            if (doBreak) break;
            newVal += gameScript.shiftVal;
        }
        ghostTetrominoObject.transform.position = new Vector2(ghostTetrominoObject.transform.position.x, ghostTetrominoObject.transform.position.y - newVal);
        foreach (Transform child in ghostTetrominoObject.transform)
        {
            if (child.position.y < gridBottom)
                ghostTetrominoObject.transform.position = new Vector2(ghostTetrominoObject.transform.position.x, ghostTetrominoObject.transform.position.y + gameScript.shiftVal);
        }
    }
    public float[] GetPiecePositions()
    {
        float[] positions = new float[8];
        switch (choice)
        {
            //global values, not local
            //xStart = 0.0f;
            //yStart = 0.855f;
            //y: 0 = 0.855, 1 = 0.765, 
            //x: 3 = -0.135, 4 = -0.045, 5 = 0.045
            //[0] = x, [1] = y and so on
            case (int)Tetromino.BOX:
                positions[0] = grid.transform.position.x - 0.045f; positions[1] = 0.855f;
                positions[2] = grid.transform.position.x + 0.045f; positions[3] = 0.855f;
                positions[4] = grid.transform.position.x - 0.045f; positions[5] = 0.765f;
                positions[6] = grid.transform.position.x + 0.045f; positions[7] = 0.765f;
                break;
            case (int)Tetromino.LBACKWARD:
                positions[0] = grid.transform.position.x + 0.045f; positions[1] = 0.855f;
                positions[2] = grid.transform.position.x - 0.135f; positions[3] = 0.765f;
                positions[4] = grid.transform.position.x - 0.045f; positions[5] = 0.765f;
                positions[6] = grid.transform.position.x + 0.045f; positions[7] = 0.765f;
                break;
            case (int)Tetromino.LFORWARD:
                positions[0] = grid.transform.position.x - 0.135f; positions[1] = 0.855f;
                positions[2] = grid.transform.position.x - 0.135f; positions[3] = 0.765f;
                positions[4] = grid.transform.position.x - 0.045f; positions[5] = 0.765f;
                positions[6] = grid.transform.position.x + 0.045f; positions[7] = 0.765f;
                break;
            case (int)Tetromino.LINE:
                positions[0] = grid.transform.position.x - 0.135f; positions[1] = 0.765f;
                positions[2] = grid.transform.position.x - 0.045f; positions[3] = 0.765f;
                positions[4] = grid.transform.position.x + 0.045f; positions[5] = 0.765f;
                positions[6] = grid.transform.position.x + 0.135f; positions[7] = 0.765f;
                break;
            case (int)Tetromino.SQUIGGLEFORWARD:
                positions[0] = grid.transform.position.x - 0.045f; positions[1] = 0.855f;
                positions[2] = grid.transform.position.x + 0.045f; positions[3] = 0.855f;
                positions[4] = grid.transform.position.x - 0.135f; positions[5] = 0.765f;
                positions[6] = grid.transform.position.x - 0.045f; positions[7] = 0.765f;
                break;
            case (int)Tetromino.SQUIGGLEBACKWARD:
                positions[0] = grid.transform.position.x - 0.135f; positions[1] = 0.855f;
                positions[2] = grid.transform.position.x - 0.045f; positions[3] = 0.855f;
                positions[4] = grid.transform.position.x - 0.045f; positions[5] = 0.765f;
                positions[6] = grid.transform.position.x + 0.045f; positions[7] = 0.765f;
                break;
            case (int)Tetromino.TEE:
                positions[0] = grid.transform.position.x - 0.045f; positions[1] = 0.855f;
                positions[2] = grid.transform.position.x - 0.135f; positions[3] = 0.765f;
                positions[4] = grid.transform.position.x - 0.045f; positions[5] = 0.765f;
                positions[6] = grid.transform.position.x + 0.045f; positions[7] = 0.765f;
                break;
        }
        return positions;
    }
    public void HandleChoice()
    {
        if (choice == -1)
        {
            choice = gameScript.rnd.Next(7);
            nextChoice = choice;
        }
        else choice = nextChoice;
        while (nextChoice == choice) nextChoice = gameScript.rnd.Next(7);
        nextObject.GetComponent<SpriteRenderer>().sprite = gameScript.tetrominos[nextChoice];
    }
}
