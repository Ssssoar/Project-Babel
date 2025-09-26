using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour{

    [Header("Submenus")]
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject secretCanvas;

    [Header("Buttons")]
    [SerializeField] Button startButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button[] backToMenuButtons;
    [SerializeField] Button[] secretMenuButtons;
    [SerializeField] Button horizontalSizeDecreaseButton;
    [SerializeField] Button horizontalSizeIncreaseButton;
    [SerializeField] Button verticalSizeDecreaseButton;
    [SerializeField] Button verticalSizeIncreaseButton;
    [SerializeField] Button initialFillTimeDecreaseButton;
    [SerializeField] Button initialFillTimeIncreaseButton;
    [SerializeField] Button singeFillTimeDecreaseButton;
    [SerializeField] Button singeFillTimeIncreaseButton;
    [SerializeField] Button betweenTimeDecreaseButton;
    [SerializeField] Button betweenTimeIncreaseButton;
    [SerializeField] Button fillBonusDecreaseButton;
    [SerializeField] Button fillBonusIncreaseButton;
    [SerializeField] Button replacePenaltyDecreaseButton;
    [SerializeField] Button replacePenaltyIncreaseButton;
    [SerializeField] Button boardCompleteBonusDecreaseButton;
    [SerializeField] Button boardCompleteBonusIncreaseButton;

    [Header("Text Displays")]
    [SerializeField] TMP_Text horizontalSizeDisplay;
    [SerializeField] TMP_Text verticalSizeDisplay;
    [SerializeField] TMP_Text initialFillTimeDisplay;    
    [SerializeField] TMP_Text singleFillTimeDisplay;
    [SerializeField] TMP_Text betweenTimeDisplay;
    [SerializeField] TMP_Text fillBonusDisplay;
    [SerializeField] TMP_Text replacePenaltyDisplay;
    [SerializeField] TMP_Text boardCompleteBonusDisplay;    

    [Header("Parameters")]
    [SerializeField] float secretMenuTimeLimit;

    //----ENUMS-----
    enum Menu{Main,Setting,Secret};

    [Header("Variables")]
    private Menu currentMenu = Menu.Main;
    private int correctSecretCombinationPresses = 0;
    private float secretCombinationTimeLeft = 0f;

    void Start(){
        SubscribeButtons();
        UpdateSizeDisplay();
        EnableMenuCanvases();
    }

    void Update(){
        if (correctSecretCombinationPresses != 0){
            secretCombinationTimeLeft -= Time.deltaTime;
            if (secretCombinationTimeLeft <= 0f){
                ResetSecretCombination();
            }
        }
    }

    public void OpenSettingsMenu(){
        if (currentMenu == Menu.Setting){
            return;
        }
        currentMenu = Menu.Setting;
        EnableMenuCanvases();
    }

    public void OpenMainMenu(){
        if (currentMenu == Menu.Main){
            return;
        }
        currentMenu = Menu.Main;
        EnableMenuCanvases();
    }

    public void OpenSecretMenu(){
        if (currentMenu == Menu.Secret){
            return;
        }
        currentMenu = Menu.Secret;
        EnableMenuCanvases();
    }

    void EnableMenuCanvases(){
        if(settingsCanvas != null) settingsCanvas.SetActive(currentMenu == Menu.Setting);
        if(menuCanvas != null) menuCanvas.SetActive(currentMenu == Menu.Main);
        if(secretCanvas != null) secretCanvas.SetActive(currentMenu == Menu.Secret);
    }

    void SecretMenuButtonPress(Button buttonPressed){
        if (buttonPressed == secretMenuButtons[correctSecretCombinationPresses]){
            correctSecretCombinationPresses += 1;
            secretCombinationTimeLeft = secretMenuTimeLimit;
            if (correctSecretCombinationPresses == secretMenuButtons.Length){
                ResetSecretCombination();
                OpenSecretMenu();
                UpdateSecretMenu();
            }
        }else{
            OpenMainMenu();
            ResetSecretCombination();
        }
    }

    void ResetSecretCombination(){
        correctSecretCombinationPresses = 0;
        secretCombinationTimeLeft = 0f;
    }

    void ModifyPlaySize(bool vertical, bool increase){
        Vector2Int newFieldSize = SettingsManager.Instance.GetPlayFieldSize();
        if (vertical){
            newFieldSize.y = (increase? newFieldSize.y +1 : newFieldSize.y -1);
        }else{
            newFieldSize.x = (increase? newFieldSize.x +1 : newFieldSize.x -1);
        }
        var changeAttemptSuccessful = SettingsManager.Instance.TryChangePlayFieldSize(newFieldSize);
        if (changeAttemptSuccessful){
            UpdateSizeDisplay();
        }
    }

    void UpdateSizeDisplay(){
        Vector2Int fieldSize = SettingsManager.Instance.GetPlayFieldSize();
        horizontalSizeDisplay.text = fieldSize.x.ToString();
        verticalSizeDisplay.text = fieldSize.y.ToString();
    }

    void UpdateSecretMenu(){
        initialFillTimeDisplay.text = SettingsManager.Instance.timeBeforeFirstFill.ToString();
        singleFillTimeDisplay.text = SettingsManager.Instance.timeToFillTile.ToString();
        betweenTimeDisplay.text = SettingsManager.Instance.timeBetweenTileFills.ToString();
        fillBonusDisplay.text = SettingsManager.Instance.scoreBonusForFilledTile.ToString();
        replacePenaltyDisplay.text = SettingsManager.Instance.scorePenaltyForReplacedTile.ToString();
        boardCompleteBonusDisplay.text = SettingsManager.Instance.scoreBonusForSuccess.ToString();
    }

    void SubscribeButtons(){
        if (startButton != null){
            startButton.onClick.AddListener(SceneLoader.Instance.LoadGame);
        }
        if (quitButton != null){
            quitButton.onClick.AddListener(SceneLoader.Instance.QuitGame);
        }
        if (settingsButton != null){
            settingsButton.onClick.AddListener(OpenSettingsMenu);
        }
        foreach(Button backToMenuButton in backToMenuButtons){
            backToMenuButton.onClick.AddListener(OpenMainMenu);
        }
        foreach(Button secretMenuButton in secretMenuButtons){
            secretMenuButton.onClick.AddListener(
                delegate{
                    SecretMenuButtonPress(secretMenuButton);
                }
            );
        }
        if (horizontalSizeDecreaseButton != null){
            horizontalSizeDecreaseButton.onClick.AddListener(
                delegate{
                    bool vertical = false;
                    bool increase = false;
                    ModifyPlaySize(vertical, increase);
                }
            );
        }
        if (horizontalSizeIncreaseButton != null){
            horizontalSizeIncreaseButton.onClick.AddListener(
                delegate{
                    bool vertical = false;
                    bool increase = true;
                    ModifyPlaySize(vertical, increase);
                }
            );
        }
        if (verticalSizeDecreaseButton != null){
            verticalSizeDecreaseButton.onClick.AddListener(
                delegate{
                    bool vertical = true;
                    bool increase = false;
                    ModifyPlaySize(vertical, increase);
                }
            );
        }
        if (verticalSizeIncreaseButton != null){
            verticalSizeIncreaseButton.onClick.AddListener(
                delegate{
                    bool vertical = true;
                    bool increase = true;
                    ModifyPlaySize(vertical, increase);
                }
            );
        }
        if (initialFillTimeDecreaseButton != null){
            initialFillTimeDecreaseButton.onClick.AddListener(
                delegate{
                    SettingsManager.Instance.TryInitialFillTimeChange(-.5f);
                    UpdateSecretMenu();
                }
            );
        }
        if (initialFillTimeIncreaseButton != null){
            initialFillTimeIncreaseButton.onClick.AddListener(
                delegate{
                    SettingsManager.Instance.TryInitialFillTimeChange(.5f);
                    UpdateSecretMenu();
                }
            );
        }
        if (singeFillTimeDecreaseButton != null){
            singeFillTimeDecreaseButton.onClick.AddListener(
                delegate{
                    SettingsManager.Instance.TrySingleFillTimeChange(-.5f);
                    UpdateSecretMenu();
                }
            );
        }
        if (singeFillTimeIncreaseButton != null){
            singeFillTimeIncreaseButton.onClick.AddListener(
                delegate{
                    SettingsManager.Instance.TrySingleFillTimeChange(.5f);
                    UpdateSecretMenu();
                }
            );
        }
        if (betweenTimeDecreaseButton != null){
            betweenTimeDecreaseButton.onClick.AddListener(
                delegate{
                    SettingsManager.Instance.TryBetweenTimeChange(-.5f);
                    UpdateSecretMenu();
                }
            );
        }
        if (betweenTimeIncreaseButton != null){
            betweenTimeIncreaseButton.onClick.AddListener(
                delegate{
                    SettingsManager.Instance.TryBetweenTimeChange(.5f);
                    UpdateSecretMenu();
                }
            );
        }
        if (fillBonusDecreaseButton != null){
            fillBonusDecreaseButton.onClick.AddListener(
                delegate{
                    SettingsManager.Instance.TryFillBonusChange(-100);
                    UpdateSecretMenu();
                }
            );
        }
        if (fillBonusIncreaseButton != null){
            fillBonusIncreaseButton.onClick.AddListener(
                delegate{
                    SettingsManager.Instance.TryFillBonusChange(100);
                    UpdateSecretMenu();
                }
            );
        }
        if (replacePenaltyDecreaseButton != null){
            replacePenaltyDecreaseButton.onClick.AddListener(
                delegate{
                    SettingsManager.Instance.TryReplacePenaltyChange(-100);
                    UpdateSecretMenu();
                }
            );
        }
        if (replacePenaltyIncreaseButton != null){
            replacePenaltyIncreaseButton.onClick.AddListener(
                delegate{
                    SettingsManager.Instance.TryReplacePenaltyChange(100);
                    UpdateSecretMenu();
                }
            );
        }
        if (boardCompleteBonusDecreaseButton != null){
            boardCompleteBonusDecreaseButton.onClick.AddListener(
                delegate{
                    SettingsManager.Instance.TryCompleteBoardBonusChange(-100);
                    UpdateSecretMenu();
                }
            );
        }
        if (boardCompleteBonusIncreaseButton != null){
            boardCompleteBonusIncreaseButton.onClick.AddListener(
                delegate{
                    SettingsManager.Instance.TryCompleteBoardBonusChange(100);
                    UpdateSecretMenu();
                }
            );
        }
    }
}
