using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CasseBrique
{
    class Menu : DrawableGameComponent
    {
        private Vector2 _windowSize;
        private SpriteBatch _spriteBatch;
        private Texture2D _background;

        private Texture2D _options;
        private Texture2D _play;
        private Texture2D _exit;

        private Rectangle _rectanglePlay,
                          _rectangleOptions,
                          _rectangleExit;

        public bool IsGamePaused
        {
            set { _isGamePaused = value; }
            get { return _isGamePaused; }
        }
        private bool _isGamePaused;

        public bool IsGameStarted
        {
            set { _isGameStarted = value; }
            get { return _isGameStarted; }
        }
        private bool _isGameStarted;

        public Menu(Game game, Vector2 windowSize) : base(game)
        {
            _windowSize = windowSize;
            this.Game.Components.Add(this);
        }

        public override void Initialize()
        {
            _isGameStarted = true;
            _isGamePaused = false;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _background = Game.Content.Load<Texture2D>(@"images\fondmenu");
            _options = Game.Content.Load<Texture2D>(@"images\options");
            _exit = Game.Content.Load<Texture2D>(@"images\exit");
            _play = Game.Content.Load<Texture2D>(@"images\play");

            Vector2 positionPlay = new Vector2(_windowSize.X / 2 - _play.Width/2, _windowSize.Y/3 - _play.Height);
            Vector2 positionOptions = new Vector2(_windowSize.X / 2 - _options.Width / 2, _windowSize.Y / 2 - _options.Height);
            Vector2 positionExit = new Vector2(_windowSize.X / 2 - _exit.Width / 2, 2 * _windowSize.Y / 3 - _exit.Height);

            // Rectangle afin de gérer les interactions avec la souris :
            _rectanglePlay = new Rectangle((int)positionPlay.X, (int)positionPlay.Y, _play.Width, _play.Height);
            _rectangleOptions = new Rectangle((int)positionOptions.X, (int)positionOptions.Y, _options.Width, _options.Height);
            _rectangleExit = new Rectangle((int)positionExit.X, (int)positionExit.Y, _exit.Width, _exit.Height);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if(_isGameStarted && !_isGamePaused)
            {
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //_spriteBatch.Begin();

            //if(!_isGameStarted)
            //{
            //    // Drawing backgroung
            //    _spriteBatch.Draw(_background, new Rectangle(0, 0, (int)_windowSize.X, (int)_windowSize.Y), Color.White);
            //    _spriteBatch.Draw(_play, _rectanglePlay, Color.White);
            //    _spriteBatch.Draw(_options, _rectangleOptions, Color.White);
            //    _spriteBatch.Draw(_exit, _rectangleExit, Color.White);
            //}
            

            //_spriteBatch.End();
            base.Draw(gameTime);
        }

        public void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            // Si on clique sur Play : il se passe ceci :

            //_speed = SPEED;
            //if (keyboardState.IsKeyDown(Keys.Right)/* || keyboardState.IsKeyDown(Keys.D)*/)
            //{
            //    _direction = Vector2.UnitX;
            //}
            //else if (keyboardState.IsKeyDown(Keys.Left)/* || keyboardState.IsKeyDown(Keys.Q)*/)
            //{
            //    _direction = -Vector2.UnitX;
            //}
            //else _speed = 0;
        }
    }
}
