
local spr_background = 0
local spr_background_left = 0
local spr_background_right = 0
local spr_gradation = 0
local spr_titleBase = 0
local spr_chara_left = 0
local spr_chara_right = 0
local spr_skippable_plate = 0
local spr_skippable_text = 0
local spr_title = 0
local spr_subtitle = 0
local spr_hints = { 0 }

local hint_index = 1
local chara_left_x = -81
local chara_right_x = 1521
local chara_left_y = 546
local chara_right_y = 546
local chara_moveSize_x = 370
local chara_moveSize_y = -150
local skippable_plate_x = 42
local skippable_plate_y = 50
local skippable_text_x = 350
local skippable_text_y = 106
local title_base_x = 960
local title_base_y = 400
local title_base_width = 0
local title_base_height = 0
local title_x = 960
local title_y = 395
local title_scale = 1
local subtitle_x = 960
local subtitle_y = 472
local subtitle_scale = 1

function fadeIn()

end

function fadeOut()
    
end

function loadAssets()
    spr_background = func:AddSprite('Background.png')
    spr_background_left = func:AddSprite('Background_Left.png')
    spr_background_right = func:AddSprite('Background_Right.png')
    spr_gradation = func:AddSprite('Gradation.png')
    spr_titleBase = func:AddSprite('Title_Base.png')
    spr_chara_left = func:AddSprite('Chara_Left.png')
    spr_chara_right = func:AddSprite('Chara_Right.png')
    spr_skippable_plate = func:AddSprite('Skippable_Plate.png')
    spr_skippable_text = func:AddSubFontSprite("演奏スキップON", 0, 0, 0, 30)
    for i = 1, 2 do 
        spr_hints[i] = func:AddSprite('Hints/'..tostring(i - 1)..'.png')
    end
    title_base_width = func:GetSpriteWidth(spr_titleBase)
    title_base_height = func:GetSpriteHeight(spr_titleBase)
end

function init()
    func:DisposeSprite(spr_title)
    spr_title = func:AddMainFontEdgeSprite(title, 1, 1, 1, 60, 0, 0, 0, 0.25)

    func:DisposeSprite(spr_subtitle)
    spr_subtitle = func:AddMainFontSprite(subtitle, 0, 0, 0, 35)

    hint_index = math.random(1, #spr_hints)
end

function update()
    if state == 'In' then

    elseif state == 'Idle' then

    elseif state == 'Out' then

    end
end

function draw()
    if state == 'In' then

        if counter < 0.5 then
            in_value = math.min(counter * 2, 1)
            scale = 0.5 + (in_value * 0.5)
            move_x = -480 + (in_value * 480)
            func:DrawSpriteOriginScale(0 + move_x, 0, scale, 1, "Left_Up", spr_background_left)
            func:DrawSpriteOriginScale(1920 - move_x, 0, scale, 1, "Right_Up", spr_background_right)
        else 
            in_value = (counter - 0.5) * 2
            func:DrawSprite(0, 0, spr_background)
            func:DrawSpriteBlendOpacity(0, 0, in_value, "Add", spr_gradation)
            func:DrawSpriteFull(title_base_x, title_base_y, in_value, 1, 0, 0, 1920, 250, false, false, 0, 1, 1, 1, in_value, "Center", "Screen", spr_titleBase)

            chara_move_x = ((-1.0 - math.cos(in_value * math.pi)) / 2.0) * chara_moveSize_x
            chara_move_y = math.sin(in_value * math.pi) * chara_moveSize_y
            func:DrawSpriteOpacity(chara_left_x + chara_move_x, chara_left_y + chara_move_y, in_value, spr_chara_left)
            func:DrawSpriteOpacity(chara_right_x - chara_move_x, chara_right_y + chara_move_y, in_value, spr_chara_right)
        end
    elseif state == 'Idle' then
        func:DrawSprite(0, 0, spr_background)
        func:DrawSpriteBlendOpacity(0, 0, 1, "Add", spr_gradation)
        func:DrawSpriteFull(title_base_x, title_base_y, 1, 1, 0, 0, 1920, 250, false, false, 0, 1, 1, 1, 1, "Center", "Screen", spr_titleBase)

        func:DrawSprite(chara_left_x, chara_left_y, spr_chara_left)
        func:DrawSprite(chara_right_x, chara_right_y, spr_chara_right)

        func:DrawSprite(skippable_plate_x, skippable_plate_y, spr_skippable_plate)
        func:DrawSpriteOriginScaleAlpha(skippable_text_x, skippable_text_y, 1, 1, 1, "Center", spr_skippable_text)

        func:DrawSpriteOriginScale(title_x, title_y, title_scale, 1, "Center", spr_title)
        func:DrawSpriteOriginScale(subtitle_x, subtitle_y, subtitle_scale, 1, "Center", spr_subtitle)

        func:DrawSprite(0, 0, spr_hints[hint_index])

    elseif state == 'Out' then

        if counter < 0.5 then
            out_value = math.min(counter * 2, 1)
            skippableplate_opacity = 1 - out_value
            titlebase_opacity = 1 - math.min(out_value * 2, 1)
            func:DrawSprite(0, 0, spr_background)
            func:DrawSpriteFull(title_base_x, title_base_y, 1, titlebase_opacity, 0, 0, title_base_width, title_base_height, false, false, 0, 1, 1, 1, titlebase_opacity, "Center", "Screen", spr_titleBase)

            func:DrawSpriteOpacity(skippable_plate_x, skippable_plate_y, skippableplate_opacity, spr_skippable_plate)
            func:DrawSpriteOriginScaleAlpha(skippable_text_x, skippable_text_y, 1, 1, skippableplate_opacity, "Center", spr_skippable_text)
            
            chara_value = out_value * 3
            chara_move_y = 0
            chara_opacity = 1
            if chara_value < 1 then
                chara_move_y = math.sin(chara_value * math.pi) * 30
            else
                chara_out_value = (chara_value - 1.0) / 2
                chara_move_y = chara_out_value * -500
                chara_opacity = 1 - chara_out_value
            end
            func:DrawSpriteOpacity(chara_left_x, chara_left_y + chara_move_y, chara_opacity, spr_chara_left)
            func:DrawSpriteOpacity(chara_right_x, chara_right_y + chara_move_y, chara_opacity, spr_chara_right)
        else
            out_value = (counter - 0.5) * 2
            out_bg = 1 - out_value
            scale = 0.5 + (out_bg * 0.5)
            move_x = -480 + (out_bg * 480)
            func:DrawSpriteOriginScale(0 + move_x, 0, scale, 1, "Left_Up", spr_background_left)
            func:DrawSpriteOriginScale(1920 - move_x, 0, scale, 1, "Right_Up", spr_background_right)
        end

    end
end