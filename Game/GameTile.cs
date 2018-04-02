using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class GameTile : MonoBehaviour, IPointerDownHandler
{
    public int x;
    public int y;

    public int originalX;
    public int originalY;

    [SerializeField]
    private bool btnDown = false;
    [SerializeField]
    private Vector2 initialBtnDownPos = -Vector2.one;
    [SerializeField]
    private float minMoveDistance = 5.0f;

    [SerializeField]
    private RawImage myImage;
    public Rect imageUVRect;
    public Color imageCol;

    public bool showDebugText = false;
    [SerializeField]
    private TextMeshProUGUI cellInfoText;

    [SerializeField]
    private bool isEmpty = false;

    [SerializeField]
    private Color enabledColour;
    [SerializeField]
    private Color disabledColour;

    private void Start()
    {
        cellInfoText.text = showDebugText ? "x: " + x + "\ny: " + y : "";

        originalX = x;
        originalY = y;

        imageUVRect = myImage.uvRect;
    }

    // on input
    // check if i can move by asking tile manager
    private void Update()
    {
        if ((Input.mousePosition.x != initialBtnDownPos.x ||
            Input.mousePosition.y != initialBtnDownPos.y)
            && btnDown && !isEmpty)
        {
            if (Vector2.Distance(Input.mousePosition, initialBtnDownPos) < minMoveDistance)
            {
                return;
            }

            Vector2 mouseVec2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            Vector2 dir = (initialBtnDownPos - mouseVec2);

            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            //Debug.Log(angle);

            MOVE_DIR moveDirection = MOVE_DIR.NONE;
            int moveX = 0;
            int moveY = 0;

            if (angle > -45 && angle < 45)
            {
                moveDirection = MOVE_DIR.DOWN;
                moveY = -1;
            }
            else if (angle <= 45 && angle > -135)
            {
                moveDirection = MOVE_DIR.RIGHT;
                moveX = 1;
            }
            else if (angle <= 135 && angle >= 45)
            {
                moveDirection = MOVE_DIR.LEFT;
                moveX = -1;
            }
            else
            {
                moveDirection = MOVE_DIR.UP;
                moveY = 1;
            }

            Debug.Log(moveDirection);

            // ask tile manager if i can move in that direction
            if (x + moveX < TileManager.Instance.columns && x + moveX >= 0 &&
                y + moveY < TileManager.Instance.rows && y + moveY >= 0 &&
                TileManager.Instance.Moved(this, moveX, moveY))
            {
                Debug.Log("moved");
            }
            else
            {
                Debug.Log("Can't move");
            }

            btnDown = false;
            initialBtnDownPos = -Vector2.one;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        btnDown = true;
        initialBtnDownPos = eventData.position;
    }

    public void CopyFrom(GameTile t)
    {
        // copy values from t
        // take x, y, image colour, image rect, initial x + y

        imageUVRect = t.imageUVRect;
        originalX = t.originalX;
        originalY = t.originalY;
    }

    public void SetIsEmpty(bool b)
    {
        isEmpty = b;
        myImage.color = isEmpty ? disabledColour : enabledColour;
        imageCol = myImage.color;
    }

    public void Init()
    {
        myImage.uvRect = imageUVRect;
    }

    // checks if the cell is in its correct position
    public bool CheckCell()
    {
        return x == originalX && y == originalY;
    }

    public bool GetIsEmpty()
    {
        return isEmpty;
    }
}
