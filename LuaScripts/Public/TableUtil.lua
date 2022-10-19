local function isArrayTable(tab)
    if type(tab) ~= "table" then
        return false
    end
    local n = #tab
    for i, v in pairs(tab) do
        if type(i) ~= "number" then
            return false
        end
        if i > n then
            return false
        end
    end
    return true
end

local function containsSubTable(tab)
    if type(tab) ~= "table" then
        return false
    end
    for k, v in pairs(tab) do
        if type(v) == "table" then
            return true
        end
    end
    return false
end

local function getDictCount(tablevalue)
    local nCount = 0
    for _, _ in pairs(tablevalue) do
        nCount = nCount + 1
    end
    return nCount
end

local tStrTemp = {}
local nIndent = 0
local function doIndent()
    for i = 1, nIndent do
        table.insert(tStrTemp, "    ")
    end
end
local function toStr(element)
    local tp = type(element)
    if tp ~= "table" then
        if tp == "string" then
            table.insert(tStrTemp, "\"")
            table.insert(tStrTemp, element)
            table.insert(tStrTemp, "\"")
        else
            table.insert(tStrTemp, tostring(element))
        end
        return
    end
    if isArrayTable(element) then
        if containsSubTable(element) then
            nIndent = nIndent + 1
            table.insert(tStrTemp, "{\n")
            for _, v in ipairs(element) do
                doIndent()
                toStr(v)
                table.insert(tStrTemp, ",\n")
            end
            nIndent = nIndent - 1
            doIndent()
            table.insert(tStrTemp, "}")
        else
            table.insert(tStrTemp, "{")
            for i, v in ipairs(element) do
                toStr(v)
                if i < #element then
                    table.insert(tStrTemp, ", ")
                end
            end
            table.insert(tStrTemp, "}")
        end
    else
        if containsSubTable(element) then
            nIndent = nIndent + 1
            table.insert(tStrTemp, "{\n")
            for k, v in pairs(element) do
                doIndent()
                if type(k) == "string" then
                    if tonumber(k) == nil then
                        table.insert(tStrTemp, k)
                        table.insert(tStrTemp, " = ")
                    else
                        table.insert(tStrTemp, "[\"")
                        table.insert(tStrTemp, tostring(k))
                        table.insert(tStrTemp, "\"] = ")
                    end
                else
                    table.insert(tStrTemp, "[")
                    table.insert(tStrTemp, tostring(k))
                    table.insert(tStrTemp, "] = ")
                end
                toStr(v)
                table.insert(tStrTemp, ",\n")
            end
            nIndent = nIndent - 1
            doIndent()
            table.insert(tStrTemp, "}")
        else
            table.insert(tStrTemp, "{")
            local nDictCount = getDictCount(element)
            local nIdx = 0
            for k, v in pairs(element) do
                if type(k) == "string" then
                    if tonumber(k) == nil then
                        table.insert(tStrTemp, k)
                        table.insert(tStrTemp, " = ")
                    else
                        table.insert(tStrTemp, "[\"")
                        table.insert(tStrTemp, tostring(k))
                        table.insert(tStrTemp, "\"] = ")
                    end
                    
                else
                    table.insert(tStrTemp, "[")
                    toStr(k)
                    table.insert(tStrTemp, "] = ")
                end
                toStr(v)
                nIdx = nIdx + 1
                if nIdx < nDictCount then
                    table.insert(tStrTemp, ", ")
                end
            end
            table.insert(tStrTemp, "}")
        end
    end
end

function table2str(tab)
    tStrTemp = {}
    toStr(tab)
    return table.concat(tStrTemp)
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
    return getDictCount(tablevalue)
end