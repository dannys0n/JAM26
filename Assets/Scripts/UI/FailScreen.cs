using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailScreen : MonoBehaviour
{
    [Header("UI References")]
    public GameObject FailPanel;
    public TargetOutfitUI targetDisplay;
    public TargetOutfitUI killedDisplay;

    public void ShowFailScreen(FullOutfit target, FullOutfit killed)
    {
        FailPanel.SetActive(true);

        //set the UI of the target outfits
        targetDisplay.UpdateDisplay(target);
        killedDisplay.UpdateDisplay(killed);



    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
