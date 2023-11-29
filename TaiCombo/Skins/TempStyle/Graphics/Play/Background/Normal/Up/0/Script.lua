
--func:AddSprite('fileName')
--func:DrawSprite(x, y, 'fileName')

local x = 0
local y = { 0, 804 }
local width = 492
local animeCounter = 0
local animeCounter2 = 0
local animeValue2 = 0
local scroll_y = 0
local clearOpacity = { 0, 0 }

local isClear = { false, false }


local spr_1st_player = { 0, 0 }
local spr_2nd_player = { 0, 0 }
local spr_3rd_player = { 0, 0 }
local spr_3rd_1_player = { { 0, 0, 0 }, { 0, 0, 0 } }
local spr_3rd_2_player = { { 0, 0, 0 }, { 0, 0, 0 } }
local spr_3rd_2_alt_player = { 0, 0 }
local spr_3rd_3_player = { { 0, 0, 0 }, { 0, 0, 0 } }
local spr_1st_clear = { 0, 0 }
local spr_2nd_clear = { 0, 0 }
local spr_3rd_clear = { 0, 0 }
local spr_3rd_1_clear = { 0, 0 }
local spr_3rd_2_clear = { 0, 0 }
local spr_3rd_2_alt_clear = 0
local spr_3rd_3_clear = { 0, 0 }



function addRollEffect(player)
end

function clearIn(player)
    isClear[player] = true
end

function clearOut(player)
    isClear[player] = false
end

function maxIn(player)
end

function maxOut(player)
end

function loadAssets()
    for player = 1, 2 do
        spr_1st_player[player] = func:AddSprite("1st_"..tostring(player).."P.png")
        spr_2nd_player[player] = func:AddSprite("2nd_"..tostring(player).."P.png")
        spr_3rd_player[player] = func:AddSprite("3rd_0_"..tostring(player).."P.png")
        
        spr_3rd_2_alt_player[player] = func:AddSprite("3rd_2_2_Alt_"..tostring(player).."P.png")
        for i = 0, 2 do
            spr_3rd_1_player[player][i + 1] = func:AddSprite("3rd_1_"..tostring(i).."_"..tostring(player).."P.png")
            spr_3rd_2_player[player][i + 1] = func:AddSprite("3rd_2_"..tostring(i).."_"..tostring(player).."P.png")
            spr_3rd_3_player[player][i + 1] = func:AddSprite("3rd_3_"..tostring(i).."_"..tostring(player).."P.png")
        end
    end
    spr_1st_clear = func:AddSprite("1st_Clear.png")
    spr_2nd_clear = func:AddSprite("2nd_Clear.png")
    spr_3rd_clear = func:AddSprite("3rd_0_Clear.png")

    spr_3rd_2_alt_clear = func:AddSprite("3rd_2_2_Alt_Clear.png")
    for i = 0, 2 do
        spr_3rd_1_clear[i + 1] = func:AddSprite("3rd_1_"..tostring(i).."_Clear.png")
        spr_3rd_2_clear[i + 1] = func:AddSprite("3rd_2_"..tostring(i).."_Clear.png")
        spr_3rd_3_clear[i + 1] = func:AddSprite("3rd_3_"..tostring(i).."_Clear.png")
    end
end

function init()
end

function update()
    x = x - (90 * deltaTime)
    if x < -width * 3 then 
        x = 0
    end

    animeCounter = animeCounter + (0.85 * deltaTime)
    animeCounter = animeCounter % 2
    if animeCounter < 1 then 
        scroll_y = animeCounter
    else 
        scroll_y = 2.0 - animeCounter
    end

    animeCounter2 = animeCounter2 + (0.45 * deltaTime)
    animeCounter2 = animeCounter2 % 2
    if animeCounter2 < 1 then 
        animeValue2 = animeCounter2
    else 
        animeValue2 = math.max(1.5 - animeCounter2, 0)
    end


    for player = 1, playerCount do
        if isClear[player] then 
            clearOpacity[player] = math.min(clearOpacity[player] + (4 * deltaTime), 1)
        else 
            clearOpacity[player] = math.max(clearOpacity[player] - (4 * deltaTime), 0)
        end
    end
end

function draw()
    for player = 1, playerCount do
        side = player
        if p1IsBlue then
            side = 2
        end
        
        for i = 0, 15 do
            y_1st = y[player]
            y_2nd = y[player] - 26 + (scroll_y * 20)
            y_3rd = y[player] - 26 + (scroll_y * 15)
            
            move_3rd = 0
            move_3rd_r = 0
            if animeCounter2 < 1 then
                move_3rd = math.min(animeValue2 * 10, 1) * 150
                move_3rd_r = (1.0 - math.min(animeValue2 * 4, 1)) * 150
            else
                move_3rd = math.min(animeValue2 * 10, 1) * 150
                move_3rd_r = (1.0 - math.min(animeValue2 * 5, 1)) * 150
            end

            func:DrawSprite(x + (width * i), y_1st, spr_1st_player[side])
            func:DrawSprite(x + (width * i), y_2nd, spr_2nd_player[side])

            if move_3rd_r == 150 then 
                func:DrawSprite(x + (width * i), y_3rd, spr_3rd_player[side])
            else 
                if i % 3 == 0 then
                    if move_3rd_r <= 120 then 
                        func:DrawSprite(x + (width * i) - move_3rd_r, y_3rd, spr_3rd_1_player[side][3])
                    end

                    if move_3rd <= 80 then 
                        func:DrawSprite(x + (width * i) - move_3rd, y_3rd, spr_3rd_1_player[side][2])
                    end

                    func:DrawSprite(x + (width * i), y_3rd, spr_3rd_1_player[side][1])
                elseif i % 3 == 1 then
                    if move_3rd_r <= 80 then 
                        func:DrawSprite(x + (width * i), y_3rd + move_3rd_r, spr_3rd_2_player[side][3])
                    elseif move_3rd_r <= 120 then 
                        func:DrawSprite(x + (width * i), y_3rd + move_3rd_r, spr_3rd_2_alt_player[side])
                    end

                    if move_3rd <= 80 then 
                        func:DrawSprite(x + (width * i), y_3rd + move_3rd, spr_3rd_2_player[side][2])
                    end

                    func:DrawSprite(x + (width * i), y_3rd, spr_3rd_2_player[side][1])
                elseif i % 3 == 2 then
                    if move_3rd_r <= 120 then 
                        func:DrawSprite(x + (width * i) + move_3rd_r, y_3rd, spr_3rd_3_player[side][3])
                    end

                    if move_3rd <= 80 then 
                        func:DrawSprite(x + (width * i) + move_3rd, y_3rd, spr_3rd_3_player[side][2])
                    end

                    func:DrawSprite(x + (width * i), y_3rd, spr_3rd_3_player[side][1])
                end
            end


            func:DrawSpriteOpacity(x + (width * i), y_1st, clearOpacity[player], spr_1st_clear)

            func:DrawSpriteOpacity(x + (width * i), y_2nd, clearOpacity[player], spr_2nd_clear)

            if move_3rd_r == 150 then 
                func:DrawSpriteOpacity(x + (width * i), y_3rd, clearOpacity[player], spr_3rd_clear)
            else 
                if i % 3 == 0 then
                    if move_3rd_r <= 120 then 
                        func:DrawSpriteOpacity(x + (width * i) - move_3rd_r, y_3rd, clearOpacity[player], spr_3rd_1_clear[3])
                    end

                    if move_3rd <= 80 then 
                        func:DrawSpriteOpacity(x + (width * i) - move_3rd, y_3rd, clearOpacity[player], spr_3rd_1_clear[2])
                    end

                    func:DrawSpriteOpacity(x + (width * i), y_3rd, clearOpacity[player], spr_3rd_1_clear[1])
                elseif i % 3 == 1 then
                    if move_3rd_r <= 80 then 
                        func:DrawSpriteOpacity(x + (width * i), y_3rd + move_3rd_r, clearOpacity[player], spr_3rd_2_clear[3])
                    elseif move_3rd_r <= 120 then 
                        func:DrawSpriteOpacity(x + (width * i), y_3rd + move_3rd_r, clearOpacity[player], spr_3rd_2_alt_clear)
                    end

                    if move_3rd <= 80 then 
                        func:DrawSpriteOpacity(x + (width * i), y_3rd + move_3rd, clearOpacity[player], spr_3rd_2_clear[2])
                    end

                    func:DrawSpriteOpacity(x + (width * i), y_3rd, clearOpacity[player], spr_3rd_2_clear[1])
                elseif i % 3 == 2 then
                    if move_3rd_r <= 120 then 
                        func:DrawSpriteOpacity(x + (width * i) + move_3rd_r, y_3rd, clearOpacity[player], spr_3rd_3_clear[3])
                    end

                    if move_3rd <= 80 then 
                        func:DrawSpriteOpacity(x + (width * i) + move_3rd, y_3rd, clearOpacity[player], spr_3rd_3_clear[2])
                    end

                    func:DrawSpriteOpacity(x + (width * i), y_3rd, clearOpacity[player], spr_3rd_3_clear[1])
                end
            end
        end
    end
end