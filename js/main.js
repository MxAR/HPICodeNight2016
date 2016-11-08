// 2016 HPI Code Night || Veconomy Main JS

// GEN SHADES HTML

for(var i = 0; i < 2; i++) {
	var title0 = document.createElement("p");
	title0.className = 'title';
	var title0Conent = document.createTextNode("Lighter Shades");
	title0.appendChild(title0Conent);
	document.getElementById('color' + i).appendChild(title0);
	var wrapper0 = document.createElement("div");
	wrapper0.className = 'wrapper';
	for(var j = 9; j >= 0; j--) {
		var link = document.createElement("a");
		link.href = 'javascript:search(' + "'" + i + j + "', true" + ')';
		var shadeBox = document.createElement("div");
		shadeBox.className = 'shadeBox';
		link.appendChild(shadeBox);
		var colorBox = document.createElement("div");
		colorBox.className = 'colorBox color' + i + j;
		shadeBox.appendChild(colorBox);
		var textColor = document.createElement("p");
		textColor.id = 'textColor' + i + j;
		shadeBox.appendChild(textColor);
		wrapper0.appendChild(link);
	}
	document.getElementById('color' + i).appendChild(wrapper0);
	
	var title1 = document.createElement("p");
	title1.className = 'title';
	var title1Conent = document.createTextNode("Darker Shades");
	title1.appendChild(title1Conent);
	document.getElementById('color' + i).appendChild(title1);
	var wrapper1 = document.createElement("div");
	wrapper1.className = 'wrapper';
	for(var j = 11; j <= 20; j++) {
		var link = document.createElement("a");
		link.href = 'javascript:search(' + "'" + i + j + "', true" + ')';
		var shadeBox = document.createElement("div");
		shadeBox.className = 'shadeBox';
		link.appendChild(shadeBox);
		var colorBox = document.createElement("div");
		colorBox.className = 'colorBox color' + i + j;
		shadeBox.appendChild(colorBox);
		var textColor = document.createElement("p");
		textColor.id = 'textColor' + i + j;
		shadeBox.appendChild(textColor);
		wrapper1.appendChild(link);
	}
	document.getElementById('color' + i).appendChild(wrapper1);
}

function cutHex(h) {
  	return (h.charAt(0) == "#") ? h.substring(1, 7) : h
}

function hexToRgb(h) {
	var r = parseInt((cutHex(h)).substring(0, 2), 16);
	var g = parseInt((cutHex(h)).substring(2, 4), 16);
	var b = parseInt((cutHex(h)).substring(4, 6), 16);
	return [r,g,b];
}

function rgbToHsv (r,g,b) {
	var computedH = 0;
	var computedS = 0;
	var computedV = 0;

	//remove spaces from input RGB values, convert to int
	var r = parseInt( (''+r).replace(/\s/g,''),10 ); 
	var g = parseInt( (''+g).replace(/\s/g,''),10 ); 
	var b = parseInt( (''+b).replace(/\s/g,''),10 ); 

	if ( r==null || g==null || b==null ||
		isNaN(r) || isNaN(g)|| isNaN(b) ) {
		console.log ('Please enter numeric RGB values!');
		return;
	}
 	if (r<0 || g<0 || b<0 || r>255 || g>255 || b>255) {
		console.log ('RGB values must be in the range 0 to 255.');
    	return;
	}
	r=r/255; g=g/255; b=b/255;
	var minRGB = Math.min(r,Math.min(g,b));
	var maxRGB = Math.max(r,Math.max(g,b));

 	// Black-gray-white
 	if (minRGB==maxRGB) {
  		computedV = minRGB;
  		return [0,0,computedV];
	}

	// Colors other than black-gray-white:
	var d = (r==minRGB) ? g-b : ((b==minRGB) ? r-g : b-r);
	var h = (r==minRGB) ? 3 : ((b==minRGB) ? 1 : 5);
	computedH = 60*(h - d/(maxRGB - minRGB));
	computedS = (maxRGB - minRGB)/maxRGB;
	computedV = maxRGB;
	return [computedH,computedS,computedV];
}

function lum(hex, lum) {

	// validate hex string
	hex = String(hex).replace(/[^0-9a-f]/gi, '');
	if (hex.length < 6) {
		hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
	}
	lum = lum || 0;

	// convert to decimal and change luminosity
	var newHex = "#", c, i;
	for (i = 0; i < 3; i++) {
		c = parseInt(hex.substr(i * 2, 2), 16);
		c = Math.round(Math.min(Math.max(0, c + (c * lum)), 255)).toString(16);
		newHex += ("00" + c).substr(c.length);
	}

	return newHex;
}

function hsvCut (hsv) {
	hsv = hsv * 100;
	hsv = Math.round(hsv);
	return hsv + '%';
}

function hexToCMYK (hex) {
	var computedC = 0;
	var computedM = 0;
	var computedY = 0;
	var computedK = 0;

	hex = (hex.charAt(0)=="#") ? hex.substring(1,7) : hex;

	if (hex.length != 6) {
	alert ('Invalid length of the input hex value!');   
	return; 
	}
	if (/[0-9a-f]{6}/i.test(hex) != true) {
	alert ('Invalid digits in the input hex value!');
	return; 
	}

	var r = parseInt(hex.substring(0,2),16); 
	var g = parseInt(hex.substring(2,4),16); 
	var b = parseInt(hex.substring(4,6),16); 

	// BLACK
	if (r==0 && g==0 && b==0) {
	computedK = 1;
	return [0,0,0,1];
	}

	computedC = 1 - (r/255);
	computedM = 1 - (g/255);
	computedY = 1 - (b/255);

	var minCMY = Math.min(computedC,Math.min(computedM,computedY));

	computedC = (computedC - minCMY) / (1 - minCMY) ;
	computedM = (computedM - minCMY) / (1 - minCMY) ;
	computedY = (computedY - minCMY) / (1 - minCMY) ;
	computedK = minCMY;

	return [computedC,computedM,computedY,computedK];
}

var colorOne = '#FFFFFF';
var colorTwo = '#000000';

function hexSearch() {
	var hex = $('#hexSearch').val().replace(/\#/g, '');
	if (hex.length == 0 || hex.length == 3 || hex.length == 6) {

		var re = /[0-9A-Fa-f]{6}/g;
		var re2 = /[0-9A-Fa-f]{3}/g;

		if(re.test(hex) || re2.test(hex) || hex.length == 0) {
		    $(".notification").css('display', 'none');
			if(hex.length == 3) {
				hex = hex.split('');
				hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
			}
		} else {
		    $('#notificationText').html('The thing you typed into the input is not a hex code, please check and write a valid hex code. Note: You dont have to write a hex to get results.');
			$(".notification").css('display', 'block');
			return;
		}
	} else {
		$('#notificationText').html('The thing you typed into the input is not a hex code, please check and write a valid hex code. Note: You dont have to write a hex to get results.');
		$(".notification").css('display', 'block');
		return;
	}

	// var color0 = '#7822c8';
	// var color1 = '#99fd59';

	// colorOne = color0;
	// colorTwo = color1;

	// // Color For Loop
	// for (var i = 0; i < 2; i++) {
	// 	// Background & Text Color Changes
	// 	var color = i == 0 ? color0 : color1;
	// 	$('#color' + i + '10').css('background-color', color);
	// 	$('.textColor' + i + '10').css('color', color);
	// 	// HEX Output
	// 	$('#hex' + i).html(color);
	// 	$('#hex' + i).attr('data-clipboard-text', color);
	// 	// RGB Output
	// 	var rgb = hexToRgb(color);
	// 	$('#rgb' + i).html('rgb(' + rgb[0] + ', ' + rgb[1] + ', ' + rgb[2] + ')');
	// 	$('#rgb' + i).attr('data-clipboard-text', 'rgb(' + rgb[0] + ', ' + rgb[1] + ', ' + rgb[2] + ')');
	// 	// HSV Output
	// 	var hsv = rgbToHsv(rgb[0],rgb[1],rgb[2]);
	// 	$('#hsv' + i).html('hsv(' + Math.round(hsv[0]) + ', ' + hsvCut(hsv[1]) + ', ' + hsvCut(hsv[2]) + ')');
	// 	// CYMK Output
	// 	var cymk = hexToCMYK(color);
	// 	$('#cymk' + i).html('cymk(' + cymk[0].toFixed(4) + ', ' + cymk[1].toFixed(4) + ', ' + cymk[2].toFixed(4) + ', ' + cymk[3].toFixed(4) + ')');

	// 	// Tone For Loop
	// 	for (var j = 0; j < 21; j++) {
	// 		var shade = (lum(color, ((j * -0.1) + 1))).toUpperCase();
	// 		if (j !== 10) {
	// 			$('.color' + i + j).css('background-color', shade);
	// 			$('#textColor' + i + j).html(shade);
	// 		}
	// 	}
	// }

	$.ajax({
		type: "GET",
		url: "http://localhost:5000/api/color",
		data: {IC: hex},
		success: function(response){

			var color0 = '#' + response[0].toUpperCase();
			var color1 = '#' + response[1].toUpperCase();

			colorOne = color0;
			colorTwo = color1;

			// Color For Loop
			for (var i = 0; i < 2; i++) {
				// Background & Text Color Changes
				var color = i == 0 ? color0 : color1;
				$('#color' + i + '10').css('background-color', color);
				$('.textColor' + i + '10').css('color', color);
				// HEX Output
				$('#hex' + i).html(color);
				$('#hex' + i).attr('data-clipboard-text', color);
				// RGB Output
				var rgb = hexToRgb(color);
				$('#rgb' + i).html('rgb(' + rgb[0] + ', ' + rgb[1] + ', ' + rgb[2] + ')');
				$('#rgb' + i).attr('data-clipboard-text', 'rgb(' + rgb[0] + ', ' + rgb[1] + ', ' + rgb[2] + ')');
				// HSV Output
				var hsv = rgbToHsv(rgb[0],rgb[1],rgb[2]);
				$('#hsv' + i).html('hsv(' + Math.round(hsv[0]) + ', ' + hsvCut(hsv[1]) + ', ' + hsvCut(hsv[2]) + ')');
				// CYMK Output
				var cymk = hexToCMYK(color);
				$('#cymk' + i).html('cymk(' + cymk[0].toFixed(4) + ', ' + cymk[1].toFixed(4) + ', ' + cymk[2].toFixed(4) + ', ' + cymk[3].toFixed(4) + ')');

				// Tone For Loop
				for (var j = 0; j < 21; j++) {
					var shade = (lum(color, ((j * -0.1) + 1))).toUpperCase();
					if (j !== 10) {
						$('.color' + i + j).css('background-color', shade);
						$('#textColor' + i + j).html(shade);
					}
				}
			}
		},
		dataType: 'json'
	});

}

function search (colorCode, condition) {
	var color = colorCode.split('');
	var i = color[0] == '0' ? colorOne : colorTwo;
	var j = color.length > 2 ? color[1] + color[2] : color[1];
	var shade = (lum(i, ((j * -0.1) + 1))).toUpperCase();
	if (condition) {
		$('#hexSearch').val(shade);
		hexSearch();
	} else {
		return shade;
	}
}

hexSearch();


