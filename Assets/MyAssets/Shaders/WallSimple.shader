Shader "MyShaders/WallSimple"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Brightness("Brightness", Range(0,1)) = 0.2
		_Stregth("Stregth", Range(0,1)) = 0.2
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }

        Pass//Model
        {
			Stencil{
				Ref 1
				Comp NotEqual
				ReadMask 255
				Fail IncrSat
			}
			
			//ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			
			sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Color;
			float _Brightness;
			float _Stregth;

            struct Meshdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct Interpolators{
                float2 uv : TEXCOORD0;
                float3 normal: TEXCOORD1;
                float4 vertex : SV_POSITION;
            };
			
			float diffuse(float3 normal,float3 lightPos){
				float3 norm = normalize(normal);
				float3 lightDir = normalize(lightPos);
				
				return max(dot(norm,lightDir),0.0);
			}

            Interpolators vert (Meshdata v){
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            float4 frag (Interpolators i) : SV_Target{
                float4 col = tex2D(_MainTex, i.uv);
                if(col.a < 0.1)
                    discard;

				col *=  _Stregth * _Color + diffuse(i.normal,_WorldSpaceLightPos0) + _Brightness;
                return col;
            }
            ENDCG
        }
		
		Pass{//Malla
			Tags { "Queue"="Transparent" }
			Stencil{
				Ref 2
				Comp Equal
				ReadMask 255
			}
			
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			
			#define TAU 6.2831
			
			sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Color;

            struct Meshdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float2 screenPos : TEXCOORD2;
            };

            struct Interpolators{
				float2 screen : TEXCOORD2;
				float4 vertex : SV_POSITION;         
            };

            

            Interpolators vert (Meshdata v){
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.screen = v.screenPos;
                return o;
            }

            float4 frag (Interpolators i) : SV_Target{	
                return _Color;
            }
            ENDCG
		}
    }
}