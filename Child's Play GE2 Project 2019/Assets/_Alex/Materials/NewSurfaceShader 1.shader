// Upgrade NOTE: commented out 'float4x4 _CameraToWorld', a built-in variable
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SSDecal2" {
	Properties
	{
		_Color("Tint Color", Color) = (1, 1, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent"  }

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			ZTest Greater
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform sampler2D _CameraDepthTexture;
			sampler2D _MainTex;
			float4 _Color;
			// float4x4 _CameraToWorld;
			float4x4 _Camera2World;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD1;
				float4 projPos : TEXCOORD2;
			};

			float4 _MainTex_ST;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.projPos = ComputeScreenPos(o.pos);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				float2 uv = i.projPos.xy;
				uv.y = 1 - uv.y;

				float z = tex2D(_CameraDepthTexture, uv);
				if (z > 0.99) discard;

				float2 xy = uv * 2 - 1;

				float4 posProjected = float4(xy, z, 1);

				float4 posWS = mul(_Camera2World, posProjected);

				posWS = posWS / posWS.w;


				return tex2D(_MainTex, mul(unity_WorldToObject, posWS).xy);
			}

			ENDCG
		}
	}
		FallBack "VertexLit"
}
