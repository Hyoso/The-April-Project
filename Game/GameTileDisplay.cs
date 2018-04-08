using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class GameTileDisplay : MonoBehaviour, IPointerDownHandler
{
    public GameTile myTile;

    [SerializeField]
    private bool btnDown = false;
    [SerializeField]
    private Vector2 initialBtnDownPos = -Vector2.one;
    [SerializeField]
    private RawImage myImage = null;

    public bool showDebugText = false;
    [SerializeField]
    private TextMeshProUGUI cellInfoText = null;

    public bool initialised = false;


    private void Start()
    {
        cellInfoText.text = showDebugText ? "x: " + myTile.x + "\ny: " + myTile.y : "";

        myTile.Init();

        // flag to tell tile manager it can procede to randomise tiles
        initialised = true;
    }

    // on input
    // check if i can move by asking tile manager
    private void Update()
    {
        if (btnDown && !myTile.GetIsEmpty())
        {
            TileManager.Instance.Moved(myTile);

            btnDown = false;
            initialBtnDownPos = -Vector2.one;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        btnDown = true;
        initialBtnDownPos = eventData.position;
    }

    public void SetIsEmpty(bool b)
    {
        myTile.SetIsEmpty(b);
        myImage.color = myTile.GetIsEmpty() ? myTile.disabledColour : myTile.enabledColour;
    }

    public void Init()
    {
        myTile.Init();
        SetImageRect();
    }

    public void SetImageRect()
    {
        myImage.uvRect = myTile.imageUVRect;
    }
}
