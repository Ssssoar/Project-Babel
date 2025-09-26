using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour{
    //SINGLETON
    public static SceneLoader Instance { get; private set;}
    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this);
        }else{
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    [SerializeField] string gameSceneName;
    [SerializeField] string menuSceneName;
    public void LoadGame(){
        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadMenu(){
        SceneManager.LoadScene(menuSceneName);
    }

    public void QuitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
