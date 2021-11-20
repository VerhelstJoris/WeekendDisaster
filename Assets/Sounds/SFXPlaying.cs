using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlaying : MonoBehaviour
{
    public AudioClip BookPage; 
    public AudioClip Writing; 
    public AudioClip YouLose; 
    public AudioClip YouWin; 
    public AudioClip Stamp;

    public GameState State; 

    public void PlayBookPage(){
        State.PlayAudioClip(BookPage);
    }

    public void PlayWriting(){
        State.PlayAudioClip(Writing);

    }

    public void PlayYouLose(){
        State.PlayAudioClip(YouLose);

    }

    public void PlayYouWin(){
         State.PlayAudioClip(YouWin);
    
    }

    public void PlayStamp(){
        State.PlayAudioClip(Stamp);
    }
}
