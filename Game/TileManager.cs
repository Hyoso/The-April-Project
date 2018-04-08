using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;  

// creates the tile system
// holds handles to them all
public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get { return instance; } }
    private static TileManager instance = null;

    [SerializeField]
    private List<GameObject> tilesList = new List<GameObject>();
    private Dictionary<Vector2Int, GameObject> tilesDictionary = new Dictionary<Vector2Int, GameObject>();

    public int rows = 0;
    public int columns = 0;

    public bool enableShuffle = false;

    private int RANDOMISE_TILES_COUNT = 100;

    [SerializeField]
    private Text raycastBlock = null;

    [SerializeField]
    private TextMeshProUGUI gameStatusText = null;

    string[] generatingTextArray = { "Generating", "Generating.", "Generating..", "Generating..." };

    private void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        rows = columns = (int)Mathf.Sqrt(tilesList.Count);
        // populate dictionary
        for (int i = 0; i != tilesList.Count; ++i)
        {
            tilesList[i].GetComponent<GameTileDisplay>().Init();
            tilesDictionary.Add(new Vector2Int(tilesList[i].GetComponent<GameTileDisplay>().myTile.x, tilesList[i].GetComponent<GameTileDisplay>().myTile.y), tilesList[i].gameObject);
        }

        StartCoroutine(RemoveRandomTileCoroutine());
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    // x1, y1 cell for the moving tile
    private void SwapTiles(int x1, int y1, int x2, int y2)
    {
        GameTileDisplay toCheckTile = tilesDictionary[new Vector2Int(x2, y2)].GetComponent<GameTileDisplay>();
        GameTileDisplay movingTile = tilesDictionary[new Vector2Int(x1, y1)].GetComponent<GameTileDisplay>();

        GameTile copy = ScriptableObject.CreateInstance<GameTile>();
        copy.CopyFrom(movingTile.myTile); // save the moving tile

        // set the moving tile to the empty tile stats
        movingTile.myTile.CopyFrom(toCheckTile.myTile);
        movingTile.SetIsEmpty(toCheckTile.myTile.GetIsEmpty()); // true if swapping with empty cell

        // set the empty tile stats to the copied tile
        toCheckTile.myTile.CopyFrom(copy);
        toCheckTile.SetIsEmpty(copy.GetIsEmpty()); // false
        toCheckTile.SetImageRect();

        Destroy(copy);
    }

    public bool CheckMove(int startX, int startY, int dirX, int dirY)
    {
        int newX = startX + dirX;
        int newY = startY + dirY;

        bool canMove = false;
        if (newX < columns && newX >= 0 &&
            newY < rows && newY >= 0)
        {
            GameTile toCheckTile = tilesDictionary[new Vector2Int(newX, newY)].GetComponent<GameTileDisplay>().myTile; // this is the empty tile if alpha is true
            canMove = toCheckTile.GetIsEmpty();
            if (canMove)
            {
                // swap the two tiles
                SwapTiles(startX, startY, newX, newY);

                // check tiles
                bool won = true;
                for (int i = 0; i != tilesList.Count; ++i)
                {
                    if (!tilesList[i].GetComponent<GameTileDisplay>().myTile.CheckCell())
                    {
                        won = false;
                        break;
                    }
                }

                if (won)
                {
                    Debug.Log("player won");
                    StartCoroutine(RestartGameCoroutine());
                }
            }
            else { } // do nothing
        }
        return canMove;
    }

    public bool Moved(GameTile t)
    {
        return CheckMove(t.originalX, t.originalY, -1, 0) ||// left
            CheckMove(t.originalX, t.originalY, 1, 0) ||// right
            CheckMove(t.originalX, t.originalY, 0, -1) || // down
            CheckMove(t.originalX, t.originalY, 0, 1); // up
    }

    private void SwapTilesAndDeepCpy(Vector2Int tappedTile, Vector2Int toCheckTile)
    {
        GameTileDisplay toCheckTileDisplay = tilesDictionary[toCheckTile].GetComponent<GameTileDisplay>();
        GameTileDisplay movingTileDisplay = tilesDictionary[tappedTile].GetComponent<GameTileDisplay>();

        GameTile copy = ScriptableObject.CreateInstance<GameTile>();
        copy.CopyFrom(movingTileDisplay.myTile);
        copy.originalX = movingTileDisplay.myTile.originalX;
        copy.originalY = movingTileDisplay.myTile.originalY;
        copy.originalUVRect = movingTileDisplay.myTile.originalUVRect;
        copy.disabledColour = movingTileDisplay.myTile.disabledColour;
        copy.enabledColour = movingTileDisplay.myTile.enabledColour;
		copy.SetIsEmpty(movingTileDisplay.myTile.GetIsEmpty());

        // set the moving tile to the empty tile stats
        movingTileDisplay.myTile.CopyFrom(toCheckTileDisplay.myTile);
        movingTileDisplay.SetIsEmpty(toCheckTileDisplay.myTile.GetIsEmpty()); // true if swapping with empty cell
        movingTileDisplay.SetImageRect();

        // set the empty tile stats to the copied tile
        toCheckTileDisplay.myTile.CopyFrom(copy);
        toCheckTileDisplay.SetIsEmpty(copy.GetIsEmpty()); // false
        toCheckTileDisplay.SetImageRect();
    }

    private Vector2Int[] directionArray = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0)};
	private static Vector2Int previousDir = Vector2Int.zero;
    private Vector2Int CalcNextDirection(Vector2Int currentTile)
    {
        Vector2Int dir = directionArray[Random.Range(0, directionArray.Length)];

        bool foundNewDir = false;
        while (!foundNewDir)
        {
			Vector2Int nextTile = currentTile + dir;
            if (nextTile.x < columns &&
                nextTile.y < rows &&
                nextTile.x >= 0 &&
				nextTile.y >= 0 &&
				previousDir != dir)
            {
				previousDir = dir * -1;
				foundNewDir = true;
				break;
            }

			dir = directionArray[Random.Range(0, directionArray.Length)];
		}


        return dir;
    }

    IEnumerator RestartGameCoroutine()
    {
        raycastBlock.enabled = true;
        for (int i = 3; i != 0; i--)
        {
            gameStatusText.text = "Finished!\nRestarting in " + i;
            yield return new WaitForSeconds(1.0f);
        }

        raycastBlock.enabled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator RemoveRandomTileCoroutine()
    {
        raycastBlock.enabled = true;
        // choose a random cell to set as the empty
        yield return new WaitWhile(() => tilesList.Count(a => a.GetComponent<GameTileDisplay>().initialised) == tilesList.Count - 1);

		tilesDictionary[new Vector2Int(0, 0)].GetComponent<GameTileDisplay>().SetIsEmpty(true);
		yield return new WaitForSeconds(0.25f);

		Vector2Int startingCell = Vector2Int.zero;
        
        if (enableShuffle)
        {
            Vector2Int cell1 = startingCell;
            Vector2Int cell2 = new Vector2Int(-1, -1);
            int textArrayCount = 0;
            for (int i = 0; i != RANDOMISE_TILES_COUNT; ++i)
            {
				Vector2Int dir = CalcNextDirection(cell1);
				cell2 = dir + cell1;
				SwapTilesAndDeepCpy(cell1, cell2);
				cell1 = cell2;

                // set text status
                gameStatusText.text = generatingTextArray[textArrayCount];
                textArrayCount++;
                if (textArrayCount >= generatingTextArray.Length)
                {
                    textArrayCount = 0;
                }
				yield return new WaitForSeconds(0.05f);
            }
        }

        gameStatusText.text = "";
        raycastBlock.enabled = false;
        yield return null;
    }

}
