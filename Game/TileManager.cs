using System.Collections;
using System.Collections.Generic;
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
    private Dictionary<KeyValuePair<int, int>, GameObject> tilesDictionary = new Dictionary<KeyValuePair<int, int>, GameObject>();

    public int rows = 0;
    public int columns = 0;

    [SerializeField]
    private Text raycastBlock = null;

    [SerializeField]
    private TextMeshProUGUI gameStatusText;

    private bool gameOver = false;

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
            tilesDictionary.Add(new KeyValuePair<int, int>(tilesList[i].GetComponent<GameTile>().x, tilesList[i].GetComponent<GameTile>().y), tilesList[i].gameObject);
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
        GameTile toCheckTile = tilesDictionary[new KeyValuePair<int, int>(x2, y2)].GetComponent<GameTile>();
        GameTile movingTile = tilesDictionary[new KeyValuePair<int, int>(x1, y1)].GetComponent<GameTile>();

        GameTile copy = new GameTile();
        copy.CopyFrom(movingTile); // save the moving tile

        // set the moving tile to the empty tile stats
        movingTile.CopyFrom(toCheckTile);
        movingTile.SetIsEmpty(toCheckTile.GetIsEmpty()); // true if swapping with empty cell

        // set the empty tile stats to the copied tile
        toCheckTile.CopyFrom(copy);
        toCheckTile.SetIsEmpty(copy.GetIsEmpty()); // false
        toCheckTile.Init();
    }

    public bool Moved(GameTile t, int moveX, int moveY)
    {
        int newX = t.x + moveX;
        int newY = t.y + moveY;

        GameTile toCheckTile = tilesDictionary[new KeyValuePair<int, int>(newX, newY)].GetComponent<GameTile>(); // this is the empty tile if alpha is true
        bool canMove = toCheckTile.GetIsEmpty();
        if (canMove)
        {
            // swap the two tiles
            GameTile copy = new GameTile();
            copy.CopyFrom(t); // save the moving tile

            SwapTiles(t.x, t.y, newX, newY);

            // check tiles
            bool won = true;
            for (int i = 0; i != tilesList.Count; ++i)
            {
                if (!tilesList[i].GetComponent<GameTile>().CheckCell())
                {
                    won = false;
                    break;
                }
            }

            if (won)
            {
                Debug.Log("player won");
                gameOver = true;

                StartCoroutine(RestartGameCoroutine());
            }
        }
        else { } // do nothing

        return canMove;
    }

    private void SwapTwoRandCells()
    {
        // -1 because there is a tile at (0, 0)
        int randSwapX1 = -1;
        int randSwapX2 = -1;

        int randSwapY1 = -1;
        int randSwapY2 = -1;

        for (int i = 0; i != 10; i++)
        {
            while (randSwapX1 == randSwapX2)
            {
                randSwapX2 = Random.Range(0, columns);
                randSwapX1 = Random.Range(0, columns);
            }

            while (randSwapY1 == randSwapY2)
            {
                randSwapY2 = Random.Range(0, rows);
                randSwapY1 = Random.Range(0, rows);
            }

            // check if the cells are valid swaps,
            // if yes then break out
            if (!tilesDictionary[new KeyValuePair<int, int>(randSwapX1, randSwapY1)].GetComponent<GameTile>().GetIsEmpty() &&
                !tilesDictionary[new KeyValuePair<int, int>(randSwapX2, randSwapY2)].GetComponent<GameTile>().GetIsEmpty())
            {
                break;
            }
        }


        SwapTilesAndDeepCpy(randSwapX1, randSwapY1, randSwapX2, randSwapY2);
    }

    private void SwapTilesAndDeepCpy(int x1, int y1, int x2, int y2)
    {
        GameTile toCheckTile = tilesDictionary[new KeyValuePair<int, int>(x2, y2)].GetComponent<GameTile>();
        GameTile movingTile = tilesDictionary[new KeyValuePair<int, int>(x1, y1)].GetComponent<GameTile>();

        GameTile copy = new GameTile();
        copy.CopyFrom(movingTile); // save the moving tile

        // set the moving tile to the empty tile stats
        movingTile.CopyFrom(toCheckTile);
        movingTile.SetIsEmpty(false); // true if swapping with empty cell
        movingTile.Init();

        // set the empty tile stats to the copied tile
        toCheckTile.CopyFrom(copy);
        toCheckTile.SetIsEmpty(false); // false
        toCheckTile.Init();
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
        int randX = Random.Range(0, rows);
        int randY = Random.Range(0, columns);
        Debug.Log("randcell " + randX + "" + randY);


        //tilesDictionary[new KeyValuePair<int, int>(0, 0)].GetComponent<GameTile>().SetIsEmpty(true);

        yield return new WaitForSeconds(0.25f);
#if !UNITY_EDITOR
        for (int i = 3; i != 0; i--)
        {
            gameStatusText.text = "Generating in " + i;
            yield return new WaitForSeconds(1.0f);
        }
#endif

        for (int i = 0; i != 5; ++i)
        {
            SwapTwoRandCells();
            gameStatusText.text = "Generating";
            yield return new WaitForSeconds(0.25f);
            SwapTwoRandCells();
            gameStatusText.text = "Generating.";
            yield return new WaitForSeconds(0.25f);
            SwapTwoRandCells();
            gameStatusText.text = "Generating..";
            yield return new WaitForSeconds(0.25f);
            SwapTwoRandCells();
            gameStatusText.text = "Generating...";
        }

        yield return new WaitForSeconds(0.25f);
        tilesDictionary[new KeyValuePair<int, int>(randX, randY)].GetComponent<GameTile>().SetIsEmpty(true);

        gameStatusText.text = "";
        raycastBlock.enabled = false;
        yield return null;
    }

}
