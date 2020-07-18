#version 460 core
layout (location = 0) in vec4 InputPosition;
layout (location = 1) in vec2 InputTextureCoordinates;
out vec2 TextureCoordinates;

layout (location = 20) uniform mat4 ModelView;

void main(void)
{
    gl_Position = ModelView * InputPosition;
    TextureCoordinates = InputTextureCoordinates;
}