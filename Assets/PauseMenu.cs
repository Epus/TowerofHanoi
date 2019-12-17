using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    [SerializeField]
    private Text numPlatesText;
    [SerializeField]
    private Text startTowerText;
    [SerializeField]
    private GameObject resumeText;

    private int numPlates;
    private int startTower;

    private void Start ()
    {
        numPlates = Game.instance.minPlate;
        numPlatesText.text = "" + numPlates;
        startTowerText.text = "" + (startTower + 1);
    }
   

    public void IncreasePlate ()
    {
        numPlates++;
        if (numPlates > Game.instance.maxPlate)
            numPlates = Game.instance.maxPlate;
        numPlatesText.text = "" + numPlates;
    }

    public void DecreasePlate ()
    {
        numPlates--;
        if (numPlates < Game.instance.minPlate)
            numPlates = Game.instance.minPlate;
        numPlatesText.text = "" + numPlates;
    }

    public void IncreaseStartTower ()
    {
        startTower++;
        if (startTower == Game.instance.maxTower)
            startTower = Game.instance.maxTower- 1;
        startTowerText.text = "" + (startTower + 1);
    }

    public void DecreaseStartTower ()
    {
        startTower--;
        if (startTower < 0)
            startTower = 0;
        startTowerText.text = "" + (startTower + 1);
    }

    public void BackToMenu ()
    {
        SceneManager.LoadScene("Menu");
    }

    public void BeginGame()
    {
        Game.instance.Reset(numPlates, startTower);
        resumeText.SetActive(true);
    }

}
