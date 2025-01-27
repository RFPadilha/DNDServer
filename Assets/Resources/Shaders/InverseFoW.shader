Shader "Custom/InverseFoW"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Textura do mapa, se necessário
        _FogColor ("Fog Color", Color) = (0, 0, 0, 1) // Cor da área sem visão (escura)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" }
        Pass
        {
            ZTest Always
            Cull Off
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha // Permite transparência

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _FogColor; // Cor da área sem visão

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Aqui desenhamos a cor da fog nas áreas onde não há visão
                return _FogColor;
            }
            ENDCG
        }
    }
}
