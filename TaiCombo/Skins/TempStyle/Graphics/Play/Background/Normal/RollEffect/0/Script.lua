
--AddSprite('fileName')
--DrawSprite(x, y, 'fileName')

local spr_arrow = 0

local states = { }

function addRollEffect(player)
    origin_x = math.random(0, 960) - (250)
    origin_y = 0



    if playerCount == 1 then
        origin_y = 1080
    else
        if player == 1 then
            origin_y = 276
        else
            origin_y = 1080
        end
    end

    state = {
        value = 0,
        x = origin_x,
        y = origin_y,
        move_x = 800,
        move_y = -800,
        player_ = player
    }
    states[#states + 1] = state
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
    spr_arrow = func:AddSprite('Arrow.png')
end

function init()
end

function update()
    for i = 1, #states do
        state = states[i]

        state.x = state.x + (state.move_x * deltaTime)
        state.y = state.y + (state.move_y * deltaTime)

        state.value = state.value + deltaTime

    end
    if #states > 1 and states[1].value > 2 then
        table.remove(states, 1)
    end
end

function draw()
    for i = 1, #states do
        state = states[i]

        func:DrawSprite(state.x, state.y, spr_arrow)
    end
end