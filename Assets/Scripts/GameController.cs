using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour{
    [Header("Prefabs")]
    [SerializeField] GameObject emptyTile;

    [Header("References")]
    [SerializeField] GridLayoutGroup buttonContainer;

    void Start(){
        GenerateField(MenuController.Instance.fieldSize);
    }

    void GenerateField(Vector2Int fieldSize){
        //Give a border to the field
        fieldSize.x += 2;
        fieldSize.y += 2;
        for(int column = 0; column < fieldSize.x; column++){
            for (int row = 0; row < fieldSize.y; row++){
                Button newButton = Instantiate(emptyTile,buttonContainer.transform).GetComponent<Button>();
                if(
                    (column == 0) ||
                    (column == fieldSize.x-1)||
                    (row == 0) ||
                    (row == fieldSize.y-1)
                ){
                    newButton.interactable = false;
                }
            }
        }
        buttonContainer.constraintCount = fieldSize.x;
        float cellSize = 58/3 + 1694/(3*fieldSize.y);
        buttonContainer.cellSize = new Vector2(cellSize,cellSize);
    }
}