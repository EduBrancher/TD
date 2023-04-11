using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class GameController : MonoBehaviour
{   
    private class Tile{
        public bool isBlocked;
        public int row;
        public int col;

        public Tile(int row, int col){
            isBlocked = false;
            this.row = row;
            this.col = col;
        }

        public Vector2 GetCenterOfTile(){
            return new Vector2(col + 0.5f, row + 0.5f);
        }

        public override string ToString(){
            return "(" + row.ToString() + ", " + col.ToString() + ")";
        }
    }

    List<List<Tile>> grid;
    
    private InputAction mousePositionAction;
    [SerializeField] GameObject gameCursor;
    [SerializeField] bool building;
    [SerializeField] bool testPathfinding;
    int width = 840;
    int height = 720;
    int squareSize = 40;
    int cols = 21;
    int rows = 18;
    Vector2Int mouseMatrixPosition;
    Vector2 mousePosition;
    
    void printMatrix<T>(List<List<T>> list){
        string print = "";
        for (int i = 0; i < list.Count; i++){
            for (int j = 0; j < list[i].Count; j++){
                print += (list[i][j].ToString() + " ");
            }
            print += "\n";
        }
        Debug.Log(print);
    }

    void Awake() {
        mousePositionAction = new InputAction("MousePosition");
        mousePositionAction.AddBinding("<Mouse>/position");
        mousePositionAction.Enable();
        Cursor.visible = false;
        if (testPathfinding){
            //instantiate grid
            grid = new List<List<Tile>>();
            for (int i = 0; i < rows; i++){
                grid.Add(new List<Tile>());
                for (int j = 0; j < cols; j++){
                    grid[i].Add(new Tile(i, j));
                }
            }
            Debug.Log(grid.Count);
            Debug.Log(grid[0].Count);
            printMatrix<Tile>(grid);

            List<Vector2> path = GetPath(new Tile(rows-1,cols-1), new Tile(0,0), grid);
            foreach (Vector2 step in path){
                Debug.Log(step.x + ", " + step.y);
            }
        }
    }

    void OnDisable() {
        mousePositionAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   
        //Debug.Log(gameCursor.transform.position);
        ConstrainCursorToScreen();
    }

    List<Vector2> GetPath(Tile target, Tile source, List<List<Tile>> grid){
        List<List<Tile>> predecessorMatrix = BFS(target, source, grid);
        List<Vector2> path = new List<Vector2>();
        Tile currentTile = target;
        while (currentTile != source){
            path.Add(currentTile.GetCenterOfTile());
            currentTile = predecessorMatrix[currentTile.row][currentTile.col];
        }
        path.Reverse();
        return path;
    }

    List<List<Tile>> BFS(Tile target, Tile source, List<List<Tile>> grid){
        //initialize data structures
        List<List<Tile>> predecessorMatrix = new List<List<Tile>>();
        List<List<bool>> visited = new List<List<bool>>();
        for (int i = 0; i < rows; i++){
            predecessorMatrix.Add(new List<Tile>());
            visited.Add(new List<bool>());
            for (int j = 0; j < cols; j++){
                predecessorMatrix[i].Add(new Tile(i, j));
                visited[i].Add(false);
            }
        }
        //BFS
        Queue<Tile> queue = new Queue<Tile>();
        visited[source.row][source.col] = true;
        predecessorMatrix[source.row][source.col] = null;
        queue.Enqueue(source);

        while (queue.Count > 0){
            Tile previous = queue.Dequeue();
            List<Tile> neighbours = getNeighbours(previous);
            foreach (Tile neighbour in neighbours){
                if (neighbour.isBlocked == false && !visited[neighbour.row][neighbour.col]){
                    queue.Enqueue(neighbour);
                    visited[neighbour.row][neighbour.col] = true;
                    predecessorMatrix[neighbour.row][neighbour.col] = previous;
                }
            }
        }
        return predecessorMatrix;
    }

    List<Tile> getNeighbours(Tile tile){
        List<Tile> neighbours = new List<Tile>();
        if (tile.col > 0){
            neighbours.Add(grid[tile.row][tile.col-1]);
        }
        if (tile.row > 0){
            neighbours.Add(grid[tile.row-1][tile.col]);
        }
        if (tile.col < cols-1){
            neighbours.Add(grid[tile.row][tile.col+1]);
        }
        if (tile.row < rows-1){
            neighbours.Add(grid[tile.row+1][tile.col]);
        }
        return neighbours;
    }

    Tile getTileAtPosition(int row, int col){
        return grid[row][col];
    }

    

    void ConstrainCursorToScreen(){
        float minX = 0.5f;
        float maxX = 23.5f;
        float minY = -17.5f;
        float maxY = -0.5f;

        if (building){
            minY = -17;
            maxX = 20;
        }
        
        float x = gameCursor.transform.position.x;
        float y = gameCursor.transform.position.y;

        if (gameCursor.transform.position.x <= minX){
            x = minX;
        }
        if (gameCursor.transform.position.x >= maxX){
            x = maxX;
        }
        if (gameCursor.transform.position.y <= minY){
            y = minY;
        }
        if (gameCursor.transform.position.y >= maxY){
            y = maxY;
        }
        
        gameCursor.transform.position = new Vector2(x, y);
    }

    void OnLook(InputValue value){
        mousePosition = mousePositionAction.ReadValue<Vector2>();
        //Debug.Log(mousePosition);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //calibrate sprite position
        cursorPosition.x += 0.175f;
        cursorPosition.y -= 0.3f;
        cursorPosition.z = 0;
        gameCursor.transform.position = cursorPosition;

        if (mousePosition != null)
            mouseMatrixPosition = ConvertMouseToMatrixPosition(mousePosition);
            if (building){
                BuildingSnapToGrid();
            }
    }

    Vector2Int ConvertMouseToMatrixPosition(Vector2 mousePosition){
        
        int matrixRow = (height - (int)mousePosition.y) / 40;
        int matrixCol = (int)mousePosition.x / 40;
        //Debug.Log((matrixRow, matrixCol));
        return new Vector2Int(matrixRow, matrixCol);
    }

    void BuildingSnapToGrid(){
        
        int y = (int)gameCursor.transform.position.y;
        int x = (int)gameCursor.transform.position.x;
        Debug.Log((x, y));
        Vector2 snapPosition;
        if (y == rows){
            y++;
        }
        if (y == 0){
            y--;
        }
        if (x == cols){
            x--;
        }
        if (x == 0){
            x++;
        }
        snapPosition = new Vector2(x, y);
        gameCursor.transform.position = snapPosition;
    }
    
    Vector2Int GetGridPositionFromTransform(Transform transform){
        return new Vector2Int((int) transform.position.x, (int) transform.position.y);
    }

}
