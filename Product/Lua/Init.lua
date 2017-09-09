
-- global variables / functions
--local zb = "D:\\Software\\ZeroBrane\\"
--package.path = package.path .. ";" .. zb .. "lualibs\\?\\?.lua;" .. zb .. "lualibs\\?.lua;"
--package.cpath = package.cpath .. ";" .. zb .. "bin\\?.dll;" .. zb .. "bin\\clibs\\?.dll;"

--import("mobdebug").start();
-- simple class extends
-- function extends(class, base)
--     base.__index = base
--     setmetatable(class, base)
-- end

-- -- foreach C# ienumerable
-- function foreach(csharp_ienumerable)
--     return Slua.iter(csharp_ienumerable)
-- end

-- -- simple new table to a object
-- function new(table, ctorFunc)
--     assert(table ~= nil)

--     table.__index = table

--     local tb = {}
--     setmetatable(tb, table)

--     if ctorFunc then
--         ctorFunc(tb)
--     end

--     return tb
-- end

print("Init.lua script finish!")
