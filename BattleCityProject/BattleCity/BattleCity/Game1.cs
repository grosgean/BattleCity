using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using MyDataTypes;

//using System.Xml;
//using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;


namespace BattleCity
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //GameState gameState = GameState.ScoreScreen;
        GameState gameState = GameState.Menu;
        GameState globalGameState = GameState.Play;

        // audio components
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;

        Cue cuePlayerMove;
        Cue cueEnemyMove;

        // PlayerTank

        PlayerTank playerTank;

        int playerCount = 1;

        static List<Bullet> bullets = new List<Bullet>();

        // levelData

        ScoreField scoreField;

        LevelData[] levelData;
        TileRow[] tilesData;

        List<Tile> tiles = new List<Tile>();

        Queue<int> enemyTanksData = new Queue<int>();

        Stack<EnemyTankIcon> enemyTankIcons = new Stack<EnemyTankIcon>();

        int enemyTanksTotalCount;
        int enemyTankCurrentNumber = 0;
        int enemyTankRespawnTime = 0;
        int enemyTankElapsedSpawnTime = 0;
        bool enemyTankCanSpawn = true;

        int enemyTankRespawnCounter = 1;
        int[] enemyTankRespawnPlace = {0, 6, 12};

        static Texture2D enemyTankSprite1;
        static Texture2D enemyTankSprite2;

        List<EnemyTank> enemyTanks = new List<EnemyTank>();

        List<PopUpEnemyTank> popUpEnemyTanks = new List<PopUpEnemyTank>();

        Texture2D tilesSprite;
        static Texture2D bulletSprite;

        int currentLevel = 1;

        List<Explosion> explosions = new List<Explosion>(); 

        List<Bonus> bonuses = new List<Bonus>();
        bool bonusTimerIsActive = false;
        int elapsedGameTimeBonusTimer = 0;
        bool bonusEagleArmorIsActive = false;
        int elapsedGameTimeBonusEagleArmor = 0;
        bool bonusEagleArmorVisible = false;
        int elapsedGameTimeEagleArmorVisible = 0;

        int waterElapsedCooldownTime = 0;
        bool waterChangeSprite = true;

        MenuScreen menuScreen;
        NextLevel nextLevelScreen;
        EndGameScreen endGameScreen;

        int levelEndTimer = 0;

        bool pauseIsActive = false;
        bool prevEnterState = false;

        Texture2D scoreSprite;
        Rectangle gameOverDrawRectangle;
        Rectangle gameOverSourceRectangle;

        Rectangle pauseDrawRectangle;
        Rectangle pauseSourceRectangle;

        int gameOverScoreDelay = 0;
        int endGameScreenDelay = 0;
        int pauseBlinkTime = 0;

        ScoreScreen scoreScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = GameConstants.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = GameConstants.WINDOW_HEIGHT;
            
            // creating XML файла
            //LevelData ExampleData = new LevelData();
            
            //XmlWriterSettings settings = new XmlWriterSettings();
            //settings.Indent = true;

            //using (XmlWriter writer = XmlWriter.Create("example.xml", settings))
            //{
            //    IntermediateSerializer.Serialize(writer, ExampleData, null);
            //}
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            RandomNumberGenerator.Initialize();

            GameData.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            scoreField = new ScoreField(Content, GraphicsDevice, currentLevel);

            playerTank = new PlayerTank(Content, "playertank1");

            enemyTankSprite1 = Content.Load<Texture2D>("enemytank1");
            enemyTankSprite2 = Content.Load<Texture2D>("enemytank2");

            bulletSprite = Content.Load<Texture2D>("bulletSprite");

            levelData = Content.Load<LevelData[]>("levelData");

            tilesSprite = Content.Load<Texture2D>("minitiles");

            // load audio content
            audioEngine = new AudioEngine(@"Content\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Sound Bank.xsb");

            cuePlayerMove = soundBank.GetCue("w6_playerMove");

            cueEnemyMove = soundBank.GetCue("w13_enemyMove");
            cueEnemyMove.Play();
            cueEnemyMove.Pause();

            menuScreen = new MenuScreen(Content);
            scoreScreen = new ScoreScreen(Content, currentLevel, soundBank);
            nextLevelScreen = new NextLevel(Content, GraphicsDevice, currentLevel);
            endGameScreen = new EndGameScreen(Content);

            scoreSprite = Content.Load<Texture2D>("scoreSprite");
            gameOverDrawRectangle = new Rectangle(
                GameConstants.TILE_WIDTH * (GameConstants.FIELD_WIDTH_TILES / 2) - GameConstants.SMALL_TILE_WIDTH,
                GameConstants.FIELD_HEIGHT,
                GameConstants.TILE_WIDTH * 2, GameConstants.TILE_WIDTH);
            gameOverSourceRectangle = new Rectangle(
                0, GameConstants.TILE_WIDTH + GameConstants.SMALL_TILE_WIDTH,
                GameConstants.TILE_WIDTH * 2, GameConstants.TILE_WIDTH);

            pauseDrawRectangle = new Rectangle(
                GameConstants.TILE_WIDTH * (GameConstants.FIELD_WIDTH_TILES / 2) - GameConstants.SMALL_TILE_WIDTH,
                GameConstants.GAME_OVER_Y_POSITION,
                GameConstants.TILE_WIDTH * 2, GameConstants.SMALL_TILE_WIDTH);
            pauseSourceRectangle = new Rectangle(
                0, GameConstants.TILE_WIDTH,
                GameConstants.TILE_WIDTH * 2 + GameConstants.SMALL_TILE_WIDTH, GameConstants.SMALL_TILE_WIDTH);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyboard = Keyboard.GetState();

            if (gameState == GameState.Menu)
            {
                menuScreen.Update(gameTime);
            }

            if (gameState == GameState.NextLevelScreen && nextLevelScreen.IsDone)
            {
                gameState = GameState.Play;
                nextLevelScreen.IsDone = false;
                scoreScreen.IsDone = false;

                InitNewLevel();
            }

            if (gameState == GameState.Menu && keyboard.IsKeyDown(Keys.Enter))
            {
                gameState = GameState.NextLevelScreen;
            }

            if (gameState == GameState.Play || gameState == GameState.End)
            {
                if (!pauseIsActive)
                    GamePlayUpdate(gameTime, keyboard);
            }

            if (gameState == GameState.Play && !prevEnterState && keyboard.IsKeyDown(Keys.Enter))
            {
                if (pauseIsActive)
                {
                    pauseIsActive = false;
                    pauseBlinkTime = 0;

                    if (cueEnemyMove.IsPaused)
                        cueEnemyMove.Resume();
                    else if (!cueEnemyMove.IsPlaying && !cueEnemyMove.IsPaused)
                        cueEnemyMove.Play();
                }
                else
                {
                    pauseIsActive = true;

                    if (!cueEnemyMove.IsPaused)
                        cueEnemyMove.Pause();

                    soundBank.PlayCue("w5_pause");
                }
            }

            prevEnterState = keyboard.IsKeyDown(Keys.Enter);

            if (gameState == GameState.NextLevelScreen)
            {
                nextLevelScreen.Update(gameTime);

                if (nextLevelScreen.IsDone && keyboard.IsKeyDown(Keys.Enter))
                {
                    gameState = GameState.Play;
                }
            }

            if (gameState == GameState.End)
            {
                if (gameOverDrawRectangle.Y > GameConstants.GAME_OVER_Y_POSITION)
                {
                    gameOverDrawRectangle.Y -= 4;
                }
                else
                {
                    gameOverScoreDelay += gameTime.ElapsedGameTime.Milliseconds;
                    if (gameOverScoreDelay > 3000)
                    {
                        if (!cueEnemyMove.IsPaused)
                            cueEnemyMove.Pause();

                        if (!cuePlayerMove.IsPaused)
                            cuePlayerMove.Pause();

                        scoreScreen.UpdateData(currentLevel);
                        gameState = GameState.ScoreScreen;
                    }
                }
            }

            if (gameState == GameState.ScoreScreen)
            {
                if (scoreScreen.IsDone)
                {
                    enemyTanks.Clear();
                    bullets.Clear();
                    explosions.Clear();

                    gameOverScoreDelay = 0;
                    gameOverDrawRectangle.Y = GameConstants.FIELD_HEIGHT;

                    if (globalGameState == GameState.Play)
                    {
                        // adding level
                        currentLevel += 1;
                        currentLevel = currentLevel < levelData.Count() ? currentLevel : 0;
                        nextLevelScreen.ChangeStage(currentLevel);

                        gameState = GameState.NextLevelScreen;
                    }
                    else
                    {
                        nextLevelScreen.ChangeStage(1);
                        gameState = GameState.EndGameScreen;
                        soundBank.PlayCue("w11_gameOver");
                    }
                }
                else
                {
                    scoreScreen.Updade(gameTime);
                }
            }

            if (gameState == GameState.EndGameScreen)
            {
                if (endGameScreenDelay < 3000)
                {
                    endGameScreenDelay += gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    endGameScreenDelay = 0;
                    currentLevel = 1;
                    gameState = GameState.Menu;
                    globalGameState = GameState.Play;
                }
            }

            base.Update(gameTime);
        }

        protected void GamePlayUpdate(GameTime gameTime, KeyboardState keyboard)
        {
            // Spawn EnemyTank if appropriate
            if (enemyTankCanSpawn && enemyTanksData.Count > 0)
            {
                PopUpEnemyTank popUpEnemyTank = new PopUpEnemyTank(Content, GameConstants.TILE_WIDTH * enemyTankRespawnPlace[enemyTankRespawnCounter], 0);

                popUpEnemyTanks.Add(popUpEnemyTank);

                enemyTankCanSpawn = false;
            }

            if (!enemyTankCanSpawn)
            {
                enemyTankElapsedSpawnTime += gameTime.ElapsedGameTime.Milliseconds;
                if (enemyTanks.Count < GameConstants.ENEMY_TANK_MAX_COUNT && enemyTankElapsedSpawnTime > enemyTankRespawnTime)
                {
                    enemyTankElapsedSpawnTime = 0;
                    enemyTankCanSpawn = true;
                }
            }


            foreach (Explosion explosion in explosions)
            {
                explosion.Update(gameTime);
            }

            if (bonusTimerIsActive)
            {
                elapsedGameTimeBonusTimer -= gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedGameTimeBonusTimer < 0)
                {
                    elapsedGameTimeBonusTimer = 0;
                    bonusTimerIsActive = false;
                }
            }

            if (bonusEagleArmorIsActive)
            {
                BonusEagleArmor(gameTime);
            }


            foreach (EnemyTank enemyTank in enemyTanks)
            {
                enemyTank.CanMoveLeft = enemyTank.MovementSpeed;
                enemyTank.CanMoveRight = enemyTank.MovementSpeed;
                enemyTank.CanMoveUp = enemyTank.MovementSpeed;
                enemyTank.CanMoveDown = enemyTank.MovementSpeed;

                enemyTank.RefreshMovementSpeed();

                // проверить возможность движения enemyTank во всех направлениях

                foreach (Tile tile in tiles)
                {
                    // et Left
                    if (!tile.Crossable && (tile.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Left - enemyTank.MovementSpeed, enemyTank.CollisionRectangle.Top) ||
                        tile.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Left - enemyTank.MovementSpeed, enemyTank.CollisionRectangle.Bottom - 1)))
                    {
                        if ((enemyTank.CollisionRectangle.Left - tile.CollisionRectangle.Right) < enemyTank.CanMoveLeft)
                            enemyTank.CanMoveLeft = enemyTank.CollisionRectangle.Left - tile.CollisionRectangle.Right;
                    }
                    // et Right
                    if (!tile.Crossable && (tile.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Right + enemyTank.MovementSpeed, enemyTank.CollisionRectangle.Top) ||
                        tile.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Right + enemyTank.MovementSpeed, enemyTank.CollisionRectangle.Bottom - 1)))
                    {
                        if ((tile.CollisionRectangle.Left - enemyTank.CollisionRectangle.Right) < enemyTank.CanMoveRight)
                            enemyTank.CanMoveRight = tile.CollisionRectangle.Left - enemyTank.CollisionRectangle.Right;
                    }
                    // et Up
                    if (!tile.Crossable && (tile.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Left, enemyTank.CollisionRectangle.Top - enemyTank.MovementSpeed) ||
                        tile.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Right - 1, enemyTank.CollisionRectangle.Top - enemyTank.MovementSpeed)))
                    {
                        if ((enemyTank.CollisionRectangle.Top - tile.CollisionRectangle.Bottom) < enemyTank.CanMoveUp)
                            enemyTank.CanMoveUp = enemyTank.CollisionRectangle.Top - tile.CollisionRectangle.Bottom;
                    }
                    // et Down
                    if (!tile.Crossable && (tile.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Left, enemyTank.CollisionRectangle.Bottom + enemyTank.MovementSpeed) ||
                        tile.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Right - 1, enemyTank.CollisionRectangle.Bottom + enemyTank.MovementSpeed)))
                    {
                        if ((tile.CollisionRectangle.Top - enemyTank.CollisionRectangle.Bottom) < enemyTank.CanMoveDown)
                            enemyTank.CanMoveDown = tile.CollisionRectangle.Top - enemyTank.CollisionRectangle.Bottom;
                    }
                }


                // проверка et на столкновение с pt 
                if (playerTank.IsActive)
                {
                    // et Left
                    if (playerTank.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Left - enemyTank.MovementSpeed, enemyTank.CollisionRectangle.Top) ||
                        playerTank.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Left - enemyTank.MovementSpeed, enemyTank.CollisionRectangle.Bottom - 1))
                    {
                        if ((enemyTank.CollisionRectangle.Left - playerTank.CollisionRectangle.Right) < enemyTank.CanMoveLeft)
                            enemyTank.CanMoveLeft = enemyTank.CollisionRectangle.Left - playerTank.CollisionRectangle.Right;
                    }
                    // et Right
                    if (playerTank.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Right + enemyTank.MovementSpeed, enemyTank.CollisionRectangle.Top) ||
                        playerTank.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Right + enemyTank.MovementSpeed, enemyTank.CollisionRectangle.Bottom - 1))
                    {
                        if ((playerTank.CollisionRectangle.Left - enemyTank.CollisionRectangle.Right) < enemyTank.CanMoveRight)
                            enemyTank.CanMoveRight = playerTank.CollisionRectangle.Left - enemyTank.CollisionRectangle.Right;
                    }
                    // et Up
                    if (playerTank.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Left, enemyTank.CollisionRectangle.Top - enemyTank.MovementSpeed) ||
                        playerTank.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Right - 1, enemyTank.CollisionRectangle.Top - enemyTank.MovementSpeed))
                    {
                        if ((enemyTank.CollisionRectangle.Top - playerTank.CollisionRectangle.Bottom) < enemyTank.CanMoveUp)
                            enemyTank.CanMoveUp = enemyTank.CollisionRectangle.Top - playerTank.CollisionRectangle.Bottom;
                    }
                    // et Down
                    if (playerTank.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Left, enemyTank.CollisionRectangle.Bottom + enemyTank.MovementSpeed) ||
                        playerTank.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Right - 1, enemyTank.CollisionRectangle.Bottom + enemyTank.MovementSpeed))
                    {
                        if ((playerTank.CollisionRectangle.Top - enemyTank.CollisionRectangle.Bottom) < enemyTank.CanMoveDown)
                            enemyTank.CanMoveDown = playerTank.CollisionRectangle.Top - enemyTank.CollisionRectangle.Bottom;
                    }
                }


                foreach (EnemyTank enemyTank2 in enemyTanks)
                {
                    // проверка et на столкновение с et 
                    // et Left
                    if (enemyTank2.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Left - enemyTank.MovementSpeed, enemyTank.CollisionRectangle.Top) ||
                        enemyTank2.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Left - enemyTank.MovementSpeed, enemyTank.CollisionRectangle.Bottom - 1))
                    {
                        if ((enemyTank.CollisionRectangle.Left - enemyTank2.CollisionRectangle.Right) < enemyTank.CanMoveLeft)
                            enemyTank.CanMoveLeft = enemyTank.CollisionRectangle.Left - enemyTank2.CollisionRectangle.Right;
                    }
                    // et Right
                    if (enemyTank2.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Right + enemyTank.MovementSpeed, enemyTank.CollisionRectangle.Top) ||
                        enemyTank2.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Right + enemyTank.MovementSpeed, enemyTank.CollisionRectangle.Bottom - 1))
                    {
                        if ((enemyTank2.CollisionRectangle.Left - enemyTank.CollisionRectangle.Right) < enemyTank.CanMoveRight)
                            enemyTank.CanMoveRight = enemyTank2.CollisionRectangle.Left - enemyTank.CollisionRectangle.Right;
                    }
                    // et Up
                    if (enemyTank2.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Left, enemyTank.CollisionRectangle.Top - enemyTank.MovementSpeed) ||
                        enemyTank2.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Right - 1, enemyTank.CollisionRectangle.Top - enemyTank.MovementSpeed))
                    {
                        if ((enemyTank.CollisionRectangle.Top - enemyTank2.CollisionRectangle.Bottom) < enemyTank.CanMoveUp)
                            enemyTank.CanMoveUp = enemyTank.CollisionRectangle.Top - enemyTank2.CollisionRectangle.Bottom;
                    }
                    // et Down
                    if (enemyTank2.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Left, enemyTank.CollisionRectangle.Bottom + enemyTank.MovementSpeed) ||
                        enemyTank2.CollisionRectangle.Contains(enemyTank.CollisionRectangle.Right - 1, enemyTank.CollisionRectangle.Bottom + enemyTank.MovementSpeed))
                    {
                        if ((enemyTank2.CollisionRectangle.Top - enemyTank.CollisionRectangle.Bottom) < enemyTank.CanMoveDown)
                            enemyTank.CanMoveDown = enemyTank2.CollisionRectangle.Top - enemyTank.CollisionRectangle.Bottom;
                    }
                }

                enemyTank.Destination = new Vector2(playerTank.CollisionRectangleX, playerTank.CollisionRectangleY);

                enemyTank.Update(gameTime, bonusTimerIsActive);

            }


            playerTank.RefreshMovementSpeed();

            foreach (EnemyTank enemyTank in enemyTanks)
            {

                // проверить возможность движения playerTank во всех направлениях
                // pt Left
                if (enemyTank.CollisionRectangle.Contains(playerTank.CollisionRectangle.Left - GameConstants.PLAYER_TANK_MOVEMENT_SPEED, playerTank.CollisionRectangle.Top) ||
                    enemyTank.CollisionRectangle.Contains(playerTank.CollisionRectangle.Left - GameConstants.PLAYER_TANK_MOVEMENT_SPEED, playerTank.CollisionRectangle.Bottom - 1))
                {
                    if ((playerTank.CollisionRectangle.Left - enemyTank.CollisionRectangle.Right) < playerTank.CanMoveLeft)
                        playerTank.CanMoveLeft = playerTank.CollisionRectangle.Left - enemyTank.CollisionRectangle.Right;
                }
                // pt Right
                if (enemyTank.CollisionRectangle.Contains(playerTank.CollisionRectangle.Right + GameConstants.PLAYER_TANK_MOVEMENT_SPEED, playerTank.CollisionRectangle.Top) ||
                    enemyTank.CollisionRectangle.Contains(playerTank.CollisionRectangle.Right + GameConstants.PLAYER_TANK_MOVEMENT_SPEED, playerTank.CollisionRectangle.Bottom - 1))
                {
                    if ((enemyTank.CollisionRectangle.Left - playerTank.CollisionRectangle.Right) < playerTank.CanMoveRight)
                        playerTank.CanMoveRight = enemyTank.CollisionRectangle.Left - playerTank.CollisionRectangle.Right;
                }
                // pt Up
                if (enemyTank.CollisionRectangle.Contains(playerTank.CollisionRectangle.Left, playerTank.CollisionRectangle.Top - GameConstants.PLAYER_TANK_MOVEMENT_SPEED) ||
                    enemyTank.CollisionRectangle.Contains(playerTank.CollisionRectangle.Right - 1, playerTank.CollisionRectangle.Top - GameConstants.PLAYER_TANK_MOVEMENT_SPEED))
                {
                    if ((playerTank.CollisionRectangle.Top - enemyTank.CollisionRectangle.Bottom) < playerTank.CanMoveUp)
                        playerTank.CanMoveUp = playerTank.CollisionRectangle.Top - enemyTank.CollisionRectangle.Bottom;
                }
                // pt Down
                if (enemyTank.CollisionRectangle.Contains(playerTank.CollisionRectangle.Left, playerTank.CollisionRectangle.Bottom + GameConstants.PLAYER_TANK_MOVEMENT_SPEED) ||
                    enemyTank.CollisionRectangle.Contains(playerTank.CollisionRectangle.Right - 1, playerTank.CollisionRectangle.Bottom + GameConstants.PLAYER_TANK_MOVEMENT_SPEED))
                {
                    if ((enemyTank.CollisionRectangle.Top - playerTank.CollisionRectangle.Bottom) < playerTank.CanMoveDown)
                        playerTank.CanMoveDown = enemyTank.CollisionRectangle.Top - playerTank.CollisionRectangle.Bottom;
                }

            }

            playerTank.Update(gameTime, keyboard, soundBank, cuePlayerMove);


            foreach (Tile tile in tiles)
            {
                switch (playerTank.LastKey)
                {
                    case "Left":
                        if (!tile.Crossable && tile.CollisionRectangle.Intersects(playerTank.CollisionRectangle))
                            playerTank.CollisionRectangleX -= playerTank.CollisionRectangle.Left - tile.CollisionRectangle.Right;
                        break;

                    case "Right":
                        if (!tile.Crossable && tile.CollisionRectangle.Intersects(playerTank.CollisionRectangle))
                            playerTank.CollisionRectangleX -= playerTank.CollisionRectangle.Right - tile.CollisionRectangle.Left;
                        break;

                    case "Up":
                        if (!tile.Crossable && tile.CollisionRectangle.Intersects(playerTank.CollisionRectangle))
                            playerTank.CollisionRectangleY -= playerTank.CollisionRectangle.Top - tile.CollisionRectangle.Bottom;
                        break;

                    case "Down":
                        if (!tile.Crossable && tile.CollisionRectangle.Intersects(playerTank.CollisionRectangle))
                            playerTank.CollisionRectangleY -= playerTank.CollisionRectangle.Bottom - tile.CollisionRectangle.Top;
                        break;

                    default:
                        break;
                }

                // столкновение пули со стеной
                foreach (Bullet bullet in bullets)
                {
                    if (tile.CollisionRectangle.Intersects(bullet.CollisionRectangle) && tile.TileNumber > 0 && tile.TileNumber < 17)
                    {
                        bullet.IsActive = false;

                        //взрыв
                        Explosion explosion = new Explosion(bulletSprite, bullet.CollisionRectangle.Center.X, bullet.CollisionRectangle.Center.Y);
                        explosions.Add(explosion);

                        if (!(tile.TileNumber == 16 && bullet.BulletPower == BulletPower.Low))     // броня не разбивается слабой пулей
                        {
                            tile.TileChangeState(GameData.TilesChangeState[bullet.Direction][tile.TileNumber]);
                            if (bullet.Type == BulletType.Player)
                                soundBank.PlayCue("w12_bullet2brick");
                        }
                        else
                        {
                            if (bullet.Type == BulletType.Player)
                                soundBank.PlayCue("w15_bullet2wall");
                        }
                    }

                    // уничтожение штаба
                    if (tile.TileNumber > 19 && tile.CollisionRectangle.Intersects(bullet.CollisionRectangle))
                    {
                        // конец игры
                        bullet.IsActive = false;
                        gameState = GameState.End;
                        globalGameState = GameState.End;
                        playerTank.IsActive = false;

                        soundBank.PlayCue("w14_playerExplosion");
                    }
                }
            }

            // переход на следующий уровень
            if (enemyTanksData.Count == 0 && enemyTanks.Count == 0 && playerTank.Lives > 0)
            {
                // счетчик для автом окончания
                levelEndTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (levelEndTimer > 4000)
                {
                    scoreScreen.UpdateData(currentLevel);
                    levelEndTimer = 0;

                    if (!cuePlayerMove.IsPaused)
                        cuePlayerMove.Pause();

                    gameState = GameState.ScoreScreen;

                }

                if (!cueEnemyMove.IsPaused)
                    cueEnemyMove.Pause();
            }

            foreach (Bullet bullet in bullets)
            {
                bullet.Update(gameTime, soundBank);
            }

            foreach (Bullet bullet in bullets)
            {
                // уничтожение playerTank
                if (playerTank.IsActive && bullet.Type == BulletType.Enemy && bullet.CollisionRectangle.Intersects(playerTank.CollisionRectangle))
                {
                    bullet.IsActive = false;

                    if (!playerTank.ShieldIsActive)
                    {
                        Explosion explosion = new Explosion(bulletSprite, playerTank.CollisionRectangle.Center.X, playerTank.CollisionRectangle.Center.Y);
                        explosions.Add(explosion);

                        soundBank.PlayCue("w14_playerExplosion");

                        playerTank.Lives -= 1;
                        scoreField.ChangeLives(playerTank.Lives);
                        playerTank.Visible = false;
                        playerTank.IsActive = false;

                        if (playerTank.Lives > 0)
                            playerTank.Respawn("death");
                        else
                        {
                            playerTank.IsActive = false;

                            if (!cuePlayerMove.IsPaused)
                                cuePlayerMove.Pause();

                            gameState = GameState.End;
                            globalGameState = GameState.End;
                        }
                    }
                }

                // end game
                if (gameState == GameState.End)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile.TileNumber == 25)
                        {
                            Explosion explosion = new Explosion(bulletSprite, tile.CollisionRectangle.X, tile.CollisionRectangle.Y);
                            explosions.Add(explosion);
                        }
                        if (tile.TileNumber > 19 && tile.TileNumber < 28)
                            tile.TileChangeState(GameData.TilesChangeState["Left"][tile.TileNumber]);
                    }
                }

                // попадание пули во вражеский танк
                foreach (EnemyTank enemyTank in enemyTanks)
                {
                    if (bullet.Type == BulletType.Player && bullet.CollisionRectangle.Intersects(enemyTank.CollisionRectangle))
                    {
                        bullet.IsActive = false;
                        enemyTank.Lives -= 1;
                        if (enemyTank.HasBonus)
                        {
                            soundBank.PlayCue("w4_popUpBonus");
                            bonuses.Clear();
                            Bonus bonus = new Bonus(bulletSprite);
                            bonuses.Add(bonus);

                            enemyTank.DisableBonus();
                        }

                        if (enemyTank.Lives == 0)
                        {
                            scoreScreen.DestroyedTanksCount[enemyTank.TankType] += 1;
                            scoreScreen.PlayerScore += enemyTank.TankType * 100 + 100;

                            Explosion explosion = new Explosion(bulletSprite, enemyTank.CollisionRectangle.Center.X, enemyTank.CollisionRectangle.Center.Y);
                            explosions.Add(explosion);
                            soundBank.PlayCue("w7_tankExplosion");
                        }
                        else
                        {
                            soundBank.PlayCue("w15_bullet2wall");
                        }
                    }
                }
            }

            // взятие бонуса
            foreach (Bonus bonus in bonuses)
            {
                bonus.Update(gameTime);

                if (bonus.CollisionRectangle.Intersects(playerTank.CollisionRectangle))
                {
                    if (bonus.Type == 5)
                    {
                        soundBank.PlayCue("w10_bonusLife");
                    }
                    else
                    {
                        soundBank.PlayCue("w2_pickBonus");
                        if (bonus.Type == 4)
                            soundBank.PlayCue("w7_tankExplosion");
                    }

                    scoreScreen.PlayerScore += 500;
                    BonusApply(bonus.Type);
                    bonus.IsActive = false;
                }
            }

            // столкновение двух пуль
            for (int i = 0; i < bullets.Count; i++)
            {
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (i != j && bullets[i].CollisionRectangle.Intersects(bullets[j].CollisionRectangle))
                    {
                        bullets[i].IsActive = false;
                        bullets[j].IsActive = false;
                    }
                }
            }

            // мерцание при появлении вражеского танка
            foreach (PopUpEnemyTank popUpEnemyTank in popUpEnemyTanks)
            {
                popUpEnemyTank.Update(gameTime);
            }

            // появление вражеского танка
            for (int i = popUpEnemyTanks.Count - 1; i >= 0; i--)
            {
                if (popUpEnemyTanks[i].PopUpTimer == 0)
                {
                    popUpEnemyTanks.RemoveAt(i);
                    SpawnEnemyTank();
                }
            }

            // смена спрайта воды
            waterElapsedCooldownTime += gameTime.ElapsedGameTime.Milliseconds;
            if (waterElapsedCooldownTime > 800)
            {
                waterElapsedCooldownTime = 0;

                foreach (Tile tile in tiles)
                {
                    if (tile.TileNumber == 17)
                        tile.WaterChangeState(waterChangeSprite);
                }

                waterChangeSprite = waterChangeSprite ? false : true;
            }

            for (int i = enemyTanks.Count - 1; i >= 0; i--)
            {
                if (enemyTanks[i].Lives == 0)
                {
                    enemyTanks.RemoveAt(i);
                }
            }

            for (int i = tiles.Count - 1; i >= 0; i--)
            {
                if (!tiles[i].IsActive)
                {
                    tiles.RemoveAt(i);
                }
            }

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                if (!bullets[i].IsActive)
                {
                    bullets.RemoveAt(i);
                }
            }

            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                if (explosions[i].Finished)
                {
                    explosions.RemoveAt(i);
                }
            }

            for (int i = bonuses.Count - 1; i >= 0; i--)
            {
                if (!bonuses[i].IsActive)
                {
                    bonuses.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (gameState == GameState.Play || gameState == GameState.End)
            {

                scoreField.Draw(spriteBatch);

                foreach (Tile tile in tiles)
                {
                    if (tile.TileNumber != 18)
                        tile.Draw(spriteBatch);
                }

                playerTank.Draw(spriteBatch);

                foreach (EnemyTank enemyTank in enemyTanks)
                {
                    enemyTank.Draw(spriteBatch);
                }

                foreach (Bullet bullet in bullets)
                {
                    bullet.Draw(spriteBatch);
                }

                foreach (Tile tile in tiles)
                {
                    if (tile.TileNumber == 18)
                        tile.Draw(spriteBatch);
                }

                foreach (Explosion explosion in explosions)
                {
                    explosion.Draw(spriteBatch);
                }

                foreach (Bonus bonus in bonuses)
                {
                    bonus.Draw(spriteBatch);
                }

                foreach (PopUpEnemyTank popUpEnemyTank in popUpEnemyTanks)
                {
                    popUpEnemyTank.Draw(spriteBatch);
                }

                foreach (EnemyTankIcon enemyTankIcon in enemyTankIcons)
                {
                    enemyTankIcon.Draw(spriteBatch);
                }

                if (pauseIsActive)
                {
                    pauseBlinkTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (pauseBlinkTime < 300)
                        spriteBatch.Draw(scoreSprite, pauseDrawRectangle, pauseSourceRectangle, Color.White);
                    else if (pauseBlinkTime > 600)
                        pauseBlinkTime = 0;
                }

            }
            else if (gameState == GameState.Menu)
            {
                menuScreen.Draw(spriteBatch);
            }
            else if (gameState == GameState.ScoreScreen)
            {
                scoreScreen.Draw(spriteBatch);
            }
            else if (gameState == GameState.NextLevelScreen)
            {
                nextLevelScreen.Draw(spriteBatch);
            }
            else if (gameState == GameState.EndGameScreen)
            {
                endGameScreen.Draw(spriteBatch);
            }

            if (gameState == GameState.End)
            {
                spriteBatch.Draw(scoreSprite, gameOverDrawRectangle, gameOverSourceRectangle, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void InitNewLevel()
        {
            if (currentLevel == 1)
            {
                playerTank.Lives = GameConstants.PLAYER_ONE_LIVES_COUNT;
                scoreScreen.PlayerScore = 0;
            }

            enemyTankIcons.Clear();

            scoreField.ChangeStage(currentLevel);
            scoreField.ChangeLives(playerTank.Lives);

            enemyTanksData = new Queue<int>(levelData[currentLevel].EnemyTanks);

            enemyTanksTotalCount = enemyTanksData.Count;

            // difficulty
            //enemyTankRespawnTime = (int)(190 - currentLevel * 4 - (playerCount - 1)) * 1000 / 60;
            enemyTankRespawnTime = (int)(120 - currentLevel * 8 - (playerCount - 1)) * 1000 / 60;

            TileRow[] defaultLevelTiles = levelData[0].TilesData;

            tilesData = levelData[currentLevel].TilesData;

            tiles.Clear();

            for (int row = 0; row < defaultLevelTiles.Count(); row++)
            {
                for (int col = 0; col < defaultLevelTiles[row].Tiles.Count(); col++)
                {
                    int tilePosition = defaultLevelTiles[row].Tiles[col] != 0 ? defaultLevelTiles[row].Tiles[col] : tilesData[row].Tiles[col];

                    bool eagleArmor = defaultLevelTiles[row].Tiles[col] != 0 && defaultLevelTiles[row].Tiles[col] != 14 && defaultLevelTiles[row].Tiles[col] != 17 ?
                        true : false;        

                    if (GameData.BigTilesContent[tilePosition].X != 0)
                        tiles.Add(new Tile(tilesSprite, col, row, GameData.BigTilesContent[tilePosition].X, 0, 0, eagleArmor));
                    if (GameData.BigTilesContent[tilePosition].Y != 0)
                        tiles.Add(new Tile(tilesSprite, col, row, GameData.BigTilesContent[tilePosition].Y, 1, 0, eagleArmor));
                    if (GameData.BigTilesContent[tilePosition].Z != 0)
                        tiles.Add(new Tile(tilesSprite, col, row, GameData.BigTilesContent[tilePosition].Z, 0, 1, eagleArmor));
                    if (GameData.BigTilesContent[tilePosition].W != 0)
                        tiles.Add(new Tile(tilesSprite, col, row, GameData.BigTilesContent[tilePosition].W, 1, 1, eagleArmor));
                }
            }

            for (int icon = 0; icon < enemyTanksData.Count(); icon++)
            {
                EnemyTankIcon enemyTankIcon = new EnemyTankIcon(Content, icon);
                enemyTankIcons.Push(enemyTankIcon);
            }

            bonusTimerIsActive = false;
            bonusEagleArmorIsActive = false;
            bonuses.Clear();
            scoreScreen.ClearData();

            enemyTankRespawnCounter = 1;
            enemyTankCurrentNumber = 0;
            playerTank.Respawn("newlevel");

            soundBank.PlayCue("w8_startGame");
        }

        protected void SpawnEnemyTank()
        {
            int tankType = enemyTanksData.Dequeue();

            EnemyTank enemyTank = new EnemyTank(Content, tankType, enemyTankCurrentNumber,
                    GameConstants.TILE_WIDTH * enemyTankRespawnPlace[enemyTankRespawnCounter], 0, enemyTankRespawnTime);

            enemyTankRespawnCounter++;
            if (enemyTankRespawnCounter > 2 )
                enemyTankRespawnCounter = 0;

            enemyTankCurrentNumber++; 

            enemyTanks.Add(enemyTank);
            enemyTankIcons.Pop();

            if (cueEnemyMove.IsPaused)
                cueEnemyMove.Resume();
            else if (!cueEnemyMove.IsPlaying && !cueEnemyMove.IsPaused)
                cueEnemyMove.Play();
        }

        public static Texture2D GetBulletSprite()
        {
            return bulletSprite; ;
        }

        public static Texture2D GetEnemyTankSprite()
        {
            return enemyTankSprite1;
        }

        public static void AddBullet(Bullet bullet)
        {
            bullets.Add(bullet);
        }

        public static int PlayerBulletCount()
        {
            return bullets.Count(b => b.Type == BulletType.Player);
        }

        public static int EnemyBulletCount(int tankNumber)
        {
            return bullets.Count(b => b.Type == BulletType.Enemy && b.TankNumber == tankNumber);
        }

        protected void BonusApply(int bonusType)
        {
            switch (bonusType)
            {
                case 0: // щит
                    playerTank.BonusShield();
                    break;

                case 1: // остановка танков
                    bonusTimerIsActive = true;
                    elapsedGameTimeBonusTimer = GameConstants.PLAYER_SHIELD_ACTIVE_TIME * GameConstants.PLAYER_BONUS_LENGTH;
                    break;

                case 2: // броня штаба
                    foreach (Tile tile in tiles)
                    {
                        if (tile.EagleArmor == true)
                        {
                            tile.TileChangeStateEagleArmor(true);
                        }
                    }
                    elapsedGameTimeBonusEagleArmor = GameConstants.PLAYER_SHIELD_ACTIVE_TIME * GameConstants.PLAYER_BONUS_LENGTH;
                    bonusEagleArmorIsActive = true;

                    break;

                case 3: // звезда
                    playerTank.BonusStar();
                    break;

                case 4: // взрыв танков
                    for (int i = enemyTanks.Count - 1; i >= 0; i--)
                    {
                        enemyTanks[i].Lives = 0;
                        Explosion explosion = new Explosion(bulletSprite, enemyTanks[i].CollisionRectangle.Center.X, enemyTanks[i].CollisionRectangle.Center.Y);
                        explosions.Add(explosion);
                    }
                    break;

                case 5: // жизнь
                    playerTank.Lives += 1;
                    scoreField.ChangeLives(playerTank.Lives);
                    break;

                default:
                    break;
            }
        }

        protected void BonusEagleArmor(GameTime gameTime)
        {
            if (elapsedGameTimeBonusEagleArmor < GameConstants.PLAYER_SHIELD_ACTIVE_TIME)
            {
                elapsedGameTimeEagleArmorVisible += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedGameTimeEagleArmorVisible > 300)
                {
                    elapsedGameTimeEagleArmorVisible = 0;
                    bonusEagleArmorVisible = bonusEagleArmorVisible ? false : true;

                    foreach (Tile tile in tiles)
                    {
                        if (tile.EagleArmor == true)
                        {
                            tile.TileChangeStateEagleArmor(bonusEagleArmorVisible);
                        }
                    }
                }
            }

            elapsedGameTimeBonusEagleArmor -= gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedGameTimeBonusEagleArmor < 0)
            {
                elapsedGameTimeBonusEagleArmor = 0;
                bonusEagleArmorIsActive = false;

                foreach (Tile tile in tiles)
                {
                    if (tile.EagleArmor == true)
                    {
                        tile.TileChangeStateEagleArmor(false);
                    }
                }
            }
        }
    }
}
