using SFML.Graphics;

namespace Hexmap;

public interface IState
{
    void Load(Game game);
    void Suspend(Game game);
    void Update(Game game);
    void Draw(Game game, RenderTarget target);
}