﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMEnums
{
    public enum TileType { NONE = -1, EMPTY, LASER, RECEIVER, MIRROR, SPLITTER, ROCKS, CHARACTER, MERGER, BIG_ROCK, DUPLICATOR }
    //LIGHT BENDER? BENDS MULTIPLE LASERS IN DIFFERENT DIRECTIONS.
    //MIRROR OBJECTS COPIES MOVEMENT
    // MULTIPLE PLAYERS
    // DEATH RAY destroys objects and player
    // DONT NEED TO MATCH COLOR WITH MERGER (IF RECIEVES MULTIPLE RAYS THAT GENERATES SOLUTION COLOR IS OK)
    // DEATH RAY CAN DESTRY RECIEVER LEADING TO WIN HTE LEVEL
    // CINTA TRANSPORTADORA con direction
    //SWITCHES EN EL PISO HABILITAN PUERTAS
    public enum LevelDifficulty { TUTORIAL, EASY, MEDIUM, HARD, EXPERT}
}

