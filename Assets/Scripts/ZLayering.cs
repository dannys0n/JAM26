using UnityEngine;

public class ZLayering : MonoBehaviour
{
    public float MaxZ = 10.0f;
    public float MinZ = 0.0f;


    // Update is called once per frame
    void Update()
    {
        SetZ();
    }

    private void SetZ()
    {
        //get y position on screen
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        //divide y position by screen height
        float zRatio = screenPos.y / Screen.height;

        //set the Z position as minZ + (|minZ - maxZ| * zRatio)
        float newZ = MinZ + (Mathf.Abs(MinZ - MaxZ) * zRatio);

        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);

    }
}
