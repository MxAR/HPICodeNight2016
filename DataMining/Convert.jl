module Convert
    function Hex2Vector(str::String)
        if (isodd(length(str))) str = string(str, str) end
        return convert(Array{Float64}, hex2bytes(str))
    end

    function ColorHex2Vector(str::String)
        return Hex2Vector(strip(str, [ '"', '#' ]))
    end

    function Vector2Hex(arr)
        str = ""
        for i = 1:3 str = string((arr[i] < 16 ? "0" : ""), str, hex(arr[i])) end
        return str
    end
end 
