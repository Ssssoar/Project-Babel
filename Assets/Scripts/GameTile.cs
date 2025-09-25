using UnityEngine;
using UnityEngine.UI;

public class GameTile : MonoBehaviour{
    [Header("References")]
    [SerializeField] Button buttonComponent;
    [SerializeField] Image[] emptyTileSpriteDisplayers;
    [SerializeField] Image[] filledTileSpritesDisplayers;

    [Header("Parameters")]
    [SerializeField] TileTypeData[] possibleTiles;

    [Header("Variables")]
    TileType currentTileType = TileType.Empty;
    Vector2Int tilePosition;

    //----ENUMS-----
    public enum TileType{Border,Vertical,Horizontal,Cross,Northwest,Northeast,Southwest,Southeast,Empty};

    //----STRUCTS----
    [System.Serializable] //if I were doing this forreal I would do this a scriptable object
    struct TileTypeData{
        public TileType type {get; private set;}
        public Sprite emptySprite {get; private set;}
        public Sprite filledSprite {get; private set;}
        public AnimationClip[] fillAnimations {get; private set;} //in the order top right bottom left, with the next eight for the second fills
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
}
