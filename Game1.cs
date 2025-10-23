using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace trickortreat;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _pixelfont;
    
    private int _Selected = 0;

    private double _KeysCountdown = 0;

    private bool _blinking = true;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _pixelfont = Content.Load<SpriteFont>("pixelfont");
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds * 1000;

        if(_KeysCountdown >= 0)
        {
            _KeysCountdown -= elapsed;
        }

        if (_KeysCountdown <= 0 && (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.Up)))
        {
            _Selected = (_Selected == 0) ? 1 : 0;
            _KeysCountdown = 300;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray);
        
        int Height = GraphicsDevice.Viewport.Height;
        int Width = GraphicsDevice.Viewport.Width;

        Vector2 LabelM = _pixelfont.MeasureString("Trick Or Treat");
        Vector2 Label = new Vector2((Width / 2) - (LabelM.X / 2), (Height / 4) - (LabelM.Y / 2));
    
        string playText = _Selected == 0 && _blinking ? "> Play <" : "  Play  ";
        string quitText = _Selected == 1 && _blinking ? "> Quit <" : "  Quit  ";

        Vector2 PlayM = _pixelfont.MeasureString(playText);
        Vector2 Play = new Vector2((Width / 2) - (PlayM.X / 2), (Height / 4) - (PlayM.Y / 2) + (100));

        Vector2 QuitM = _pixelfont.MeasureString(quitText);
        Vector2 Quit = new Vector2((Width / 2) - (QuitM.X / 2), (Height / 4) - (QuitM.Y / 2) + (150));

        _spriteBatch.Begin();

        _spriteBatch.DrawString(_pixelfont, "Trick Or Treat", Label, Color.Orange);

        if(_Selected == 0)
        {
        _spriteBatch.DrawString(_pixelfont, playText, Play, Color.Orange);
        _spriteBatch.DrawString(_pixelfont, quitText, Quit, Color.Orange);
        } else {
        _spriteBatch.DrawString(_pixelfont, playText, Play, Color.Orange);
        _spriteBatch.DrawString(_pixelfont, quitText, Quit, Color.Orange);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
