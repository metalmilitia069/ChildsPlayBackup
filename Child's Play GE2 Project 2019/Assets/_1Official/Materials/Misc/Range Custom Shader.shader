// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Starting point of script:
// https://forum.unity.com/threads/approach-for-displaying-intersection-between-plane-and-sphere.344079

Shader "Custom/Intersection Additive" {

	Properties{
		_Color("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Main Texture", 2D) = "white" {}
		_BumpMap("Normals ", 2D) = "bump" {}
		_EdgeFade("Edge Fade Factor", Range(0.01,1.0)) = 0.5
		_Distort("Distort Factor", Range(0.0,1.0)) = 0.05
		_Displace("Diplace Factor", Range(0.1,100.0)) = 0.5
		_BumpDirection("Bump Direction & Speed", Vector) = (1.0 ,1.0, -1.0, 1.0)
	}

	Category{

		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			//ColorMask RGB
			Cull Off 
			Lighting Off 
			ZWrite Off 
			//ZTest LEqual
			
		SubShader
		{
			Tags
			{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}

			GrabPass
			{
				"_BumpMap"
			}

			Pass{
				CGPROGRAM
				#include "UnityCG.cginc"
								
				#pragma vertex vert
				#pragma fragment frag

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _Color;
				sampler2D _BumpMap;
				float4 _BumpDirection;
				sampler2D_float _CameraDepthTexture;
				float _EdgeFade;
				float _Distort;
				float _Displace;

				struct v2f {
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					float4 grabPos : TEXCOORD1;
					float3 normal : NORMAL;
				};

				v2f vert(appdata_full v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord.xyzw = (_Time.xxxx * _BumpDirection.xyzw);
					o.texcoord.xy += TRANSFORM_TEX(v.texcoord,_MainTex);
					o.grabPos = ComputeScreenPos(o.vertex);
					COMPUTE_EYEDEPTH(o.grabPos.z);
					o.color = v.color;
					o.normal = UnityObjectToWorldNormal(v.normal);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.grabPos)));
					float partZ = i.grabPos.z;
					float fade = saturate(_EdgeFade * (sceneZ - partZ));
					_Color.a += saturate(1 - fade);

					fixed4 col = 2.0f * i.color * _Color *tex2D(_MainTex, i.texcoord);
					float4 offset = tex2D(_MainTex, i.texcoord * _Distort);
					i.grabPos.xy -= offset.xy * _Displace;
					col *= tex2Dproj(_BumpMap, i.grabPos);
					return col;
				}
				ENDCG
			}
		}
	}
}