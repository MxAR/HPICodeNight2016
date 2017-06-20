const mlum = Math.sqrt(3*Math.pow(255, 2))

function gencomp(V, cc = 0.4) {
    var lumV = norm(V) / mlum
    var O = rotvec(V.map(function(x) {return x-127.5}), 90, ouvec(V))
        .map(function(x){ return Math.round(Math.min(127.5, Math.max(-127.5, x))+127.5)})
    while (Math.abs(lumV-(norm(O) / mlum)) < cc) {O = O.map(function(x){return x*(lumV>0.5?.5:1.5)})}
    //console.log(ndist(V, O))
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

function rand_rgb() {
    return ([0, 0, 0]).map(function (x) { return Math.abs(Math.round(Math.random() * 255.0)) })
}

function ouvec(V) {
    var W = ([0, 0, 0]).map(function(x){return Math.random()})
    if (Math.max.apply(V) != 0) { W[2] = (V[0] * W[0] + V[1] * W[1]) / (-1 * (V[2] == 0 ? 1 : V[2])) }
    return W.map(function (x) { return x / norm(W)})
}

function norm(V) {
    return Math.sqrt(Math.pow(V[0], 2) + Math.pow(V[1], 2) + Math.pow(V[2], 2))
}

function dot(P, Q) {
    return P[0] * Q[0] + P[1] * Q[1] + P[2] * Q[2]
}

function log(x, b) {
    return Math.log(x)/Math.log(b)
}

//function ndist(u, v) {
//    u = u.map(function(x){return x/norm(u)})
//    v = v.map(function(x){return x/norm(v)})
//    return Math.sqrt(Math.pow(u[0]-v[0], 2)+Math.pow(u[1]-v[1], 2)+Math.pow(u[2]-v[2], 2))/Math.sqrt(3)
//}