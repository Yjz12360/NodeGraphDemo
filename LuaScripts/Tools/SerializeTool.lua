
require("Tools/LuaPanda").start("127.0.0.1", 8818)

require "Tools/TableTool"
json = require "Tools/json"

function json2LuaTable(sJsonContext)
    local tJson = json.decode(sJsonContext)
    tJson = str2NumKeyTable(tJson)
    return table2str(tJson)
end

function luaTable2Json(tableValue)
    local tJson = num2StrKeyTable(tableValue)
    return json.encode(tJson)
end