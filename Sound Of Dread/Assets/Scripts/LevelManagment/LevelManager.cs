using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour{
    public enum Level{
        ForestScene,
        Map
    }

    public Level levelToChange;
    public Animator animator;
    private static int levelToLoad;
    
    private void OnTriggerEnter(Collider collision){
        if(levelToChange == Level.ForestScene)
            FadeToLevel(0);
        else if(levelToChange == Level.Map)
            FadeToLevel(1);
    }   

    public void FadeToLevel(int levelIndex){
        animator.SetTrigger("FadeOut");
        levelToLoad = levelIndex;
        Debug.Log(levelToLoad);
    }

    public void OnFadeComplete(){
        Debug.Log(levelToLoad);
        SceneManager.LoadScene(levelToLoad, LoadSceneMode.Single);
    }
}
