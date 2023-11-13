using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour{
    public enum Level{
        ForestScene,
        InsideLevel,
        Church
    }

    public Level levelToChange;
    
    private void OnTriggerEnter(Collider collision){
        if(levelToChange == Level.ForestScene)
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        else if(levelToChange == Level.InsideLevel)
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        else if(levelToChange == Level.Church)
            SceneManager.LoadScene(2, LoadSceneMode.Single);
    }   
}
