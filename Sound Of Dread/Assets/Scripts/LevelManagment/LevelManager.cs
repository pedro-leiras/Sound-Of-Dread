using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour{
    public enum Level{
        ForestScene,
        Map
    }

    public Level levelToChange;
    public Animator animator;
    private static string levelToLoad;
    
    private void OnTriggerEnter(Collider collision){
        if(levelToChange == Level.ForestScene)
            FadeToLevel("ForestScene");
        else if(levelToChange == Level.Map)
            FadeToLevel("Map");
    }   

    public void FadeToLevel(string levelIndex){
        animator.SetTrigger("FadeOut");
        levelToLoad = levelIndex;
    }

    public void OnFadeComplete(){
        SceneManager.LoadScene(levelToLoad);
    }

    public void OnEnter(){
        FadeToLevel("Cutscene");
    }
}
