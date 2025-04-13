using SFML.Graphics;

namespace Hexmap;

public interface IState
{
    public string Name { get; }
    
    void Load(Game game);
    void Pause(Game game);
    void Resume(Game game);
    void Update(Game game);
    void Draw(Game game, RenderTarget target);
}