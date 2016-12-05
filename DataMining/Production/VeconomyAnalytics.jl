# Pkg.add("JSON")
# Pkg.add("ParallelAccelerator")

module VeconomyAnalytics
    using JSON
    using ParallelAccelerator

    function Run() 
        if !isfile("/home/mxar/Documents/GIT_REPOs/Veconomy/DataMining/Production/raw_data.json") GetAPIData() end 
        IO = open("/home/mxar/Documents/GIT_REPOs/Veconomy/DataMining/Production/raw_data.json", "w")
        RD = JSON.parse(IO; dicttype=Associative{Any, 1})
        close(IO)
    end

    function GetAPIData()
        output = []
        getURL(url) = open(readlines, download(url))
        response = JSON.parse(getURL("https://randoma11y.com/combos/top")[1])
        for entity in response push!(output, (ColorHex2Vector(entity["color_one"]), ColorHex2Vector(entity["color_two"]))); end 
        IO = open("/home/mxar/Documents/GIT_REPOs/Veconomy/DataMining/Production/raw_data.json", "w")
        JSON.print{Any, 1}(IO, output)
        close(IO)
    end

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