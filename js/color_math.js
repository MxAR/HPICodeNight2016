const SIG = 15.903165825358679 / 255.0;
const MLUM = L2Norm([255, 255, 255]);
const COEF = [
    108.29450583260494, 
    0.9192165698606374,
    -18.95601236539241,
    -116.97676027964721, 
    150.03360140094875,
    -19.080783430139363
];

function GetCombo(V, P = 255.0) {
    var ROMA = math.zeros(3, 3);
    var Axis = OV = [0, 0, 0];
    var Angle = s = 0;
    var FT = true;

    var W = V.map(function (x) { return Math.abs(x); });                        // 
    if (Math.max(W) == 0) {                                                     // handling of 
        W = W.map(function (x) { return (Math.random() * 255) - 127.5; });      // black color codes
        s = Math.sqrt((W.reduce(function (p, q) { return p + q; }, 0)) / 10);   //
        V = W.map(function (x) { return x / s; });                              //
    }                                                                           //
    console.log(V, W);

    var Lum = L2Norm(V) / MLUM;                                                                                                                             // calculation of 
    var RefAngle = (COEF[0] * Math.pow((Lum - COEF[1]), 4)) + (COEF[2] * Math.pow(Lum, 3)) + (COEF[3] * Math.pow(Lum, 2)) + (COEF[4] * (Lum)) + (COEF[5]);  // the angle 

    do {
        Angle = RefAngle + Math.exp(Math.pow(Math.random() - 0.5, 2) * -7.5) * P * SIG;
        Axis = GetOrthogonalUnitVector(V);

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

        s = OV[0] < 0 && OV[1] < 0 && OV[2] < 0 ? -1 : 1;
        OV = OV.map(function (x) { return x * s; });

        s = (MLUM / L2Norm(OV)) * Math.exp(Math.pow(Math.random() - 0, 5, 2) * 1.1);
        OV = OV.map(function (x) { return Math.round(Math.max(0, x * s)); });

        // console.log(V, OV, s, Axis, ROMA["_data"]);
        s = (DotProduct(V, OV) / (L2Norm(V) * L2Norm(OV)));
    } while (0.5 < s && s > 0.7);
    return OV;

}

function RandomRGB() {
    return ([0, 0, 0]).map(function (x) { return Math.round(Math.random() * 255.0); })
}

function GetOrthogonalUnitVector(V) {
    var W = [Math.random() * 10, Math.random() * -10, 0];
    W[2] = (V[0] * W[0] + V[1] * W[1]) / (-1 * (V[2] == 0 ? 1 : V[2]));
    var size = L2Norm(W);
    return W.map(function (x) { return x / size; });
}

function L2Norm(V) {
    return Math.sqrt(Math.pow(V[0], 2) + Math.pow(V[1], 2) + Math.pow(V[2], 2));
}

function DotProduct(P, Q) {
    return P[0] * Q[0] + P[1] * Q[1] + P[2] * Q[2];
}