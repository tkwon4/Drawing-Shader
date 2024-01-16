texture Texture1;

sampler PaintedMap = sampler_state {
	texture = <Texture1>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

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

VertexShaderOutput PaintVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output;
	output.TextureCoordinate = input.TextureCoordinate;
	output.Position = input.Position;
	return output;
}

float4 PaintPixelShader(VertexShaderOutput input) : COLOR0
{
	float4 color1 = tex2D(PaintedMap, input.TextureCoordinate);
	color1.a = 1;
	return color1;
}

technique Painted
{
	pass P0
	{
		VertexShader = compile vs_4_0 PaintVertexShader();
		PixelShader = compile ps_4_0 PaintPixelShader();
	}
}