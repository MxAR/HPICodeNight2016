const mlum = Math.sqrt(3*Math.pow(255, 2))

function gencomp(V, cc = 0.4) {
    var lumV = norm(V) / mlum
    var O = rotvec(V.map(function(x) {return x-127.5}), 90, ouvec(V))
        .map(function(x){ return Math.min(127.5, Math.max(-127.5, x))+127.5})
    while (Math.abs(lumV-(norm(O) / mlum)) < cc) {
        O = O.map(function(x){return x*(lumV>0.5?.5:1.5)})
    }
    return O.map(function(x){return Math.min(255, Math.max(0, Math.round(x)))})
}

function rotvec(V, Angle, Axis) {
    var RX = [
        Math.cos(Angle) + (Math.pow(Axis[0], 2) * (1 - Math.cos(Angle))),           //
        (Axis[0] * Axis[1] * (1 - Math.cos(Angle))) - (Axis[2] * Math.sin(Angle)),  // X-Row
        (Axis[0] * Axis[2] * (1 - Math.cos(Angle))) + (Axis[1] * Math.sin(Angle))   //
    ]

    var RY = [
        (Axis[0] * Axis[1] * (1 - Math.cos(Angle))) + (Axis[2] * Math.sin(Angle)),  //
        Math.cos(Angle) + (Math.pow(Axis[1], 2) * (1 - Math.cos(Angle))),           // Y-Row
        (Axis[1] * Axis[2] * (1 - Math.cos(Angle))) - (Axis[0] * Math.sin(Angle))   //
    ]

    var RZ = [
        (Axis[0] * Axis[2] * (1 - Math.cos(Angle))) - (Axis[1] * Math.sin(Angle)),  //
        (Axis[1] * Axis[2] * (1 - Math.cos(Angle))) + (Axis[0] * Math.sin(Angle)),  // Z-Row
        Math.cos(Angle) + (Math.pow(Axis[2], 2) * (1 - Math.cos(Angle)))            //
    ]

    return [dot(RX, V), dot(RY, V), dot(RZ, V)]
}

function rand_rgb(l = 3) {
    return (new Array(l).fill(0)).map(function (x) { return Math.abs(Math.round(Math.random()*255)) })
}

function ouvec(V) {
    var W = ([0, 0, 0]).map(function(x){return Math.random()})
    if (Math.max.apply(V) != 0) { W[2] = (V[0] * W[0] + V[1] * W[1]) / (-1 * (V[2] == 0 ? 1 : V[2])) }
    return W.map(function (x) { return x / norm(W)})
}

function norm(V, p = 2) {
    return Math.pow((V.map(function(x){return Math.pow(x, p)}).reduce(function(x, s){return x+s}, 0)), 1/p)
}

function dot(P, Q) {
    var s = 0
    for (i = 0; i < P.length; i++){s += P[i] * Q[i]}
    return s
}
