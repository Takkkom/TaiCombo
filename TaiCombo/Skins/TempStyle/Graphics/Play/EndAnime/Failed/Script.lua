
local x = { 498, 498 }
local y = { 288, 552 }
local speed = 1

local playing = { false, false }

local counter = { 0, 0 }
local textAnimeState = { "None", "None" }
local charaAnimeState = { "None", "None" }


local spr_chara_left_clear_in = { 0 }
local spr_chara_right_clear_in = { 0 }
local spr_chara_left_failed_in = { 0 }
local spr_chara_right_failed_in = { 0 }
local spr_text_failed = { 0 }
local spr_failed_effect = 0

local left_begin_offset_x = 522
local right_begin_offset_x = 894
local left_begin_offset_y = -46
local right_begin_offset_y = -46

local left_failed_begin_offset_x = 322
local right_failed_begin_offset_x = 1094
local left_failed_begin_offset_y = -60
local right_failed_begin_offset_y = -60

local left_failed_effect_offset_x = 523
local right_failed_effect_offset_x = 900
local left_failed_effect_offset_y = 216
local right_failed_effect_offset_y = 216
local chara_clear_move_x = 348
local chara_failed_move_x = 113
local chara_failed_move_y = 37

local text_failed_full_offset_x = 446
local text_failed_full_offset_y = 32
local text_failed_counter = { 0, 0 }


function playAnime(player)
    playing[player] = true
    counter[player] = 0
    charaAnimeState[player] = "In"
    textAnimeState[player] = "In"
    text_failed_counter[player] = -1.25
end

function loadAssets()
    for i = 1, 3 do
        spr_chara_left_clear_in[i] = func:AddSprite('../Chara_Left_Clear_'..tostring(i - 1)..'.png')
        spr_chara_right_clear_in[i] = func:AddSprite('../Chara_Right_Clear_'..tostring(i - 1)..'.png')
    end
    for i = 1, 2 do
        spr_chara_left_failed_in[i] = func:AddSprite('../Chara_Left_Failed_'..tostring(i - 1)..'.png')
        spr_chara_right_failed_in[i] = func:AddSprite('../Chara_Right_Failed_'..tostring(i - 1)..'.png')
    end
    for i = 1, 5 do
        spr_text_failed[i] = func:AddSprite('../Text_Failed_'..tostring(i - 1)..'.png')
    end
    spr_failed_effect = func:AddSprite('../Failed_Effect.png')
end

function init()
end

function update()
    for player = 1, playerCount do
        
        if playing[player] then

            if textAnimeState[player] == "In" then
                text_failed_counter[player] = text_failed_counter[player] + (4 * deltaTime) * speed
                if text_failed_counter[player] > 1 then
                    text_failed_counter[player] = 0
                    textAnimeState[player] = "Wait"
                end
            elseif textAnimeState[player] == "Wait" then
                text_failed_counter[player] = text_failed_counter[player] + (0.6 * deltaTime) * speed
                if text_failed_counter[player] > 1 then
                    text_failed_counter[player] = 0
                    textAnimeState[player] = "Out"
                end
            elseif textAnimeState[player] == "Out" then
                text_failed_counter[player] = text_failed_counter[player] + (10 * deltaTime) * speed
                if text_failed_counter[player] > 1 then
                    text_failed_counter[player] = 1
                end
            end

            if charaAnimeState[player] == "In" then

                counter[player] = counter[player] + (6.0 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = 0
                    charaAnimeState[player] = "Clear_Move"
                end

            elseif charaAnimeState[player] == "Clear_Move" then

                counter[player] = counter[player] + (2.2 * deltaTime) * speed

                if counter[player] > 0.2 then
                    counter[player] = 0
                    charaAnimeState[player] = "Failed_Up"
                end

            elseif charaAnimeState[player] == "Failed_Up" then

                counter[player] = counter[player] + (4.0 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = 0
                    charaAnimeState[player] = "Failed_Down"
                end

            elseif charaAnimeState[player] == "Failed_Down" then

                counter[player] = counter[player] + (6.0 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = 0
                    charaAnimeState[player] = "Failed_Scaling"
                end
            elseif charaAnimeState[player] == "Failed_Scaling" then

                counter[player] = counter[player] + (10.0 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = 1
                    --counter[player] = 0
                    --charaAnimeState[player] = "Clear_End"
                end
            end
        end
    end
end

function draw()
    for player = 1, playerCount do
        if playing[player] then

            left_x = x[player] + left_begin_offset_x
            left_y = y[player] + left_begin_offset_y
            right_x = x[player] + right_begin_offset_x
            right_y = y[player] + right_begin_offset_y
            
            if textAnimeState[player] == "In" then
                text_basevalue = text_failed_counter[player]
                func:DrawSpriteOriginScaleAlpha(x[player] + text_failed_full_offset_x, y[player] + text_failed_full_offset_y, 1, 1, text_basevalue, "Left_Up", spr_text_failed[1])
            elseif textAnimeState[player] == "Wait" then
                func:DrawSpriteOriginScaleAlpha(x[player] + text_failed_full_offset_x, y[player] + text_failed_full_offset_y, 1, 1, 1, "Left_Up", spr_text_failed[1])
            elseif textAnimeState[player] == "Out" then
                text_basevalue = 1 + math.min(math.ceil(text_failed_counter[player] * 5), 4)
                func:DrawSpriteOriginScaleAlpha(x[player] + text_failed_full_offset_x, y[player] + text_failed_full_offset_y, 1, 1, 1, "Left_Up", spr_text_failed[text_basevalue])
            end






            if charaAnimeState[player] == "In" then
                opacity = counter[player]
                func:DrawSpriteOriginScaleAlpha(left_x, left_y, 1, 1, opacity, "Left_Up", spr_chara_left_clear_in[1])
                func:DrawSpriteOriginScaleAlpha(right_x, right_y, 1, 1, opacity, "Right_Up", spr_chara_right_clear_in[1])
            elseif charaAnimeState[player] == "Clear_Move" then
                move_x = math.sin((math.sin(counter[player] * math.pi / 2.0)) * math.pi / 2.0) * chara_clear_move_x
                func:DrawSpriteOriginScaleAlpha(left_x - move_x, left_y, 1, 1, opacity, "Left_Up", spr_chara_left_clear_in[2])
                func:DrawSpriteOriginScaleAlpha(right_x + move_x, right_y, 1, 1, opacity, "Right_Up", spr_chara_right_clear_in[2])
            elseif charaAnimeState[player] == "Failed_Up" then

                left_x = x[player] + left_failed_begin_offset_x
                left_y = y[player] + left_failed_begin_offset_y
                right_x = x[player] + right_failed_begin_offset_x
                right_y = y[player] + right_failed_begin_offset_y

                move_value = math.sin(counter[player] * math.pi / 2.0) 
                move_x = move_value * chara_failed_move_x
                move_y = move_value * chara_failed_move_y
                rotation = move_value * math.pi / 7.5

                effect_opacity = math.sin(math.min(counter[player] * 2, 1) * math.pi)
                effect_scale = 0.5 + (counter[player] * 2)
                func:DrawSpriteOriginScaleAlpha(x[player] + left_failed_effect_offset_x, y[player] + left_failed_effect_offset_y, effect_scale, effect_scale, effect_opacity, 'Center', spr_failed_effect)
                func:DrawSpriteOriginScaleAlpha(x[player] + right_failed_effect_offset_x, y[player] + right_failed_effect_offset_y, effect_scale, effect_scale, effect_opacity, 'Center', spr_failed_effect)

                func:DrawSpriteOriginRotation(left_x - move_x, left_y - move_y, rotation, "Left_Up", spr_chara_left_failed_in[1])
                func:DrawSpriteOriginRotation(right_x + move_x, right_y - move_y, -rotation, "Right_Up", spr_chara_right_failed_in[1])

            elseif charaAnimeState[player] == "Failed_Down" then
                
                left_x = x[player] + left_failed_begin_offset_x
                left_y = y[player] + left_failed_begin_offset_y
                right_x = x[player] + right_failed_begin_offset_x
                right_y = y[player] + right_failed_begin_offset_y

                move_value = 1
                move_x = move_value * chara_failed_move_x
                move_y = (move_value * chara_failed_move_y) - ((1.0 - math.cos(counter[player] * math.pi / 2.0)) * 20)
                rotation = 1 * math.pi / 7.5

                func:DrawSpriteOriginRotation(left_x - move_x, left_y - move_y, rotation, "Left_Up", spr_chara_left_failed_in[1])
                func:DrawSpriteOriginRotation(right_x + move_x, right_y - move_y, -rotation, "Right_Up", spr_chara_right_failed_in[1])
                
            elseif charaAnimeState[player] == "Failed_Scaling" then
                left_x = x[player] + left_failed_begin_offset_x
                left_y = y[player] + left_failed_begin_offset_y
                right_x = x[player] + right_failed_begin_offset_x
                right_y = y[player] + right_failed_begin_offset_y

                move_value = 1
                move_x = move_value * chara_failed_move_x
                move_y = (move_value * chara_failed_move_y) - (1 * 20)
                scale_value = math.sin(counter[player] * math.pi)
                scale_y = 1.0 - (scale_value / 10.0)

                func:DrawSpriteOriginScaleAlpha(left_x - move_x, left_y + (scale_value * 30), 1, scale_y, opacity, "Left_Up", spr_chara_left_failed_in[2])
                func:DrawSpriteOriginScaleAlpha(right_x + move_x, right_y + (scale_value * 30), 1, scale_y, opacity, "Right_Up", spr_chara_right_failed_in[2])
            end


        end
    end
end