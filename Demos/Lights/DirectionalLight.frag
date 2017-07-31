﻿#version 330 core

uniform mat4 MV; // model view matrix
uniform vec3 lightDirection; // light direction in model space
uniform vec3 diffuseColor; // diffuse color of surface
uniform vec3 ambientColor = vec3(0.2, 0.2, 0.2);

// inputs from vertex shader
smooth in vec3 vEyeSpaceNormal; // interpolated normal in eye space

layout (location = 0) out vec4 vFragColor; // fargment shader output

void main()
{
	vec3 vEyeSpaceLightDirection = (MV * vec4(lightDirection)).xyz;
	vec3 L = normalize(vEyeSpaceLightDirection.xyz); // light vector

	float diffuse = max(0, dot(vEyeSpaceNormal, L));
	if (vEyeSpaceNormal != normalize(vEyeSpaceNormal)) { diffuse = 1; }

	vFragColor = vec4(ambientColor + diffuse * diffuseColor, 1.0);
}