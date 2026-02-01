using UnityEngine;

public class GameplayLogic : MonoBehaviour
{
    [Header("Gameplay Variables")]
    public FullOutfit currentTarget;

    [Header("System References")]
    public OutfitManager outfitManager;
    public GameManager gameManager;

    [Header("UI References")]
    public StartLevelScreen startScreen;
    

    //GAMEPLAY STATES:
    /*
     * 1 - Pre-game
     * 2 - game starts
     * 3 - target killed -> game over
     */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PreGame();
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

    }

    //method for when the start button is pressed
    public void GameStart()
    {
        //start the timer

        
    }



    
}
