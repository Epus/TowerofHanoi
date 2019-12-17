using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    [SerializeField]
    private Tower[] towers;

    [SerializeField]
    private Plate[] plates;

    [SerializeField]
    private Transform arrow;

    [SerializeField]
    private Text logs;

    private int activeTower;
    private int startTower = 0;
    private Plate activePlate;
    private Coroutine clear;
    private bool isCompleted;

    public static Game instance;

    [NonSerialized]
    public int numPlates;
    [NonSerialized]
    public int maxPlate;
    [NonSerialized]
    public int minPlate = 3;
    [NonSerialized]
    public int maxTower;

    List<Move> moves = new List<Move>();
    private int moveIndex;
    private Plate currentPlate;
    private bool isMoving;

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject guideButton;
    [SerializeField]
    private Text numMovesText;

    private bool isPaused;
    private bool isPlayingGuide;
    private int numMoves;

    // Use this for initialization
    void Start () {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }

        maxPlate = plates.Length;
        maxTower = towers.Length;

    }

    private void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    private void Reset ()
    {
        Reset(numPlates, startTower);
    }

    public void Reset(int numPlates, int startTower)
    {
        StopAllCoroutines();
        moves.Clear();
        isMoving = false;
        moveIndex = 0;
        numMoves = 0;
        numMovesText.text = "Moves: " + numMoves;

        this.numPlates = numPlates;
        this.startTower = startTower;

        foreach (var plate in plates)
        {
            //put the plates somewhere that cannot be seen
            plate.transform.position = Vector3.up * 20;
        }

        foreach (var tower in towers)
        {
            tower.Reset();
        }
        towers[startTower].IsStartingTower = true;

        for (int i = plates.Length - numPlates; i < plates.Length; i++)
        {
            towers[startTower].Push(plates[i]);
        }

        clear = StartCoroutine(ClearLog(0));
        activeTower = startTower;
        arrow.position = towers[activeTower].transform.position + Vector3.up * towers[activeTower].transform.lossyScale.y / 2f;
        isCompleted = false;
        isPlayingGuide = false;
        guideButton.SetActive(true);

        UnPause();
    }

    public void PlayGuide()
    {
        Hanoi(numPlates, startTower, (startTower + 1) % 3, (startTower + 2) % 3);
        isPlayingGuide = true;
        HideGuideButton();
    }

    public void Log(string message, float time)
    {
        Log(message);
        StopCoroutine(clear);
        clear = StartCoroutine(ClearLog(time));
    }

    public void Log(string message)
    {
        logs.text = message;
    }

    IEnumerator ClearLog(float time)
    {
        yield return new WaitForSeconds(time);
        logs.text = "";
    }

    public void Completed()
    {
        isCompleted = true;
        Log("Game Completed! Press R to Restart");
    }

    public void HideGuideButton()
    {
        guideButton.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                Pause();
            else
                UnPause();
        }

        if (Input.GetKeyDown (KeyCode.R))
        {
            if(!isPaused)
                Reset();
        }

        if (isPaused || isCompleted)
            return;

        if (isPlayingGuide)
        {
            if (!isMoving)
                StartCoroutine(ExecuteMoveAfterSeconds(0.5f));
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            activeTower = (activeTower + 1) % towers.Length;
            SwitchTower();          
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            activeTower--;
            if (activeTower == -1)
                activeTower = towers.Length - 1;
            SwitchTower();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (activePlate == null)
            {
                activePlate = towers[activeTower].Pop();
                if (activePlate != null)
                {
                    HideGuideButton();
                    activePlate.transform.position = arrow.transform.position;
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (towers[activeTower].Push(activePlate))
            {
                activePlate = null;
                numMoves++;
                numMovesText.text = "MOVES: " + numMoves;
            }

        }

    }

    void SwitchTower ()
    {
        if (activePlate != null)
        {
            activePlate.transform.position = towers[activeTower].transform.position + Vector3.up * towers[activeTower].transform.lossyScale.y / 2f;
        }
        arrow.transform.position = towers[activeTower].transform.position + Vector3.up * towers[activeTower].transform.lossyScale.y / 2f;
    }

    void Hanoi(int n, int source, int temp, int dest)
    {
        if (n == 1)
        {
            Transfer(source, dest);
        }
        else
        {
            Hanoi(n - 1, source, dest, temp);
            Transfer(source, dest);
            Hanoi(n - 1, temp, source, dest);
        }

    }

    void Transfer(int source, int dest)
    {
        moves.Add(new Move(false, source));
        moves.Add(new Move(true, dest));
    }

    IEnumerator ExecuteMoveAfterSeconds(float time)
    {
        isMoving = true;
        yield return new WaitForSeconds(time);
        ExecuteMove();
        isMoving = false;
    }

    void ExecuteMove()
    {
        var move = moves[moveIndex];
        if (move.isPush)
        {
            towers[move.position].Push(currentPlate);
            currentPlate = null;
            numMoves++;
            numMovesText.text = "MOVES: " + numMoves;
        }
        else
        {
            currentPlate = towers[move.position].Pop();
        }
        moveIndex++;
    }

    void PrintMoves()
    {
        for (int i = 0; i < moves.Count; i += 2)
        {
            Debug.Log(moves[i].position + " -> " + moves[i + 1].position);
        }
    }

}

class Move
{
    public bool isPush;
    public int position;

    public Move(bool isPush, int position)
    {
        this.isPush = isPush;
        this.position = position;
    }
}

