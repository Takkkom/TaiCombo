
local spr_texture = 0

function fadeIn()

end

function fadeOut()
    
end


function loadAssets()
    spr_texture = func:AddSprite('Texture.png')
end

function init()
end

function update()
    if state == 'In' then

    elseif state == 'Idle' then

    elseif state == 'Out' then

    end
end

function draw()
    if state == 'In' then
        func:DrawSpriteOpacity(0, 0, counter, spr_texture)
    elseif state == 'Idle' then
        func:DrawSprite(0, 0, spr_texture)
    elseif state == 'Out' then
        func:DrawSpriteOpacity(0, 0, 1 - counter, spr_texture)
    end
end