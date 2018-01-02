@everywhere module veconomy
	##===================================================================================
	## veconomy core function
	##===================================================================================
	function veconomy_core{T<:AbstractFloat}(v::Array{T, 1}, cc::T = 0.4)
		lumV = norm(v) / MAX_LUM
		o = prison(rotmat_3d(rand_orthonormal_vec(v), 90)*(v-127.5), -127.5, 127.5)
		while abs(lumv-(norm(o)/MAX_LUM)) < cc; o = map((x) -> x*(lumV>0.5?.5:1.5), o) end
		return map((x) -> prison(round(x), 0, 255), o)
	end
end
