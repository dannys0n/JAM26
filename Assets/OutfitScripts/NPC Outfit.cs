using UnityEngine;
using System.Collections.Generic;

public class NPCOutfit : MonoBehaviour
{
    public string ID = "";
    public FullOutfit outfitData;

    public SpriteRenderer BodySR;
    public SpriteRenderer HairSR;
    public SpriteRenderer HatSR;
    public SpriteRenderer MaskSR;
    public SpriteRenderer TopSR;
    public SpriteRenderer BottomSR;
    public SpriteRenderer FullSR;

    private OutfitManager manager;

    void Awake()
    {
        //get a reference to the outfit manager for sprites
        manager = FindFirstObjectByType<OutfitManager>();
    }

    public void SetOutfit(FullOutfit outfit)
    {
        //set outfit data
        outfitData = outfit;

        //get sprites from manager based on indexes given
        OutfitSprite sBody = manager.BodySprites[outfit.Body];
        BodySR.sprite = sBody.sprite;

        OutfitSprite sHair = manager.HairSprites[outfit.Hair];
        HairSR.sprite = sHair.sprite;

        OutfitSprite sHat = manager.HatSprites[outfit.Hat];
        HatSR.sprite = sHat.sprite;

        OutfitSprite sMask = manager.MaskSprites[outfit.Mask];
        MaskSR.sprite = sMask.sprite;

        //check to see if this sprite uses the full outfit
        if(outfit.useFull)
        {
            //if so, hide the top and bottom sprite renderers
            TopSR.gameObject.SetActive(false);
            BottomSR.gameObject.SetActive(false);

            //set full sprite
            OutfitSprite sFull = manager.FullSprites[outfit.Full];
            FullSR.sprite = sFull.sprite;

        }
        else
        {
            //hide full sprite renderer
            FullSR.gameObject.SetActive(false);

            OutfitSprite sTop = manager.TopSprites[outfit.Top];
            TopSR.sprite = sTop.sprite;

            OutfitSprite sBottom = manager.BottomSprites[outfit.Bottom];
            BottomSR.sprite = sBottom.sprite;

        }



    }

    
}






