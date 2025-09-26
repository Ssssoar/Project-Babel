using UnityEngine;

public class SettingsManager : MonoBehaviour{
    public static SettingsManager Instance { get; private set;}

    [SerializeField] Vector2Int playFieldMaxSize;
    [SerializeField] Vector2Int playFieldMinSize;
    [SerializeField] Vector2 TimeLimits;
    [SerializeField] Vector2Int ScoreLimits;
    public Vector2Int fieldSize = new Vector2Int(10,10);
    public float timeToFillTile;
    public float timeBeforeFirstFill;
    public float timeBetweenTileFills;
    public int hiScore;
    public int scorePenaltyForReplacedTile;
    public int scoreBonusForFilledTile;
    public int scoreBonusForSuccess;

    void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this);
        }else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

    public int UpdateHighScore(int newScore){
        if (newScore > hiScore){
            hiScore = newScore;
        }
        return hiScore;
    }

    public int ForceHighScore(int newScore){
        hiScore = newScore;
        return hiScore;
    }

    public void TryInitialFillTimeChange(float changeAmmount){
        timeBeforeFirstFill = ClampValue(TimeLimits.x,timeBeforeFirstFill + changeAmmount,TimeLimits.y);
    }
    public void TrySingleFillTimeChange(float changeAmmount){
        timeToFillTile = ClampValue(TimeLimits.x,timeToFillTile + changeAmmount,TimeLimits.y);
    }
    public void TryBetweenTimeChange(float changeAmmount){
        timeBetweenTileFills = ClampValue(TimeLimits.x,timeBetweenTileFills + changeAmmount,TimeLimits.y);
    }
    public void TryFillBonusChange(int changeAmmount){
        scoreBonusForFilledTile = ClampValue(ScoreLimits.x,scoreBonusForFilledTile + changeAmmount, ScoreLimits.y);
    }
    public void TryReplacePenaltyChange(int changeAmmount){
        scorePenaltyForReplacedTile = ClampValue(ScoreLimits.x,scorePenaltyForReplacedTile + changeAmmount, ScoreLimits.y);
    }
    public void TryCompleteBoardBonusChange(int changeAmmount){
        scoreBonusForSuccess = ClampValue(ScoreLimits.x,scoreBonusForSuccess + changeAmmount, ScoreLimits.y);
    }
    int ClampValue(int min, int value, int max){
        if (value <= min){
            return min;
        }else if(value >= max){
            return max;
        }else{
            return value;
        }
    }
    
    float ClampValue(float min, float value, float max){
        if (value <= min){
            return min;
        }else if(value >= max){
            return max;
        }else{
            return value;
        }
    }
}