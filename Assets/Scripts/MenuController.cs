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

    [Header("Text Displays")]
    [SerializeField] TMP_Text horizontalSizeDisplay;
    [SerializeField] TMP_Text verticalSizeDisplay;

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
        if (SettingsManager.Instance.TryChangePlayFieldSize(newFieldSize)){
            UpdateSizeDisplay();
        }
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

    void UpdateSizeDisplay(){
        Vector2Int fieldSize = SettingsManager.Instance.GetPlayFieldSize();
        horizontalSizeDisplay.text = fieldSize.x.ToString();
        verticalSizeDisplay.text = fieldSize.y.ToString();
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
    }

}
