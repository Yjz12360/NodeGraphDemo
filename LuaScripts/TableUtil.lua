-- function serialize(obj)
--     local lua = ""  
--     local t = type(obj)  
--     if t == "number" then  
--         lua = lua .. obj  
--     elseif t == "boolean" then  
--         lua = lua .. tostring(obj)  
--     elseif t == "string" then  
--         lua = lua .. string.format("%q", obj)  
--     elseif t == "table" then  
--         lua = lua .. "{"  
--         for k, v in pairs(obj) do  
--             lua = lua .. "[" .. serialize(k) .. "]=" .. serialize(v) .. ","  
--         end  
--         local metatable = getmetatable(obj)  
--         if metatable ~= nil and type(metatable.__index) == "table" then  
--             for k, v in pairs(metatable.__index) do  
--                 lua = lua .. "[" .. serialize(k) .. "]=" .. serialize(v) .. ","  
--             end  
--         end  
--         lua = lua .. "}"  
--     elseif t == "nil" then  
--         return nil  
--     else  
--         error("can not serialize a " .. t .. " type.")  
--     end  
--     return lua
-- end

local function ToString(tab, cnt)
    cnt = cnt or 1
    local tp = type(tab)
    if tp ~= 'table' then
        return tostring(tab)
    end

    if cnt >= 4 then -- 这里的4代表嵌套层数，比如：{--1{--2{--3{}}}}
        return tostring(tab)
    end

    local function getSpace(count)
        local temp = {}
        for i = 1, count * 4 do
            table.insert(temp, ' ')
        end
        return table.concat(temp)
    end
    local tabStr = {}
    table.insert(tabStr, '{\n')
    local spaceStr = getSpace(cnt)
    for k, v in pairs(tab) do
        table.insert(tabStr, spaceStr)
        if tonumber(k) == nil then
            table.insert(tabStr, ToString(k, cnt + 1))
            table.insert(tabStr, ' = ')
        else
            table.insert(tabStr, '["')
            table.insert(tabStr, ToString(k, cnt + 1))
            table.insert(tabStr, '"] = ')
        end

        if type(v) == "table" then
            table.insert(tabStr, ToString(v, cnt + 1))
            table.insert(tabStr, ',\n')
        elseif type(v) == "string" then
            table.insert(tabStr, ToString('"' .. v .. '"', cnt + 1))
            table.insert(tabStr, ',\n')
        else
            table.insert(tabStr, ToString(v, cnt + 1))
            table.insert(tabStr, ',\n')
        end

    end
    table.insert(tabStr, getSpace(cnt - 1))
    table.insert(tabStr, '}')

    return table.concat(tabStr)
end

function table2str(tablevalue)
    return ToString(tablevalue)
end

function clear(tablevalue)
    for k in pairs(tablevalue) do
        tablevalue[k] = nil
    end
end

function clearArray(tablevalue)
    for i, _ in ipairs(tablevalue) do
        tablevalue[i] = nil
    end
end

function isEmpty(tablevalue)
    return tablevalue == nil or not next(tablevalue)
end

function dictCount(tablevalue)
    local nCount = 0
    for _, _ in pairs(tablevalue) do
        nCount = nCount + 1
    end
    return nCount
end