using UnityEngine;

public class MMConstants 
{
    public const bool DEBUG_MODE = false;

    public const string TAG_MIRROR          = "Mirror";
    public const string TAG_SPLITTER        = "Splitter";
    public const string TAG_MERGER          = "Merger";
    public const string TAG_LINE_SPAWN      = "LineSpawn";
    public const string TAG_RECEIVER        = "Receiver";
    public const string TAG_LASER           = "Laser";
    public const string TAG_PLAYER          = "Player";
    public const string TAG_TILE            = "Tile";
    public const string TAG_GENERIC_RAY_KILLER = "GenericRayKiller";

    
    public const string TAG_WALL            = "Wall";
    public const string TAG_GAME_CONTROLLER = "GameController";
    public const string TAG_CANVAS          = "Canvas";
    public const string TAG_LEVEL_SELECT    = "LevelSelect";

    public const string INPUT_HORIZONTAL    = "Horizontal";
    public const string INPUT_VERTICAL      = "Vertical";
    public const string INPUT_BUMPER_RIGHT  = "BumperRight";
    public const string INPUT_BUMPER_LEFT   = "BumperLeft";
    public const string INPUT_TRIGGERS      = "Triggers";


    public const string LANG_LEVEL_COMPLETE      = "Level Completed!";
    public const string LANG_MENU                = "R --> Restart level" +
                                                    " ESC --> EXIT       " +
                                                    "U --> UNDO";

    public const char LEVEL_SEPARATOR       = ',';

    public static readonly Color32 BLUE        = new Color32(0,    0,      255,    255);
    public static readonly Color32 RED         = new Color32(255,  0,      0,      255);
    public static readonly Color32 YELLOW      = new Color32(255,  255,    0,      255);

    public static readonly Color32 GREEN       = new Color32(0,  255,      0,    255);
    public static readonly Color32 ORANGE      = new Color32(255,  128,      0,    255);
    public static readonly Color32 PURPLE      = new Color32(128,  0,      255,    255);

    public static readonly Color32 WHITE      = new Color32(255,    255,      255,    255);

    public const float TIME_TO_TRYTOSOLVE_AGAIN = .25f; //sec
}
