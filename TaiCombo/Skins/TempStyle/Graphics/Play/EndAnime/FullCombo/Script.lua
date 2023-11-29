
local x = { 498, 498 }
local y = { 288, 552 }
local speed = 1

local playing = { false, false }

local counter = { 0, 0 }
local charaAnimeState = { "None", "None" }


local spr_chara_left_clear_in = { 0 }
local spr_chara_right_clear_in = { 0 }
local spr_text_clear = { 0 }
local spr_text_clear_full = 0
local spr_text_clear_full_flash = 0
local spr_text_fullcombo = 0
local spr_text_fullcombo_flash = 0
local spr_text_fullcombo_back = 0
local spr_fan_left = { 0 }
local spr_fan_right = { 0 }
local spr_paperEffects_left = { 0 }
local spr_paperEffects_right = { 0 }
local spr_star = 0

local left_begin_offset_x = 522
local right_begin_offset_x = 894
local left_begin_offset_y = -46
local right_begin_offset_y = -46
local text_clear_offset_x = { 451, 538, 628, 727, 835 }
local text_clear_offset_y = { 30, 30, 30, 30, 30 }
local text_clear_full_offset_x = 446
local text_clear_full_offset_y = 32
local text_fullcombo_offset_x = 364
local text_fullcombo_offset_y = 3
local fan_left_offset_x = 71
local fan_left_offset_y = -60
local fan_right_offset_x = 1015
local fan_right_offset_y = -60

local paper_left_offset_x = 104
local paper_left_offset_y = -104
local paper_right_offset_x = 910
local paper_right_offset_y = -104
local fan_loop_counter = { 0, 0 }
local chara_clear_move_x = 348
local chara_fullcombo_move_x = 300
local chara_fullcombo_move_y = 250
local text_clear_counter = { 0, 0 }
local papereffect_counter = { 0, 0 }
local starAnimeValue = { 0, 0 }

function drawStar(star_x, star_y, value)
    if value > 0 then
        opacity = 1 - (math.max(math.min((value / 0.16) - 1.0, 1), 0) * 1)
        scale = math.min(value / 0.16, 1)

        func:DrawSpriteOriginScaleAlpha(star_x, star_y - (40 * text_value), scale, scale, opacity, "Center", spr_star)
    end
end

function playAnime(player)
    playing[player] = true
    counter[player] = 0
    charaAnimeState[player] = "In"
    text_clear_counter[player] = -1.25
end

function loadAssets()
    for i = 1, 8 do
        spr_chara_left_clear_in[i] = func:AddSprite('../Chara_Left_Clear_'..tostring(i - 1)..'.png')
        spr_chara_right_clear_in[i] = func:AddSprite('../Chara_Right_Clear_'..tostring(i - 1)..'.png')
    end
    for i = 1, 5 do
        spr_text_clear[i] = func:AddSprite('../Text_Clear_'..tostring(i - 1)..'.png')
    end
    spr_text_clear_full = func:AddSprite('../Text_Clear_Full.png')
    spr_text_clear_full_flash = func:AddSprite('../Text_Clear_Full_Flash.png')
    spr_text_fullcombo = func:AddSprite('../Text_FullCombo.png')
    spr_text_fullcombo_flash = func:AddSprite('../Text_FullCombo_Flash.png')
    spr_text_fullcombo_back = func:AddSprite('../Text_FullCombo_Back.png')
    for i = 1, 6 do
        spr_fan_left[i] = func:AddSprite('../Fan_Left_'..tostring(i - 1)..'.png')
        spr_fan_right[i] = func:AddSprite('../Fan_Right_'..tostring(i - 1)..'.png')
    end
    for i = 1, 52 do
        spr_paperEffects_left[i] = func:AddSprite('../PaperEffects_Left/'..tostring(i - 1)..'.png')
        spr_paperEffects_right[i] = func:AddSprite('../PaperEffects_Right/'..tostring(i - 1)..'.png')
    end
    spr_star = func:AddSprite('../Star.png')
end

function init()
end

function update()
    for player = 1, playerCount do
        text_clear_counter[player] = text_clear_counter[player] + (3.5 * deltaTime) * speed
        if playing[player] then
            
            fan_loop_counter[player] = fan_loop_counter[player] + (10 * deltaTime)
            if fan_loop_counter[player] >= 1 then
                fan_loop_counter[player] = 0
            end

            papereffect_counter[player] = papereffect_counter[player] + (0.5 * deltaTime)
            if papereffect_counter[player] >= 1 then
                papereffect_counter[player] = 0
            end
            
            if text_clear_counter[player] >= 5.25 then
                starAnimeValue[player] = starAnimeValue[player] + (speed * deltaTime)
                if starAnimeValue[player] > 1.57 then
                    starAnimeValue[player] = 0
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

                if counter[player] > 1 then
                    counter[player] = 0
                    charaAnimeState[player] = "Clear_Scaling"
                end

            elseif charaAnimeState[player] == "Clear_Scaling" then

                counter[player] = counter[player] + (8.0 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = 0
                    charaAnimeState[player] = "Clear_End"
                end

            elseif charaAnimeState[player] == "Clear_End" then

                counter[player] = counter[player] + (4.0 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = 1

                    counter[player] = 0
                    charaAnimeState[player] = "FullCombo_In"
                end
                
            elseif charaAnimeState[player] == "FullCombo_In" then

                counter[player] = counter[player] + (3.0 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = 1

                    counter[player] = 0
                    charaAnimeState[player] = "FullCombo_Move"
                end

            elseif charaAnimeState[player] == "FullCombo_Move" then

                counter[player] = counter[player] + (1.75 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = 1

                    counter[player] = 0
                    charaAnimeState[player] = "FullCombo_Out"
                end

            elseif charaAnimeState[player] == "FullCombo_Out" then

                counter[player] = counter[player] + (6.0 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = -1.5
                    charaAnimeState[player] = "FullCombo_Idle"
                end
            
            elseif charaAnimeState[player] == "FullCombo_Idle" then

                counter[player] = counter[player] + (1.5 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = 0
                    charaAnimeState[player] = "FullCombo_JumpScaling"

                end
            elseif charaAnimeState[player] == "FullCombo_JumpScaling" then

                counter[player] = counter[player] + (3.0 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = 0
                    charaAnimeState[player] = "FullCombo_Jump"
                end
            elseif charaAnimeState[player] == "FullCombo_Jump" then

                counter[player] = counter[player] + (2.25 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = 0
                    charaAnimeState[player] = "FullCombo_JumpOut"
                end
            elseif charaAnimeState[player] == "FullCombo_JumpOut" then

                counter[player] = counter[player] + (5.0 * deltaTime) * speed

                if counter[player] > 1 then
                    counter[player] = 0
                    charaAnimeState[player] = "FullCombo_Idle"
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
            
            if text_clear_counter[player] > 2.5 then
                text_flash_basevalue = (text_clear_counter[player] - 2.5) / 2
                text_flash_value = math.sin(math.min(text_flash_basevalue, 1) * math.pi)
                text_fc_flash_basevalue = math.min(text_flash_basevalue - 0.2, 1)
                text_fc_flash_value = math.min(math.sin(text_fc_flash_basevalue * math.pi) * 1.20, 1)
                fc_scale_value = math.sin(math.min(math.max((text_flash_basevalue - 1.2) * 4, 0), 1) * math.pi) / 8.0
                fc_scale_y = 1.0 - fc_scale_value
                fc_move_y = (text_fc_flash_value * -30) + (fc_scale_value * 160)

                
                if fan_loop_counter[player] >= 0.5 and text_clear_counter[player] >= 5.0 then
                    func:DrawSpriteOpacity(x[player] + fan_left_offset_x, y[player] + fan_left_offset_y, fan_opacity, spr_fan_left[6])
                    func:DrawSpriteOpacity(x[player] + fan_right_offset_x, y[player] + fan_right_offset_y, fan_opacity, spr_fan_right[6])
                else
                    fan_frame = math.max(math.min(math.ceil(text_fc_flash_basevalue * 20), 5), 1)
                    fan_opacity = math.min(text_fc_flash_basevalue * 20, 1)
                    func:DrawSpriteOpacity(x[player] + fan_left_offset_x, y[player] + fan_left_offset_y, fan_opacity, spr_fan_left[fan_frame])
                    func:DrawSpriteOpacity(x[player] + fan_right_offset_x, y[player] + fan_right_offset_y, fan_opacity, spr_fan_right[fan_frame])
                end

                if text_clear_counter[player] >= 5.25 then
                    paperFrame = math.ceil(papereffect_counter[player] * 51) + 1
                    paper_opacity = (text_clear_counter[player] - 5.25)
                    func:DrawSpriteOpacity(x[player] + paper_left_offset_x, y[player] + paper_left_offset_y, paper_opacity, spr_paperEffects_left[paperFrame])
                    func:DrawSpriteOpacity(x[player] + paper_right_offset_x, y[player] + paper_right_offset_y, paper_opacity, spr_paperEffects_right[paperFrame])
                end

                if text_flash_basevalue < 0.5 then
                    func:DrawSpriteOpacity(x[player] + text_clear_full_offset_x, y[player] + text_clear_full_offset_y, 1, spr_text_clear_full)
                end
                func:DrawSpriteOpacity(x[player] + text_clear_full_offset_x, y[player] + text_clear_full_offset_y, text_flash_value, spr_text_clear_full_flash)


                if text_fc_flash_basevalue < 0.5 then
                    func:DrawSpriteOriginScaleAlpha(x[player] + text_fullcombo_offset_x, y[player] + text_fullcombo_offset_y + fc_move_y, 1, fc_scale_y, text_fc_flash_value, "Left_Up", spr_text_fullcombo_back)
                    func:DrawSpriteOriginScaleAlpha(x[player] + text_fullcombo_offset_x, y[player] + text_fullcombo_offset_y + fc_move_y, 1, fc_scale_y, text_fc_flash_value, "Left_Up", spr_text_fullcombo_flash)
                else
                    func:DrawSpriteOriginScaleAlpha(x[player] + text_fullcombo_offset_x, y[player] + text_fullcombo_offset_y + fc_move_y, 1, fc_scale_y, 1, "Left_Up", spr_text_fullcombo_back)
                    func:DrawSpriteOriginScaleAlpha(x[player] + text_fullcombo_offset_x, y[player] + text_fullcombo_offset_y + fc_move_y, 1, fc_scale_y, 1, "Left_Up", spr_text_fullcombo)
                    func:DrawSpriteOriginScaleAlpha(x[player] + text_fullcombo_offset_x, y[player] + text_fullcombo_offset_y + fc_move_y, 1, fc_scale_y, text_fc_flash_value, "Left_Up", spr_text_fullcombo_flash)
                end
                
                
                
                if text_clear_counter[player] >= 5.25 then
                    drawStar(x[player] + 709, y[player] + 24, starAnimeValue[player])
                    drawStar(x[player] + 893, y[player] + 24, starAnimeValue[player] - 0.1)
                    drawStar(x[player] + 526, y[player] + 24, starAnimeValue[player] - 0.12)
            
                    drawStar(x[player] + 590, y[player] + 169, starAnimeValue[player] - 0.28)
                    drawStar(x[player] + 948, y[player] + 148, starAnimeValue[player] - 0.33)
                    drawStar(x[player] + 466, y[player] + 143, starAnimeValue[player] - 0.45)
                    drawStar(x[player] + 815, y[player] + 170, starAnimeValue[player] - 0.5)
                end
            else
                for i = 1, 5 do
                    text_basevalue = text_clear_counter[player] - (0.15 * i)
                    text_value = math.sin(math.min(text_basevalue, 1) * math.pi);
                    text_x = x[player] + text_clear_offset_x[i]
                    text_y = y[player] + text_clear_offset_y[i]
                    text_scale = 1 + (text_value / 3.5)
                    text_opacity = math.min(text_basevalue * 2, 1)
                    func:DrawSpriteOriginScaleAlpha(text_x, text_y - (40 * text_value), 1, text_scale, text_opacity, "Left_Up", spr_text_clear[i])
                end
            end
            






            if charaAnimeState[player] == "In" then
                opacity = counter[player]
                func:DrawSpriteOriginScaleAlpha(left_x, left_y, 1, 1, opacity, "Left_Up", spr_chara_left_clear_in[1])
                func:DrawSpriteOriginScaleAlpha(right_x, right_y, 1, 1, opacity, "Right_Up", spr_chara_right_clear_in[1])
            elseif charaAnimeState[player] == "Clear_Move" then
                opacity = 1
                move_x = math.sin((math.sin(counter[player] * math.pi / 2.0)) * math.pi / 2.0) * chara_clear_move_x
                func:DrawSpriteOriginScaleAlpha(left_x - move_x, left_y, 1, 1, opacity, "Left_Up", spr_chara_left_clear_in[2])
                func:DrawSpriteOriginScaleAlpha(right_x + move_x, right_y, 1, 1, opacity, "Right_Up", spr_chara_right_clear_in[2])
            elseif charaAnimeState[player] == "Clear_Scaling" then
                scale_value = (math.sin(counter[player] * math.pi) / 20.0)
                scale_x = 1.0 - scale_value
                move_x = chara_clear_move_x - (scale_value * 45)
                opacity = 1
                func:DrawSpriteOriginScaleAlpha(left_x - move_x, left_y, scale_x, 1, opacity, "Left_Up", spr_chara_left_clear_in[3])
                func:DrawSpriteOriginScaleAlpha(right_x + move_x, right_y, scale_x, 1, opacity, "Right_Up", spr_chara_right_clear_in[3])
            elseif charaAnimeState[player] == "Clear_End" then
                move_x = chara_clear_move_x
                frame = math.ceil(counter[player] * 5)
                opacity = 1
                func:DrawSpriteOriginScaleAlpha(left_x - move_x, left_y, 1, 1, opacity, "Left_Up", spr_chara_left_clear_in[3 + frame])
                func:DrawSpriteOriginScaleAlpha(right_x + move_x, right_y, 1, 1, opacity, "Right_Up", spr_chara_right_clear_in[3 + frame])


            elseif charaAnimeState[player] == "FullCombo_In" then
                move_x = chara_clear_move_x
                scale_value = (math.sin(counter[player] * math.pi) / 10.0)
                scale_y = 1.0 - scale_value
                move_y = (scale_value * 380)
                func:DrawSpriteOriginScaleAlpha(left_x - move_x, left_y + move_y, 1, scale_y, 1, "Left_Up", spr_chara_left_clear_in[8])
                func:DrawSpriteOriginScaleAlpha(right_x + move_x, right_y + move_y, 1, scale_y, 1, "Right_Up", spr_chara_right_clear_in[8])
            elseif charaAnimeState[player] == "FullCombo_Move" then
                move_value = counter[player]
                move_x = chara_clear_move_x + ((1.0 - math.cos(move_value * math.pi)) * chara_fullcombo_move_x / 2.0)
                move_y = (math.sin(move_value * math.pi) * chara_fullcombo_move_y)
                func:DrawSpriteOriginScaleAlpha(left_x - move_x, left_y - move_y, 1, 1, 1, "Left_Up", spr_chara_left_clear_in[8])
                func:DrawSpriteOriginScaleAlpha(right_x + move_x, right_y - move_y, 1, 1, 1, "Right_Up", spr_chara_right_clear_in[8])
            elseif charaAnimeState[player] == "FullCombo_Out" then
                move_x = chara_clear_move_x + chara_fullcombo_move_x
                scale_value = (math.sin(counter[player] * math.pi) / 10.0)
                scale_y = 1.0 - scale_value
                move_y = (scale_value * 380)
                func:DrawSpriteOriginScaleAlpha(left_x - move_x, left_y + move_y, 1, scale_y, 1, "Left_Up", spr_chara_left_clear_in[8])
                func:DrawSpriteOriginScaleAlpha(right_x + move_x, right_y + move_y, 1, scale_y, 1, "Right_Up", spr_chara_right_clear_in[8])
            elseif charaAnimeState[player] == "FullCombo_Idle" then
                move_x = chara_clear_move_x + chara_fullcombo_move_x
                func:DrawSpriteOriginScaleAlpha(left_x - move_x, left_y, 1, 1, 1, "Left_Up", spr_chara_left_clear_in[8])
                func:DrawSpriteOriginScaleAlpha(right_x + move_x, right_y, 1, 1, 1, "Right_Up", spr_chara_right_clear_in[8])
                
            elseif charaAnimeState[player] == "FullCombo_JumpScaling" then
                move_x = chara_clear_move_x + chara_fullcombo_move_x
                scale_value = (math.sin(counter[player] * math.pi) / 10.0)
                scale_y = 1.0 - scale_value
                move_y = (scale_value * 380)
                func:DrawSpriteOriginScaleAlpha(left_x - move_x, left_y + move_y, 1, scale_y, 1, "Left_Up", spr_chara_left_clear_in[8])
                func:DrawSpriteOriginScaleAlpha(right_x + move_x, right_y + move_y, 1, scale_y, 1, "Right_Up", spr_chara_right_clear_in[8])
            elseif charaAnimeState[player] == "FullCombo_Jump" then
                move_x = chara_clear_move_x + chara_fullcombo_move_x
                move_y = -(math.sin(counter[player] * math.pi) * chara_fullcombo_move_y / 6.0)
                func:DrawSpriteOriginScaleAlpha(left_x - move_x, left_y + move_y, 1, scale_y, 1, "Left_Up", spr_chara_left_clear_in[8])
                func:DrawSpriteOriginScaleAlpha(right_x + move_x, right_y + move_y, 1, scale_y, 1, "Right_Up", spr_chara_right_clear_in[8])
            elseif charaAnimeState[player] == "FullCombo_JumpOut" then
                move_x = chara_clear_move_x + chara_fullcombo_move_x
                scale_value = (math.sin(counter[player] * math.pi) / 10.0)
                scale_y = 1.0 - scale_value
                move_y = (scale_value * 380)
                func:DrawSpriteOriginScaleAlpha(left_x - move_x, left_y + move_y, 1, scale_y, 1, "Left_Up", spr_chara_left_clear_in[8])
                func:DrawSpriteOriginScaleAlpha(right_x + move_x, right_y + move_y, 1, scale_y, 1, "Right_Up", spr_chara_right_clear_in[8])
            end
        end
    end
end