texture Texture1;

sampler RenderMap = sampler_state {
	texture = <Texture1>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

float MouseX;
float MouseY;

float Red;
float Green;
float Blue;

float Radius;

bool canPaint;

struct VertexShaderInput
{
	float4 Position : POSITION;
	float2 TextureCoordinate: TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION;
	float2 TextureCoordinate: TEXCOORD0;
};

VertexShaderOutput RenderVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output;
	output.TextureCoordinate = input.TextureCoordinate;
	output.Position = input.Position;
	return output;
}

float4 RenderPixelShader(VertexShaderOutput input) : COLOR0
{
	if (distance(input.TextureCoordinate, float2(MouseX / 200, MouseY / 150)) < Radius && canPaint == true)
	{
		return float4(Red, Green, Blue, 1);
	}
	float4 color1 = tex2D(RenderMap, input.TextureCoordinate);
	color1.a = 1;
	return color1;
}

technique Render
{
	pass P0
	{
		VertexShader = compile vs_4_0 RenderVertexShader();
		PixelShader = compile ps_4_0 RenderPixelShader();
	}
}