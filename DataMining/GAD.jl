# CosAngle Median: 0.7824274226624149
# CosAngle Sigma: 0.1652309091634334 
# CosAngle Sigma Intervall: 0.6171965134989815 <> 0.9476583318258484

module GAD
    Pkg.add("Cairo")
    Pkg.add("JSON")
    Pkg.add("Gadfly")
    Pkg.add("Fontconfig")

    using JSON 
    using Gadfly

    include("Convert.jl")

    function GetData()
        getURL(url) = open(readlines, download(url))
        response = JSON.parse(getURL("https://randoma11y.com/combos/top")[1])
        result = zeros(100)

        for i = 1:100
            iv = Convert.ColorHex2Vector(response[i]["color_one"])
            ov = Convert.ColorHex2Vector(response[i]["color_two"])
            result[i] = min(dot(iv, ov) / (vecnorm(iv) * vecnorm(ov)), 1.0)
        end 

        median = sum(result) / 100
        sigma = 0 

        for i = 1:100 sigma += (result[i] - median)^2 end 
        sigma = sqrt((sigma/100))

        nad(x) = ((1/((sqrt(2*pi)) * sigma))) * exp(-((x - median)^2/(2*(sigma)^2)))
        p = plot([nad], -10, 10)
        draw(PNG("/home/mxar/Documents/GIT_REPOs/HPICodeNight2016/DataMining/normal_angle_distribution.png", 12inch, 9inch), p)

        println(string("Median: ", median, " Sigma: ", sigma, " Sigmaintervall: ", median-sigma, ":", median+sigma))
    end
end  