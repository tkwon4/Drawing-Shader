float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;


texture NewTexture;

sampler tsampler1 = sampler_state {
	texture = <NewTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate: TEXCOORD0;
	float4 Normal: NORMAL;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate: TEXCOORD0;
	float4 Position2D : TEXCOORD1;
	float3 Normal: TEXCOORD2;
};

VertexShaderOutput DepthMapVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output;
	output.Position = mul(mul(mul(input.Position, World), View), Projection);
	output.Position2D = output.Position;
	output.Normal = normalize(mul(input.Normal, WorldInverseTranspose).xyz);
	output.TextureCoordinate = input.TextureCoordinate;
	return output;
}

float4 DepthMapPixelShader(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(tsampler1, input.TextureCoordinate);

	return color;
}

technique DepthMap
{
	pass P0
	{
		VertexShader = compile vs_4_0 DepthMapVertexShader();
		PixelShader = compile ps_4_0 DepthMapPixelShader();
	}
}
