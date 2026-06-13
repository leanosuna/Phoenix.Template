#version 410 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec2 vTexCoords;
layout (location = 2) in vec3 vNormal;

uniform mat4 World;

layout(std140) uniform CommonData
{
    mat4 sView;
    mat4 sProjection;
    float sTime;
    float sDeltaTime;
};


out vec2 texCoords;
void main()
{
    texCoords = vTexCoords;    
    
    vec3 pos = vPos;
    vec4 world = World * vec4(pos, 1);
    
    gl_Position = sProjection * sView * world;
}