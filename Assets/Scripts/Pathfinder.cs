using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{   

    int cols = 21;
    int rows = 18;

    private class Tile{
        public bool isBlocked;
        public int row;
        public int col;
        public Tile predecessor;
        public Tile(int row, int col){
            isBlocked = false;
            predecessor = null;
            this.row = row;
            this.col = col;
        }

        public Vector2 GetCenterOfTile(){
            return new Vector2(col + 0.5f, -(row + 0.5f));
        }

        public override string ToString(){
            string ret = "(" + row.ToString() + ", " + col.ToString() + ")";
            if (isBlocked){
                ret = ret + ": blocked";
            }
            else{
                ret = ret + ": free";
            }
            return ret;
        }
    }

    List<List<Tile>> grid;

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

    public List<Vector2> GetPath(Vector2 target, Vector2 source){
        Tile sourceTile = new Tile(-(int)source.y, (int)source.x);
        Tile targetTile = new Tile(-(int)target.y, (int)target.x);
        Debug.Log("Target tile: " + targetTile.row + ", " + targetTile.col);
        return GetPath(targetTile, sourceTile, grid);
    }

    List<Vector2> GetPath(Tile target, Tile source, List<List<Tile>> grid){
        List<List<Tile>> predecessorMatrix = BFS(target, source, grid);
        Debug.Log(predecessorMatrix[target.row][target.col]);
        if (predecessorMatrix[target.row][target.col].predecessor == null){
            Debug.Log("No path found.");
            return null;
        }
        List<Vector2> path = new List<Vector2>();
        Tile currentTile = target;
        while (currentTile != source){
            path.Add(currentTile.GetCenterOfTile());
            currentTile = predecessorMatrix[currentTile.row][currentTile.col].predecessor;
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
        predecessorMatrix[source.row][source.col].predecessor = null;
        queue.Enqueue(source);

        while (queue.Count > 0){
            Tile previous = queue.Dequeue();
            Debug.Log("Current: " + previous.row + ", " + previous.col);
            if (predecessorMatrix[previous.row][previous.col].predecessor != null){
                //Debug.Log("Predecessor: " + predecessorMatrix[previous.row][previous.col].predecessor.row + ", " + predecessorMatrix[previous.row][previous.col].predecessor.col);
            }
            List<Tile> neighbours = getNeighbours(previous);
            //Debug.Log("Neighbours:");
            foreach (Tile neighbour in neighbours){
                if (neighbour.isBlocked == false && !visited[neighbour.row][neighbour.col]){
                    queue.Enqueue(neighbour);
                    //Debug.Log(neighbour.row + ", " + neighbour.col);
                    visited[neighbour.row][neighbour.col] = true;
                    predecessorMatrix[neighbour.row][neighbour.col].predecessor = previous;
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

    void Awake(){
        
        //instantiate grid
        grid = new List<List<Tile>>();
        for (int i = 0; i < rows; i++){
            grid.Add(new List<Tile>());
            for (int j = 0; j < cols; j++){
                grid[i].Add(new Tile(i, j));
            }
        }
        for (int i = 5; i < 7; i++){
            for (int j = 7; j < 13; j++){
                grid[i][j].isBlocked = true;
            }
        }

        /*bool switcher = true;
        for (int i = 0; i < rows-1; i++){
            for (int j = 0; j < cols; j++){
                if (i%2 == 1){
                    if (switcher){
                        if (j != cols-1){
                            grid[i][j].isBlocked = true;
                        }
                    }
                    else{
                        if (j != 0){
                            grid[i][j].isBlocked = true;
                        }
                    }
                }
            }
            if (switcher && i%2 == 1){
                switcher = false;
            }
            else if (!switcher && i%2 == 1){
                switcher = true;
            }
        }*/
        printMatrix<Tile>(grid);
    }
}
