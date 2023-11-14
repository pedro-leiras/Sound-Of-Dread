using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour{
    public enum Level{
        ForestScene,
        InsideLevel,
        OutsideLevel,
        Pavilion,
        Church
    }

    public Level levelToChange;
    public Animator animator;
    private static int levelToLoad;
    
    private void OnTriggerEnter(Collider collision){
        if(levelToChange == Level.ForestScene)
            FadeToLevel(0);
        else if(levelToChange == Level.InsideLevel)
            FadeToLevel(1);
        else if(levelToChange == Level.OutsideLevel)
            FadeToLevel(2);
        else if(levelToChange == Level.Pavilion)
            FadeToLevel(3);
        else if(levelToChange == Level.Church)
            FadeToLevel(4);
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
