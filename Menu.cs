using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace trickortreat;

public class Menu : IScene
{
    private readonly ContentManager contentManager;
    private readonly GraphicsDevice graphicsDevice;
    private readonly SceneManager sceneManager;

    private SpriteFont _pixelfont;
    
    private int _Selected = 0;

    private double _KeysCountdown = 0;
    private double _BlinkCountdown = 0;

    private bool _blinking = true;

    public Menu(GraphicsDevice graphicsDevice, ContentManager content, SceneManager sceneManager)
    {
        this.graphicsDevice = graphicsDevice;
        this.contentManager = content;
        this.sceneManager = sceneManager;
    }

    public void LoadContent()
    {
        _pixelfont = contentManager.Load<SpriteFont>("pixelfont");
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds * 1000;


        if(_KeysCountdown >= 0)
        {
            _KeysCountdown -= elapsed;
        }

        if(_BlinkCountdown >= 0)
        {
            _BlinkCountdown -= elapsed;
        }

        if (_KeysCountdown <= 0 && (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.Up)))
        {
            _Selected = (_Selected == 0) ? 1 : 0;
            _KeysCountdown = 300;
        }

        if(_blinking == true && _BlinkCountdown <= 0)
        {
            _blinking = false;
            _BlinkCountdown = 300;
        }

        if(_blinking == false && _BlinkCountdown <= 0)
        {
            _blinking = true;
            _BlinkCountdown = 300;
        }

        if(_Selected == 0 && state.IsKeyDown(Keys.Enter))
        {
            sceneManager.AddScene(new Play(graphicsDevice, contentManager, sceneManager), "play");
            sceneManager.ChangeScene("play");
        }

        if(_Selected == 1 && state.IsKeyDown(Keys.Enter))
        {
            new Game1().Exit();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        graphicsDevice.Clear(Color.Gray);
        
        int Height = graphicsDevice.Viewport.Height;
        int Width = graphicsDevice.Viewport.Width;

        Vector2 LabelM = _pixelfont.MeasureString("Pick Up");
        Vector2 Label = new Vector2((Width / 2) - (LabelM.X / 2), (Height / 4) - (LabelM.Y / 2));
    
        string playText = _Selected == 0 && _blinking ? "> Play <" : "  Play  ";
        string quitText = _Selected == 1 && _blinking ? "> Quit <" : "  Quit  ";

        Vector2 PlayM = _pixelfont.MeasureString(playText);
        Vector2 Play = new Vector2((Width / 2) - (PlayM.X / 2), (Height / 4) - (PlayM.Y / 2) + (100));

        Vector2 QuitM = _pixelfont.MeasureString(quitText);
        Vector2 Quit = new Vector2((Width / 2) - (QuitM.X / 2), (Height / 4) - (QuitM.Y / 2) + (150));

        spriteBatch.DrawString(_pixelfont, "Pick Up", Label, Color.Orange);

        spriteBatch.DrawString(_pixelfont, playText, Play, Color.Orange);
        spriteBatch.DrawString(_pixelfont, quitText, Quit, Color.Orange);

    }
}
