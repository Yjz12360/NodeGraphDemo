local function isArrayTable(tab)
    if type(tab) ~= "table" then
        return false
    end
    local n = #tab
    local count = 0
    for i, v in pairs(tab) do
        if type(i) ~= "number" then
            return false
        end
        if i > n then
            return false
        end
        count = count + 1
    end
    if n ~= count then
        return false
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

local function getSortedKeys(tableValue)
    if type(tableValue) ~= "table" then
        return
    end
    local sortKeys = {}
    for key, _ in pairs(tableValue) do
        table.insert(sortKeys, key)
    end
    table.sort(sortKeys)
    return sortKeys
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
            for i, v in ipairs(element) do
                doIndent()
                table.insert(tStrTemp, "[")
                table.insert(tStrTemp, tostring(i))
                table.insert(tStrTemp, "] = ")
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
        local tSortKeys = getSortedKeys(element)
        if containsSubTable(element) then
            nIndent = nIndent + 1
            table.insert(tStrTemp, "{\n")
            for _, k in ipairs(tSortKeys) do
                local v = element[k]
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
            for _, k in ipairs(tSortKeys) do
                local v = element[k]
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

function str2NumKeyTable(tableValue)
    if type(tableValue) ~= "table" then
        return tableValue
    end
    local convertTable = {}
    for key, val in pairs(tableValue) do
        local convertKey
        if type(key) == "string" then
            convertKey = tonumber(key) or key
        else
            convertKey = key
        end
        if type(val) == "table" then
            convertTable[convertKey] = str2NumKeyTable(val)
        else
            convertTable[convertKey] = val
        end
    end
    return convertTable
end

function num2StrKeyTable(tableValue)
    if type(tableValue) ~= "table" then
        return tableValue
    end
    local convertTable = {}
    if isArrayTable(tableValue) then
        for i, val in ipairs(tableValue) do
            local convertKey = tostring(i)
            if type(val) == "table" then
                convertTable[convertKey] = num2StrKeyTable(val)
            else
                convertTable[convertKey] = val
            end
        end
    else
        for key, val in pairs(tableValue) do
            local convertKey
            if type(key) == "number" then
                convertKey = tostring(key)
            else
                convertKey = key
            end
            if type(val) == "table" then
                convertTable[convertKey] = num2StrKeyTable(val)
            else
                convertTable[convertKey] = val
            end
        end
    end

    return convertTable
end