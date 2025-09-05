using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour{
    [Header("Submenus")]
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject secretMenu;
    [Header("Buttons")]
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;
    enum Menu{Main,Setting,Secret};
    private Menu currentMenu = Menu.Main;
    void Start(){
        SubscribeButtons();
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
        settingsCanvas.SetActive(currentMenu == Menu.Setting);
        menuCanvas.SetActive(currentMenu == Menu.Main);
        secretMenu.SetActive(currentMenu == Menu.Secret);
    }

    void SubscribeButtons(){
        if (startButton != null){
            startButton.onClick.AddListener(SceneLoader.Instance.LoadGame);
        }
        if (quitButton != null){
            quitButton.onClick.AddListener(SceneLoader.Instance.QuitGame);
        }
    }
}
