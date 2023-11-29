
--AddSprite('fileName')
--DrawSprite(x, y, 'fileName')

local mob_x = 0
local mob_front_y = 0
local mob_back_y = 0
local mob_back2_y = 0
local mob_back3_y = 0
local mob_height = 0
local mob_counter = 0
local mob_action_counter = 0
local mob_in_counter = 0
local mob_out_counter = 0

local mob_state = 0

local spr_Mob_Front = 0
local spr_Mob_Back_0 = 0
local spr_Mob_Back_1 = 0
local spr_Mob_Back2_0 = 0
local spr_Mob_Back2_1 = 0
local spr_Mob_Back3_0 = 0
local spr_Mob_Back3_1 = 0

function addRollEffect(player)
end

function clearIn(player)
end

function clearOut(player)
end

function maxIn(player)
    mob_state = 1
    mob_in_counter = 0
end

function maxOut(player)
    mob_state = 2
    mob_out_counter = 0
end

function loadAssets()
    spr_Mob_Front = func:AddSprite("Mob_Front.png")
    spr_Mob_Back_0 = func:AddSprite("Mob_Back_0.png")
    spr_Mob_Back_1 = func:AddSprite("Mob_Back_1.png")
    spr_Mob_Back2_0 = func:AddSprite("Mob_Back2_0.png")
    spr_Mob_Back2_1 = func:AddSprite("Mob_Back2_1.png")
    spr_Mob_Back3_0 = func:AddSprite("Mob_Back3_0.png")
    spr_Mob_Back3_1 = func:AddSprite("Mob_Back3_1.png")
    mob_height = func:GetSpriteHeight(spr_Mob_Front)
end

function init()
end

function update()
    --if mob_state == 3 and gauge[0] < 100 then
    --    mobOut()
    --end

    if mob_state == 0 then

        --if gauge[0] == 100 then
        --    mobIn()
        --end

    elseif mob_state == 1 then

        mob_in_counter = mob_in_counter + (math.abs(bpm[0]) * deltaTime / 30.0)
        if mob_in_counter > 1 then
            mob_state = 3
            mob_counter = 0.5
            mob_action_counter = 0
        end
        
        mobinValue = (1.0 - math.sin(mob_in_counter * math.pi / 2))
        mob_front_y = 1080 + (540 * mobinValue)
        mob_back_y = 1080 + (540 * mobinValue)
        mob_back2_y = 1080 + (540 * mobinValue)
        mob_back3_y = 1080 + (540 * mobinValue)



    elseif mob_state == 2 then

        mob_out_counter = mob_out_counter + (math.abs(bpm[0]) * deltaTime / 30.0)
        if mob_out_counter > 1 then
            mob_state = 0
        end
        
        mobOutValue = (1 - math.cos(mob_out_counter * math.pi))
        mob_front_y = 1080 + (540 * mobOutValue)
        mob_back_y = 1080 + (540 * mobOutValue)
        mob_back2_y = 1080 + (540 * mobOutValue)
        mob_back3_y = 1080 + (540 * mobOutValue)

    elseif mob_state == 3 then

        mob_counter = mob_counter + (math.abs(bpm[0]) * deltaTime / 60.0)
        if mob_counter > 1 then 
            mob_counter = 0
        end
    
        
        mob_action_counter = mob_action_counter + (math.abs(bpm[0]) * deltaTime / 65.0)
        if mob_action_counter > 1 then 
            mob_action_counter = 0
        end
    
    
        mob_loop_value = (1.0 - math.sin(mob_counter * math.pi))
        mob_front_y = 1080 + (mob_loop_value * 45)
        mob_back_y = 1080 + (mob_loop_value * 90)
        mob_back2_y = 1080 + (mob_loop_value * 92)
        mob_back3_y = 1080 + (mob_loop_value * 92)
    end
end

function draw()
    if mob_state == 0 then
    else
        if mob_action_counter > 0.25 and mob_action_counter < 0.5 then
            func:DrawSprite(mob_x, mob_back_y - mob_height, spr_Mob_Back_1)
            func:DrawSprite(mob_x, mob_back2_y - mob_height, spr_Mob_Back2_1)
        else
            func:DrawSprite(mob_x, mob_back_y - mob_height, spr_Mob_Back_0)
            func:DrawSprite(mob_x, mob_back2_y - mob_height, spr_Mob_Back2_0)
        end
        if (mob_counter > 0.500 and mob_counter < 0.6) or (mob_counter > 0.7 and mob_counter < 0.8) then
            func:DrawSprite(mob_x, mob_back3_y - mob_height, spr_Mob_Back3_1)
        else
            func:DrawSprite(mob_x, mob_back3_y - mob_height, spr_Mob_Back3_0)
        end
        func:DrawSprite(mob_x, mob_front_y - mob_height, spr_Mob_Front)
    end
end