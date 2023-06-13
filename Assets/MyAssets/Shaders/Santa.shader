Shader "MyShaders/Santa"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_LineColor ("Outline Color", Color) = (1,1,1,1)
    }
    SubShader
    {
		Pass//Model
        {
			Tags{
				"Queue"="Geometry"
			}
			
			Stencil{
				Ref 1
				Comp Always
				WriteMask 255
				Pass Replace
				ZFail Keep
				Fail Keep
			}
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			
			sampler2D _MainTex;
            float4 _MainTex_ST;

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
			
            v2f vert (MeshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
				
                return col;
            }
            ENDCG
        }
		
		Pass//Outline
        {
			Tags{
				"Queue"="Geometry"
			}
			
			Stencil{
				Ref 1
				Comp NotEqual
				ReadMask 255
				WriteMask 255
				Pass Replace
				ZFail Keep
				Fail Keep
			}
			
			//ZWrite Off
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			
			sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _LineColor;

            struct MeshData
            {
                float4 vertex : POSITION;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
            };
			
			float4 outline(float4 vertexPos){
				const float outValue = 0.2;
				
				const float4x4 scale = float4x4(
					1 + outValue, 0, 0, 0,
					0, 1 + outValue, 0, 0,
					0, 0, 1 + outValue, 0,
					0, 0, 0, 1 + outValue
				);
				
				return mul(scale,vertexPos);
			}

            Interpolators vert (MeshData v)
            {
                Interpolators o;
				float4 vPos = outline(v.vertex);
                o.vertex = UnityObjectToClipPos(vPos);
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                return _LineColor;
            }
            ENDCG
        }
        
    }
}
