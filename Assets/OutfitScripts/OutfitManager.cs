using UnityEngine;
using System.Collections.Generic;

public class OutfitManager : MonoBehaviour
{
    [Header("Debug Stuff")]
    public List<NPCOutfit> testOutfits;


    [Header("Outfit Sprites")]

    public List<OutfitSprite> BodySprites;
    public List<OutfitSprite> HatSprites;
    public List<OutfitSprite> HairSprites;
    public List<OutfitSprite> MaskSprites;
    public List<OutfitSprite> FullSprites;
    public List<OutfitSprite> TopSprites;
    public List<OutfitSprite> BottomSprites;


    [Header("Existing Outfits")]
    public List<FullOutfit> ExistingOutfits;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ExistingOutfits = new List<FullOutfit>();
        
        
    }

    private void DebugTest()
    {
        testOutfits.AddRange(FindObjectsByType<NPCOutfit>(FindObjectsSortMode.None));


        //debug stuff (only works if NPCs are in the scene at start)
        foreach (NPCOutfit npc in testOutfits)
        {
            AssignRandomOutfit(npc);
        }
    }

    public void AssignRandomOutfit(NPCOutfit npc)
    {

        //Pick random sprites/modes
        int useFull = Random.Range(0, 2);

        FullOutfit newOutfit = CreateRandomOutfit(useFull == 0);


        //check to see if outfit already exists in scene
        while(OutfitExists(newOutfit))
        {
            //Pick random sprites/modes again
            useFull = Random.Range(0, 1);

            newOutfit = CreateRandomOutfit(useFull == 0);
        }

        //add the new outfit to list of existing outfits
        ExistingOutfits.Add(newOutfit);

        //assign outfit to given NPC
        npc.SetOutfit(newOutfit);


    }

    public FullOutfit CreateRandomOutfit(bool useFull)
    {
        //Pick random sprites
        FullOutfit newOutfit = new FullOutfit();

        //getting indexes
        int bodyIndex = Random.Range(0, BodySprites.Count);
        int hatIndex = Random.Range(0, HatSprites.Count);
        int hairIndex = Random.Range(0, HairSprites.Count);
        int maskIndex = Random.Range(0, MaskSprites.Count);
        int fullIndex = Random.Range(0, FullSprites.Count);
        int topIndex = Random.Range(0, TopSprites.Count);
        int bottomIndex = Random.Range(0, BottomSprites.Count);

        //set which mode to use
        newOutfit.useFull = useFull;

        //Set data
        newOutfit.Body = bodyIndex;
        newOutfit.Hat = hatIndex;
        newOutfit.Hair = hairIndex;
        newOutfit.Mask = maskIndex;
        newOutfit.Full = fullIndex;
        newOutfit.Top = topIndex;
        newOutfit.Bottom = bottomIndex;

        return newOutfit;
    }

    public bool OutfitExists(FullOutfit outfit)
    {
        if(ExistingOutfits.Contains(outfit))
        {
            return true;
        }
        return false;
    }
    
}

[System.Serializable]
public struct OutfitSprite
{
    public OutfitTypes type;
    public string description;
    public Sprite sprite;

}

[System.Serializable]
public struct FullOutfit
{
    public int Body;
    public int Hair;
    public int Hat;
    public int Mask;
    public int Top;
    public int Bottom;
    public int Full;
    public bool useFull;
    
}


[System.Serializable]
public enum OutfitTypes
{
    None = 0,
    Body = 1,
    Hat = 2,
    Mask = 3,
    Top = 4,
    Bottom = 5,
    Full = 6,
    Hair = 7

}
