using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayLogic : MonoBehaviour
{
    public bool GameActive = false;

    [Header("Gameplay Variables")]
    public FullOutfit currentTarget;

    [Header("System References")]
    public OutfitManager outfitManager;
    public GameManager gameManager;

    [Header("UI References")]
    public StartLevelScreen startScreen;
    public FailScreen failScreen;
    public WinScreen winScreen;
    

    //GAMEPLAY STATES:
    /*
     * 1 - Pre-game
     * 2 - game starts
     * 3 - target killed -> game over
     */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScreen = FindFirstObjectByType<StartLevelScreen>();
        failScreen = FindFirstObjectByType<FailScreen>();
        winScreen = FindFirstObjectByType<WinScreen>();

        PreGame();
    }

    void Update()
    {
        if(GameActive)
        {
            //check for kill submit
            if (Input.GetMouseButtonDown(2))
            {
                KillSubmitted();
            }
        }
    }

    //method for setting up game variables
    public void PreGame()
    {
        //Assign random outfits to the NPCs
        outfitManager.AssignAllOutfits();

        //get a random outfit from existing outfits
        currentTarget = outfitManager.GetRandomExisting();

        //Show the start screen UI with target data
        startScreen.ShowPanel(currentTarget);

        //hide the other panels
        failScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);

    }

    //method for when the start button is pressed
    public void GameStart()
    {
        GameActive = true;
        Time.timeScale = 1;

        //start the timer

        
    }

    public bool CompareOutfits(FullOutfit first, FullOutfit second)
    {
        if (first.Body != second.Body)
            return false;
        if (first.Hat != second.Hat)
            return false;
        if (first.Mask != second.Mask)
            return false;
        if (first.Full != second.Full)
            return false;
        if (first.Hair != second.Hair)
            return false;

        return true;
    }

    //called when the kill button is pressed or if player right clicks
    public void KillSubmitted()
    {
        Time.timeScale = 0;

        //check if there is an NPC being controlled
        if(PlayerController.GetControlledPlayer() == null)
        {
            return;
        }

        FullOutfit killedNPC = PlayerController.GetControlledPlayer().transform.GetChild(0).GetComponent<NPCOutfit>().outfitData; 

        //check to see if the data matches
        if(CompareOutfits(killedNPC, currentTarget))
        {
            Debug.Log("Correct target killed");


            FindFirstObjectByType<UIAudioManager>()?.PlayLevel1CorrectGuess();

            winScreen.gameObject.SetActive(true);
            winScreen.ShowWinScreen(currentTarget);

        }
        else
        {
            Debug.Log("YOU FAILED");

            FindFirstObjectByType<UIAudioManager>()?.PlayIncorrectGuess();

            failScreen.gameObject.SetActive(true);
            failScreen.ShowFailScreen(currentTarget, killedNPC);
        }

        //stop the timer

        //freeze the scene

        //zoom in

        //kill the npc

        //bring up results screen

    }

    public void RetryLevel()
    {
        // Get the name of the current scene
        string sceneName = SceneManager.GetActiveScene().name;

        // Load the current scene again
        SceneManager.LoadScene(sceneName);
    }


    public void NextLevel()
    {
        // Load the next scene in the build index?
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    
}
