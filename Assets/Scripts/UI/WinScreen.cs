using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{

    [Header("UI References")]
    public GameObject WinPanel;
    public TargetOutfitUI killedDisplay;
    public Image Body;
    public Sprite Skeleton;


    public void ShowWinScreen(FullOutfit killed)
    {
        WinPanel.SetActive(true);
        killedDisplay.UpdateDisplay(killed);
        Body.sprite = Skeleton;

    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
