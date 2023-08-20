--@meta 

--@class 
function Keyboard() end

--@param key Keyboard.Key
function Keyboard.IsKeyPressed(key) end

--@class
--@field X number
--@field Y number
function Vector2f() end

--@class
--@field Tag string
--@field Position Vector2f
function GameObject() end

--@type GameObject
gameObject = nil