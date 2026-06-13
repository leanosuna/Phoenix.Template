#version 410 core
out vec4 oColor;

uniform vec3 Color;

void main()
{
    oColor = vec4(Color,1);
}