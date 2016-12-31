# CosAngle Median: 35.86253191726658
# CosAngle Sigma: 15.903165825358679 
# CosAngle Sigma Intervall: 19.959366091907903 <> 51.76569774262526

# IbrightnessRelativeAngle Median: 13.4163407720139
# IbrightnessRelativeAngle Sigma: 11.330263267612782
# IbrightnessRelativeAngle SigmaIntervall: 2.0860775044011177 <> 24.746604039626682

# Pkg.add("Cairo")
# Pkg.add("JSON")
# Pkg.add("Gadfly")
# Pkg.add("Fontconfig")

module GAD
    using JSON 
    using Gadfly

    include("/home/mxar/Documents/GIT_REPOs/SGP/projekte/Libraries/Julia/APL.jl")

    function Fetch()
        container = []
        p = q = zeros(3)
        if !isfile("Data/data.json")
            getURL(url) = open(readlines, download(url))
            for N in JSON.parse(getURL("https://randoma11y.com/combos/top")[1])
                p = APL.HexConvert.ColorHex2Vector(N["color_one"])
                q = APL.HexConvert.ColorHex2Vector(N["color_two"])
                push!(container, [p, q, APL.Vector.Angle(p, q)])
            end
            IO = open("Data/data.json", "w")
            JSON.print(IO, container)
            close(IO)
        else 
            IO = open("Data/data.json")
            container = JSON.parse(IO)
            close(IO)
        end
        return container
    end

    function ProcessData()
        data = Fetch()
        count = length(data)

        s = count * 2
        m = zeros(3, 3)

        for C = 1:3
            Inputs = zeros(s, 3)
            ExpectedResults = zeros(s)
            for (i, n) in enumerate(data) 
                if n[2][3] > 50 
                    ExpectedResults[i] = n[2][2][C] / 255
                    Inputs[i, 1] = n[2][1][1] / 255
                    Inputs[i, 2] = n[2][1][2] / 255
                    Inputs[i, 3] = n[2][1][3] / 255

                    s = i+count
                    ExpectedResults[i+count] = n[2][1][C] / 255
                    Inputs[s, 1] = n[2][2][1] / 255
                    Inputs[s, 2] = n[2][2][2] / 255
                    Inputs[s, 3] = n[2][2][3] / 255
                end
            end
            for (i, n) in enumerate(LAPACK.gelsd!(Inputs, transpose(transpose(ExpectedResults)))[1])
                m[C, i] = n
            end
        end
        
        sample = []
        q = p = zeros(3)
        for i = 1:100
            q = APL.Vector.RandomRGB()
            p = map((x) -> convert(Int, abs(round(x))), *(m, q))
            println(APL.Vector.Angle(q, p), " ",  APL.HexConvert.Vector2Hex(q), " ",  APL.HexConvert.Vector2Hex(p))
            push!(sample, [q, p])
        end
    end

    function TemporaryThreshold(Input, Threshold)
        return Input
    end
end  