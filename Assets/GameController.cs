using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    /*** singleton management ***/
    public static GameController instance;
    private void Awake()
    {
        instance = this;
    }
    /****************************/
    
    public CharacterManager characterManager;
}
