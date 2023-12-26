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
    private static int levelToLoad;
    
    private void OnTriggerEnter(Collider collision){
        if(levelToChange == Level.ForestScene)
            FadeToLevel(2);
        else if(levelToChange == Level.Map)
            FadeToLevel(3);
    }   

    public void FadeToLevel(int levelIndex){
        animator.SetTrigger("FadeOut");
        levelToLoad = levelIndex;
    }

    public void OnFadeComplete(){
        SceneManager.LoadScene(levelToLoad);
    }

    public void OnEnter(){
        FadeToLevel(0);
    }
}
