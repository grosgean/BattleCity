using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace BattleCity
{
    public static class GameConstants
    {
        // resolution
        //public const int WINDOW_WIDTH = 800;
        // public const int WINDOW_HEIGHT = 600;

        // tile

        public const int TILE_WIDTH = 48;
        public const int SMALL_TILE_WIDTH = TILE_WIDTH/2;

        public const int FIELD_WIDTH_TILES = 13;

        // field
        public const int FIELD_WIDTH = TILE_WIDTH * FIELD_WIDTH_TILES;
        public const int FIELD_HEIGHT = TILE_WIDTH * FIELD_WIDTH_TILES;

        public const int WINDOW_WIDTH = FIELD_WIDTH + TILE_WIDTH * 2;
        public const int WINDOW_HEIGHT = FIELD_HEIGHT;

        public const int GAME_OVER_Y_POSITION = TILE_WIDTH * (FIELD_WIDTH_TILES / 2) - SMALL_TILE_WIDTH;

        // player tank
        public const int PLAYER_TANK_MOVEMENT_SPEED = 3;
        public const int PLAYER_TANK_MOVEMENT_SPEED_FAST = 4;
        public const int PLAYER_SHOOT_COOLDOWN_MILLISECONDS = 500;
        public const int PLAYER_RESPAWN_COOLDOWN_TIME = 700;
        public const int PLAYER_SHIELD_ACTIVE_TIME = 2500;
        public const int PLAYER_BONUS_LENGTH = 4;
        public const int PLAYER_ONE_LIVES_COUNT = 2;

        // enemy tank
        public const int ENEMY_TANK_MOVEMENT_SPEED = 2;
        public const int ENEMY_TANK_MOVEMENT_SPEED_FAST = 3;
        public const int ENEMY_TANK_MAX_COUNT = 4;

        public const int ENEMY_TANK_MIN_FIRING_DELAY = 1000;
        public const int ENEMY_TANK_FIRING_RATE_RANGE = 1000;

        // bullet
        public const int BULLET_WIDTH = 9;
        public const int BULLET_HEIGHT = 12;

        public const float BULLET_SPEED = 0.4f;
        public const float BULLET_SPEED_FAST = 0.8f;

        // explosion
        public const int EXPLOSION_FRAMES_PER_ROW = 3;
        public const int EXPLOSION_NUM_ROWS = 1;
        public const int EXPLOSION_NUM_FRAMES = 3;
        public const int EXPLOSION_FRAME_TIME = 60;

    }
}
