// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SSDecal"
{

	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	}

	SubShader{

		Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1"}
		ColorMask 0
		ZWrite off

		Stencil {
			Ref 1
			Comp always
			Pass replace
		}

		CGINCLUDE
		#include "UnityCG.cginc"
		sampler2D _MainTex;
		fixed4 _Color;
		struct appdata {
			float4 vertex : POSITION;
		};

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		v2f vert(appdata v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			//o.pos = UnityObjectToViewPos(v.vertex);
			/*float4 projPos = UnityObjectToClipPos(v.vertex);
			float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
			float4 posObj = mul(unity_WorldToObject, posWorld);
			posObj += 0.5;
			o.uv = posObj.xz;
*/
			return o;
		}

		half4 frag(v2f i) : COLOR
		{
			fixed4 c = tex2D(_MainTex, i.uv) * _Color;
			return c;
		}

		ENDCG

		Pass {
			Cull Front
			ZTest Less
		}

		Pass {
			Cull Back
			ZTest Greater
		}

		Pass {
			Tags { "RenderType" = "Opaque" "Queue" = "Geometry+2"}
			ColorMask RGB
			Cull Front
			ZTest Always

			Stencil {
				Ref 1
				Comp notequal
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}
