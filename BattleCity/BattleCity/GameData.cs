using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace BattleCity
{
    public static class GameData
    {
        public static Vector4[] bigTilesContent;

        public static Dictionary<string, int[]> tilesChangeState;

        public static Dictionary<string, Vector2> charPosition;


        public static Vector4[] BigTilesContent
        {
            get { return bigTilesContent; }
        }

        public static Dictionary<string, int[]> TilesChangeState
        {
            get { return tilesChangeState; }
        }

        public static Dictionary<string, Vector2> CharPosition
        {
            get { return charPosition; }
        }

        public static void Initialize()
        {
            // содержание bigTiles при загрузке уровня
            bigTilesContent = new Vector4[] { 
                new Vector4(0,0,0,0),       //0
                new Vector4(0,15,0,15),     //1
                new Vector4(0,0,15,15),     //2
                new Vector4(15,0,15,0),     //3
                new Vector4(15,15,0,0),     //4
                new Vector4(15,15,15,15),   //5
                new Vector4(0,16,0,16),     //6
                new Vector4(0,0,16,16),     //7
                new Vector4(16,0,16,0),     //8
                new Vector4(16,16,0,0),     //9
                new Vector4(16,16,16,16),   //10
                new Vector4(17,17,17,17),   //11
                new Vector4(18,18,18,18),   //12
                new Vector4(19,19,19,19),   //13

                new Vector4(0,0,0,0),    //14
                new Vector4(0,0,0,15),   //15   для отрисовки штаба
                new Vector4(0,0,15,0),   //16
                                
                new Vector4(20,21,24,25),   //17   для отрисовки орла
                new Vector4(22,23,26,27),   //18   испорченый орел

            };

            // tile с номером ключа массива заменяется на значение
            tilesChangeState = new Dictionary<string, int[]>()
            {
                {"Left", new int[] { 0, 0, 0, 1, 0, 0, 4, 5, 0, 1, 0, 1, 4, 5, 4, 5, 0, 17, 18, 19, 22, 23, 22, 23, 26, 27, 26, 27 }},
                {"Right", new int[] { 0, 0, 0, 2, 0, 0, 2, 2, 0, 8, 0, 10, 8, 8, 10, 10, 0, 17, 18, 19 }},
                {"Up", new int[] { 0, 0, 0, 0, 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 0, 17, 18, 19 }},
                {"Down", new int[] { 0, 0, 0, 0, 0, 4, 4, 4, 0, 8, 8, 8, 0, 12, 12, 12, 0, 17, 18, 19 }}
            };

            charPosition = new Dictionary<string, Vector2>()
            { 
                {"0", new Vector2(5, 3)}, 
                {"1", new Vector2(6, 3)}, 
                {"2", new Vector2(7, 3)}, 
                {"3", new Vector2(8, 3)}, 
                {"4", new Vector2(9, 3)}, 
                {"5", new Vector2(5, 4)}, 
                {"6", new Vector2(6, 4)}, 
                {"7", new Vector2(7, 4)}, 
                {"8", new Vector2(8, 4)}, 
                {"9", new Vector2(9, 4)},
 
                {" ", new Vector2(4, 3)},
                {"<", new Vector2(8, 5)},

                {"H", new Vector2(0, 5)},
                {"I", new Vector2(1, 5)},
                {"-", new Vector2(2, 5)},
                {"S", new Vector2(3, 5)},
                {"C", new Vector2(4, 5)},
                {"O", new Vector2(5, 5)},
                {"R", new Vector2(6, 5)},
                {"E", new Vector2(7, 5)},

                {"P", new Vector2(2, 6)},
                {"L", new Vector2(3, 6)},
                {"A", new Vector2(4, 6)},
                {"Y", new Vector2(5, 6)},

                {"T", new Vector2(6, 2)},
                {"G", new Vector2(8, 2)}

            };
        }
    }
}
