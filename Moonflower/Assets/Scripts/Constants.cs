using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    // Scene Names
    public const string SCENE_GAME = "SampleScene";
    public const string SCENE_PERSISTENT = "Persistent";
    public const string SCENE_MAINMENU = "Main Menu";
    public const string SCENE_LOADING = "Loading";
    public const string SCENE_STARTUP = "StartUp";
    public const string SCENE_QUITPOPUP = "Quit Nag Popup";
    public const string SCENE_PAUSEMENU = "Pause Menu"; 

    // SaveData Keys
    public const string SAVE_BALL_POSITION = "BallPosition";
    public const string SAVE_BALL_VELOCITY = "BallVelocity";
    public const string SAVE_PLAYER_POSITION = "PlayerPosition";
    public const string SAVE_CPU_POSITION = "CPUPosition";
    public const string SAVE_PLAYER_SCORE = "PlayerScore";
    public const string SAVE_CPU_SCORE = "CPUScore";

    //icon names
    const string iconFolder = "Icons/";
    public const string ANAI_ICON = iconFolder + "Anai";
    public const string NAIA_ICON = iconFolder + "Naia";
    public const string AMARU_ICON = iconFolder + "Amaru";
    public const string PINON_ICON = iconFolder + "Pinon";
    public const string SYPAVE_ICON = iconFolder + "Sypave";
    public const string TEGU_ICON = iconFolder + "Tegu.PNG";
    public const string KURUPI_ICON = iconFolder + "Kurupi";
    public const string MONAI_ICON = iconFolder + "Monai";

    //life petal names
    const string flowerFolder = "Life Petals/";
    public const string HEALTHY_PETAL = flowerFolder + "Full";
    public const string DECAY_PETAL1 = flowerFolder + "Decay1";
    public const string DECAY_PETAL2 = flowerFolder + "Decay2";
    public const string DECAY_PETAL3 = flowerFolder + "Decay3"; 
}
