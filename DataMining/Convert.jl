module Convert
    function Hex2Vector(str::String)
        return convert(Array{Float64}, hex2bytes(str))
    end

    function Vector2Hex(arr)
        str = ""
        for i = 1:3 str = string((arr[i] < 16 ? "0" : ""), str, hex(arr[i])) end
        return str
    end
end 
