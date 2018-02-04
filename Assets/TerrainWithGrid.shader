Shader "Custom/TerrainWithGrid" {
	Properties {
		_DrawGrid("DrawGrid", Range(0,1)) = 0
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("NormalMap", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_GridSize("GridSize", Vector) = (0,0,0,0)
		_GridOffset("GridOffset", Vector) = (0,0,0,0)
		_GridColor("GridColor", 2D) = "white" {}
		_GridSpacing("Spacing", float) = 10
		_GridThickness("GridThickness", float) = 0.05
		_UseUV("UseUV", Range(0,1)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _GridColor;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _DrawGrid;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float4 _GridSize;
		float4 _GridOffset;
		float _GridThickness;
		float _GridSpacing;
		half _UseUV;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color 
			float3 localPos = IN.worldPos +  mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;
			float2 pos = float2(fmod(abs(localPos.x - _GridOffset.x), _GridSpacing), fmod(abs(localPos.z - _GridOffset.y), _GridSpacing));
			float2 TexPos;
			if (_UseUV > 0.5)
				TexPos = IN.uv_MainTex;
			else
				TexPos = pos / _GridSpacing;
			fixed4 c = tex2D (_MainTex, TexPos) * _Color;
			o.Albedo = c.rgb;
			if (_DrawGrid > 0.5)
			{
				float2 CurrCell = float2(floor((localPos.x - _GridOffset.x) / _GridSpacing), floor((localPos.z - _GridOffset.y) / _GridSpacing));
				float2 MaxVal = _GridSize.xy * _GridSpacing + _GridOffset.xy;
				//if (localPos.x >= _GridOffset.x && localPos.z >= _GridOffset.y && localPos.x <= MaxVal.x && localPos.z <= MaxVal.y)
				if (CurrCell.x >= 0 && CurrCell.x < _GridSize.x && CurrCell.y >= 0 && CurrCell.y < _GridSize.y)
					if ((pos.x > _GridThickness) && (pos.x < _GridSpacing - _GridThickness) && (pos.y > _GridThickness) && (pos.y < _GridSpacing - _GridThickness))
						o.Emission = tex2D(_GridColor, float2(CurrCell.x / _GridSize.x, CurrCell.y / _GridSize.y));
			}
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1;
			o.Normal = UnpackNormal(tex2D(_NormalMap, TexPos));
		}
		ENDCG
	}
	FallBack "Diffuse"
}
