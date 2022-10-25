

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