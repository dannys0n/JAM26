using UnityEngine;
using UnityEngine.UI;

public class StartLevelScreen : MonoBehaviour
{
    [Header("UI References")]
    public GameObject StartPanel;
    public TargetOutfitUI targetUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //method to bring up panel when scene is loaded
    public void ShowPanel(FullOutfit targetOutfit)
    {
        StartPanel.SetActive(true);

        //set the target UI outfit
        targetUI.UpdateDisplay(targetOutfit);
    }

    //method called when "Begin" button has been pressed
    public void StartGame()
    {
        //hide this screen
        StartPanel.SetActive(false);

        FindFirstObjectByType<GameplayLogic>().GameStart();

        
    }
    
}
