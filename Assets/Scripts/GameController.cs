using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameTile emptyTile;

    [Header("References")]
    [SerializeField] GridLayoutGroup buttonContainer;
    GameTile[,] tileField;
    GameTile[] inputOutputCandidates;
    GameTile inputTile; //where the water starts flowing from
    GameTile outputTile; //where the water has to be flowed to
    [SerializeField] GameTile[] queueTiles;
    [SerializeField] TMP_Text gameScoreDisplay;
    [SerializeField] TMP_Text endScoreDisplay;
    [SerializeField] TMP_Text hiScoreDisplay;
    [SerializeField] GameObject gameOverPanel;

    [Header("Variables")]
    int nextCandidateTileIndexToAdd = 0;
    float timer; //in seconds
    TimerMode currentTimerMode;
    GameTile nextTileToFlow;
    int score;

    //---ENUMS
    public enum TimerMode { InitialWait, Flowing, BetweenWait, Halt };

    void Start(){
        FieldSetup(SettingsManager.Instance.fieldSize);
        QueueSetup();
    }

    void FieldSetup(Vector2Int fieldSize){
        //Give a border to the field
        fieldSize.x += 2;
        fieldSize.y += 2;
        tileField = new GameTile[fieldSize.x, fieldSize.y];
        inputOutputCandidates = new GameTile[2 * (fieldSize.x + fieldSize.y) - 8]; //magic equation achieved through math. It's the ammount of tiles in the borders, minus the corners.
        GenerateTiles(fieldSize);
        AdjustLayoutContainer(fieldSize);
        CreateInputAndOutputTile();
        TimerSetup();
        gameOverPanel.SetActive(false);
        UpdateScoreDisplay();
    }

    void FixedUpdate(){
        RunTimer();
    }

    void GenerateTiles(Vector2Int fieldSize){
        //generate every tile in the field
        for (int row = 0; row < fieldSize.x; row++){
            for (int column = 0; column < fieldSize.y; column++){
                GameTile newTile = Instantiate(emptyTile, buttonContainer.transform);
                Vector2Int tilePosition = new Vector2Int(column, row);
                tileField[column, row] = newTile;
                if (IsBorderPosition(tilePosition, fieldSize)){
                    newTile.ChangeToBorder();
                }else{
                    newTile.ChangeToEmpty();
                }
                if (IsValidInputOutputCandidate(tilePosition, fieldSize)){
                    inputOutputCandidates[nextCandidateTileIndexToAdd] = newTile;
                    nextCandidateTileIndexToAdd++;
                }
                newTile.AssignPosition(column, row);
                newTile.buttonComponent.onClick.AddListener(
                    delegate{
                        PlaceTile(newTile);
                    }
                );
            }
        }
    }

    void CreateInputAndOutputTile(){
        inputTile = ChooseInputTile();
        if (IsTopOrBottomBorder(inputTile.GetTilePosition(), SettingsManager.Instance.GetPlayFieldSize())){
            inputTile.ChangeToVertical();
        }else{
            inputTile.ChangeToHorizontal();
        }
        nextTileToFlow = inputTile;
        inputTile.SetupInitialFlow();
        outputTile = ChooseOutputTile();
        if (IsTopOrBottomBorder(outputTile.GetTilePosition(), SettingsManager.Instance.GetPlayFieldSize())){
            outputTile.ChangeToVertical();
        }else{
            outputTile.ChangeToHorizontal();
        }
    }

    GameTile ChooseInputTile(){
        return inputOutputCandidates[Random.Range(0, inputOutputCandidates.Length)];
    }

    GameTile ChooseOutputTile(){
        GameTile outputTile;
        do{
            outputTile = inputOutputCandidates[Random.Range(0, inputOutputCandidates.Length)];
        }while (outputTile == inputTile);
        return outputTile;
    }

    bool IsBorderPosition(Vector2Int tilePosition, Vector2Int fieldSize){
        return (
            (tilePosition.x == 0) ||
            (tilePosition.x == fieldSize.x - 1) ||
            (tilePosition.y == 0) ||
            (tilePosition.y == fieldSize.y - 1)
        );
    }

    bool IsValidInputOutputCandidate(Vector2Int tilePosition, Vector2Int fieldSize){
        if (!IsBorderPosition(tilePosition, fieldSize)) return false;
        /*elseif*/
        if (IsCornerPosition(tilePosition, fieldSize)) return false;
        /*else*/
        return true;
    }

    bool IsCornerPosition(Vector2Int tilePosition, Vector2Int fieldSize){
        if (
            (((tilePosition.x == 0) || (tilePosition.x == fieldSize.x - 1))) &&
            ((tilePosition.y == 0) || (tilePosition.y == fieldSize.y - 1))
        ){
            return true;
        }else{
            return false;
        }
    }

    bool IsTopOrBottomBorder(Vector2Int tilePosition, Vector2Int fieldSize){
        //as in, either top or bottom -> true, else false.
        return (tilePosition.y == 0) || (tilePosition.y - 1 == fieldSize.y);
    }

    void AdjustLayoutContainer(Vector2Int fieldSize){
        //adjusts the layout parameters so they show up neatly
        buttonContainer.constraintCount = fieldSize.x;
        float cellSize = 58 / 3 + 1694 / (3 * fieldSize.y); //magic numbers obtained through math and some starting assumptions.
        buttonContainer.cellSize = new Vector2(cellSize, cellSize);
    }

    void QueueSetup(){
        foreach (GameTile queueTile in queueTiles){
            queueTile.ChangeToRandom();
        }
    }

    void PlaceTile(GameTile targetTile){
        targetTile.ChangeToMatchTile(GetTileToBePlaced());
        AdvanceQueue();
    }

    GameTile GetTileToBePlaced(){
        return queueTiles[queueTiles.Length - 1]; //single line function? it just looks nicer to read.
    }

    void AdvanceQueue(){
        for (int i = queueTiles.Length - 1; i > 0; i--)
        {
            queueTiles[i].ChangeToMatchTile(queueTiles[i - 1]);
        }
        queueTiles[0].ChangeToRandom();
    }

    void TimerSetup(){
        timer = SettingsManager.Instance.timeBeforeFirstFill;
        currentTimerMode = TimerMode.InitialWait;
    }

    void UpdateScoreDisplay(){
        gameScoreDisplay.text = score.ToString();
    }

    void RunTimer(){
        if (currentTimerMode == TimerMode.Halt) return;
        timer -= Time.deltaTime;
        if (timer <= 0){
            TimerExpire();
        }
    }

    void TimerExpire(){
        if (currentTimerMode == TimerMode.InitialWait){
            currentTimerMode = TimerMode.Flowing;
            timer += SettingsManager.Instance.timeToFillTile;
            InitialFlowAdvance();
        }else if (currentTimerMode == TimerMode.BetweenWait){
            currentTimerMode = TimerMode.Flowing;
            timer += SettingsManager.Instance.timeToFillTile;
            FlowAdvance();
        }else if (currentTimerMode == TimerMode.Flowing){
            currentTimerMode = TimerMode.BetweenWait;
            timer += SettingsManager.Instance.timeBetweenTileFills;
        }
    }

    void InitialFlowAdvance(){
        nextTileToFlow.StartFlow();
    }

    void FlowAdvance(){
        if (currentTimerMode == TimerMode.BetweenWait){
            return;
        }else{
            Vector2Int previousTileCoords = nextTileToFlow.GetTilePosition();
            Vector2Int nextTileCoords = nextTileToFlow.GetNextTileCoordinate();
            if (nextTileCoords == new Vector2Int(-1, -1)){
                GameOver();
            }else{
                nextTileToFlow = tileField[nextTileCoords.x, nextTileCoords.y];
                nextTileToFlow.SetupFlowFrom(previousTileCoords);
                if (!nextTileToFlow.IsFlowPossible()){
                    GameOver();
                }else{
                    nextTileToFlow.StartFlow();
                    score += SettingsManager.Instance.scoreBonusForFilledTile;
                    UpdateScoreDisplay();
                }
            }
        }
    }

    void GameOver(){
        currentTimerMode = TimerMode.Halt;
        SetUpGameOverPanel();
        gameOverPanel.SetActive(true);
    }

    void SetUpGameOverPanel(){
        endScoreDisplay.text = score.ToString();
        int hiScore = SettingsManager.Instance.UpdateHighScore(score);
        hiScoreDisplay.text = hiScore.ToString();
    }
}