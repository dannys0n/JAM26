using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TargetOutfitUI : MonoBehaviour
{
    public FullOutfit target;
    private OutfitManager manager;

    [Header("UI References")]
    public Image Body;
    public Image Hat;
    public Image Hair;
    public Image Clothes;
    public Image Mask;

    void Start()
    {
        manager = FindFirstObjectByType<OutfitManager>();
    }

    public void DebugTarget()
    {
        FullOutfit f = manager.GetRandomExisting();
    }

    public void UpdateDisplay(FullOutfit targetOutfit)
    {
        manager = FindFirstObjectByType<OutfitManager>();

        target = targetOutfit;

        //get sprites from manager based on indexes given
        OutfitSprite sBody = manager.BodySprites[target.Body];
        Body.sprite = sBody.sprite;

        OutfitSprite sHair = manager.HairSprites[target.Hair];
        Hair.sprite = sHair.sprite;

        OutfitSprite sHat = manager.HatSprites[target.Hat];
        if(Hat.sprite != null)
        {
            Hat.enabled = false;
            Hat.sprite = sHat.sprite;
        }
        else
        {
            Hat.enabled = false;
        }

        OutfitSprite sMask = manager.MaskSprites[target.Mask];
        Mask.sprite = sMask.sprite;

        OutfitSprite sClothes = manager.FullSprites[target.Full];
        Clothes.sprite = sClothes.sprite;
    }
}
