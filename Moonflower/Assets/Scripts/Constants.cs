using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    // Scene Names
    public const string SCENE_VILLAGE = "The Village";
    public const string SCENE_PERSISTENT = "Persistent";
    public const string SCENE_MAINMENU = "Main Menu Village";
    public const string SCENE_ANAIHOUSE = "Anai House";
    public const string SCENE_LOADING = "LoadingScene";
    public const string SCENE_GAMESTART = "GameStart";
    public const string SCENE_QUITPOPUP = "Quit Nag Popup";
    public const string SCENE_PAUSEMENU = "Pause Menu"; 
    public const string SCENE_GAMEOVER = "Game Over";
    public const string SCENE_CAVEENTRANCE = "CaveEntrance";
    public const string SCENE_CAVE = "The Cave";

    // SaveData Keys
    public const string SAVE_BALL_POSITION = "BallPosition";
    public const string SAVE_BALL_VELOCITY = "BallVelocity";
    public const string SAVE_PLAYER_POSITION = "PlayerPosition";
    public const string SAVE_CPU_POSITION = "CPUPosition";
    public const string SAVE_PLAYER_SCORE = "PlayerScore";
    public const string SAVE_CPU_SCORE = "CPUScore";

    //icon names
    const string characterFolder = "Icons/Characters/";
    public const string ANAI_ICON = characterFolder + "Anai";
    public const string MIMBI_ICON = characterFolder + "Mimbi";
    public const string NAIA_ICON = characterFolder + "Naia";
    public const string AMARU_ICON = characterFolder + "Amaru";
    public const string PINON_ICON = characterFolder + "Pinon";
    public const string SYPAVE_ICON = characterFolder + "Sypave";
    public const string TEGU_ICON = characterFolder + "Tegu.PNG";
    public const string KURUPI_ICON = characterFolder + "Kurupi";
    public const string MOUSE_ICON = characterFolder + "Mousy Boi";
    public const string MONAI_ICON = characterFolder + "Monai";
    public const string CATBAT_ICON = characterFolder + "Catbat";
    const string itemFolder = "Icons/Items/";
    public const string MOONFLOWER_ICON = itemFolder + "Moonflower";
    public const string WOLFAPPLE_ICON = itemFolder + "Wolf Apple";
    public const string HONEY_ICON = itemFolder + "Honey";
    public const string ROPE_ICON = itemFolder + "Rope";
    public const string CHIPA_ICON = itemFolder + "Chipa";
    public const string PUMPKIN_ICON = itemFolder + "Pumpkin";
    public const string FLUTE_ICON = itemFolder + "Flute";
    public const string JAR_ICON = itemFolder + "Jar";
    public const string PINEAPPLE_ICON = itemFolder + "Pineapple";
    public const string FISH_ICON = itemFolder + "Fish";
    public const string PEANUT_ICON = itemFolder + "Peanuts";
    public const string BOW_ICON = itemFolder + "Bow";
    public const string ARROW_ICON = itemFolder + "Arrow";
    public const string CORN_ICON = itemFolder + "Corn";
    public const string STAFF_ICON = itemFolder + "Jewel Staff";
    public const string SWEETPOTATO_ICON = itemFolder + "Sweet Potato";
    public const string PAINT_ICON = itemFolder + "Paint Pots"; 
    public const string DRUM_ICON = itemFolder + "Drum";
    public const string FEATHER_ICON = itemFolder + "Feather";
    public const string NECKLACE_ICON = itemFolder + "Necklace"; 


    //life petal names
    const string flowerFolder = "Life Petals/";
    public const string HEALTHY_PETAL = flowerFolder + "Full";
    public const string DECAY_PETAL1 = flowerFolder + "Decay1";
    public const string DECAY_PETAL2 = flowerFolder + "Decay2";
    public const string DECAY_PETAL3 = flowerFolder + "Decay3";

    //life apple names
    const string appleFolder = "Life Apple/";
    public const string HEALTHY_APPLE = appleFolder + "Healthy Apple";
    public const string APPLE_ROT_1 = appleFolder + "Apple Rot 1";
    public const string APPLE_ROT_2 = appleFolder + "Apple Rot 2";
    public const string APPLE_ROT_3 = appleFolder + "Apple Rot 3";
    public const string APPLE_ROT_4 = appleFolder + "Apple Rot 4";
    public const string APPLE_ROT_5 = appleFolder + "Apple Rot 5";
    public const string APPLE_ROT_6 = appleFolder + "Apple Rot 6";
    public const string APPLE_ROT_7 = appleFolder + "Apple Rot 7";
    public const string APPLE_ROT_8 = appleFolder + "Apple Rot 8";
    public const string APPLE_ROT_9 = appleFolder + "Apple Rot 9";
    public const string APPLE_ROT_10 = appleFolder + "Apple Rot 10";
    public const string APPLE_ROT_11 = appleFolder + "Apple Rot 11";
    public const string APPLE_ROT_12 = appleFolder + "Apple Rot 12";
    public const string APPLE_ROT_13 = appleFolder + "Apple Rot 13";
    public const string APPLE_ROT_14 = appleFolder + "Apple Rot 14";
    public const string APPLE_ROT_15 = appleFolder + "Apple Rot 15";
    public const string APPLE_ROT_16 = appleFolder + "Apple Rot 16";
    public const string APPLE_ROT_17 = appleFolder + "Apple Rot 17";
    public const string APPLE_ROT_18 = appleFolder + "Apple Rot 18";
    public const string DEAD_APPLE = appleFolder + "Dead Apple";


    //inventory names
    public const string MOONFLOWER_PICKUP = "Moon Flower";

    /*
     * Dialogue Names
     */
    const string dialogueFolder = "Dialogues/";

    //Amaru
    public const string AMARU_NAME = "Amaru"; 
    public const string AMARU_INTRO_DIALOGUE = dialogueFolder + "Amaru Intro Dialogue";
    public const string AMARU_ADVICE_DIALOGUE = dialogueFolder + "Amaru Advice";
    //Naia
    public const string NAIA_NAME = "Naia";
    public const string NAIA_INTRO_DIALOGUE = dialogueFolder + "Naia Intro";
    public const string NAIA_POSTFIGHT_DIALOGUE = dialogueFolder + "Naia Post Fight";
    public const string NAIA_ADVICE_DIALOGUE = dialogueFolder + "Naia Advice";

    //Sypave
    public const string SYPAVE_NAME = "Sypave";
    public const string SYPAVE_INTRO_DIALOGUE = dialogueFolder + "Sypave Intro";
    public const string SYPAVE_FRANTIC_DIALOGUE = dialogueFolder + "Sypave Frantic";
    public const string SYPAVE_ADVICE_DIALOGUE = dialogueFolder + "Sypave Advice";
    //Jeruti
    public const string JERUTI_NAME = "Jeruti";
    //Pinon
    public const string PINON_NAME = "Pinon";
    public const string PINON_FIRST_INTRO_DIALOGUE = dialogueFolder + "Pinon FIRST Intro";
    public const string PINON_INTRO_DIALOGUE = dialogueFolder + "Pinon Intro";
    //Ysapy
    public const string YSAPY_NAME = "Ysapy";

    //other
    public const string MOUSE_NAME = "Mousy Boi";
    public const string CATBAT_NAME = "Catbat";
}
