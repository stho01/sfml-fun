using SFML.Graphics;

namespace Chess;

public class SpriteAtlas(string texture)
{
    private Texture? _texture;
    private readonly Dictionary<string, Sprite> _sprites = new();

    public void Load()
    {
        _texture = new Texture(texture);
    }
    
    public Sprite? GetSprite(string name)
    {
        return _sprites.GetValueOrDefault(NormalizeName(name));
    }

    public void Define(string name, int left, int top, int width, int height)
    {
        _sprites[NormalizeName(name)] = new Sprite(_texture, new IntRect(left, top, width, height));        
    }

    private static string NormalizeName(string name) => name.Trim().ToLower();
}