using System.Collections;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    WayPoints _wayPointsSet;
    Node _node;

    [Header("Tiles")]
    public int mapHeight;
    public int mapWidth;
    public Material emptyTile, downPath, leftRight, leftDown, rightDown, downLeft, downRight; //debe pasarse a Gameobject o a material
    public float _seHeight;
    public float TileSice;

    [Header("References")]
    public GameObject tileReference;
    public GameObject _groundParent;
    public GameObject[] _grounds;
    public Material _hoverMaterialSetCan;
    public Material _hoverMaterialSetCant;
    public GameObject _pathParent;
    public GameObject _wayPointParent;
    private GameObject _wayPointTemp;
    public GameObject _wayPoint;
    public GameObject _start;
    public GameObject _end;


    [Header("ProcesValues-DontMove")]
    private Material spriteToUse;
    private int curY;
    private int curX;
    private bool forceDirectionChange = false;

    private bool continueLeft = false;
    private bool continueRight = false;
    private int currentCount = 0;

    private enum CurrentDirection
    {
        LEFT,
        RIGHT,
        DOWN,
        UP
    };
    private CurrentDirection curDirection = CurrentDirection.DOWN;

    public struct TileData
    {
        public Transform transform;
        public MeshRenderer MaterialRenderer;
        public int tileID;
    }

    TileData[,] tileData;

    void Awake()
    {
        tileData = new TileData[mapHeight, mapWidth];
        _wayPointsSet = _wayPointParent.GetComponent<WayPoints>();
        _node = _wayPointParent.GetComponent<Node>();
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < mapHeight; x++)
        {
            for (int y = 0; y < mapWidth; y++)
            {
                GameObject newTile = Instantiate(tileReference, new Vector3(x * TileSice - (TileSice * mapHeight / 2), 0, y * TileSice - (TileSice * mapWidth / 2)), Quaternion.identity);
                tileData[x, y].MaterialRenderer = newTile.GetComponent<MeshRenderer>();
                tileData[x, y].tileID = 0;
                //tileData[x, y].transform.parent = _groundParent.transform;
                tileData[x, y].MaterialRenderer.material = emptyTile; //Se debe pasar la textura del bloque
                //tileData[x, y].transform.parent = _groundParent.transform;
                tileData[x, y].transform = newTile.transform;
                tileData[x, y].transform.parent = _groundParent.transform;
            }
        }
        StartCoroutine(GeneratePath());
    }
    
    void RegenerateMap()
    {
        StopAllCoroutines();
        GenerateMap();
        StartCoroutine(GeneratePath());
    }

    IEnumerator GeneratePath()
    {
        StartingPoint();

        while (curX < mapWidth)
        {

            CheckCurrentDirections();
            ChooseDirection();

            if (curX < mapWidth)
            {
                UpdateMap(curY, curX, spriteToUse);
                tileData[curY, curX].transform.parent = _pathParent.transform;
            }
            if (curX < mapWidth - 1 && curDirection == CurrentDirection.DOWN)
            {
                UpdateMap(curY, curX, spriteToUse);
                tileData[curY, curX].transform.parent = _pathParent.transform;
                curX++;
            }

            yield return new WaitForSeconds(0.05f);
        }
        EndingPoint();
        
        _wayPointsSet.GetWayPoints();

        GetGround();
    }

    void StartingPoint()
    {
        Debug.Log("Start");
        curX = 0;
        curY = UnityEngine.Random.Range(0, mapHeight);
        spriteToUse = downPath;

        _start.transform.position = new Vector3(tileData[curY, curX].transform.position.x, (_seHeight / 2), tileData[curY, curX].transform.position.z);
        tileData[curY, curX].transform.parent = _pathParent.transform;
        _start.SetActive(true);
    }
    void EndingPoint()
    {
        Debug.Log("Finish");
        spriteToUse = downPath;

        if (curX > 30) curX = 30;
        _wayPointTemp = GameObject.Instantiate(_wayPoint, tileData[curY, curX - 1].transform.position, transform.rotation);
        _end.transform.position = new Vector3(tileData[curY, curX - 1].transform.position.x, (_seHeight / 2), tileData[curY, curX - 1].transform.position.z);
        _wayPointTemp.transform.parent = _wayPointParent.transform;

        tileData[curY, curX - 1].transform.parent = _pathParent.transform;
        _end.SetActive(true);
    }

    private void CheckCurrentDirections()
    {
        if (curDirection == CurrentDirection.LEFT && curY - 1 >= 0 && tileData[curY - 1, curX].tileID == 0)
        {
            curY--;
            tileData[curY, curX].transform.parent = _pathParent.transform;
        }
        else if (curDirection == CurrentDirection.RIGHT && curY + 1 <= mapHeight - 1 && tileData[curY + 1, curX].tileID == 0)
        {
            curY++;
            tileData[curY, curX].transform.parent = _pathParent.transform;
        }
        else if (curDirection == CurrentDirection.UP && curX - 1 >= 0 && tileData[curY, curX - 1].tileID == 0)
        {
            if (continueLeft && tileData[curY - 1, curX - 1].tileID == 0 ||
            continueRight && tileData[curY + 1, curX - 1].tileID == 0)
            {
                curX--;
                tileData[curY, curX].transform.parent = _pathParent.transform;
            }
            else
            {
                forceDirectionChange = true;
                tileData[curY, curX].transform.position = new Vector3(tileData[curY, curX].transform.position.x, 0,tileData[curY, curX].transform.position.z);
                tileData[curY, curX].transform.parent = _pathParent.transform;
            }
        }
        else if (curDirection != CurrentDirection.DOWN)
        {
            forceDirectionChange = true;
            tileData[curY, curX].transform.position = new Vector3(tileData[curY, curX].transform.position.x, 0, tileData[curY, curX].transform.position.z);
            tileData[curY, curX].transform.parent = _pathParent.transform;
        }
    }

    private void ChooseDirection()
    {
        if (currentCount < 3 && !forceDirectionChange)
        {
            //tileData[curY, curX].transform.parent = _pathParent.transform;
            currentCount++;
        }
        else
        {
            bool chanceToChange = Mathf.FloorToInt(UnityEngine.Random.value * 2f) == 0;

            if (chanceToChange || forceDirectionChange || currentCount > 7)
            {
                //tileData[curY, curX].transform.parent = _pathParent.transform;
                currentCount = 0;
                forceDirectionChange = false;
                ChangeDirection();
            }

            currentCount++;
        }
    }

    private void ChangeDirection()
    {
        int dirValue = Mathf.FloorToInt(UnityEngine.Random.value * 3f);

        if (dirValue == 0 && curDirection == CurrentDirection.LEFT && curY - 1 > 0
        || dirValue == 0 && curDirection == CurrentDirection.RIGHT && curY + 1 < mapHeight - 1)
        {
            if (curX - 1 >= 0)
            {
                if (tileData[curY, curX - 1].tileID == 0 &&
                tileData[curY - 1, curX - 1].tileID == 0 &&
                tileData[curY + 1, curX - 1].tileID == 0)
                {
                    GoUp();
                    //tileData[curY, curX].transform.parent = _pathParent.transform;
                    return;
                }
            }
        }

        switch (curDirection)
        {
            case CurrentDirection.LEFT:
                UpdateMap(curY, curX, leftDown);
                //tileData[curY, curX].transform.parent = _pathParent.transform;
                break;

            case CurrentDirection.RIGHT:
                UpdateMap(curY, curX, rightDown);
                //tileData[curY, curX].transform.parent = _pathParent.transform;
                break;

            case CurrentDirection.UP:
                break;

            case CurrentDirection.DOWN:
                break;
        }

        if (curX - 1 > mapWidth && curX + 1 < mapWidth  || curDirection == CurrentDirection.LEFT || curDirection == CurrentDirection.RIGHT)
        {
            GoDown();
            //tileData[curY, curX].transform.parent = _pathParent.transform;
            return;
        }

        if (curY - 1 > 0 && curY + 1 < mapHeight - 1 || continueLeft || continueRight)
        {
            if (dirValue == 1 && !continueRight || continueLeft)
            {
                ContinueLeft();
                //tileData[curY, curX].transform.parent = _pathParent.transform;
            }
            else
            {
                ContinueRight();
                //tileData[curY, curX].transform.parent = _pathParent.transform;
            }
        }
        else if (curY - 1 > 0)
        {
            //tileData[curY, curX].transform.parent = _pathParent.transform;
            spriteToUse = downLeft;
            curDirection = CurrentDirection.LEFT;
        }
        else if (curY + 1 < mapWidth - 1)
        {
            //tileData[curY, curX].transform.parent = _pathParent.transform;
            spriteToUse = downRight;
            curDirection = CurrentDirection.RIGHT;
        }

        if (curY + 1 < mapWidth - 1 && curDirection == CurrentDirection.LEFT)
        {
            GoLeft();
            //tileData[curY, curX].transform.parent = _pathParent.transform;
        }
        else if (curY + 1 < mapWidth - 1 && curDirection == CurrentDirection.RIGHT)
        {
            GoRight();
            //tileData[curY, curX].transform.parent = _pathParent.transform;
        }
    }

    private void GoUp()
    {
        if (curDirection == CurrentDirection.LEFT)
        {
            UpdateMap(curY, curX, downRight);
            //tileData[curY, curX].transform.parent = _pathParent.transform;
            //_wayPointTemp.transform.parent = _wayPointParent.transform;
            continueLeft = true;
        }
        else
        {
            UpdateMap(curY, curX, downLeft);
            //tileData[curY, curX].transform.parent = _pathParent.transform;
            //_wayPointTemp.transform.parent = _wayPointParent.transform;
            continueRight = true;
        }
        curDirection = CurrentDirection.UP;
        curX--;
        spriteToUse = downPath;
        _wayPointTemp = GameObject.Instantiate(_wayPoint, tileData[curY, curX + 1].transform.position, tileData[curY, curX + 1].transform.rotation);
        tileData[curY, curX + 1].transform.parent = _pathParent.transform;
        _wayPointTemp.transform.parent = _wayPointParent.transform;
    }

    private void GoDown()
    {
        UpdateMap(curY, curX, spriteToUse);
        curX++;
        spriteToUse = downPath;
        curDirection = CurrentDirection.DOWN;
        _wayPointTemp = GameObject.Instantiate(_wayPoint, tileData[curY, curX - 1].transform.position, tileData[curY, curX - 1].transform.rotation);
        tileData[curY, curX - 1].transform.parent = _pathParent.transform;
        _wayPointTemp.transform.parent = _wayPointParent.transform;
    }

    private void GoLeft()
    {
        UpdateMap(curY, curX, spriteToUse);
        tileData[curY, curX].transform.parent = _pathParent.transform;
        curY--;
        spriteToUse = leftRight;
        _wayPointTemp = GameObject.Instantiate(_wayPoint, tileData[curY + 1, curX].transform.position, tileData[curY + 1, curX].transform.rotation);
        _wayPointTemp.transform.parent = _wayPointParent.transform;
    }

    private void GoRight()
    {
        UpdateMap(curY, curX, spriteToUse);
        tileData[curY, curX].transform.parent = _pathParent.transform;
        curY++;
        spriteToUse = leftRight;
        _wayPointTemp = GameObject.Instantiate(_wayPoint, tileData[curY - 1, curX].transform.position, tileData[curY - 1, curX].transform.rotation);
        _wayPointTemp.transform.parent = _wayPointParent.transform;
    }

    private void ContinueLeft()
    {
        if (tileData[curY - 1, curX].tileID == 0)
        {
            if (continueLeft)
            {
                spriteToUse = rightDown;
                continueLeft = false;
            }
            else
            {
                spriteToUse = downLeft;
            }
            tileData[curY, curX].transform.parent = _pathParent.transform;
            curDirection = CurrentDirection.LEFT;
        }
        
    }

    private void ContinueRight()
    {
        if (tileData[curY + 1, curX].tileID == 0)
        {
            if (continueRight)
            {
                continueRight = false;
                spriteToUse = leftDown;
            }
            else
            {
                spriteToUse = downRight;
            }
            tileData[curY, curX].transform.parent = _pathParent.transform;
            curDirection = CurrentDirection.RIGHT;
        }
    }

    private void UpdateMap(int mapX, int mapY, Material spriteToUse)
    {
        tileData[mapX, mapY].transform.position = new Vector3(tileData[mapX, mapY].transform.position.x, 0 , tileData[mapX, mapY].transform.position.z );
        tileData[mapX, mapY].tileID = 1;
        tileData[mapX, mapY].MaterialRenderer.material = spriteToUse;
    }

    public void GetGround()
    {
        _grounds = new GameObject[_groundParent.transform.childCount];

        for (int i = 0; i < _grounds.Length; i++)
        {
            _grounds[i] = _groundParent.transform.GetChild(i).gameObject;
            _grounds[i].AddComponent<Node>()._hoverMaterialCanBuy = _hoverMaterialSetCan;
            _grounds[i].GetComponent<Node>()._hoverMaterialCantBuy = _hoverMaterialSetCant;
        }
    }   
}