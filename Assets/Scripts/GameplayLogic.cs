using UnityEngine;

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

    }

    //method for when the start button is pressed
    public void GameStart()
    {
        GameActive = true;

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
        }
        else
        {
            Debug.Log("YOU FAILED");
        }

        //stop the timer

        //freeze the scene

        //zoom in

        //kill the npc

        //bring up results screen

    }



    
}
