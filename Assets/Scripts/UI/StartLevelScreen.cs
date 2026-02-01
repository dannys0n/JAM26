using UnityEngine;
using UnityEngine.UI;

public class StartLevelScreen : MonoBehaviour
{
    [Header("UI References")]
    public GameObject StartPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //hide this until called by Game manager
    }


    //method called when "Begin" button has been pressed
    public void StartGame()
    {
        //hide this screen
        StartPanel.SetActive(false);

        //start playing music
        
    }
    
}
