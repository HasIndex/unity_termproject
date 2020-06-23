using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

// 월드 포지션 기준으로 ... 
// 32 x 32 32 32 
public class PathFindingTile ///: IComparer<PathFindingTile>
{
    public TileMapPathFinder map;
    public PathFindingTile parent;
    public Vector2Int index;
    public Vector2 position;
    public bool has_obstacle;
    public int f;
    public int g;
    public int h;

    public void CalculateCost(Vector2Int goalIdx)
    {
        // g 구하기
        if (parent != null)
        {
            int distance_x = Math.Abs(parent.index.x - index.x);
            int distance_y = Math.Abs(parent.index.y - index.y);
            if ((distance_y + distance_x) > 1)
            {
                g = parent.g + 14;
            }
            else
            {
                g = parent.g + 10;
            }
        }
        else
        {
            g = 0;
        }


        h = (Math.Abs(goalIdx.y - index.y) * 10) + (Math.Abs(goalIdx.x - index.x) * 10);
        f = g + h;
    }

}

public class ComparerOfTile : IComparer<PathFindingTile>
{
    int IComparer<PathFindingTile>.Compare(PathFindingTile x, PathFindingTile y)
    {
        return x.f - y.f;
    }
}


public class TileMapPathFinder : MonoBehaviour
{
    private HashSet<PathFindingTile> closedSet;
    private HashSet<PathFindingTile> openSet;
    private List<PathFindingTile> adjacentList;
    private PathFindingTile[,] tiles;
    private BoxCollider2D collider2d;
    private Vector2 base_position;

    public PlayerMovement playerMovement;
    public GameObject player;
    public Vector2 tile_size;
    public Vector2Int tile_count;

    private Vector2Int targetIndex;
    private Vector2Int startIndex;

    public TileMapPathFinder()
    {
    }

    public void Awake()
    {
        base_position = transform.parent.position;
        player = GameObject.Find("MainPlayer");
        collider2d = GetComponent<BoxCollider2D>();
        openSet = new HashSet<PathFindingTile>();
        closedSet = new HashSet<PathFindingTile>();
        adjacentList = new List<PathFindingTile>();

        Vector2 parent_position = transform.parent.position;
        collider2d.enabled = false;
        tiles = new PathFindingTile[tile_count.y, tile_count.x];

        for (int y = 0; y < tile_count.y; ++y)
        {
            for (int x = 0; x < tile_count.x; ++x)
            {
                tiles[y, x] = new PathFindingTile();
                tiles[y, x].map = this;
                tiles[y, x].index.y = y;
                tiles[y, x].index.x = x;
                tiles[y, x].position.x = parent_position.x + (x * tile_size.x) + (tile_size.x / 2);
                tiles[y, x].position.y = parent_position.y - ((y * tile_size.y) + (tile_size.y / 2));

                RaycastHit2D hit = Physics2D.Raycast(tiles[y, x].position, Vector2.zero, 0f);
                if (hit.collider != null)
                {
                    //Debug.Log($" {hit.collider.name}: parent : {transform.parent.name},  tiles[{y}, {x}] obstacle position y: { tiles[y, x].position.y}, x:{tiles[y, x].position.x }");
                    tiles[y, x].has_obstacle = true;
                }
                else
                {
                    tiles[y, x].has_obstacle = false;
                }
            }
        }
        collider2d.enabled = true;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.F))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetIndex = ChangeWorldPointToTileIndex(pos);

            //Debug.Log($" mouse screen  position :  { Input.mousePosition }");
            //Debug.Log($" mouse world position :  { pos } ");
            //Debug.Log($" tile idx y:  { targetIndex.y }  x:  { targetIndex.x } ");
            //Debug.Log($" player pos y:  { player.transform.position.y }  x:  {player.transform.position.x } ");

            DoPathFinding();

            PathFindingTile curTile = tiles[targetIndex.y, targetIndex.x];
            while (curTile.parent != null)
            {
                Debug.Log($"curTile ({ curTile.index.y }, { curTile.index.x }) : position : { curTile.position.y }, { curTile.position.x }     ");

                curTile = curTile.parent;
            }
        }
    }

    Vector2Int ChangeWorldPointToTileIndex(Vector2 pos)
    {
        return new Vector2Int
        {
            y = -Convert.ToInt32(pos.y - base_position.y)
            ,
            x = Convert.ToInt32(pos.x - base_position.x)
        };
    }

    private PathFindingTile GetMinFSectorFromOpenSet()
    {
        PathFindingTile curTile = openSet.First<PathFindingTile>();
        int minFCost = curTile.f;

        foreach (var tile in openSet)
        {
            if (tile.f < minFCost)
            {
                curTile = tile;
                minFCost = tile.f;
            }
        }

        openSet.Remove(curTile);

        return curTile;
    }

    public bool DoPathFinding()
    {
        Vector2 pos = player.transform.position;
        startIndex = ChangeWorldPointToTileIndex(pos);
        if (startIndex == targetIndex) // 찾은 경우
        {
            return true;
        }

        openSet.Add(tiles[startIndex.y, startIndex.x]);
        PathFindingTile curTile = null;

        for (; ; )
        {
            if (openSet.Count == 0) // 없는 경우.
            {
                return false;
            }

            curTile = GetMinFSectorFromOpenSet();
            findAdjacentSectors(curTile);
            if (adjacentList.Count == 0)
            {
                continue;
            }

            foreach (var tile in adjacentList)
            {
                if (tile == tiles[targetIndex.y, targetIndex.x])
                {
                    curTile = tile;
                    return true;
                }

                if (false == openSet.Contains(tile))
                {
                    openSet.Add(tile);
                }
            }
        }
    }

    private void findAdjacentSectors(PathFindingTile curTile)
    {
        closedSet.Add(curTile);
        adjacentList.Clear();

        int minX = (curTile.index.x - 1) < 0 ? 0 : (curTile.index.x - 1);
        int maxX = (curTile.index.x + 1) >= tile_count.x ? tile_count.x - 1 : (curTile.index.x + 1);
        int minY = (curTile.index.y - 1) < 0 ? 0 : (curTile.index.y - 1);
        int maxY = (curTile.index.y + 1) >= tile_count.y ? tile_count.y - 1 : (curTile.index.y + 1);

        for (int y = minY; y <= maxY; ++y)
        {
            for (int x = minX; x <= maxX; ++x)
            {
                if (true == tiles[y, x].has_obstacle) // 장애물이 존재해서 못가는 곳이면
                {
                    continue;
                }
                if (true == closedSet.Contains(tiles[y, x]))
                {
                    continue;
                }

                if (null == tiles[y, x].parent)
                {
                    tiles[y, x].parent = curTile;
                    tiles[y, x].CalculateCost(this.targetIndex);
                }
                else
                {
                    PathFindingTile tile = new PathFindingTile();
                    tile.index = tiles[y, x].index;
                    tile.parent = curTile;
                    tile.CalculateCost(this.targetIndex);

                    if (tile.f < tiles[y, x].f)
                    {
                        tiles[y, x].f = tile.f;
                        tiles[y, x].g = tile.g;
                        tiles[y, x].h = tile.h;
                        tiles[y, x].h = tile.h;
                        tiles[y, x].parent = tile.parent;
                    }
                }

                adjacentList.Add(tiles[y, x]);
            }
        }
    }
}

