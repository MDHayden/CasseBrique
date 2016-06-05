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

namespace CasseBrique
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CasseBrique : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Screen adjustment
        private int screenWidth = 800, screenHeight = 600;

        private const int NB_LIGNE_BRIQUE = 4;
        private const int NB_COLONNE_BRIQUE = 10;
        private const int OFFSET = 25; // Offset entre les lignes et colonnes de briques
        private const int X_SIZE_BRIQUE = 119;
        private float _briqueScale;

        // Unitiliser une enumération ici :
        private enum BriqueColor
        {
            Black = 0, // Automatiquement les suivants vont être incrémentés
            Purple,
            Red,
            Orange,
            Blue,
            Yellow,
            Grey,
            Orange2,
            Purple2,
            Milk,
            Menthe,
            Black2
        }

        public static Texture2D[] _briqueTextures = new Texture2D[20]; // Taille arbitraire

        private int _nbBriques = NB_LIGNE_BRIQUE * NB_COLONNE_BRIQUE;

        //private KeyboardState _keyboardState;
        //private MouseState _mouseState;

        private Menu _menu;
        private Balle _balle;
        private Raquette _raquette;
        private Brique[,] _briques = new Brique[NB_LIGNE_BRIQUE, NB_COLONNE_BRIQUE];

        private SpriteFont _scoreFont;
        private SpriteFont _timerFont;
        private SpriteFont _globalFont;
        //private SpriteFont _gameOverFont;

        private int _nbBalles;
        private int _score;

        private Vector2 _windowSize;
        private Texture2D _background;
        private Texture2D _menuBackground;
        private Rectangle _mainFrame;

        private SoundEffect _rebondRaquette;
        private SoundEffect _rebondBrique;
        private Song _music;

        public enum GameState
        {
            MainMenu,
            Options,
            //Levels,
            //Level1
            Playing,
            Paused
        }
        //public GameState CurrentGameState = GameState.MainMenu;
        public GameState CurrentGameState = GameState.MainMenu;

        private Bouton _play;

        public CasseBrique()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Screen stuff
            
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();

            _windowSize.X = this.graphics.PreferredBackBufferWidth;
            _windowSize.Y = this.graphics.PreferredBackBufferHeight;

            IsMouseVisible = true; // A enlever

            _mainFrame = new Rectangle(0, 0, (int)GraphicsDevice.Viewport.Width, (int)GraphicsDevice.Viewport.Height); //_windowSize.X, _windowSize.Y

            _menu = new Menu(this, _windowSize);
            //this.InitGame();
            _nbBalles = 3;
            _score = 0;
            _balle = new Balle(this, _windowSize);
            _raquette = new Raquette(this, _windowSize);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Backgrounds
            _background = Content.Load<Texture2D>(@"images/fondniveau1");
            _menuBackground = Content.Load<Texture2D>(@"images/fondmenu");

            // Boutons menu
            _play = new Bouton(Content.Load<Texture2D>(@"images/play"), graphics.GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _scoreFont = Content.Load<SpriteFont>("ScoreFont");
            _timerFont = Content.Load<SpriteFont>("TimerFont");
            _globalFont = Content.Load<SpriteFont>("ScoreFont");

            _briqueTextures[(int)BriqueColor.Black] = Content.Load<Texture2D>(@"images/briquenoire");
            _briqueTextures[(int)BriqueColor.Purple] = Content.Load<Texture2D>(@"images/briqueviolet");
            _briqueTextures[(int)BriqueColor.Red] = Content.Load<Texture2D>(@"images/briquerouge");
            _briqueTextures[(int)BriqueColor.Orange] = Content.Load<Texture2D>(@"images/briqueorange");
            _briqueTextures[(int)BriqueColor.Blue] = Content.Load<Texture2D>(@"images/briquebleue");
            _briqueTextures[(int)BriqueColor.Yellow] = Content.Load<Texture2D>(@"images/briquepoint");
            _briqueTextures[(int)BriqueColor.Grey] = Content.Load<Texture2D>(@"images/briquegrise");
            _briqueTextures[(int)BriqueColor.Orange2] = Content.Load<Texture2D>(@"images/brique_orange");
            _briqueTextures[(int)BriqueColor.Purple2] = Content.Load<Texture2D>(@"images/brique_violette");
            _briqueTextures[(int)BriqueColor.Milk] = Content.Load<Texture2D>(@"images/briquelait");
            _briqueTextures[(int)BriqueColor.Menthe] = Content.Load<Texture2D>(@"images/brique_menthe");
            _briqueTextures[(int)BriqueColor.Black2] = Content.Load<Texture2D>(@"images/brique_noire");

            // Calcul du scale d'affichage des briques afin de les centrer convenablement en fonction de leur nombre :
            float sizeX = (_windowSize.X - (2 * OFFSET)) / NB_COLONNE_BRIQUE; //_windowSize.Y * 0.4f / NB_LIGNE_BRIQUE
            _briqueScale = sizeX / _briqueTextures[(int)BriqueColor.Black].Width;

            _rebondRaquette = Content.Load<SoundEffect>(@"sons\bounce");
            //_rebondBrique = Content.Load<SoundEffect>(@"sons\rebond-terre_battue");
            _music = Content.Load<Song>(@"sons\song1-dream");
            MediaPlayer.Play(_music);
            MediaPlayer.IsRepeating = true;

            this.GenerateBrickWall();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState _keyboardState = Keyboard.GetState();

            switch(CurrentGameState)
            {
                case GameState.MainMenu:
                    MouseState mouseState = Mouse.GetState();
                    if (_play.isClicked == true) CurrentGameState = GameState.Playing;
                    if (IsMouseVisible == false) IsMouseVisible = true;
                    _play.Update(mouseState);
                    break;
                case GameState.Options:
                    break;
                case GameState.Playing:
                    if (IsMouseVisible == true) IsMouseVisible = false;
                    if (!_balle.Launched)
                    {
                        _raquette.Scale = 1f;
                        _balle.Position = new Vector2(_raquette.Position.X + _raquette.Texture.Width / 2 - _balle.Texture.Width / 2, _raquette.Position.Y - _balle.Texture.Height);
                        if(_keyboardState.IsKeyDown(Keys.Space))
                        {
                            _balle.Launched = true;
                        }
                    }
                    else
                    {
                        this.CheckIfBallOut();
                        this.CheckIfCollisionRaquette();
                        this.CheckIfCollisionBrique();
                        if (_nbBriques == 0)
                            _menu.IsGamePaused = true;
                    }
                    if (_keyboardState.IsKeyDown(Keys.Escape))
                        //CurrentGameState = GameState.Paused;
                        Exit();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {            
            GraphicsDevice.Clear(Color.Black); // CornflowerBlue
            
            
            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(_menuBackground, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    _play.Draw(spriteBatch);
                    break;
                case GameState.Options:
                    break;
                case GameState.Playing:
                    spriteBatch.Draw(_background, _mainFrame, Color.CornflowerBlue);

                    // On dessine ensuite le score en calculant au préalable la position où le placer.
                    Vector2 scoreSize = _scoreFont.MeasureString(_score.ToString());
                    Vector2 scorePosition = new Vector2(10, _windowSize.Y - 10 - scoreSize.Y);
                    spriteBatch.DrawString(_scoreFont, "Score : " + _score.ToString(), scorePosition, Color.White);

                    // Affichage du mur de briques
                    for (int x = 0; x < NB_LIGNE_BRIQUE; x++)
                    {
                        for (int y = 0; y < NB_COLONNE_BRIQUE; y++)
                        {
                            Brique brique = _briques[x, y];
                            if (!brique.Touched)
                            {
                                spriteBatch.Draw(_briqueTextures[brique.Color], brique.Position, null, Color.White, 0f, Vector2.Zero, _briqueScale, SpriteEffects.None, 0f);
                            }
                        }
                    }

                    // Game Over
                    if (_nbBalles == 0)
                    {
                        String message = "Game Over!";
                        Vector2 gameOverSize = _globalFont.MeasureString("Game Over!");
                        Vector2 gameOverPosition = new Vector2(_windowSize.X / 2 - gameOverSize.X / 2, _windowSize.Y / 2 - gameOverSize.Y / 2);
                        spriteBatch.DrawString(_globalFont, "Game Over!", gameOverPosition, Color.Red);

                        CurrentGameState = GameState.MainMenu;
                    }
                    else if (_nbBriques == 0)
                    {
                        Vector2 bravoSize = _globalFont.MeasureString("Bravo!");
                        Vector2 bravoPosition = new Vector2(_windowSize.X / 2 - bravoSize.X / 2, _windowSize.Y / 2 - bravoSize.Y / 2);
                        spriteBatch.DrawString(_globalFont, "Bravo!", bravoPosition, Color.White);
                        CurrentGameState = GameState.MainMenu;
                    }
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CheckIfBallOut()
        {
            if (_balle.Position.Y >= _windowSize.Y)
            {
                _nbBalles--;
                _balle.Launched = false;
                if(!(_nbBalles == 0))
                {
                    _score -= 50;
                    _balle.Initialize();
                    _raquette.Initialize();
                }
                else
                {
                    _menu.IsGamePaused = true;
                    // Fin jeu : "Game over" + Retour menu après petite tempo
                }
            }
        }

        private void CheckIfCollisionRaquette()
        {
            // Collision Raquette :
            if(_balle.Direction.Y > 0 && _raquette.CollisionRectangle.Contains((int)_balle.Position.X, (int)_balle.Position.Y + _balle.Texture.Height))
            {
                _rebondRaquette.Play();

                _raquette.Scale -= 0.005f;
                float radianAngle = (float)Math.Atan2(-_balle.Direction.X, -_balle.Direction.Y) * (180 / (float)Math.PI);
                //_balle.Direction = new Vector2((float)Math.Cos(radianAngle), (float)Math.Sin(radianAngle));                

                var x = Math.Cos(radianAngle * Math.PI / 180);
                var y = - Math.Sin(radianAngle * Math.PI / 180);
                _balle.Direction = new Vector2((float)x, (float)y);
                /*
                if (_raquette.RelativePosition(_balle.Position) < 0)
                    _balle.Direction = new Vector2(-1, -1);
                else if (_raquette.RelativePosition(_balle.Position) > 0)
                    _balle.Direction = new Vector2(1, -1);
                else
                    _balle.Direction = new Vector2(_balle.Direction.X, -_balle.Direction.Y);
                */
                if (_balle.Speed < Balle.MAX_SPEED)
                    _balle.Speed += 0.03f;
            }
        }

        private void CheckIfCollisionBrique()
        {
            Rectangle balleRectangle = new Rectangle((int)_balle.Position.X, (int)_balle.Position.Y, _balle.Texture.Width, _balle.Texture.Height);
            for (int x = 0; x < NB_LIGNE_BRIQUE; x++)
            {
                for (int y = 0; y < NB_COLONNE_BRIQUE; y++)
                {
                    if (!_briques[x, y].Touched)
                    {
                        //if ((_briques[x, y].CollisionRectangle.Contains((int)_balle.Position.X + _balle.Texture.Width, (int)_balle.Position.Y + _balle.Texture.Height)) // Coin inférieur-droit
                        //    || (_briques[x, y].CollisionRectangle.Contains((int)_balle.Position.X + _balle.Texture.Width, (int)_balle.Position.Y)) // Coin supérieur-droit
                        //    || (_briques[x, y].CollisionRectangle.Contains((int)_balle.Position.X, (int)_balle.Position.Y + _balle.Texture.Height)) // Coin inférieur-gauche
                        //    || (_briques[x, y].CollisionRectangle.Contains((int)_balle.Position.X, (int)_balle.Position.Y))) // Coin supérieur-gauche
                        if (_briques[x, y].CollisionRectangle.Intersects(balleRectangle))
                        {
                            //_rebondBrique.Play();
                            _briques[x, y].Touched = true;
                            _nbBriques--;
                            _score += _briques[x, y].Score;
                            _balle.Direction = -_balle.Direction;
                        }
                    }
                }
            }
        }

        private void GenerateBrickWall()
        {
            int xpos, ypos;
            int color = 1, score = 40;

            Texture2D texture = _briqueTextures[(int)BriqueColor.Black];
            Vector2 size = new Vector2(_briqueScale * texture.Width, _briqueScale * texture.Height); // == Vector2(sizeX, scale * texture.Height)
            
            for (int x = 0; x < NB_LIGNE_BRIQUE; x++)
            {
                ypos = (int)(OFFSET + x * size.Y);
                for (int y = 0; y < NB_COLONNE_BRIQUE; y++)
                {
                    xpos = (int)(OFFSET + y * size.X);

                    _briques[x, y] = new Brique(new Vector2(xpos, ypos), size, color, score);
                }
                color += 1;
                score -= 10;
            }
        }

        //private void InitGame()
        //{
        //    _nbBalles = 3;
        //    _score = 0;
        //    _balle = new Balle(this, _windowSize);
        //    _raquette = new Raquette(this, _windowSize);
        //}
    }
}
