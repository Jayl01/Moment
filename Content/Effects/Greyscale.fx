sampler inputTexture;

float4 MainPS(float2 UV : TEXCOORD0) : COLOR0
{
    float4 gamePixelColor = tex2D(inputTexture, UV);
    gamePixelColor.b = gamePixelColor.r = gamePixelColor.g;
    return gamePixelColor;
}

technique Techninque1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 MainPS();
        /*AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;*/
    }
};