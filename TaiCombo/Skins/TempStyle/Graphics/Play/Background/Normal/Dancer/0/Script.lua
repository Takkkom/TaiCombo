
local isClear = { false, false }

local count = 5
local ptn_in = 7
local ptn_idle = 8
local ptn_out = 9
local motion_in = { 0,1,2,3,4,4,4,4,4,5,6 }
local motion_idle = { 0,0,1,1,0,0,1,1,0,0,2,3,4,4,4,0,0,1,1,0,0,1,1,0,0,5,6,7,7,7 }
local motion_out = { 0,1,2,3,4,5,6,6,7,8,8,8,8,8,8,8 }
local beat_in = 2
local beat_idle = 8
local beat_out = 2
local x = { 960, 645, 1284, 322, 1605 }
local y = { 750, 750, 750, 750, 750 }
local dancer_gauge = { 0, 0, 0, 40, "Clear" }

local spr_in = { { 0 }, { 0 }, { 0 }, { 0 }, { 0 } }
local spr_idle = { { 0 }, { 0 }, { 0 }, { 0 }, { 0 } }
local spr_out = { { 0 }, { 0 }, { 0 }, { 0 }, { 0 } }

local dancerState = { "None", "None", "None", "None", "None" }
local in_counter = { 0, 0, 0, 0, 0 }
local idle_counter = 0
local out_counter = { 0, 0, 0, 0, 0 }


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
    for i = 1, count do
        for inFrame = 1, ptn_in do
            spr_in[i][inFrame] = func:AddSprite('In/'..tostring(i - 1)..'/'..tostring(inFrame - 1)..'.png')
        end
        for idleFrame = 1, ptn_idle do
            spr_idle[i][idleFrame] = func:AddSprite('Idle/'..tostring(i - 1)..'/'..tostring(idleFrame - 1)..'.png')
        end
        for outFrame = 1, ptn_out do
            spr_out[i][outFrame] = func:AddSprite('Out/'..tostring(i - 1)..'/'..tostring(outFrame - 1)..'.png')
        end
    end
end

function init()
    for i = 1, count do
    end
end

function update()
    idle_counter = idle_counter + (math.abs(bpm[0]) * deltaTime / 60.0 / beat_idle)

    if idle_counter >= 1 then
        idle_counter = 0
    end

    for i = 1, count do
        if dancerState[i] == "None" then

            if dancer_gauge[i] == "Clear" then
                if isClear[1] then
                    dancerState[i] = "In"
                    in_counter[i] = 0
                end
            elseif gauge[0] >= dancer_gauge[i] then
                dancerState[i] = "In"
                in_counter[i] = 0
            end

        elseif dancerState[i] == "In" then

            in_counter[i] = in_counter[i] + (math.abs(bpm[0]) * deltaTime / 60.0 / beat_in)

            if in_counter[i] >= 1 then
                dancerState[i] = "Idle"
            end

        elseif dancerState[i] == "Idle" then

            if dancer_gauge[i] == "Clear" then
                if not(isClear[1]) then
                    dancerState[i] = "Out"
                    out_counter[i] = 0
                end
            elseif gauge[0] < dancer_gauge[i] then
                --dancerState[i] = "Out"
                --out_counter[i] = 0
            end

        elseif dancerState[i] == "Out" then

            out_counter[i] = out_counter[i] + (math.abs(bpm[0]) * deltaTime / 60.0 / beat_out)

            if out_counter[i] >= 1 then
                dancerState[i] = "None"
            end

        end
    end
end

function draw()
    for i = 1, count do
        if dancerState[i] == "None" then

        elseif dancerState[i] == "In" then
            frame = math.min(math.floor(in_counter[i] * #motion_in) + 1, #motion_in)
            func:DrawSpriteOrigin(x[i], y[i], "Center", spr_in[i][motion_in[frame] + 1])
        elseif dancerState[i] == "Idle" then
            frame = math.min(math.floor(idle_counter * #motion_idle) + 1, #motion_idle)
            func:DrawSpriteOrigin(x[i], y[i], "Center", spr_idle[i][motion_idle[frame] + 1])
        elseif dancerState[i] == "Out" then
            frame = math.min(math.floor(out_counter[i] * #motion_out) + 1, #motion_out)
            func:DrawSpriteOrigin(x[i], y[i], "Center", spr_out[i][motion_out[frame] + 1])
        end
    end
end