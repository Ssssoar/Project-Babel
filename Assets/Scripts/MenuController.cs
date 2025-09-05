using UnityEngine;

public class MenuController : MonoBehaviour{
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject menuCanvas;
    private bool settingsOpen = false;
    public void SwitchSettings(){
        settingsOpen = !settingsOpen;
        settingsCanvas.SetActive(settingsOpen);
        menuCanvas.SetActive(!settingsOpen);
    }
}
