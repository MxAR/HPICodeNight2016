module GAD
    import JSON
    include("Convert.jl");

    function GetData()
        getURL(url) = open(readlines, download(url))
        result = [256^3]

        for x = 0:0
            for y = 0:0 
                for z = 0:0
                    iv = [x, y, z]
                    response = JSON.parse(getURL(string("https://randoma11y.com/combos?hex=", Convert.Vector2Hex(iv)))[1])
                    return response
                    # acos(dot(iv, b) / (norm(iv) * norm(b)))
                end 
            end 
        end

        # return result
    end
end  