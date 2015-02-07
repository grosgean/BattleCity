using System;
using System.Collections.Generic;
using System.Linq;


namespace MyDataTypes
{

    public class LevelData
    {
        public int Level;
        public int[] EnemyTanks;

        public TileRow[] TilesData;
    }

    public class TileRow
    {
        public int[] Tiles;
    }

}
