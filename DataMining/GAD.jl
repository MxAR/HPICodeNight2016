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

    include("FORA.jl")
    include("Convert.jl")

    function GetTopData()
        getURL(url) = open(readlines, download(url))
        ProcessData(JSON.parse(getURL("https://randoma11y.com/combos/top")[1]))
    end

    function ProcessData(Response::Array{Any,1})
        h = [0., 0.]
        angle = zeros(100)
        Ibrightness = zeros(100)
        Obrightness = zeros(100)

        for i = 1:100
            iv = Convert.ColorHex2Vector(Response[i]["color_one"])
            ov = Convert.ColorHex2Vector(Response[i]["color_two"])
            angle[i] = acosd(min(dot(iv, ov) / (vecnorm(iv) * vecnorm(ov)), 1.0))
            Ibrightness[i] = RGBBrightness(iv)
            Obrightness[i] = RGBBrightness(ov)
            if angle[i] > h[2] 
                h[1] = Ibrightness[i]
                h[2] = angle[i]
            end
        end 

        println(string("hx: ", median(Ibrightness), " hy: ", h[2]))

        med = median(angle)
        sig = Yamartino(angle)

        p2 = plot(x=1:100, y=(Obrightness+Ibrightness), Geom.point, Geom.line, Geom.smooth)
        draw(PNG("/home/mxar/Documents/GIT_REPOs/Veconomy/DataMining/Images/Brightness_relation.png", 12inch, 9inch), p2)

        p1 = plot(x=Ibrightness, y=angle, Geom.point, Geom.line, Geom.smooth, Guide.xlabel("Brightness of the input color"), Guide.ylabel("Angle of the output color"), Theme(panel_fill=colorant"black"))
        draw(PNG("/home/mxar/Documents/GIT_REPOs/Veconomy/DataMining/Images/angle-variance_brightness_relation.png", 12inch, 9inch), p1)

        p0 = plot([nad(x) = (med/cosd(sig)) * exp(-((x - cosd(med))^2 / (2*(cosd(sig))^2)))], -10, 10, Guide.xlabel("Brightness of the input color"), Guide.ylabel("Angle of the output color"), Theme(panel_fill=colorant"black"))
        draw(PNG("/home/mxar/Documents/GIT_REPOs/Veconomy/DataMining/Images/angle_distribution.png", 12inch, 9inch), p0)

        push!(angle, 50); push!(angle, 50);
        push!(Ibrightness, 0); push!(Ibrightness, 1);
        # FORA.Run(Ibrightness, angle, 100000, 0.2, 20)

        println(string("Median: ", med, " Sigma: ", sig, " Sigmaintervall: ", med-sig, " <> ", med+sig))
    end

    function Yamartino(arr::Array{Float64})
        sa = 0.
        ra = 0.
        ca = length(arr)
        for i = 1:ca
            sa += sind(arr[i]) / ca
            ra += cosd(arr[i]) / ca
        end
        eps = sqrt(1-(sa^2 + ra^2))
        return asind(eps) * (1 + ((2/sqrt(3)) - 1) * eps^3)
    end

    function RGBBrightness(arr::Array{Float64})
        return vecnorm(arr) / sqrt(3*(255^2))
    end
end  