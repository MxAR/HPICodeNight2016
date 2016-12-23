const SIG = 15.903165825358679 / 255.0;
const MLUM = L2Norm(new Vector(255, 255, 255));
const COEF = [
    113.32578213409947,
    -0.08078343013936262,
    -36.246805457141605,
    -90.70399420073976,
    57.738600082142526,
    -36.08078343013936
];

function GetCombo(V, P) {
    var Lum = L2Norm(V) / MLUM;
    var RefAngle = (COEF[0] * Math.pow((Lum - COEF[1]), 4)) + (COEF[2] * Math.pow(Lum, 3)) + (COEF[3] * Math.pow(Lum, 2)) + (COEF[4] * (Lum)) + (COEF[5]);
    var ROMA = math.zeros(3, 3);
    var Axis = [ 0, 0, 0 ];
    var OV = [ 0, 0, 0 ];
    var Angle = 0;
    var FT = true;
    var s = 0

    do {
        Angle = RefAngle + Math.exp(Math.pow(Math.random()-0.5, 2) * -7.5) * P * SIG;
        if (FT) { Axis = GetOrthogonalUnitVector(V); FT = false } else {
            var theta = Math.random() * 90;
            var psi = Math.random() * 90;
            Axis[0] = Math.sin(theta) * Math.cos(psi);
            Axis[1] = Math.sin(theta) * Math.sin(psi);
            Axis[2] = Math.cos(theta);
            FT = true;
        }

        ROMA[0, 0] = Math.cos(Angle) + (Math.pow(Axis[0], 2) * (1 - Math.cos(Angle)));            //
        ROMA[0, 1] = (Axis[0] * Axis[1] * (1 - Math.cos(Angle))) - (Axis[2] * Math.sin(Angle));   // X-Row
        ROMA[0, 2] = (Axis[0] * Axis[2] * (1 - Math.cos(Angle))) + (Axis[1] * Math.sin(Angle));   // 

        ROMA[1, 0] = (Axis[0] * Axis[1] * (1 - Math.cos(Angle))) + (Axis[2] * Math.sin(Angle));   // 
        ROMA[1, 1] = Math.cos(Angle) + (Math.pow(Axis[1], 2) * (1 - Math.cos(Angle)));            // Y-Row 
        ROMA[1, 2] = (Axis[1] * Axis[2] * (1 - Math.cos(Angle))) - (Axis[0] * Math.sin(Angle));   //

        ROMA[2, 0] = (Axis[0] * Axis[2] * (1 - Math.cos(Angle))) - (Axis[1] * Math.sin(Angle));   // 
        ROMA[2, 1] = (Axis[1] * Axis[2] * (1 - Math.cos(Angle))) + (Axis[0] * Math.sin(Angle));   // Z-Row
        ROMA[2, 2] = Math.cos(Angle) + (Math.pow(Axis[2], 2) * (1 - Math.cos(Angle)));            //

        OV = math.multiply(V, ROMA);
        s = OV[0] < 0 && OV[1] < 0 && OV[2] < 0 ? -1 : 1;
        OV = OV.map(function(x) { return x * s; });

        if (Math.max(OV) > 255.0) { s = 255 / Math.max(OV);  OV = OV.map(function(x) { return x * s; }); }
        OV = OV.map(function(x) { return Math.max(0, Math.min(x, 255)); });
    } while((DotProduct(V, OV) / (L2Norm(V) * L2Norm(OV))) > 0.7);
    return OV;

}

function GetOrthogonalUnitVector(V) { return [ -1 * V[2], 0, V[0] ]; }

function L2Norm(V) { return Math.sqrt(Math.pow(V[0], 2), Math.pow(V[1], 2), Math.pow(V[2], 2)); }

function DotProduct(P, Q) {
    return [
        P[1] * Q[2] - P[2] * Q[1],
        P[2] * Q[0] - P[0] * Q[2],
        P[0] * Q[1] - P[1] * Q[0]
    ];
}