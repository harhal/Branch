// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TerrainWithGrid" {
	Properties 
	{
		// Splat Map Control Texture
		[HideInInspector] _Control("Control (RGBA)", 2D) = "red" {}

		// Textures
		[HideInInspector] _Splat3("Layer 3 (A)", 2D) = "white" {}
		[HideInInspector] _Splat2("Layer 2 (B)", 2D) = "white" {}
		[HideInInspector] _Splat1("Layer 1 (G)", 2D) = "white" {}
		[HideInInspector] _Splat0("Layer 0 (R)", 2D) = "white" {}

		// Normal Maps
		[HideInInspector] _Normal3("Normal 3 (A)", 2D) = "bump" {}
		[HideInInspector] _Normal2("Normal 2 (B)", 2D) = "bump" {}
		[HideInInspector] _Normal1("Normal 1 (G)", 2D) = "bump" {}
		[HideInInspector] _Normal0("Normal 0 (R)", 2D) = "bump" {}

		_DrawGrid("DrawGrid", Range(0,1)) = 0
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_GridSize("GridSize", Vector) = (0,0,0,0)
		_GridOffset("GridOffset", Vector) = (0,0,0,0)
		_GridSpacing("Spacing", float) = 10
		_GridThickness("GridThickness", float) = 0.05
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:SplatmapVert fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
			
		uniform sampler2D _Control;
		uniform sampler2D _Splat0, _Splat1, _Splat2, _Splat3;
		uniform sampler2D _Normal0, _Normal1, _Normal2, _Normal3;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;        
			float2 uv_Control : TEXCOORD0;
			float2 uv_Splat0 : TEXCOORD1;
			float2 uv_Splat1 : TEXCOORD2;
			float2 uv_Splat2 : TEXCOORD3;
			float2 uv_Splat3 : TEXCOORD4;
		};

		half _DrawGrid;
		half _Glossiness;
		half _Metallic;
		float4 _GridSize;
		float4 _GridOffset;
		float _GridThickness;
		float _GridSpacing;

		uniform half _CellArray[100];

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void SplatmapVert(inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);
			float4 pos = UnityObjectToClipPos(v.vertex);

			v.tangent.xyz = cross(v.normal, float3(0, 0, 1));
			v.tangent.w = 1;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {  
			fixed4 splat_control = tex2D (_Control, IN.uv_Control);
			fixed3 col;
			col = splat_control.r * tex2D(_Splat0, IN.uv_Splat0).rgb;
			col += splat_control.g * tex2D(_Splat1, IN.uv_Splat1).rgb;
			col += splat_control.b * tex2D(_Splat2, IN.uv_Splat2).rgb;
			col += splat_control.a * tex2D(_Splat3, IN.uv_Splat3).rgb;
			o.Albedo = col;
			fixed4 normal;
			normal = splat_control.r * tex2D(_Normal0, IN.uv_Splat0);
			normal += splat_control.g * tex2D(_Normal1, IN.uv_Splat1);
			normal += splat_control.b * tex2D(_Normal2, IN.uv_Splat2);
			normal += splat_control.a * tex2D(_Normal3, IN.uv_Splat3);
			fixed3 FinalNormal = UnpackNormal(normal);
			o.Normal = FinalNormal;


			o.Normal = col;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1;

			float3 localPos = IN.worldPos -  mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;
			float2 pos = float2(fmod(abs(localPos.x + _GridOffset.x), _GridSpacing), fmod(abs(localPos.z + _GridOffset.y), _GridSpacing));
			if (_DrawGrid > 0.5)
			{
				float2 CurrCell = float2(floor((localPos.x - _GridOffset.x) / _GridSpacing), floor((localPos.z - _GridOffset.y) / _GridSpacing));
				float2 MaxVal = _GridSize.xy * _GridSpacing + _GridOffset.xy;
				if (CurrCell.x >= 0 && CurrCell.x < _GridSize.x && CurrCell.y >= 0 && CurrCell.y < _GridSize.y)
					if ((pos.x > _GridThickness) && (pos.x < _GridSpacing - _GridThickness) && (pos.y > _GridThickness) && (pos.y < _GridSpacing - _GridThickness))
					{
						if (_CellArray[CurrCell.y * _GridSize.x + CurrCell.x] == 0)
							o.Emission = float4(1, 0, 0, 1);
						else
							o.Emission = float4(0, 1, 0, 1);
					}
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}