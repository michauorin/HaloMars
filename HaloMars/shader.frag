#version 460 core
in vec2 TextureCoordinates;
out vec4 FragColor;
uniform sampler2D TextureStorage;

void main(void)
{
    FragColor = texelFetch(TextureStorage, ivec2(TextureCoordinates.x, TextureCoordinates.y),0);
}