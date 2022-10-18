// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SciFi/HologramCreats"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_AlbedoColor("AlbedoColor", Color) = (1,1,1,0)
		_Normal("Normal", 2D) = "bump" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_NormalPower("NormalPower", Float) = 0
		_AO("AO", 2D) = "white" {}
		_ObjectHigh("ObjectHigh", Float) = 0
		_ObjectLow("ObjectLow", Float) = 0
		_SpawnPoint("SpawnPoint", Vector) = (0,0,0,0)
		_DotHighControl("DotHighControl", Float) = 0.02
		_Opacity("Opacity", Range( 0 , 1)) = 0
		_RaySpeed("RaySpeed", Float) = 1
		_RayLow("RayLow", Float) = 0
		_RayWidth("RayWidth", Float) = 2
		[HDR]_RayColor("RayColor", Color) = (0,0,0,0)
		[HDR]_LineColor("LineColor", Color) = (0,0,0,0)
		_LineHight("LineHight", Float) = 0
		_LineWidth("LineWidth", Float) = 5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform half3 _SpawnPoint;
		uniform float _ShaderDisplacement;
		uniform float _ObjectLow;
		uniform float _ObjectHigh;
		uniform half _DotHighControl;
		uniform half _NormalPower;
		uniform sampler2D _Normal;
		uniform half4 _Normal_ST;
		uniform sampler2D _Albedo;
		uniform half4 _Albedo_ST;
		uniform half4 _AlbedoColor;
		uniform half4 _RayColor;
		uniform half _RaySpeed;
		uniform half _RayLow;
		uniform half _RayWidth;
		uniform half4 _LineColor;
		uniform half _LineHight;
		uniform half _LineWidth;
		uniform half _Metallic;
		uniform half _Smoothness;
		uniform sampler2D _AO;
		uniform half4 _AO_ST;
		uniform half _Opacity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			half temp_output_19_0 = ( (_ObjectLow + (_ShaderDisplacement - 0.0) * (_ObjectHigh - _ObjectLow) / (1.0 - 0.0)) - ase_vertex3Pos.y );
			half clampResult22 = clamp( temp_output_19_0 , 0.0 , 1.0 );
			half3 temp_output_21_0 = ( _SpawnPoint * ( 1.0 - clampResult22 ) );
			half4 transform36 = mul(unity_ObjectToWorld,half4( ase_vertex3Pos , 0.0 ));
			half4 lerpResult47 = lerp( half4( temp_output_21_0 , 0.0 ) , ( ( ( transform36 - half4( ase_vertex3Pos , 0.0 ) ) * _DotHighControl ) + half4( temp_output_21_0 , 0.0 ) ) , temp_output_21_0.y);
			v.vertex.xyz += lerpResult47.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackScaleNormal( tex2D( _Normal, uv_Normal ), _NormalPower );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = ( tex2D( _Albedo, uv_Albedo ) * _AlbedoColor ).rgb;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			half temp_output_19_0 = ( (_ObjectLow + (_ShaderDisplacement - 0.0) * (_ObjectHigh - _ObjectLow) / (1.0 - 0.0)) - ase_vertex3Pos.y );
			half clampResult22 = clamp( temp_output_19_0 , 0.0 , 1.0 );
			half3 temp_output_21_0 = ( _SpawnPoint * ( 1.0 - clampResult22 ) );
			half mulTime140 = _Time.y * _RaySpeed;
			half temp_output_136_0 = ( ase_vertex3Pos.y + (( 3.0 * -1.0 ) + (frac( mulTime140 ) - 0.0) * (( _RayLow * -1.0 ) - ( 3.0 * -1.0 )) / (1.0 - 0.0)) );
			half clampResult147 = clamp( ( 1.0 - ( abs( temp_output_136_0 ) * _RayWidth ) ) , 0.2 , 1.0 );
			half clampResult285 = clamp( ( 1.0 - ( abs( temp_output_136_0 ) * 0.04 ) ) , 0.1 , 1.0 );
			half temp_output_320_0 = ( temp_output_19_0 + _LineHight );
			half clampResult323 = clamp( temp_output_320_0 , 0.0 , 1.0 );
			half clampResult318 = clamp( ( temp_output_320_0 * ( 1.0 - clampResult323 ) * _LineWidth ) , 0.0 , 5.0 );
			half clampResult327 = clamp( pow( clampResult318 , 155.0 ) , 0.0 , 3.0 );
			half4 lerpResult126 = lerp( ( _RayColor * temp_output_21_0.y * clampResult147 * ( clampResult285 * 0.32 ) ) , _LineColor , clampResult327);
			o.Emission = lerpResult126.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			float2 uv_AO = i.uv_texcoord * _AO_ST.xy + _AO_ST.zw;
			o.Occlusion = tex2D( _AO, uv_AO ).r;
			half clampResult53 = clamp( temp_output_21_0.y , 0.0 , 1.0 );
			half clampResult58 = clamp( ( ( 1.0 - clampResult53 ) + _Opacity ) , 0.0 , 1.0 );
			o.Alpha = clampResult58;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
-1913;7;1906;1004;294.1074;115.3721;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;292;-918.5735,-204.1724;Inherit;False;Property;_RaySpeed;RaySpeed;12;0;Create;True;0;0;False;0;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-661.3577,464.0225;Float;False;Global;_ShaderDisplacement;_ShaderDisplacement;17;0;Create;True;0;0;False;0;0;0.1237612;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-545.0088,544.7075;Float;False;Property;_ObjectLow;ObjectLow;8;0;Create;True;0;0;False;0;0;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;146;-666.8611,-127.2715;Inherit;False;Constant;_RayHigh;RayHigh;22;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-671.5286,-35.72461;Inherit;False;Property;_RayLow;RayLow;13;0;Create;True;0;0;False;0;0;-3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;140;-664.9587,-198.306;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-545.6048,633.902;Float;False;Property;_ObjectHigh;ObjectHigh;7;0;Create;True;0;0;False;0;0;1.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;17;-416.6522,140.1017;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;294;-492.255,-29.83478;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;293;-493.5832,-122.8348;Inherit;False;2;2;0;FLOAT;-1;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;18;-298.8708,468.7333;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;139;-473.9586,-197.306;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;19;-19.83076,470.092;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;142;-288.9586,-197.306;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-25;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;321;-140.8515,145.7884;Inherit;False;Property;_LineHight;LineHight;17;0;Create;True;0;0;False;0;0;-0.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;136;-12.60871,-221.9412;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;22;167.3614,469.8341;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;320;146.4485,127.5883;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;323;412.5958,169.9918;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;287;140.9999,47.11334;Inherit;False;Constant;_Const2;Const2;14;0;Create;True;0;0;False;0;0.04;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;67;171.1317,287.4914;Inherit;False;Property;_SpawnPoint;SpawnPoint;9;0;Create;True;0;0;False;0;0,0,0;10.04,26.66,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.AbsOpNode;282;176.1595,-47.43645;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;34;-50.00343,643.635;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;23;380.0666,469.427;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;133;186.9875,-223.7759;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;125;786.4854,220.8427;Inherit;False;Property;_LineWidth;LineWidth;18;0;Create;True;0;0;False;0;5;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;578.394,296.4364;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;286;171.2999,-145.8867;Inherit;False;Property;_RayWidth;RayWidth;14;0;Create;True;0;0;False;0;2;7.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;37;215.6962,840.6249;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;36;208.4965,644.0252;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;283;339.8395,-48.22586;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;322;570.601,169.6847;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;971.2523,82.3977;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;24;864.3699,293.0344;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;40;516.3109,781.7116;Inherit;False;Property;_DotHighControl;DotHighControl;10;0;Create;True;0;0;False;0;0.02;0.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;284;527.1918,-49.17375;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;38;488.3109,645.7116;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;364.6675,-224.5653;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;318;1140.808,84.17657;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;726.3109,646.7116;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;129;977.8234,203.9285;Inherit;False;Constant;_Const5;Const5;12;0;Create;True;0;0;False;0;155;185.82;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;134;554.0198,-225.5132;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;53;1187.729,455.1463;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;285;733.0087,-49.31036;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.1;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;290;610.9484,67.73199;Inherit;False;Constant;_Const3;Const3;15;0;Create;True;0;0;False;0;0.32;0.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;52;1450.729,455.1463;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;964.4135,647.4435;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ClampOpNode;147;808.8367,-225.0498;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.2;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;291;1029.948,-336.268;Inherit;False;Property;_RayColor;RayColor;15;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0.2662049,2.118547,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;289;966.9484,-46.26801;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;1371.195,560.9498;Inherit;False;Property;_Opacity;Opacity;11;0;Create;True;0;0;False;0;0;0.119;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;128;1387.732,83.23958;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;1344.913,-85.30019;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;127;1328.598,226.7502;Inherit;False;Property;_LineColor;LineColor;16;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1.660838,2.356072,7.377211,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;327;1558.893,92.62794;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;47;1189.293,626.3533;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;31;1531.192,-427.4849;Inherit;False;Property;_AlbedoColor;AlbedoColor;1;0;Create;True;0;0;False;0;1,1,1,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;1434.794,-639.0801;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;-1;f8b6b2d6cf795c84f9f02d42d2720e06;f8b6b2d6cf795c84f9f02d42d2720e06;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;57;1718.389,505.9913;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;303;1399.797,-239.5673;Inherit;False;Property;_NormalPower;NormalPower;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;1652.31,139.921;Inherit;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;False;0;0;0.266;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;123;1981.81,621.2286;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ClampOpNode;58;1864.678,506.1307;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;126;1707.712,12.46012;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;122;1650.31,222.921;Inherit;False;Property;_Smoothness;Smoothness;4;0;Create;True;0;0;False;0;0;0.635;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;29;1646.503,301.8492;Inherit;True;Property;_AO;AO;6;0;Create;True;0;0;False;0;-1;689b1cbee7b42d14ea3d5e22e818f064;689b1cbee7b42d14ea3d5e22e818f064;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;1845.181,-334.4612;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;296;1643.885,-216.7133;Inherit;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;-1;f8b6b2d6cf795c84f9f02d42d2720e06;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2247.665,83.09139;Half;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SciFi/HologramCreats;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;19;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;140;0;292;0
WireConnection;294;0;131;0
WireConnection;293;0;146;0
WireConnection;18;0;20;0
WireConnection;18;3;16;0
WireConnection;18;4;15;0
WireConnection;139;0;140;0
WireConnection;19;0;18;0
WireConnection;19;1;17;2
WireConnection;142;0;139;0
WireConnection;142;3;293;0
WireConnection;142;4;294;0
WireConnection;136;0;17;2
WireConnection;136;1;142;0
WireConnection;22;0;19;0
WireConnection;320;0;19;0
WireConnection;320;1;321;0
WireConnection;323;0;320;0
WireConnection;282;0;136;0
WireConnection;23;0;22;0
WireConnection;133;0;136;0
WireConnection;21;0;67;0
WireConnection;21;1;23;0
WireConnection;36;0;34;0
WireConnection;283;0;282;0
WireConnection;283;1;287;0
WireConnection;322;0;323;0
WireConnection;124;0;320;0
WireConnection;124;1;322;0
WireConnection;124;2;125;0
WireConnection;24;0;21;0
WireConnection;284;0;283;0
WireConnection;38;0;36;0
WireConnection;38;1;37;0
WireConnection;138;0;133;0
WireConnection;138;1;286;0
WireConnection;318;0;124;0
WireConnection;39;0;38;0
WireConnection;39;1;40;0
WireConnection;134;0;138;0
WireConnection;53;0;24;1
WireConnection;285;0;284;0
WireConnection;52;0;53;0
WireConnection;46;0;39;0
WireConnection;46;1;21;0
WireConnection;147;0;134;0
WireConnection;289;0;285;0
WireConnection;289;1;290;0
WireConnection;128;0;318;0
WireConnection;128;1;129;0
WireConnection;25;0;291;0
WireConnection;25;1;24;1
WireConnection;25;2;147;0
WireConnection;25;3;289;0
WireConnection;327;0;128;0
WireConnection;47;0;21;0
WireConnection;47;1;46;0
WireConnection;47;2;24;1
WireConnection;57;0;52;0
WireConnection;57;1;55;0
WireConnection;123;0;47;0
WireConnection;58;0;57;0
WireConnection;126;0;25;0
WireConnection;126;1;127;0
WireConnection;126;2;327;0
WireConnection;30;0;28;0
WireConnection;30;1;31;0
WireConnection;296;5;303;0
WireConnection;0;0;30;0
WireConnection;0;1;296;0
WireConnection;0;2;126;0
WireConnection;0;3;120;0
WireConnection;0;4;122;0
WireConnection;0;5;29;0
WireConnection;0;9;58;0
WireConnection;0;11;123;0
ASEEND*/
//CHKSM=866C84F47CE37AA151EB87285CD68C32ECB64384