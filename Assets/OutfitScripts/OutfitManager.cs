using UnityEngine;
using System.Collections.Generic;

public class OutfitManager : MonoBehaviour
{
    [Header("Outfit Sprites - ID")]

    public List<OutfitSprite> BodySprites;
    public List<OutfitSprite> HatSprites;
    public List<OutfitSprite> HairSprites;
    public List<OutfitSprite> MaskSprites;
    public List<OutfitSprite> FullSprites;
    public List<OutfitSprite> TopSprites;
    public List<OutfitSprite> BottomSprites;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
public enum OutfitTypes
{
    None = 0,
    Body = 1,
    Hat = 2,
    Mask = 3,
    Top = 4,
    Bottom = 5,
    Full = 6

}
