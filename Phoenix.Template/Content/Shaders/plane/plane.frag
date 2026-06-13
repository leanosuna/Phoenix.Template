#version 410 core

out vec4 oColor;

in vec2 texCoords;
in vec3 normal;
uniform vec2 Resolution;
layout(std140) uniform CommonData
{
    mat4 sView;
    mat4 sProjection;
    float sTime;
    float sDeltaTime;
};
highp float rand(vec2 co) {
    return fract(sin(mod(dot(co.xy ,vec2(12.9898,78.233)),3.14))*43758.5453);
}
float tnoise(vec2 co){
    vec2 w=co;
    co.y+=co.x/2.;
    const vec2 s=vec2(1.,0.);
	vec2 p=floor(co);
    if(fract(co.x)<fract(co.y))p+=0.5;    
  	return rand(p);
}

void main()
{
    vec2 uv = (texCoords - 0.5) * 80.0;
    float n = tnoise(uv);
    oColor = vec4(sin(sTime * n * 7.0 + n * 3.141592653589793 * 2.0) * 0.5 + 0.5) * 0.3 + 0.5;
    oColor += sin((uv.x - uv.y) * 30.0) / 2.0;
    oColor += rand(uv) / 2.0;
    oColor /= 4.0;
}