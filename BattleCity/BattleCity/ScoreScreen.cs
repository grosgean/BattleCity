using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleCity
{
    class ScoreScreen
    {
        Texture2D scoreSprite;

        NumberSprite stageNumberSprite;
        NumberSprite playerScoreSprite;
        NumberSprite totalSprite;

        List<StringSprite> text = new List<StringSprite>();

        int playerScore = 0;
        int[] destroyedTanksCount = new int[] { 0, 0, 0, 0 };

        double[] scoreYposition = new double[] { 5.5, 7, 8.5, 10 };

        bool isDone = false;
        int workTime = 0;
        int currentTankType = 0;

        Dictionary<int,TankScore> tankScores = new Dictionary<int,TankScore>();

        SoundBank soundBank;

        Texture2D tankSprite;

        Rectangle tank0drawRectangle;
        Rectangle tank0SourceRectangle;
        Rectangle tank1drawRectangle;
        Rectangle tank1SourceRectangle;
        Rectangle tank2drawRectangle;
        Rectangle tank2SourceRectangle;
        Rectangle tank3drawRectangle;
        Rectangle tank3SourceRectangle;


        public int PlayerScore
        {
            get { return playerScore; }
            set { playerScore = value; }
        }

        public int[] DestroyedTanksCount
        {
            get { return destroyedTanksCount; }
            set { destroyedTanksCount = value; }
        }

        public bool IsDone
        {
            get { return isDone; }
            set { isDone = value; }
        }

        public ScoreScreen(ContentManager contentManager, int currentLevel, SoundBank soundBank)
        {
            scoreSprite = contentManager.Load<Texture2D>("scoreSprite");
            tankSprite = contentManager.Load<Texture2D>("enemytank1");
            this.soundBank = soundBank;

            text.Add(new StringSprite(contentManager, "HI-SCORE", Color.Red,
                GameConstants.TILE_WIDTH * 4, GameConstants.TILE_WIDTH));
            text.Add(new StringSprite(contentManager, "STAGE", Color.White,
                GameConstants.TILE_WIDTH * 6, GameConstants.TILE_WIDTH * 2));
            text.Add(new StringSprite(contentManager, "I-PLAYER", Color.Red,
                GameConstants.TILE_WIDTH + GameConstants.SMALL_TILE_WIDTH, GameConstants.TILE_WIDTH * 3));

            text.Add(new StringSprite(contentManager, "PTS", Color.White,
                GameConstants.TILE_WIDTH * 4, (int)(GameConstants.TILE_WIDTH * scoreYposition[0])));
            text.Add(new StringSprite(contentManager, "PTS", Color.White,
                GameConstants.TILE_WIDTH * 4, (int)(GameConstants.TILE_WIDTH * scoreYposition[1])));
            text.Add(new StringSprite(contentManager, "PTS", Color.White,
                GameConstants.TILE_WIDTH * 4, (int)(GameConstants.TILE_WIDTH * scoreYposition[2])));
            text.Add(new StringSprite(contentManager, "PTS", Color.White,
                GameConstants.TILE_WIDTH * 4, (int)(GameConstants.TILE_WIDTH * scoreYposition[3])));
            text.Add(new StringSprite(contentManager, "TOTAL", Color.White,
                GameConstants.TILE_WIDTH * 3, GameConstants.TILE_WIDTH * 11));

            stageNumberSprite = new NumberSprite(contentManager, currentLevel, TextDirection.Right, Color.White,
                GameConstants.TILE_WIDTH * 10, GameConstants.TILE_WIDTH * 2);

            playerScoreSprite = new NumberSprite(contentManager, playerScore, TextDirection.Right, Color.Orange,
                GameConstants.TILE_WIDTH * 5 + GameConstants.SMALL_TILE_WIDTH, GameConstants.TILE_WIDTH * 4);

            tankScores.Add(0, new TankScore(contentManager, scoreYposition[0], 0));
            tankScores.Add(1, new TankScore(contentManager, scoreYposition[1], 1));
            tankScores.Add(2, new TankScore(contentManager, scoreYposition[2], 2));
            tankScores.Add(3, new TankScore(contentManager, scoreYposition[3], 3));

            totalSprite = new NumberSprite(contentManager, 0, TextDirection.Right, Color.White,
                GameConstants.TILE_WIDTH * 7, GameConstants.TILE_WIDTH * 11);

            text.Add(new StringSprite(contentManager, "<", Color.White,
                GameConstants.TILE_WIDTH * 7, (int)(GameConstants.TILE_WIDTH * scoreYposition[0])));
            text.Add(new StringSprite(contentManager, "<", Color.White,
                GameConstants.TILE_WIDTH * 7, (int)(GameConstants.TILE_WIDTH * scoreYposition[1])));
            text.Add(new StringSprite(contentManager, "<", Color.White,
                GameConstants.TILE_WIDTH * 7, (int)(GameConstants.TILE_WIDTH * scoreYposition[2])));
            text.Add(new StringSprite(contentManager, "<", Color.White,
                GameConstants.TILE_WIDTH * 7, (int)(GameConstants.TILE_WIDTH * scoreYposition[3])));

            tank0drawRectangle = new Rectangle(
                GameConstants.TILE_WIDTH * 7 + GameConstants.SMALL_TILE_WIDTH, (int)(GameConstants.TILE_WIDTH * scoreYposition[0] - GameConstants.SMALL_TILE_WIDTH/2),
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            tank0SourceRectangle = new Rectangle(0, GameConstants.TILE_WIDTH * 4,
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            tank1drawRectangle = new Rectangle(
                GameConstants.TILE_WIDTH * 7 + GameConstants.SMALL_TILE_WIDTH, (int)(GameConstants.TILE_WIDTH * scoreYposition[1] - GameConstants.SMALL_TILE_WIDTH / 2),
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            tank1SourceRectangle = new Rectangle(0, GameConstants.TILE_WIDTH * 5,
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            tank2drawRectangle = new Rectangle(
                GameConstants.TILE_WIDTH * 7 + GameConstants.SMALL_TILE_WIDTH, (int)(GameConstants.TILE_WIDTH * scoreYposition[2] - GameConstants.SMALL_TILE_WIDTH / 2),
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            tank2SourceRectangle = new Rectangle(0, GameConstants.TILE_WIDTH * 6,
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            tank3drawRectangle = new Rectangle(
                GameConstants.TILE_WIDTH * 7 + GameConstants.SMALL_TILE_WIDTH, (int)(GameConstants.TILE_WIDTH * scoreYposition[3] - GameConstants.SMALL_TILE_WIDTH / 2),
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            tank3SourceRectangle = new Rectangle(0, GameConstants.TILE_WIDTH * 7,
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            
        }

        public void Updade(GameTime gameTime)
        {
            workTime += gameTime.ElapsedGameTime.Milliseconds;

            if (workTime > 150)
            {
                if (destroyedTanksCount[currentTankType] > 0)
                {
                    soundBank.PlayCue("w1_scoreCount");
                    destroyedTanksCount[currentTankType]--;
                    tankScores[currentTankType].IncrementNumber();
                    workTime = 0;
                }
                else if (currentTankType < 3)
                {
                    currentTankType++;
                    workTime = -300;
                }

                else if (workTime > 4000)
                {
                    isDone = true;
                }
            }

        }

        public void UpdateData(int currentLevel)
        {
            stageNumberSprite.UpdateNumber(currentLevel);
            playerScoreSprite.UpdateNumber(playerScore);
            totalSprite.UpdateNumber(destroyedTanksCount.Sum());
        }

        public void ClearData()
        {
            for (int i = 0; i < tankScores.Count(); i++)
            {
                tankScores[i].ClearNumber();
            }
            workTime = 0;
            currentTankType = 0;
            destroyedTanksCount = new int[] { 0, 0, 0, 0 };
            isDone = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (StringSprite txt in text)
            {
                txt.Draw(spriteBatch);
            }

            spriteBatch.Draw(tankSprite, tank0drawRectangle, tank0SourceRectangle, Color.White);
            spriteBatch.Draw(tankSprite, tank1drawRectangle, tank1SourceRectangle, Color.White);
            spriteBatch.Draw(tankSprite, tank2drawRectangle, tank2SourceRectangle, Color.White);
            spriteBatch.Draw(tankSprite, tank3drawRectangle, tank3SourceRectangle, Color.White);

            stageNumberSprite.Draw(spriteBatch);
            playerScoreSprite.Draw(spriteBatch);

            for (int i = 0; i < tankScores.Count(); i++)
            {
                tankScores[i].Draw(spriteBatch);
            }

            if (workTime > 500)
                totalSprite.Draw(spriteBatch);
        }
    }

    class TankScore
    {
        NumberSprite scoreCountSprite;
        NumberSprite tankCountSprite;
        int tankType;

        public TankScore(ContentManager contentManager, double yPosition, int tankType)
        {
            this.tankType = tankType;
            scoreCountSprite = new NumberSprite(contentManager, 0, TextDirection.Right, Color.White,
                GameConstants.TILE_WIDTH * 3 + GameConstants.SMALL_TILE_WIDTH, (int)(GameConstants.TILE_WIDTH * yPosition));

            tankCountSprite = new NumberSprite(contentManager, 0, TextDirection.Right, Color.White,
                GameConstants.TILE_WIDTH * 7, (int)(GameConstants.TILE_WIDTH * yPosition));
        }

        public void IncrementNumber()
        {
            scoreCountSprite.UpdateNumber(scoreCountSprite.NumberValue + 100*(1 + tankType));
            tankCountSprite.UpdateNumber(tankCountSprite.NumberValue + 1);
        }

        public void ClearNumber()
        {
            scoreCountSprite.UpdateNumber(0);
            tankCountSprite.UpdateNumber(0);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            scoreCountSprite.Draw(spriteBatch);
            tankCountSprite.Draw(spriteBatch);
        }
    }
}
