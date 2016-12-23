const SIG = 15.903165825358679 / 255.0;
const MLUM = L2Norm([255, 255, 255]);
const COEF = [
    113.32578213409947,
    -0.08078343013936262,
    -36.246805457141605,
    -90.70399420073976,
    57.738600082142526,
    -36.08078343013936
];

function GetCombo(V, P = 255.0) {
    var ROMA = math.zeros(3, 3);
    var Axis = OV = [ 0, 0, 0 ];
    var Angle = s = 0;
    var FT = true;

    console.log(V);

    var W = V.map(function(x) { return Math.abs(x); });                         // 
    if (Math.max(W) == 0) {                                                     // handling of 
        W = W.map(function(x) { return (Math.random() * 255) - 127.5; });       // black color codes
        s = Math.sqrt((W.reduce(function(p, q) { return p + q; }, 0)) / 10);    //
    } 

    console.log(V);

    var Lum = L2Norm(V) / MLUM;                                                                                                                             // calculation of 
    var RefAngle = (COEF[0] * Math.pow((Lum - COEF[1]), 4)) + (COEF[2] * Math.pow(Lum, 3)) + (COEF[3] * Math.pow(Lum, 2)) + (COEF[4] * (Lum)) + (COEF[5]);  // the angle 

    console.log(RefAngle);

    do {
        Angle = RefAngle + Math.exp(Math.pow(Math.random()-0.5, 2) * -7.5) * P * SIG;
        if (FT) { Axis = GetOrthogonalUnitVector(V); FT = false } else {
            var theta = Math.random() * 90;                                 //
            var psi = Math.random() * 90;                                   //
            Axis[0] = Math.sin(theta) * Math.cos(psi);                      // manual generation 
            Axis[1] = Math.sin(theta) * Math.sin(psi);                      // of a random axis
            Axis[2] = Math.cos(theta);                                      //
            FT = true;                                                      // 
        }

        console.log(Axis);

        ROMA.subset(math.index(0, 0), Math.cos(Angle) + (Math.pow(Axis[0], 2) * (1 - Math.cos(Angle))));            //
        ROMA.subset(math.index(0, 1), (Axis[0] * Axis[1] * (1 - Math.cos(Angle))) - (Axis[2] * Math.sin(Angle)));   // X-Row
        ROMA.subset(math.index(0, 2), (Axis[0] * Axis[2] * (1 - Math.cos(Angle))) + (Axis[1] * Math.sin(Angle)));   // 

        ROMA.subset(math.index(1, 0), (Axis[0] * Axis[1] * (1 - Math.cos(Angle))) + (Axis[2] * Math.sin(Angle)));   // 
        ROMA.subset(math.index(1, 1), Math.cos(Angle) + (Math.pow(Axis[1], 2) * (1 - Math.cos(Angle))));            // Y-Row 
        ROMA.subset(math.index(1, 2), (Axis[1] * Axis[2] * (1 - Math.cos(Angle))) - (Axis[0] * Math.sin(Angle)));   //

        ROMA.subset(math.index(2, 0), (Axis[0] * Axis[2] * (1 - Math.cos(Angle))) - (Axis[1] * Math.sin(Angle)));   // 
        ROMA.subset(math.index(2, 1), (Axis[1] * Axis[2] * (1 - Math.cos(Angle))) + (Axis[0] * Math.sin(Angle)));   // Z-Row
        ROMA.subset(math.index(2, 2), Math.cos(Angle) + (Math.pow(Axis[2], 2) * (1 - Math.cos(Angle))));            //

        OV = math.multiply(V, ROMA)["_data"];
        console.log(OV, ROMA);
        s = OV[0] < 0 && OV[1] < 0 && OV[2] < 0 ? -1 : 1;
        OV = OV.map(function(x) { return x * s; });

        if (Math.max(OV) > 255.0) { s = 255 / Math.max(OV);  OV = OV.map(function(x) { return x * s; }); }
        OV = OV.map(function(x) { return Math.round(Math.max(0, Math.min(x, 255))); });
    } while((DotProduct(V, OV) / (L2Norm(V) * L2Norm(OV))) > 0.7);
    return OV;

}

function RandomRGB() {
    return [
        Math.round(Math.random() * 255.0),
        Math.round(Math.random() * 255.0),
        Math.round(Math.random() * 255.0)
    ];
}

function GetOrthogonalUnitVector(V) { 
    var W = [ (Math.random() * 20) - 10, (Math.random() * 20) - 10, 0 ];
    W[2] = (V[0] * W[0] + V[1] * W[1]) / (-1 * V[2]);
    var size = L2Norm(V);
    return W.map(function(x) { return x / size; });
}

function L2Norm(V) { 
    return Math.sqrt(Math.pow(V[0], 2), Math.pow(V[1], 2), Math.pow(V[2], 2)); 
}

function DotProduct(P, Q) {
    return P[0]*Q[0] + P[1]*Q[1] + P[2]*Q[2];
}