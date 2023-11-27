using UnityEngine;
using UnityEngine.Playables;

public class Cutscene : MonoBehaviour{
    public float timeScale = 1.0f;

    public void Awake(){
        var director = GetComponent<PlayableDirector>();
        if(director != null){
            director.played += SetSpeed;
            SetSpeed(director);
        }
    }

    public void SetSpeed(PlayableDirector director){
        if(director != null && director.playableGraph.IsValid())
            director.playableGraph.GetRootPlayable(0).SetSpeed(timeScale);
    }

    public void OnValidate(){
        SetSpeed(GetComponent<PlayableDirector>());
    }
}
