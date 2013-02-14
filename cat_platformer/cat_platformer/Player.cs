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

namespace cat_platformer
{
    class Player
    {
        public Texture2D _playerTexture;
        public Vector2 _spritePosition;
        public Vector2 _spriteSpeed;
        bool _applyGravity;
        float _timer;
        bool _jumping;

        
        

        KeyboardState _oldState, _newState; 

        public Player(ContentManager Content)
        {
            _playerTexture = Content.Load<Texture2D>("player"); 
            _spritePosition = Vector2.Zero;
            _spriteSpeed = new Vector2(100.0f, 300.0f);
            _timer = 0.0f;
            _applyGravity = false;
            _jumping = false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Draw(_playerTexture, _spritePosition + offset, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public void Move(GameTime gameTime)
        {
            if (_oldState == null)
                _oldState = Keyboard.GetState();

            _newState = Keyboard.GetState();

            if(_newState.IsKeyDown(Keys.A))
            {
                //if(!_oldState.IsKeyDown(Keys.W))
                    _spritePosition.X  -= _spriteSpeed.X * (float)gameTime.ElapsedGameTime.TotalSeconds; 
            }
            if (_newState.IsKeyDown(Keys.D))
            {
                //if(!_oldState.IsKeyDown(Keys.W))
                _spritePosition.X += _spriteSpeed.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (_newState.IsKeyDown(Keys.Space))
            {
                if (!_oldState.IsKeyDown(Keys.Space))
                {
                    _jumping = true;                    
                }
            }

            if (_jumping)
            {
                _spriteSpeed.Y += 5;
                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;                
                _spritePosition.Y -= _spriteSpeed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_timer > 1)
                {
                    _timer = 0;
                    _jumping = false;                    
                }
            }

            if (!_jumping && _applyGravity)
            {
                _spritePosition.Y += _spriteSpeed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            _oldState = _newState; 
           
        }

        public void ApplyGravity(int floorPosition)
        {
            if (_spritePosition.Y + _playerTexture.Height < floorPosition)
                _applyGravity = true;
            else if (_spritePosition.Y + _playerTexture.Height > floorPosition)
            {
                //_spritePosition.Y = floorPosition - _playerTexture.Height;
                _applyGravity = false;
            }
            else if (_spritePosition.Y + _playerTexture.Height == floorPosition)
                _applyGravity = false; 
        }
         
    }
}
