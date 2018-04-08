using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameTile", menuName = "ScriptableObjects/GameTile", order = 1)]
public class GameTile : ScriptableObject
{
    public int x;
    public int y;

    public int originalX;
    public int originalY;

    public Rect imageUVRect;
    public Rect originalUVRect;

    public Color enabledColour;
    public Color disabledColour;

    [SerializeField]
    private bool isEmpty = false;

    public void Init()
    {
        isEmpty = false;
        x = originalX;
        y = originalY;
        imageUVRect = originalUVRect;
    }

    public void CopyFrom(GameTile t)
    {
        imageUVRect = t.imageUVRect;
        x = t.x;
        y = t.y;
    }

    public void SetIsEmpty(bool b)
    {
        isEmpty = b;
    }

    public bool GetIsEmpty()
    {
        return isEmpty;
    }

    // checks if the cell is in its correct position
    public bool CheckCell()
    {
        return x == originalX && y == originalY;
    }
}
