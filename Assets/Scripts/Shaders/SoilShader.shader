Shader "Custom/SoilShader" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_Emission("Emission", Color) = (0,0,0,0)

		_TopMainTex("Top Albedo", 2D) = "white" {}
		_TopSmoothness("Top Smoothness", 2D) = "white" {}
		_TopNMap ("Top Normal Map", 2D) = "bump" {}

		_MiddleMainTex("Middle Albedo", 2D) = "white" {}
		_MiddleSmoothness("Middle Smoothness", 2D) = "white" {}
		_MiddleNMap("Middle Normal Map", 2D) = "bump" {}

		_BottomMainTex("Bottom Albedo", 2D) = "white" {}
		_BottomSmoothness("Bottom Smoothness", 2D) = "white" {}
		_BottomNMap("Bottom Normal Map", 2D) = "bump" {}

		_MiddleTop("Top of Middle", float) = 100
		_MiddleBottom("Bottom of Midle", float) = 0
		_OverlapSize("Overlap size", float) = 10

		_TilingSize("Tiling Size", Vector) = (100, 100, 0, 0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _TopMainTex;
		sampler2D _TopSmoothness;
		sampler2D _TopNMap;

		sampler2D _MiddleMainTex;
		sampler2D _MiddleSmoothness;
		sampler2D _MiddleNMap;

		sampler2D _BottomMainTex;
		sampler2D _BottomSmoothness;
		sampler2D _BottomNMap;

		fixed4 _Color;
		half4 _Emission;

		struct Input {
			float2 uv_MainTex;
			float3 worldNormal; INTERNAL_DATA
			float3 worldPos;
		};

		float _MiddleTop;
		float _MiddleBottom;

		float _OverlapSize;

		fixed4 _TilingSize;

		UNITY_INSTANCING_BUFFER_START(Props)

		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float3 cWN = WorldNormalVector(IN, float3(0, 0, 1));
			float xy = IN.worldPos.x + IN.worldPos.y;
			float xz = IN.worldPos.x + IN.worldPos.z;
			float yz = IN.worldPos.y + IN.worldPos.z;
			float2 TexCoord = float2(xz, IN.worldPos.y);
			if (abs(cWN.y) > 0.5) TexCoord = float2(IN.worldPos.z, IN.worldPos.x);
			float2 TexUV = float2(fmod(TexCoord.x, _TilingSize.x) / _TilingSize.x, fmod(TexCoord.y, _TilingSize.y) / _TilingSize.y);

			float halfOverlap = _OverlapSize / 2;

			float top = (IN.worldPos.y - (_MiddleTop - halfOverlap)) / _OverlapSize;
			if (top < 0) top = 0;
			if (top > 1) top = 1;
			float bottom = ((_MiddleBottom + halfOverlap) - IN.worldPos.y) / _OverlapSize;
			if (bottom < 0) bottom = 0;
			if (bottom > 1) bottom = 1;
			float middle = 1;
			if (IN.worldPos.y > _MiddleTop - halfOverlap)
				middle = 1 - top;
			if (IN.worldPos.y < _MiddleBottom + halfOverlap)
				middle = 1 - bottom;

			float3 albedo = float3(0, 0, 0);
			float smoothness = 0;
			float4 normal = float4(0, 0, 0, 0);

			if (top > 0)
			{
				albedo += top * tex2D(_TopMainTex, TexUV).rgb;
				smoothness += top * tex2D(_TopSmoothness, TexUV).r;
				normal += top * tex2D(_TopNMap, TexUV);
			}

			if (middle > 0)
			{
				albedo += middle * tex2D(_MiddleMainTex, TexUV).rgb;
				smoothness += middle * tex2D(_MiddleSmoothness, TexUV).r;
				normal += middle * tex2D(_MiddleNMap, TexUV);
			}

			if (bottom > 0)
			{
				albedo += bottom * tex2D(_BottomMainTex, TexUV).rgb;
				smoothness += bottom * tex2D(_BottomSmoothness, TexUV).r;
				normal += bottom * tex2D(_BottomNMap, TexUV);
			}
			o.Albedo = albedo * _Color.rgb;
			o.Emission = _Emission;
			o.Smoothness = smoothness;
			float3 unpackedNormal = UnpackNormal(normal);
			unpackedNormal.x = -unpackedNormal.x;
			unpackedNormal.y = -unpackedNormal.y;
			o.Normal = unpackedNormal;
			o.Metallic = 0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
