using UnityEngine;

public class SettingsManager : MonoBehaviour{
    public static SettingsManager Instance { get; private set;}

    [SerializeField] Vector2Int playFieldMaxSize;
    [SerializeField] Vector2Int playFieldMinSize;
    public Vector2Int fieldSize = new Vector2Int(10,10);

    void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this);
        }else{
            Instance = this;
        }
    }

    public bool TryChangePlayFieldSize(Vector2Int newSize){
        if (
            (newSize.x < playFieldMinSize.x) ||
            (newSize.x > playFieldMaxSize.x) ||
            (newSize.y < playFieldMinSize.y) ||
            (newSize.y > playFieldMaxSize.y) 
        ){
            Debug.Log("[SETTINGS] PlayfieldSize change refused");
            return false;
        }else{
            Debug.Log("[SETTINGS] PlayfieldSize changed to " + newSize);
            fieldSize = newSize;
            return true;
        }
    }

    public Vector2Int GetPlayFieldSize(){
        return fieldSize;
    }
}
