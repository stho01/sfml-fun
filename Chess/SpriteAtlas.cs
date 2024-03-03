using System.Text.Json;
using SFML.Graphics;

namespace Chess;

public class SpriteAtlas
{
    private Texture? _texture;
    private readonly Dictionary<string, Sprite> _sprites = new();
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    
    public void LoadTexture(string texture)
    {
        _texture = new Texture(texture);
    }
    
    public void Load(string atlasDefinition)
    {
        var json = File.ReadAllText($"Assets/{atlasDefinition}.atlas.json");
        var def = JsonSerializer.Deserialize<SpriteAtlasDefinition>(json, _options);
        if (def is null)
            throw new Exception("Cant read atlas definition");
        
        _texture = new Texture($"Assets/{def.Texture}");
        foreach (var spriteDef in def.Sprites)
        {
            _sprites[spriteDef.Key] = new Sprite(_texture, spriteDef.Value.ToIntRect());
        }
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

public class SpriteAtlasDefinition
{
    public string Texture { get; init; } = "";
    public Dictionary<string, SpriteAtlasRect> Sprites { get; init; } = new();
}

public class SpriteAtlasRect
{
    public int Left { get; init; }
    public int Top { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public IntRect ToIntRect() => new(Left, Top, Width, Height);
}