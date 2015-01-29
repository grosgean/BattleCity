using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCity
{
    public enum GameState
    {
        Menu,
        NextLevelScreen,
        Play,
        /*EndLevel,*/
        ScoreScreen,
        End,
        EndGameScreen

    }

    public enum BulletType
    {
        Enemy,
        Player
    }

    public enum BonusType
    {
        Star,
        Bomb,
        Life
    }

    public enum BulletSpeed
    {
        Fast,
        Slow
    }

    public enum BulletPower
    {
        High,
        Low
    }

    public enum TileType
    {
        Armor,
        Brick
    }

    public enum TextDirection
    {
        Left,
        Right
    }


}
