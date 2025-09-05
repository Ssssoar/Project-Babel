using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour{
    [Header("Submenus")]
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject menuCanvas;
    [Header("Buttons")]
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;
    private bool settingsOpen = false;
    void Start(){
        SubscribeButtons();
    }
    public void SwitchSettings(){
        settingsOpen = !settingsOpen;
        settingsCanvas.SetActive(settingsOpen);
        menuCanvas.SetActive(!settingsOpen);
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
