Shader "MyShaders/Gift"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color",Color) = (1,1,1,1)
        _Brightness ("Brightness", Range(0,1)) = 0.1
        _isRotate ("Rotacion",float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Geometry+1" }
		
		Stencil{
			Ref 1
			Comp Always
			WriteMask 255
			Pass Replace
			ZFail Keep
			Fail Keep
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			
			sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Color;
            float _Brightness;
            uniform float _isRotate;

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
			
			float3 rotateVertex(float3 vertexPos,float angle){
				float s = sin(angle);
				float c = cos(angle);
				
				float3x3 y_rotate = float3x3(
											c, -s, 0,
											s, c, 0,
											0, 0, 1
											);
				
				return mul(y_rotate,vertexPos);
			}
            

            v2f vert (appdata v)
            {
                v2f o;
                if(_isRotate == 1)
				    v.vertex.xyz = rotateVertex(v.vertex.xyz,_Time * 30.0);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv)*_Color + _Brightness;
					
                return col;
            }
            ENDCG
        }
    }
}
