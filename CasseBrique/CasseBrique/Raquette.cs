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
    class Raquette : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private const float SPEED = 0.5f;
        private const float MAX_SCALE = 1.5f;
        private const float MIN_SCALE = 0.5f;

        private SpriteBatch _spriteBatch;

        private Vector2 _windowSize;

        public enum RelativePositions : int
        {
            LEFT = -1,
            MIDDLE = 0,
            RIGHT = 1
        }
        
        public Texture2D Texture
        {
            get { return _texture; }
        }
        private Texture2D _texture;
        
        public Vector2 Position
        {
            get { return _position; }
        }
        private Vector2 _position;

        private Vector2 _direction;

        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }
        private float _scale;
        
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        private float _speed;

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, (int)(_texture.Width*_scale), _texture.Height);
            }
        }

        public int RelativePosition(Vector2 position)
        {
            int width = (int)(_texture.Width * _scale / 3);

            if (position.X > _position.X && position.X < _position.X + width)
                return (int)RelativePositions.LEFT; // -1
            
            if (position.X > _position.X + 2*width && position.X < _position.X + _texture.Width *_scale)
                return (int)RelativePositions.RIGHT; // 1

            //if (position.X > _position.X + width && position.X < _position.X + 2 * width)
            else
                return (int)RelativePositions.MIDDLE; // 0
        }

        public Raquette(Game game, Vector2 windowSize/*, SpriteBatch spriteBatch*/)
            : base(game)
        {
            _windowSize = windowSize;
            //_spriteBacth = spriteBatch;
            this.Game.Components.Add(this);
        }

        public override void Initialize()
        {
            _scale = 1f;
            base.Initialize();             
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _texture = Game.Content.Load<Texture2D>(@"images\raquette");

            _position = new Vector2(_windowSize.X / 2 - (_texture.Width * _scale / 2), _windowSize.Y - 10 - _texture.Height);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (((CasseBrique)Game).CurrentGameState == CasseBrique.GameState.Playing)
            {
                this.HandleInput();

                if (_position.X <= 0 && _direction.X < 0)
                    _speed = 0;
                if (_position.X >= _windowSize.X - _texture.Width && _direction.X > 0)
                    _speed = 0;

                if (_scale > MIN_SCALE && _scale < MAX_SCALE)
                    _scale -= 0.00001f;

                _position += _direction * _speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                base.Update(gameTime);
            }
        }

        public void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            _speed = SPEED;
            if (keyboardState.IsKeyDown(Keys.Right)/* || keyboardState.IsKeyDown(Keys.D)*/)
            {
                _direction = Vector2.UnitX;
            }
            else if (keyboardState.IsKeyDown(Keys.Left)/* || keyboardState.IsKeyDown(Keys.Q)*/)
            {
                _direction = -Vector2.UnitX;
            }
            else _speed = 0;
        }

        public override void Draw(GameTime gameTime)
        {
            if (((CasseBrique)Game).CurrentGameState == CasseBrique.GameState.Playing)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_texture, _position, null, Color.White * 1.0f, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
                _spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }
}
