
--AddSprite('fileName')
--DrawSprite(x, y, 'fileName')

local clear_back_opacity = 0
local clear_back_x = 0
local clear_header_y = 0
local clear_footer_y = 0
local clear_scroll_up_x = 0
local clear_scroll_down_x = 0
local clear_scroll_up_y = 0
local clear_scroll_down_y = 0
local clear_cloud_scroll_left_x = 0
local clear_cloud_scroll_right_x = 0
local clear_koma_red_y = 0
local clear_koma_left_y = 0
local clear_koma_right_y = 0
local clear_koma_counter = 0

local clearAnimeState = 0
local clearInCounter = 0


local spr_Clear_Back = 0

local spr_Clear_Cloud_Header = 0
local spr_Clear_Cloud_Footer = 0

local spr_Clear_Scroll_Up = 0
local spr_Clear_Scroll_Down = 0

local spr_Clear_Cloud_Scroll_Left = 0
local spr_Clear_Cloud_Scroll_Right = 0

local spr_Clear_Koma_Red = 0
local spr_Clear_Koma_Left = 0
local spr_Clear_Koma_Right = 0


function addRollEffect(player)
end

function clearIn(player)
    clearAnimeState = 1
    clearInCounter = 0
    clear_koma_counter = 0
end

function clearOut(player)
    clearAnimeState = 2
    clearInCounter = 0.45
end

function maxIn(player)
end

function maxOut(player)
end

function inAnime(value, in1)
    in_base = math.min(value * 5, 1)
    return -in1 + (in_base * in1)
end

function inAnimeBound(value, in1, in2)
    result = inAnime(value, in1)

    if result == 0 then
        in_base2 = math.min((value - 0.2) * 8, 1)
        if in_base2 < 0.75 then
            in_base2 = in_base2 * 1.5
        else
            in_base2 = 1 - ((in_base2 - 0.75) * 4)
        end
        in_base2 = in_base2 * in2

        result = result - in_base2
    end

    return result
end

function loadAssets()
    spr_Clear_Back = func:AddSprite("Clear_Back.png")
    
    spr_Clear_Cloud_Header = func:AddSprite("Clear_Cloud_Header.png")
    spr_Clear_Cloud_Footer = func:AddSprite("Clear_Cloud_Footer.png")
    
    spr_Clear_Scroll_Up = func:AddSprite("Clear_Scroll_Up.png")
    spr_Clear_Scroll_Down = func:AddSprite("Clear_Scroll_Down.png")
    
    spr_Clear_Cloud_Scroll_Left = func:AddSprite("Clear_Cloud_Scroll_Left.png")
    spr_Clear_Cloud_Scroll_Right = func:AddSprite("Clear_Cloud_Scroll_Right.png")
    
    spr_Clear_Koma_Red = func:AddSprite("Clear_Koma_Red.png")
    spr_Clear_Koma_Left = func:AddSprite("Clear_Koma_Left.png")
    spr_Clear_Koma_Right = func:AddSprite("Clear_Koma_Right.png")
end

function init()
end

function update()
    
    if clearAnimeState == 0 then
        
    elseif clearAnimeState == 1 then 

        --clearInCounter = clearInCounter + (0.25 * deltaTime)
        clearInCounter = clearInCounter + (1 * deltaTime)
        
        in_base = math.min(clearInCounter * 5, 1)

        clear_back_opacity = math.min(clearInCounter * 1800, 255)
        clear_back_x = 0
        clear_header_y = inAnime(clearInCounter, 120)
        clear_footer_y = inAnime(clearInCounter, -120)
        clear_scroll_up_x = 0
        clear_scroll_down_x = 0
        clear_scroll_up_y = inAnimeBound(clearInCounter, 240, 25)
        clear_scroll_down_y = inAnimeBound(clearInCounter, -240, -25)

        clear_cloud_scroll_left_x = -960 + (math.min(clearInCounter * 3, 1) * 960)
        clear_cloud_scroll_right_x = 960 - (math.min(clearInCounter * 3, 1) * 960)

        clear_koma_red_y = inAnimeBound((clearInCounter * 1.5) - 0.23, -380, -25)
        clear_koma_right_y = inAnimeBound((clearInCounter * 1.5) - 0.33, 380, 25)
        clear_koma_left_y = inAnimeBound((clearInCounter * 1.5) - 0.33, -380, -25)

        if clearInCounter > 0.45 then 
            clearAnimeState = 3
        end

    elseif clearAnimeState == 2 then

        clearInCounter = clearInCounter - (1 * deltaTime)
        
        in_base = math.min(clearInCounter * 5, 1)

        clear_back_opacity = math.min(clearInCounter * 1800, 255)
        --clear_back_x = 0
        clear_header_y = inAnime(clearInCounter, 120)
        clear_footer_y = inAnime(clearInCounter, -120)
        --clear_scroll_up_x = 0
        --clear_scroll_down_x = 0
        clear_scroll_up_y = inAnimeBound(clearInCounter, 240, 25)
        clear_scroll_down_y = inAnimeBound(clearInCounter, -240, -25)

        clear_cloud_scroll_left_x = -960 + (math.min(clearInCounter * 3, 1) * 960)
        clear_cloud_scroll_right_x = 960 - (math.min(clearInCounter * 3, 1) * 960)

        clear_koma_red_y = inAnimeBound((clearInCounter * 1.5) - 0.23, -380, -25)
        clear_koma_right_y = inAnimeBound((clearInCounter * 1.5) - 0.33, 380, 25)
        clear_koma_left_y = inAnimeBound((clearInCounter * 1.5) - 0.33, -380, -25)

        if clearInCounter < 0 then 
            clearAnimeState = 0
        end

    elseif clearAnimeState == 3 then
        
        clear_back_opacity = 255

        clear_back_x = clear_back_x - (52 * deltaTime)
        if clear_back_x < -1920 then
            clear_back_x = 0 
        end
        
        clear_scroll_up_x = clear_scroll_up_x - (90 * deltaTime)
        if clear_scroll_up_x < -1920 then
            clear_scroll_up_x = 0 
        end
        clear_scroll_down_x = clear_scroll_down_x - (90 * deltaTime)
        if clear_scroll_down_x < -1920 then
            clear_scroll_down_x = 0 
        end

        clear_cloud_scroll_left_x = clear_cloud_scroll_left_x - (55 * deltaTime)
        if clear_cloud_scroll_left_x < -1920 then
            clear_cloud_scroll_left_x = 0 
        end
        clear_cloud_scroll_right_x = clear_cloud_scroll_right_x - (55 * deltaTime)
        if clear_cloud_scroll_right_x < -1920 then
            clear_cloud_scroll_right_x = 0 
        end

        clear_koma_counter = clear_koma_counter + (deltaTime * 0.9)
        if clear_koma_counter > 1 then
            clear_koma_counter = 0
        end

        komaValue = 0
        if clear_koma_counter < 0.75 then
            komaValue = (clear_koma_counter / 0.75)
        else 
            komaValue = (1 - ((clear_koma_counter - 0.75) * 4))
        end
        clear_koma_red_y = komaValue * -45
        clear_koma_left_y = komaValue * -45
        clear_koma_right_y = komaValue * -45

    end

    clear_back_opacity = clear_back_opacity / 255
end

function draw()
    loopCount = 1
    if clearAnimeState == 1 or clearAnimeState == 2 then
        loopCount = 0
    end

    if clearAnimeState == 0 then

    else
        for i = 0 , loopCount do
            func:DrawSpriteOpacity(clear_back_x + (1920 * i), 540, clear_back_opacity, spr_Clear_Back)
        end

        func:DrawSpriteOpacity(0, 540 + clear_header_y, clear_back_opacity, spr_Clear_Cloud_Header)
        func:DrawSpriteOpacity(0, 540 + clear_footer_y, clear_back_opacity, spr_Clear_Cloud_Footer)

        for i = 0 , loopCount do
            func:DrawSpriteOpacity(clear_scroll_up_x + (1920 * i), 540 + clear_scroll_up_y, clear_back_opacity, spr_Clear_Scroll_Up)
            func:DrawSpriteOpacity(clear_scroll_down_x + (1920 * i), 540 + clear_scroll_down_y, clear_back_opacity, spr_Clear_Scroll_Down)

            func:DrawSpriteOpacity(clear_cloud_scroll_left_x + (1920 * i), 540, clear_back_opacity, spr_Clear_Cloud_Scroll_Left)
            func:DrawSpriteOpacity(clear_cloud_scroll_right_x + (1920 * i), 540, clear_back_opacity, spr_Clear_Cloud_Scroll_Right)
        end

        func:DrawSpriteOpacity(0, 540 + clear_koma_red_y, clear_back_opacity, spr_Clear_Koma_Red)
        func:DrawSpriteOpacity(0, 540 + clear_koma_left_y, clear_back_opacity, spr_Clear_Koma_Left)
        if clear_koma_red_y < 180 then
            func:DrawSpriteOpacity(0, 540 + clear_koma_right_y, clear_back_opacity, spr_Clear_Koma_Right)
        end
    end
end