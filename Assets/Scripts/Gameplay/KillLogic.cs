using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class KillLogic : MonoBehaviour
{
    private List<PlayerController> npcs;
    private PlayerController currentControlled;

    private bool playerJumped = false;


    public UnityEvent PlayerSwitched;
    public UnityEvent WrongTargetKilled;
    public UnityEvent RightTargetKilled;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //when the game starts, get a list of all player-controllable objects in scene
        npcs = new List<PlayerController>();
        npcs.AddRange(FindObjectsByType<PlayerController>(FindObjectsSortMode.None));


    }

    //method called by PlayerController when taken over
    public void PlayerJumped(PlayerController npc)
    {
        if (playerJumped)
            return;

        playerJumped = true;

        Debug.Log("Player Jumped to new NPC: " + npc.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        //reset playerJumped var
        playerJumped = false;
    }
}
