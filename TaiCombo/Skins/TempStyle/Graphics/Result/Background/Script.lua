
local spr_base = { 0, 0 }
local spr_base_clear = 0
local spr_title_base = 0
local spr_title = 0

local title_x = 960
local title_y = 100
local title_scale = 1
local cleared = { false, false }

function playClearAnime(player)
    cleared[player] = true
end

function loadAssets()
    spr_base[1] = func:AddSprite('Base_1P.png')
    spr_base[2] = func:AddSprite('Base_2P.png')
    spr_base_clear = func:AddSprite('Base_Clear.png')
    spr_title_base = func:AddSprite('Title_Base.png')
end

function init()
    func:DisposeSprite(spr_title)
    spr_title = func:AddMainFontEdgeSprite(title, 1, 1, 1, 60, 0, 0, 0, 0.25)
    
    for player = 1, playerCount do
        cleared[player] = false
    end
end

function update()
end

function draw()
    if playerCount == 1 then
        if p1IsBlue then
            func:DrawSprite(0, 0, spr_base[2])
        else
            func:DrawSprite(0, 0, spr_base[1])
        end
        func:DrawSpriteOpacity(0, 0, 1, spr_title_base)
    else
        func:DrawSpriteFull(0, 0, 1, 1, 0, 0, 960, 1080, false, false, 0, 1, 1, 1, 1, "Left_Up", "Normal", spr_base[1])
        func:DrawSpriteFull(960, 0, 1, 1, 0, 0, 960, 1080, false, false, 0, 1, 1, 1, 1, "Left_Up", "Normal", spr_base[2])
        --func:DrawSpriteFull(0, 0, 1, 1, 0, 0, 960, 1080, false, false, 0, 1, 1, 1, 1, "Left_Up", "Normal", spr_base_clear)
        --func:DrawSpriteFull(960, 0, 1, 1, 0, 0, 960, 1080, false, false, 0, 1, 1, 1, 1, "Left_Up", "Normal", spr_base_clear)
    end
    
    func:DrawSprite(0, 0, spr_title_base)
    func:DrawSpriteOriginScale(title_x, title_y, title_scale, 1, "Center", spr_title)






end