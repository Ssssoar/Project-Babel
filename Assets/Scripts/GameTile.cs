using UnityEngine;
using UnityEngine.UI;

public class GameTile : MonoBehaviour{
    [Header("References")]
    [SerializeField] Button buttonComponent;

    [Header("Parameters")]
    [SerializeField] TileTypeData[] possibleTiles;

    [Header("Variables")]
    TileType currentTileType = TileType.Empty;
    Vector2Int tilePosition;

    //----ENUMS-----
    enum TileType{Border,Vertical,Horizontal,Cross,Northwest,Northeast,Southwest,Southeast,Empty};

    //----STRUCTS----
    [System.Serializable] //if I were doing this forreal I would do this a scriptable object
    struct TileTypeData{
        [SerializeField] TileType type;
        [SerializeField] Sprite emptySprite;
        [SerializeField] Sprite filledSprite;
        [SerializeField] AnimationClip[] fillAnimations; //in the order top right bottom left, with the next eight for the second fills
    }

    public void MarkAsBorder(){
        currentTileType = TileType.Border;
        buttonComponent.interactable = false;
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
        UndoBorder();
        currentTileType = TileType.Vertical;
    }

    public void ChangeToHorizontal(){
        UndoBorder();
        currentTileType = TileType.Horizontal;
    }

    void UndoBorder(){
        if (currentTileType != TileType.Border) return;
        buttonComponent.interactable = true;
    }
}
