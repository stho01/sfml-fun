local SPEED = 100

function OnLoad()
    local tag = gameObject ~= nil and gameObject.Tag or ""
    print("loading movement.lua for gameObject " .. tag)
end

function OnUpdate(dt)
    
    local x = gameObject.Position.X
    local y = gameObject.Position.Y
    
    if (Keyboard.IsKeyPressed(Keyboard.Key.A)) then x = x - SPEED * dt end
    if (Keyboard.IsKeyPressed(Keyboard.Key.D)) then x = x + SPEED * dt end
    if (Keyboard.IsKeyPressed(Keyboard.Key.W)) then y = y - SPEED * dt end
    if (Keyboard.IsKeyPressed(Keyboard.Key.S)) then y = y + SPEED * dt end
    
    gameObject.Position = Vector2f(x, y)
end