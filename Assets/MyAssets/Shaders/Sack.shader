Shader "MyShaders/Sack"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Geometry" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			
			sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Color;

            struct Meshdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators{
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            

            Interpolators vert (Meshdata v){
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (Interpolators i) : SV_Target{
                float4 col = tex2D(_MainTex, i.uv);
                return _Color;
            }
            ENDCG
        }
    }
}
