
--AddSprite('fileName')
--DrawSprite(x, y, 'fileName')

local light_counter = 0
local light_opacity = 0

local spr_base = 0
local spr_light = 0


function addRollEffect(player)
end

function clearIn(player)
end

function clearOut(player)
end

function maxIn(player)
end

function maxOut(player)
end

function loadAssets()
    spr_base = func:AddSprite("Base.png")
    spr_light = func:AddSprite("Light.png")
end

function init()
end

function update()
    light_counter = light_counter + (5 * deltaTime)
    if light_counter > 1 then 
        light_counter = 0
    end

    if light_counter < 0.5 then 
        light_opacity = light_counter * 2
    else 
        light_opacity = 1.0 - ((light_counter - 0.5) * 2)
    end
    light_opacity = 0.8 + (light_opacity / 5.0)
end

function draw()
    func:DrawSprite(0, 540, spr_base)

    func:DrawSpriteBlendOpacity(0, 540, light_opacity, "Add", spr_light)
end