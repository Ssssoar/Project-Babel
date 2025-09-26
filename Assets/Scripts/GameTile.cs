using UnityEngine;
using UnityEngine.UI;

public class GameTile : MonoBehaviour{
    [Header("References")]
    public Button buttonComponent;
    [SerializeField] Image[] emptyTileSpriteDisplayers;
    [SerializeField] Image[] filledTileSpritesDisplayers;
    [SerializeField] Animator animator;

    [Header("Parameters")]
    [SerializeField] TileTypeData[] possibleTiles;

    [Header("Variables")]
    TileType currentTileType = TileType.Empty;
    Vector2Int tilePosition;
    Direction receivingFlowFrom;
    int timesFilled;

    //----ENUMS-----
    public enum TileType{Border,Vertical,Horizontal,Cross,Northwest,Northeast,Southwest,Southeast,Empty};
    public enum Direction{Up,Down,Left,Right};

    //----STRUCTS----
    [System.Serializable] //if I were doing this forreal I would do this a scriptable object
    struct TileTypeData{
        public TileType type;
        public Sprite emptySprite;
        public Sprite filledSprite;
        public AnimationClip[] fillAnimations; //in the order top right bottom left, with the next eight for the second fills
    }

    public void AssignPosition(int column, int row){
        if(tilePosition == null){
            tilePosition = new Vector2Int(column,row);
        }else{
            tilePosition.x = column;
            tilePosition.y = row;
        }
    }

    public Vector2Int GetTilePosition(){
        return tilePosition;
    }

    public void ChangeToRandom(){
        TileType randomTileType;
        do{
            randomTileType = ChooseRandomTile();
        }while((randomTileType == TileType.Border) || (randomTileType == TileType.Empty));
        ChangeToTileType(randomTileType);
    }

    TileType ChooseRandomTile(){
        var type = typeof(TileType);
        TileType[] values = (TileType[]) type.GetEnumValues();
        int randomValue = Random.Range(0, values.Length);
        return (TileType) values[randomValue];
    }

    public void ChangeToVertical(){
        ChangeToTileType(TileType.Vertical);
    }

    public void ChangeToHorizontal(){
        ChangeToTileType(TileType.Horizontal);
    }

    public void ChangeToEmpty(){
        ChangeToTileType(TileType.Empty);
    }

    public void ChangeToBorder(){
        ChangeToTileType(TileType.Border);
    }

    public void ChangeToMatchTile(GameTile tileToMatch){
        ChangeToTileType( tileToMatch.GetTileType() );
    }

    public void ChangeToTileType(TileType typeToChangeTo){
        if (typeToChangeTo != TileType.Border){
            UndoBorder();
        }else{
            buttonComponent.interactable = false;
        }
        ChangeSprites(typeToChangeTo);
        currentTileType = typeToChangeTo;
    }

    void UndoBorder(){
        if (currentTileType != TileType.Border) return;
        buttonComponent.interactable = true;
    }

    void ChangeSprites(TileType typeToChangeTo){
        foreach(TileTypeData possibleTile in possibleTiles){
            if(possibleTile.type == typeToChangeTo){
                foreach(Image emptyTileSpriteDisplayer in emptyTileSpriteDisplayers){
                    emptyTileSpriteDisplayer.sprite = possibleTile.emptySprite;
                }
                foreach(Image filledTileSpritesDisplayer in filledTileSpritesDisplayers){
                    filledTileSpritesDisplayer. sprite = possibleTile.filledSprite;
                }
            }
        }
    }

    public TileType GetTileType(){
        return currentTileType;
    }

    public void SetupFlowFrom(Vector2Int flowOriginCoords){
        if ((flowOriginCoords.x == tilePosition.x +1 ) && (flowOriginCoords.y == tilePosition.y)){
            receivingFlowFrom = Direction.Right;
        }else if((flowOriginCoords.x == tilePosition.x -1 ) && (flowOriginCoords.y == tilePosition.y)){
            receivingFlowFrom = Direction.Left;
        }else if((flowOriginCoords.y == tilePosition.y +1 ) && (flowOriginCoords.x == tilePosition.x)){
            receivingFlowFrom = Direction.Down;
        }else if((flowOriginCoords.y == tilePosition.y -1 ) && (flowOriginCoords.x == tilePosition.x)){
            receivingFlowFrom = Direction.Up;
        }else{
            Debug.Log("THIS REALLY SHOULDNT BE POSSIBLE. Attempt to flow from non-adjacent tile");
            Debug.Log("Flow from");
            Debug.Log(flowOriginCoords);
            Debug.Log("Flow to");
            Debug.Log(tilePosition);
        }
    }

    public void SetupInitialFlow(){
        if (tilePosition.x == 0){
            receivingFlowFrom = Direction.Left;
        }else if (tilePosition.y == 0){
            receivingFlowFrom = Direction.Up;
        }else if(tilePosition.x == SettingsManager.Instance.GetPlayFieldSize().x+1){
            receivingFlowFrom = Direction.Right;
        }else{
            receivingFlowFrom = Direction.Down;
        }
    }

    public bool IsFlowPossible(){
        if (GetNextTileCoordinate() == new Vector2Int(-1,-1)) return false;
        /* else */ return true;
    }

    public void StartFlow(){
        buttonComponent.interactable = false;
        var newColorBlock = buttonComponent.colors;
        newColorBlock.disabledColor = Color.white;
        buttonComponent.colors = newColorBlock;
        AnimationClip[] possibleAnimations = GetPossibleAnimations(currentTileType);
        AnimationClip animationToPlay = possibleAnimations[GetAnimationIndex()];
        animator.speed = 1/SettingsManager.Instance.timeToFillTile;
        animator.Play(animationToPlay.name);
        timesFilled++;
    }

    int GetAnimationIndex(){
        int index = 0;
        if (receivingFlowFrom == Direction.Up) index = 0;
        else if(receivingFlowFrom == Direction.Right) index = 1;
        else if(receivingFlowFrom == Direction.Down) index = 2;
        else if(receivingFlowFrom == Direction.Left) index = 3;
        if (timesFilled > 0) index += 4;
        return index;
    }

    AnimationClip[] GetPossibleAnimations(TileType tileType){
        foreach(TileTypeData possibleTile in possibleTiles){
            if(possibleTile.type == tileType){
                return possibleTile.fillAnimations;
            }
        }
        return null;
    }

    public Vector2Int GetNextTileCoordinate(){
        if(
            (currentTileType == TileType.Vertical) && (receivingFlowFrom == Direction.Up) ||
            (currentTileType == TileType.Cross) && (receivingFlowFrom == Direction.Up) ||
            (currentTileType == TileType.Southeast) && (receivingFlowFrom == Direction.Right) ||
            (currentTileType == TileType.Southeast) && (receivingFlowFrom == Direction.Left)
        ){
            return new Vector2Int(tilePosition.x , tilePosition.y +1); //one down
        }
        if(
            (currentTileType == TileType.Vertical) && (receivingFlowFrom == Direction.Down) ||
            (currentTileType == TileType.Cross) && (receivingFlowFrom == Direction.Down) ||
            (currentTileType == TileType.Northeast) && (receivingFlowFrom == Direction.Right) ||
            (currentTileType == TileType.Northwest) && (receivingFlowFrom == Direction.Left)
        ){
            return new Vector2Int(tilePosition.x , tilePosition.y -1); //one up
        }
        if(
            (currentTileType == TileType.Horizontal) && (receivingFlowFrom == Direction.Left) ||
            (currentTileType == TileType.Cross) && (receivingFlowFrom == Direction.Left) ||
            (currentTileType == TileType.Northeast) && (receivingFlowFrom == Direction.Up) ||
            (currentTileType == TileType.Southeast) && (receivingFlowFrom == Direction.Down)
        ){
            return new Vector2Int(tilePosition.x +1, tilePosition.y); //one right
        }
        if(
            (currentTileType == TileType.Horizontal) && (receivingFlowFrom == Direction.Right) ||
            (currentTileType == TileType.Cross) && (receivingFlowFrom == Direction.Right) ||
            (currentTileType == TileType.Northwest) && (receivingFlowFrom == Direction.Up) ||
            (currentTileType == TileType.Southwest) && (receivingFlowFrom == Direction.Down)
        ){
            return new Vector2Int(tilePosition.x -1, tilePosition.y); //one left
        }
        return new Vector2Int(-1,-1); //signal invalid
    }
}
