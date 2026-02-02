using UnityEngine;
using UnityEngine.UI;

public class KilledNPCLogic : MonoBehaviour
{
  public Image CorrectGuessImage;
  public Image IncorrectGuessImage;
  public static KilledNPCLogic instance;

  private void Awake()
  {
    instance = this;
	}

  private void Start()
  {
    //CorrectGuessImage.enabled = false;
    //IncorrectGuessImage.enabled = false;
  }


	public void MadeGuess(bool correctGuess)
  {
    //if (correctGuess)
    //{
    //  CorrectGuessImage.enabled = true;
    //  IncorrectGuessImage.enabled = false;
    //}
    //else
    //{
    //  CorrectGuessImage.enabled = false;
    //  IncorrectGuessImage.enabled = true;
    //}
	}
}
