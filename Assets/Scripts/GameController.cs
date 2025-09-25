using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour{
    [Header("Prefabs")]
    [SerializeField] GameTile emptyTile;

    [Header("References")]
    [SerializeField] GridLayoutGroup buttonContainer;
    GameTile[,] tileField;
    GameTile[] inputOutputCandidates;
    GameTile inputTile; //where the water starts flowing from
    GameTile outputTile; //where the water has to be flowed to

    [Header("Variables")]
    int nextCandidateTileIndexToAdd = 0;

    void Start(){
        FieldSetup(SettingsManager.Instance.fieldSize);
    }

    void FieldSetup(Vector2Int fieldSize){
        //Give a border to the field
        fieldSize.x += 2;
        fieldSize.y += 2;
        tileField = new GameTile[ fieldSize.x , fieldSize.y];
        inputOutputCandidates = new GameTile[2*(fieldSize.x + fieldSize.y) - 8]; //magic equation achieved through math. It's the ammount of tiles in the borders, minus the corners.
        GenerateTiles(fieldSize);
        AdjustLayoutContainer(fieldSize);
        CreateInputAndOutputTile();
    }

    void GenerateTiles(Vector2Int fieldSize){
        //generate every tile in the field
        for(int column = 0; column < fieldSize.x; column++){
            for (int row = 0; row < fieldSize.y; row++){
                GameTile newTile = Instantiate(emptyTile,buttonContainer.transform);
                Vector2Int tilePosition = new Vector2Int(column,row);
                tileField[column,row] = newTile;
                if( IsBorderPosition( tilePosition , fieldSize ) ){
                    newTile.MarkAsBorder();
                }
                if( IsValidInputOutputCandidate( tilePosition, fieldSize)){
                    inputOutputCandidates[ nextCandidateTileIndexToAdd ] = newTile;
                    nextCandidateTileIndexToAdd++;
                }
                newTile.AssignPosition(column,row);
            }
        }
    }

    void CreateInputAndOutputTile(){
        inputTile = ChooseInputTile();
        if(IsTopOrBottomBorder(inputTile.GetTilePosition() , SettingsManager.Instance.GetPlayFieldSize())){
            inputTile.ChangeToVertical();
        }else{
            inputTile.ChangeToHorizontal();
        }
        outputTile = ChooseOutputTile();
        if(IsTopOrBottomBorder(inputTile.GetTilePosition() , SettingsManager.Instance.GetPlayFieldSize())){
            outputTile.ChangeToVertical();
        }else{
            outputTile.ChangeToHorizontal();
        }
    }

    GameTile ChooseInputTile(){
        return inputOutputCandidates[Random.Range(0,inputOutputCandidates.Length)];
    }

    GameTile ChooseOutputTile(){
        GameTile outputTile;
        do{
            outputTile = inputOutputCandidates[Random.Range(0,inputOutputCandidates.Length)];
        }while (outputTile == inputTile);
        return outputTile;
    }

    bool IsBorderPosition(Vector2Int tilePosition, Vector2Int fieldSize){
        return (
            (tilePosition.x == 0) ||
            (tilePosition.x == fieldSize.x-1) ||
            (tilePosition.y == 0) ||
            (tilePosition.y == fieldSize.y-1)
        );
    }

    bool IsValidInputOutputCandidate(Vector2Int tilePosition, Vector2Int fieldSize){
        if (!IsBorderPosition( tilePosition, fieldSize )) return false;
        /*elseif*/ if (IsCornerPosition(tilePosition, fieldSize)) return false;
        /*else*/ return true;
    }

    bool IsCornerPosition(Vector2Int tilePosition, Vector2Int fieldSize){
        if (
            (((tilePosition.x == 0) || (tilePosition.x == fieldSize.x -1))) &&
            ((tilePosition.y == 0) || (tilePosition.y == fieldSize.y -1))
        ){
            return true;
        }else{
            return false;
        }
    }

    bool IsTopOrBottomBorder(Vector2Int tilePosition, Vector2Int fieldSize){ //as in, either top or bottom -> true, else false.
        return (tilePosition.y == 0) || (tilePosition.y == fieldSize.y -1);
    }

    void AdjustLayoutContainer(Vector2Int fieldSize){
        //adjusts the layout parameters so they show up neatly
        buttonContainer.constraintCount = fieldSize.x;
        float cellSize = 58/3 + 1694/(3*fieldSize.y); //magic numbers obtained through math and some starting assumptions.
        buttonContainer.cellSize = new Vector2(cellSize,cellSize);

    }
}