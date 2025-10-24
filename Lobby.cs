using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace trickortreat;

public class Lobby : IScene
{
    private readonly ContentManager contentManager;
    private readonly GraphicsDevice graphicsDevice;
    private readonly SceneManager sceneManager;

    private SpriteFont _pixelfont;

    private int cica;

    public Lobby(GraphicsDevice graphicsDevice, ContentManager contentManager, SceneManager sceneManager)
    {
        this.graphicsDevice = graphicsDevice;
        this.contentManager = contentManager;
        this.sceneManager = sceneManager;
    }

    public void LoadContent()
    {
        _pixelfont = contentManager.Load<SpriteFont>("pixelfont");
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();

        if(state.IsKeyDown(Keys.K))
        {
            sceneManager.ChangeScene("menu");
        }

        if(state.IsKeyDown(Keys.Up))
        {
            cica++;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(_pixelfont, cica.ToString(), new Vector2(100, 100), Color.White);
    }
}