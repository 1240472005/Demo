--- Created by qc.
--- DateTime: 2021/4/21 11:35
---保存类类型的虚表
---@class BaseClass
local _class ={}
--自定义类型
ClassType =
{
  class = 1,--普通类
  instance = 2,--单例
}

function BaseClass(classname,super)
    --判断类名是否是合法的
    assert(type(classname)=="string" and #classname >0)
    --生成一个类型
    local class_type = {}
    --创建的时候自动调用
    class_type.__init =false
    class_type.__delete = false
    class_type.__cname =classname
    class_type.__ctype = ClassType.class
    
    class_type.super = super
    class_type.New =function(...)
        --生成一个类的对象 
        local obj = {}
        obj._class_type = class_type
        obj.__ctype=ClassType.instance
        --在初始化之前注册类的基类方法
        -- 以string 为类型标识
        setmetatable(
                obj,
                {
                    __index = _class[class_type]
                }
        )
        --调用初始化方法
        do
            local create
            create =function(c,...)
                if c.super then
                    create(c.super,...)
                    end
                if c.__init then
                    c.__init(obj,...)
                end
            end
            create(class_type,...)
        end
        
        --注册一个delete方法
        obj.Delete=function(self) 
            local now_super =self._class_type
            while now_super ~=nil do
                if now_super.__delete then
                    now_super.__delete(self)
                end
                now_super = now_super.super
            end
        end
        return obj
    end

    local  vtbl={}
    _class[class_type]=vtbl
    setmetatable(
        class_type,
        {
            __newindex =function (t,k,v)
                vtbl[k]=v
            end,
            __index =vtbl
        }

    )
    if(super) then
        setmetatable(
            vtbl,
            {
                __index =function (t,k)
                local  ret =_class[super][k]
                return ret
                    -- body
                end

            }
        )
        end
        
        return class_type
end