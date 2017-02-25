const MLUM = L2Norm([255, 255, 255]);

function GetCombo(V) {
    var Axis, OV, OVX, OVY, OVZ, Angle, s
    var lum = L2Norm(V) / MLUM                                                                                                                  
 
    if (Math.max.apply(0, V) == 0) { V = RandomRGB() }                                                                          

    Angle = 50 + (Math.random() * 10)
    Axis = GetOrthogonalUnitVector(V)

    s = L2Norm(V)
    V = V.map(function (x) { return x / s })

    OVX = [
        Math.cos(Angle) + (Math.pow(Axis[0], 2) * (1 - Math.cos(Angle))),           //
        (Axis[0] * Axis[1] * (1 - Math.cos(Angle))) - (Axis[2] * Math.sin(Angle)),  // X-Row
        (Axis[0] * Axis[2] * (1 - Math.cos(Angle))) + (Axis[1] * Math.sin(Angle))   //
    ]

    OVY = [
        (Axis[0] * Axis[1] * (1 - Math.cos(Angle))) + (Axis[2] * Math.sin(Angle)),  //
        Math.cos(Angle) + (Math.pow(Axis[1], 2) * (1 - Math.cos(Angle))),           // Y-Row
        (Axis[1] * Axis[2] * (1 - Math.cos(Angle))) - (Axis[0] * Math.sin(Angle))   //
    ]

    OVZ = [
        (Axis[0] * Axis[2] * (1 - Math.cos(Angle))) - (Axis[1] * Math.sin(Angle)),  //
        (Axis[1] * Axis[2] * (1 - Math.cos(Angle))) + (Axis[0] * Math.sin(Angle)),  // Z-Row
        Math.cos(Angle) + (Math.pow(Axis[2], 2) * (1 - Math.cos(Angle)))            //
    ]

    OV = [
        DotProduct(OVX, V),
        DotProduct(OVY, V),
        DotProduct(OVZ, V)
    ]

    OV = OV.map(function (x) { return (x - 0.5) })
    s = Math.max.apply(0, OV.map(Math.abs))
    if (s > 0.5) { OV = OV.map(function (x) { return (x / s) * 0.5 }) }
    OV = OV.map(function (x) { return x + 0.5 })
    
    s = L2Norm(OV)
    OV = OV.map(function (x) { return x / s })

    s = Math.abs((1 - lum) + (lum <= 0.4 || lum >= 0.6 ? 0 : (Math.abs(Math.pow(2, -1 * Math.pow(Math.random(), 2)) - 0.5) * (lum >= 0.5 ? -1 : 1))))
    return OV.map(function (x) { return Math.min(255, Math.round(x * s * MLUM)) })
}

function RandomRGB() {
    return ([0, 0, 0]).map(function (x) { return Math.abs(Math.round(Math.random() * 255.0)) })
}

function GetOrthogonalUnitVector(V) {
    var W = [Math.random() * 10, Math.random() * -10, 0]
    W[2] = (V[0] * W[0] + V[1] * W[1]) / (-1 * (V[2] == 0 ? 1 : V[2]))
    var size = L2Norm(W)
    return W.map(function (x) { return x / size; })
}

function L2Norm(V) {
    return Math.sqrt(Math.pow(V[0], 2) + Math.pow(V[1], 2) + Math.pow(V[2], 2))
}

function DotProduct(P, Q) {
    return P[0] * Q[0] + P[1] * Q[1] + P[2] * Q[2]
}