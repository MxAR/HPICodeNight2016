# P1: -550.6178162693743 
# P2: +0.535864229880747 
# P3: -107.35201910391194 
# P4: +90.49773451405363 
# P5: -11.103030263489412 
# P6: +35.86253191726658

# 4th order regression analysis
module FORA
    function Run(XCO::Array{Float64}, YCO::Array{Float64}, PSI::Int64, LAMBDA,  POPSize::Int64)
    
        epsilon = typemax(Int64)
        MXCO = median(XCO)
        MYCO = median(YCO)
        BAG = []
        POP = []

        for s = 1:POPSize
            push!(POP, [rand((-1*abs(MYCO)):abs(MYCO)), 
                MXCO, rand((-1*abs(MYCO)):abs(MYCO)), 
                rand((-1*abs(MYCO)):abs(MYCO)), 
                rand((-1*abs(MYCO)):abs(MYCO)), 
                MYCO])
        end
        
        for I = 1:PSI
            re = LSError(POP, XCO, YCO)
            if re[2] < epsilon
                epsilon = re[2]
                BAG = POP[convert(Int32, re[1])]
            else epsilon = typemax(Float64) end
            for i = 1:POPSize
                POP[i][1] = BAG[1] + LAMBDA * randn()
                POP[i][3] = BAG[3] + LAMBDA * randn()
                POP[i][4] = BAG[4] + LAMBDA * randn()
                POP[i][5] = BAG[5] + LAMBDA * randn()
            end
        end

        result = min(sqrt(epsilon)/length(XCO), epsilon)
        println(string("RelEPS: ", result, " P1: ", BAG[1]," P2: ", BAG[2], " P3: ", BAG[3]," P4: ", BAG[4], " P5: ", BAG[5], " P6: ", BAG[6])) 

    end

    function LSError(POP::Array{Any}, XCO::Array{Float64}, YCO::Array{Float64})
        BO = 0
        XCOl = length(XCO)
        err = [0., typemax(Float64)]
        for i = 1:length(POP)
            P = POP[i]
            err[1] = 0.
            for j = 1:XCOl
                err[1] += ((P[1] * (XCO[j] - P[2])^4 + P[3] * (XCO[j])^3 + P[4] * (XCO[j])^2 + P[5] * (XCO[j]) + P[6]) - YCO[j])^2 / XCOl
            end
            if err[1] < err[2] 
                err[2] = err[1]
                BO = i
            end
        end
        return [BO, err[2]]
    end
end