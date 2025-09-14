using UnityEngine;
using UnityEngine.UI;

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
            ResetSecretCombination();
        }
    }

    void ResetSecretCombination(){
        correctSecretCombinationPresses = 0;
        secretCombinationTimeLeft = 0f;
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
    }
}
