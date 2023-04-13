using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class GameController : MonoBehaviour
{   
    
    
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
    
    void Awake() {
        mousePositionAction = new InputAction("MousePosition");
        mousePositionAction.AddBinding("<Mouse>/position");
        mousePositionAction.Enable();
        Cursor.visible = false;
        
    }

    void OnDisable() {
        mousePositionAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {   
        //Debug.Log(gameCursor.transform.position);
        ConstrainCursorToScreen();
    }

    void ConstrainCursorToScreen(){
        float minX = 0f;
        float maxX = 24.5f;
        float minY = -18f;
        float maxY = -0f;

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
        
        int row = Mathf.Clamp(-(int)gameCursor.transform.position.y, 0, rows-1);
        int col = Mathf.Clamp((int)gameCursor.transform.position.x, 0, cols-1);

        //Debug.Log(row + ", " + col);
        return new Vector2Int(row, col);
    }

    void BuildingSnapToGrid(){
        
        int y = (int)gameCursor.transform.position.y;
        int x = (int)gameCursor.transform.position.x;
        //Debug.Log((x, y));
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
