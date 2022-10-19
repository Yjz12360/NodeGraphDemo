
function add(nX1, nY1, nZ1, nX2, nY2, nZ2)
    return nX1 + nX2, nY1 + nY2, nZ1 + nZ2
end

function sub(nX1, nY1, nZ1, nX2, nY2, nZ2)
    return nX1 - nX2, nY1 - nY2, nZ1 - nZ2
end

function mul(nX, nY, nZ, nScalar)
    return nX * nScalar, nY * nScalar, nZ * nScalar
end

function dot(nX1, nY1, nZ1, nX2, nY2, nZ2)
    return nX1 * nX2 + nY1 * nY2 + nZ1 * nZ2
end

function magnitude(nX, nY, nZ)
    return (nX * nX + nY * nY + nZ * nZ) ^ 0.5
end

function sqrMagnitude(nX, nY, nZ)
    return nX * nX + nY * nY + nZ * nZ
end

function normalize(nX, nY, nZ)
    local nMagnitude = VectorUtil.magnitude(nX, nY, nZ)
    return nX / nMagnitude, nY / nMagnitude, nZ / nMagnitude
end

function rad2Vec(nRad)
    return math.cos(nRad), 0, math.sin(nRad)
end