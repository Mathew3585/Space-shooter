// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SciFiURP/Sci-FiRefraction"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_Albedo("Albedo", 2D) = "white" {}
		_AlbedoColor("AlbedoColor", Color) = (0,0,0,0)
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Normal("Normal", 2D) = "bump" {}
		_AO("AO", 2D) = "white" {}
		_Emission("Emission", 2D) = "black" {}
		[HDR]_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_ObjectHigh("ObjectHigh", Float) = 0
		_ObjectLow("ObjectLow", Float) = 0
		_Noises("Noises", 2D) = "white" {}
		_DissolveTiling("DissolveTiling", Float) = 1
		[HDR]_DissolveColor("DissolveColor", Color) = (0.180188,4.594794,0,0)
		_DissolvePower("DissolvePower", Range( 0 , 1)) = 0
		_DissolveColorPower("DissolveColorPower", Float) = 0.02
		_DissolveSquareScale("DissolveSquareScale", Range( 0.01 , 1)) = 0
		_DissolveSquarePower("DissolveSquarePower", Range( 0 , 10)) = 1
		[HDR]_FirstTextureColor("FirstTextureColor", Color) = (0,0.07736176,0.8773585,0)
		_FirstTextureHight("FirstTextureHight", Float) = 0
		_FirstTextureScale("FirstTextureScale", Range( 0.1 , 10)) = 0
		_FirstTextureTiling("FirstTextureTiling", Float) = 0
		[HDR]_SecondTextureColor("SecondTextureColor", Color) = (0,0.07736176,0.8773585,0)
		_SecondTextureHight("SecondTextureHight", Float) = 0
		_SecondTextureScale("SecondTextureScale", Range( 0.1 , 10)) = 0
		_SecondTextureTiling("SecondTextureTiling", Float) = 0
		_DarkAreaScale("DarkAreaScale", Float) = 0
		_DarkAreaPower("DarkAreaPower", Float) = 1
		_OpacityScale("OpacityScale", Range( 0.01 , 0.9)) = 0
		_OpacityPower("OpacityPower", Range( 0 , 10)) = 1
		[ASEEnd]_NormalPower("NormalPower", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
		Cull Back
		AlphaToMask Off
		HLSLINCLUDE
		#pragma target 3.0

		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70108

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_FORWARD

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
			    #define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord : TEXCOORD0;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 lightmapUVOrVertexSH : TEXCOORD0;
				half4 fogFactorAndVertexLight : TEXCOORD1;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Albedo_ST;
			half4 _AO_ST;
			half4 _EmissionColor;
			half4 _Emission_ST;
			float4 _SecondTextureColor;
			float4 _DissolveColor;
			float4 _FirstTextureColor;
			float4 _AlbedoColor;
			half4 _Normal_ST;
			half _NormalPower;
			half _Smoothness;
			half _Metallic;
			float _ObjectLow;
			float _ObjectHigh;
			float _SecondTextureScale;
			float _SecondTextureHight;
			float _SecondTextureTiling;
			float _DarkAreaScale;
			float _FirstTextureScale;
			float _FirstTextureTiling;
			float _OpacityScale;
			float _DissolveSquarePower;
			float _DissolveSquareScale;
			float _DissolveColorPower;
			float _DissolvePower;
			float _DissolveTiling;
			float _DarkAreaPower;
			float _FirstTextureHight;
			float _OpacityPower;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Albedo;
			float _ShaderDisplacement;
			sampler2D _Normal;
			sampler2D _Noises;
			SAMPLER(sampler_Noises);
			sampler2D _Emission;
			sampler2D _AO;
			SAMPLER(sampler_AO);
			SAMPLER(sampler_Albedo);


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord7.xy = v.texcoord.xy;
				o.ase_texcoord8 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord7.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord;
					o.lightmapUVOrVertexSH.xy = v.texcoord * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );
				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif
				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				
				o.clipPos = positionCS;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				o.screenPos = ComputeScreenPos(positionCS);
				#endif
				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord1 = v.texcoord1;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif
				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif
	
				WorldViewDirection = SafeNormalize( WorldViewDirection );

				float2 uv_Albedo = IN.ase_texcoord7.xy * _Albedo_ST.xy + _Albedo_ST.zw;
				half4 tex2DNode126 = tex2D( _Albedo, uv_Albedo );
				float vpY32 = IN.ase_texcoord8.xyz.y;
				half temp_output_54_0 = (_ObjectLow + (_ShaderDisplacement - 0.0) * (_ObjectHigh - _ObjectLow) / (1.0 - 0.0));
				half temp_output_71_0 = ( -1.0 * temp_output_54_0 );
				half temp_output_76_0 = ( vpY32 + temp_output_71_0 + 0.4 );
				half clampResult139 = clamp( pow( ( temp_output_76_0 * -1.0 * _DarkAreaScale ) , _DarkAreaPower ) , 0.0 , 1.0 );
				half4 lerpResult136 = lerp( float4( 0,0,0,0 ) , ( tex2DNode126 * _AlbedoColor ) , clampResult139);
				
				float2 uv_Normal = IN.ase_texcoord7.xy * _Normal_ST.xy + _Normal_ST.zw;
				half3 unpack175 = UnpackNormalScale( tex2D( _Normal, uv_Normal ), _NormalPower );
				unpack175.z = lerp( 1, unpack175.z, saturate(_NormalPower) );
				
				half2 temp_cast_1 = (_DissolveTiling).xx;
				half2 texCoord87 = IN.ase_texcoord7.xy * temp_cast_1 + float2( 0,0 );
				half2 panner89 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord87);
				half temp_output_82_0 = ( 1.0 - tex2D( _Noises, panner89 ).r );
				half temp_output_2_0 = (1.0 + (_DissolvePower - 0.0) * (-0.1 - 1.0) / (1.0 - 0.0));
				half clampResult13 = clamp( step( temp_output_82_0 , ( temp_output_2_0 + 0.1 ) ) , 0.0 , 1.0 );
				half clampResult85 = clamp( pow( ( temp_output_76_0 * -1.0 * _DissolveSquareScale ) , _DissolveSquarePower ) , 0.0 , 1.0 );
				half lerpResult101 = lerp( ( 1.0 - clampResult13 ) , 1.0 , clampResult85);
				half4 lerpResult91 = lerp( ( _DissolveColor * ( 1.0 - step( temp_output_82_0 , ( temp_output_2_0 + _DissolveColorPower ) ) ) ) , float4( 0,0,0,0 ) , lerpResult101);
				half2 temp_cast_2 = (_FirstTextureTiling).xx;
				half2 texCoord182 = IN.ase_texcoord7.xy * temp_cast_2 + float2( 0,0 );
				half2 panner184 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord182);
				half4 tex2DNode75 = tex2D( _Noises, panner184 );
				half clampResult86 = clamp( abs( ( ( vpY32 + temp_output_71_0 + _FirstTextureHight ) * -1.0 * _FirstTextureScale ) ) , 0.0 , 1.0 );
				half lerpResult27 = lerp( tex2DNode75.g , 0.0 , clampResult86);
				half4 lerpResult131 = lerp( lerpResult91 , _FirstTextureColor , lerpResult27);
				half2 temp_cast_3 = (_SecondTextureTiling).xx;
				half2 texCoord179 = IN.ase_texcoord7.xy * temp_cast_3 + float2( 0,0 );
				half2 panner183 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord179);
				half4 tex2DNode167 = tex2D( _Noises, panner183 );
				half clampResult166 = clamp( abs( ( ( vpY32 + temp_output_71_0 + _SecondTextureHight ) * -1.0 * _SecondTextureScale ) ) , 0.0 , 1.0 );
				half lerpResult168 = lerp( tex2DNode167.b , 0.0 , clampResult166);
				half4 lerpResult170 = lerp( lerpResult131 , _SecondTextureColor , lerpResult168);
				float2 uv_Emission = IN.ase_texcoord7.xy * _Emission_ST.xy + _Emission_ST.zw;
				half4 tex2DNode218 = tex2D( _Emission, uv_Emission );
				half4 clampResult222 = clamp( ( tex2DNode218 / lerpResult170 ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
				half grayscale221 = Luminance(clampResult222.rgb);
				half4 lerpResult220 = lerp( lerpResult170 , ( tex2DNode218 * _EmissionColor ) , grayscale221);
				
				float2 uv_AO = IN.ase_texcoord7.xy * _AO_ST.xy + _AO_ST.zw;
				
				half lerpResult79 = lerp( clampResult13 , 1.0 , clampResult85);
				half lerpResult199 = lerp( tex2DNode75.g , 1.0 , clampResult86);
				half lerpResult200 = lerp( tex2DNode167.b , 1.0 , clampResult166);
				half clampResult207 = clamp( pow( ( temp_output_76_0 * -1.0 * _OpacityScale ) , _OpacityPower ) , 0.0 , 1.0 );
				half lerpResult201 = lerp( ( lerpResult79 * lerpResult199 * lerpResult200 ) , 1.0 , clampResult207);
				half temp_output_59_0 = ( -0.3 + temp_output_54_0 );
				half smoothstepResult46 = smoothstep( ( temp_output_59_0 - 0.5 ) , ( temp_output_59_0 + 0.5 ) , vpY32);
				half clampResult247 = clamp( ( ( tex2DNode126.a * lerpResult201 ) + smoothstepResult46 + -1.2 ) , 0.0 , 1.0 );
				
				float3 Albedo = lerpResult136.rgb;
				float3 Normal = unpack175;
				float3 Emission = lerpResult220.rgb;
				float3 Specular = 0.5;
				float Metallic = _Metallic;
				float Smoothness = _Smoothness;
				float Occlusion = tex2D( _AO, uv_AO ).r;
				float Alpha = ( 1.0 - clampResult247 );
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;
				inputData.shadowCoord = ShadowCoords;

				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
					inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
					inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
					inputData.normalWS = Normal;
					#endif
					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				#else
					inputData.normalWS = WorldNormal;
				#endif

				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
				#ifdef _ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#endif
				half4 color = UniversalFragmentPBR(
					inputData, 
					Albedo, 
					Metallic, 
					Specular, 
					Smoothness, 
					Occlusion, 
					Emission, 
					Alpha);

				#ifdef _TRANSMISSION_ASE
				{
					float shadow = _TransmissionShadow;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );
					half3 mainTransmission = max(0 , -dot(inputData.normalWS, mainLight.direction)) * mainAtten * Transmission;
					color.rgb += Albedo * mainTransmission;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 transmission = max(0 , -dot(inputData.normalWS, light.direction)) * atten * Transmission;
							color.rgb += Albedo * transmission;
						}
					#endif
				}
				#endif

				#ifdef _TRANSLUCENCY_ASE
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );

					half3 mainLightDir = mainLight.direction + inputData.normalWS * normal;
					half mainVdotL = pow( saturate( dot( inputData.viewDirectionWS, -mainLightDir ) ), scattering );
					half3 mainTranslucency = mainAtten * ( mainVdotL * direct + inputData.bakedGI * ambient ) * Translucency;
					color.rgb += Albedo * mainTranslucency * strength;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 lightDir = light.direction + inputData.normalWS * normal;
							half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );
							half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;
							color.rgb += Albedo * translucency * strength;
						}
					#endif
				}
				#endif

				#ifdef _REFRACTION_ASE
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, WorldNormal ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif
				
				return color;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70108

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_SHADOWCASTER

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Albedo_ST;
			half4 _AO_ST;
			half4 _EmissionColor;
			half4 _Emission_ST;
			float4 _SecondTextureColor;
			float4 _DissolveColor;
			float4 _FirstTextureColor;
			float4 _AlbedoColor;
			half4 _Normal_ST;
			half _NormalPower;
			half _Smoothness;
			half _Metallic;
			float _ObjectLow;
			float _ObjectHigh;
			float _SecondTextureScale;
			float _SecondTextureHight;
			float _SecondTextureTiling;
			float _DarkAreaScale;
			float _FirstTextureScale;
			float _FirstTextureTiling;
			float _OpacityScale;
			float _DissolveSquarePower;
			float _DissolveSquareScale;
			float _DissolveColorPower;
			float _DissolvePower;
			float _DissolveTiling;
			float _DarkAreaPower;
			float _FirstTextureHight;
			float _OpacityPower;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Albedo;
			SAMPLER(sampler_Albedo);
			sampler2D _Noises;
			SAMPLER(sampler_Noises);
			float _ShaderDisplacement;


			
			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_texcoord3 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				float3 normalWS = TransformObjectToWorldDir(v.ase_normal);

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );
				
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_Albedo = IN.ase_texcoord2.xy * _Albedo_ST.xy + _Albedo_ST.zw;
				half4 tex2DNode126 = tex2D( _Albedo, uv_Albedo );
				half2 temp_cast_0 = (_DissolveTiling).xx;
				half2 texCoord87 = IN.ase_texcoord2.xy * temp_cast_0 + float2( 0,0 );
				half2 panner89 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord87);
				half temp_output_82_0 = ( 1.0 - tex2D( _Noises, panner89 ).r );
				half temp_output_2_0 = (1.0 + (_DissolvePower - 0.0) * (-0.1 - 1.0) / (1.0 - 0.0));
				half clampResult13 = clamp( step( temp_output_82_0 , ( temp_output_2_0 + 0.1 ) ) , 0.0 , 1.0 );
				float vpY32 = IN.ase_texcoord3.xyz.y;
				half temp_output_54_0 = (_ObjectLow + (_ShaderDisplacement - 0.0) * (_ObjectHigh - _ObjectLow) / (1.0 - 0.0));
				half temp_output_71_0 = ( -1.0 * temp_output_54_0 );
				half temp_output_76_0 = ( vpY32 + temp_output_71_0 + 0.4 );
				half clampResult85 = clamp( pow( ( temp_output_76_0 * -1.0 * _DissolveSquareScale ) , _DissolveSquarePower ) , 0.0 , 1.0 );
				half lerpResult79 = lerp( clampResult13 , 1.0 , clampResult85);
				half2 temp_cast_1 = (_FirstTextureTiling).xx;
				half2 texCoord182 = IN.ase_texcoord2.xy * temp_cast_1 + float2( 0,0 );
				half2 panner184 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord182);
				half4 tex2DNode75 = tex2D( _Noises, panner184 );
				half clampResult86 = clamp( abs( ( ( vpY32 + temp_output_71_0 + _FirstTextureHight ) * -1.0 * _FirstTextureScale ) ) , 0.0 , 1.0 );
				half lerpResult199 = lerp( tex2DNode75.g , 1.0 , clampResult86);
				half2 temp_cast_2 = (_SecondTextureTiling).xx;
				half2 texCoord179 = IN.ase_texcoord2.xy * temp_cast_2 + float2( 0,0 );
				half2 panner183 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord179);
				half4 tex2DNode167 = tex2D( _Noises, panner183 );
				half clampResult166 = clamp( abs( ( ( vpY32 + temp_output_71_0 + _SecondTextureHight ) * -1.0 * _SecondTextureScale ) ) , 0.0 , 1.0 );
				half lerpResult200 = lerp( tex2DNode167.b , 1.0 , clampResult166);
				half clampResult207 = clamp( pow( ( temp_output_76_0 * -1.0 * _OpacityScale ) , _OpacityPower ) , 0.0 , 1.0 );
				half lerpResult201 = lerp( ( lerpResult79 * lerpResult199 * lerpResult200 ) , 1.0 , clampResult207);
				half temp_output_59_0 = ( -0.3 + temp_output_54_0 );
				half smoothstepResult46 = smoothstep( ( temp_output_59_0 - 0.5 ) , ( temp_output_59_0 + 0.5 ) , vpY32);
				half clampResult247 = clamp( ( ( tex2DNode126.a * lerpResult201 ) + smoothstepResult46 + -1.2 ) , 0.0 , 1.0 );
				
				float Alpha = ( 1.0 - clampResult247 );
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70108

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Albedo_ST;
			half4 _AO_ST;
			half4 _EmissionColor;
			half4 _Emission_ST;
			float4 _SecondTextureColor;
			float4 _DissolveColor;
			float4 _FirstTextureColor;
			float4 _AlbedoColor;
			half4 _Normal_ST;
			half _NormalPower;
			half _Smoothness;
			half _Metallic;
			float _ObjectLow;
			float _ObjectHigh;
			float _SecondTextureScale;
			float _SecondTextureHight;
			float _SecondTextureTiling;
			float _DarkAreaScale;
			float _FirstTextureScale;
			float _FirstTextureTiling;
			float _OpacityScale;
			float _DissolveSquarePower;
			float _DissolveSquareScale;
			float _DissolveColorPower;
			float _DissolvePower;
			float _DissolveTiling;
			float _DarkAreaPower;
			float _FirstTextureHight;
			float _OpacityPower;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Albedo;
			SAMPLER(sampler_Albedo);
			sampler2D _Noises;
			SAMPLER(sampler_Noises);
			float _ShaderDisplacement;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_texcoord3 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_Albedo = IN.ase_texcoord2.xy * _Albedo_ST.xy + _Albedo_ST.zw;
				half4 tex2DNode126 = tex2D( _Albedo, uv_Albedo );
				half2 temp_cast_0 = (_DissolveTiling).xx;
				half2 texCoord87 = IN.ase_texcoord2.xy * temp_cast_0 + float2( 0,0 );
				half2 panner89 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord87);
				half temp_output_82_0 = ( 1.0 - tex2D( _Noises, panner89 ).r );
				half temp_output_2_0 = (1.0 + (_DissolvePower - 0.0) * (-0.1 - 1.0) / (1.0 - 0.0));
				half clampResult13 = clamp( step( temp_output_82_0 , ( temp_output_2_0 + 0.1 ) ) , 0.0 , 1.0 );
				float vpY32 = IN.ase_texcoord3.xyz.y;
				half temp_output_54_0 = (_ObjectLow + (_ShaderDisplacement - 0.0) * (_ObjectHigh - _ObjectLow) / (1.0 - 0.0));
				half temp_output_71_0 = ( -1.0 * temp_output_54_0 );
				half temp_output_76_0 = ( vpY32 + temp_output_71_0 + 0.4 );
				half clampResult85 = clamp( pow( ( temp_output_76_0 * -1.0 * _DissolveSquareScale ) , _DissolveSquarePower ) , 0.0 , 1.0 );
				half lerpResult79 = lerp( clampResult13 , 1.0 , clampResult85);
				half2 temp_cast_1 = (_FirstTextureTiling).xx;
				half2 texCoord182 = IN.ase_texcoord2.xy * temp_cast_1 + float2( 0,0 );
				half2 panner184 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord182);
				half4 tex2DNode75 = tex2D( _Noises, panner184 );
				half clampResult86 = clamp( abs( ( ( vpY32 + temp_output_71_0 + _FirstTextureHight ) * -1.0 * _FirstTextureScale ) ) , 0.0 , 1.0 );
				half lerpResult199 = lerp( tex2DNode75.g , 1.0 , clampResult86);
				half2 temp_cast_2 = (_SecondTextureTiling).xx;
				half2 texCoord179 = IN.ase_texcoord2.xy * temp_cast_2 + float2( 0,0 );
				half2 panner183 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord179);
				half4 tex2DNode167 = tex2D( _Noises, panner183 );
				half clampResult166 = clamp( abs( ( ( vpY32 + temp_output_71_0 + _SecondTextureHight ) * -1.0 * _SecondTextureScale ) ) , 0.0 , 1.0 );
				half lerpResult200 = lerp( tex2DNode167.b , 1.0 , clampResult166);
				half clampResult207 = clamp( pow( ( temp_output_76_0 * -1.0 * _OpacityScale ) , _OpacityPower ) , 0.0 , 1.0 );
				half lerpResult201 = lerp( ( lerpResult79 * lerpResult199 * lerpResult200 ) , 1.0 , clampResult207);
				half temp_output_59_0 = ( -0.3 + temp_output_54_0 );
				half smoothstepResult46 = smoothstep( ( temp_output_59_0 - 0.5 ) , ( temp_output_59_0 + 0.5 ) , vpY32);
				half clampResult247 = clamp( ( ( tex2DNode126.a * lerpResult201 ) + smoothstepResult46 + -1.2 ) , 0.0 , 1.0 );
				
				float Alpha = ( 1.0 - clampResult247 );
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70108

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_META

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Albedo_ST;
			half4 _AO_ST;
			half4 _EmissionColor;
			half4 _Emission_ST;
			float4 _SecondTextureColor;
			float4 _DissolveColor;
			float4 _FirstTextureColor;
			float4 _AlbedoColor;
			half4 _Normal_ST;
			half _NormalPower;
			half _Smoothness;
			half _Metallic;
			float _ObjectLow;
			float _ObjectHigh;
			float _SecondTextureScale;
			float _SecondTextureHight;
			float _SecondTextureTiling;
			float _DarkAreaScale;
			float _FirstTextureScale;
			float _FirstTextureTiling;
			float _OpacityScale;
			float _DissolveSquarePower;
			float _DissolveSquareScale;
			float _DissolveColorPower;
			float _DissolvePower;
			float _DissolveTiling;
			float _DarkAreaPower;
			float _FirstTextureHight;
			float _OpacityPower;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Albedo;
			float _ShaderDisplacement;
			sampler2D _Noises;
			SAMPLER(sampler_Noises);
			sampler2D _Emission;
			SAMPLER(sampler_Albedo);


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_texcoord3 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = MetaVertexPosition( v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_Albedo = IN.ase_texcoord2.xy * _Albedo_ST.xy + _Albedo_ST.zw;
				half4 tex2DNode126 = tex2D( _Albedo, uv_Albedo );
				float vpY32 = IN.ase_texcoord3.xyz.y;
				half temp_output_54_0 = (_ObjectLow + (_ShaderDisplacement - 0.0) * (_ObjectHigh - _ObjectLow) / (1.0 - 0.0));
				half temp_output_71_0 = ( -1.0 * temp_output_54_0 );
				half temp_output_76_0 = ( vpY32 + temp_output_71_0 + 0.4 );
				half clampResult139 = clamp( pow( ( temp_output_76_0 * -1.0 * _DarkAreaScale ) , _DarkAreaPower ) , 0.0 , 1.0 );
				half4 lerpResult136 = lerp( float4( 0,0,0,0 ) , ( tex2DNode126 * _AlbedoColor ) , clampResult139);
				
				half2 temp_cast_1 = (_DissolveTiling).xx;
				half2 texCoord87 = IN.ase_texcoord2.xy * temp_cast_1 + float2( 0,0 );
				half2 panner89 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord87);
				half temp_output_82_0 = ( 1.0 - tex2D( _Noises, panner89 ).r );
				half temp_output_2_0 = (1.0 + (_DissolvePower - 0.0) * (-0.1 - 1.0) / (1.0 - 0.0));
				half clampResult13 = clamp( step( temp_output_82_0 , ( temp_output_2_0 + 0.1 ) ) , 0.0 , 1.0 );
				half clampResult85 = clamp( pow( ( temp_output_76_0 * -1.0 * _DissolveSquareScale ) , _DissolveSquarePower ) , 0.0 , 1.0 );
				half lerpResult101 = lerp( ( 1.0 - clampResult13 ) , 1.0 , clampResult85);
				half4 lerpResult91 = lerp( ( _DissolveColor * ( 1.0 - step( temp_output_82_0 , ( temp_output_2_0 + _DissolveColorPower ) ) ) ) , float4( 0,0,0,0 ) , lerpResult101);
				half2 temp_cast_2 = (_FirstTextureTiling).xx;
				half2 texCoord182 = IN.ase_texcoord2.xy * temp_cast_2 + float2( 0,0 );
				half2 panner184 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord182);
				half4 tex2DNode75 = tex2D( _Noises, panner184 );
				half clampResult86 = clamp( abs( ( ( vpY32 + temp_output_71_0 + _FirstTextureHight ) * -1.0 * _FirstTextureScale ) ) , 0.0 , 1.0 );
				half lerpResult27 = lerp( tex2DNode75.g , 0.0 , clampResult86);
				half4 lerpResult131 = lerp( lerpResult91 , _FirstTextureColor , lerpResult27);
				half2 temp_cast_3 = (_SecondTextureTiling).xx;
				half2 texCoord179 = IN.ase_texcoord2.xy * temp_cast_3 + float2( 0,0 );
				half2 panner183 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord179);
				half4 tex2DNode167 = tex2D( _Noises, panner183 );
				half clampResult166 = clamp( abs( ( ( vpY32 + temp_output_71_0 + _SecondTextureHight ) * -1.0 * _SecondTextureScale ) ) , 0.0 , 1.0 );
				half lerpResult168 = lerp( tex2DNode167.b , 0.0 , clampResult166);
				half4 lerpResult170 = lerp( lerpResult131 , _SecondTextureColor , lerpResult168);
				float2 uv_Emission = IN.ase_texcoord2.xy * _Emission_ST.xy + _Emission_ST.zw;
				half4 tex2DNode218 = tex2D( _Emission, uv_Emission );
				half4 clampResult222 = clamp( ( tex2DNode218 / lerpResult170 ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
				half grayscale221 = Luminance(clampResult222.rgb);
				half4 lerpResult220 = lerp( lerpResult170 , ( tex2DNode218 * _EmissionColor ) , grayscale221);
				
				half lerpResult79 = lerp( clampResult13 , 1.0 , clampResult85);
				half lerpResult199 = lerp( tex2DNode75.g , 1.0 , clampResult86);
				half lerpResult200 = lerp( tex2DNode167.b , 1.0 , clampResult166);
				half clampResult207 = clamp( pow( ( temp_output_76_0 * -1.0 * _OpacityScale ) , _OpacityPower ) , 0.0 , 1.0 );
				half lerpResult201 = lerp( ( lerpResult79 * lerpResult199 * lerpResult200 ) , 1.0 , clampResult207);
				half temp_output_59_0 = ( -0.3 + temp_output_54_0 );
				half smoothstepResult46 = smoothstep( ( temp_output_59_0 - 0.5 ) , ( temp_output_59_0 + 0.5 ) , vpY32);
				half clampResult247 = clamp( ( ( tex2DNode126.a * lerpResult201 ) + smoothstepResult46 + -1.2 ) , 0.0 , 1.0 );
				
				
				float3 Albedo = lerpResult136.rgb;
				float3 Emission = lerpResult220.rgb;
				float Alpha = ( 1.0 - clampResult247 );
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = Albedo;
				metaInput.Emission = Emission;
				
				return MetaFragment(metaInput);
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Universal2D"
			Tags { "LightMode"="Universal2D" }

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70108

			#pragma enable_d3d11_debug_symbols
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_2D

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			

			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Albedo_ST;
			half4 _AO_ST;
			half4 _EmissionColor;
			half4 _Emission_ST;
			float4 _SecondTextureColor;
			float4 _DissolveColor;
			float4 _FirstTextureColor;
			float4 _AlbedoColor;
			half4 _Normal_ST;
			half _NormalPower;
			half _Smoothness;
			half _Metallic;
			float _ObjectLow;
			float _ObjectHigh;
			float _SecondTextureScale;
			float _SecondTextureHight;
			float _SecondTextureTiling;
			float _DarkAreaScale;
			float _FirstTextureScale;
			float _FirstTextureTiling;
			float _OpacityScale;
			float _DissolveSquarePower;
			float _DissolveSquareScale;
			float _DissolveColorPower;
			float _DissolvePower;
			float _DissolveTiling;
			float _DarkAreaPower;
			float _FirstTextureHight;
			float _OpacityPower;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Albedo;
			float _ShaderDisplacement;
			SAMPLER(sampler_Albedo);
			sampler2D _Noises;
			SAMPLER(sampler_Noises);


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_texcoord3 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_Albedo = IN.ase_texcoord2.xy * _Albedo_ST.xy + _Albedo_ST.zw;
				half4 tex2DNode126 = tex2D( _Albedo, uv_Albedo );
				float vpY32 = IN.ase_texcoord3.xyz.y;
				half temp_output_54_0 = (_ObjectLow + (_ShaderDisplacement - 0.0) * (_ObjectHigh - _ObjectLow) / (1.0 - 0.0));
				half temp_output_71_0 = ( -1.0 * temp_output_54_0 );
				half temp_output_76_0 = ( vpY32 + temp_output_71_0 + 0.4 );
				half clampResult139 = clamp( pow( ( temp_output_76_0 * -1.0 * _DarkAreaScale ) , _DarkAreaPower ) , 0.0 , 1.0 );
				half4 lerpResult136 = lerp( float4( 0,0,0,0 ) , ( tex2DNode126 * _AlbedoColor ) , clampResult139);
				
				half2 temp_cast_1 = (_DissolveTiling).xx;
				half2 texCoord87 = IN.ase_texcoord2.xy * temp_cast_1 + float2( 0,0 );
				half2 panner89 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord87);
				half temp_output_82_0 = ( 1.0 - tex2D( _Noises, panner89 ).r );
				half temp_output_2_0 = (1.0 + (_DissolvePower - 0.0) * (-0.1 - 1.0) / (1.0 - 0.0));
				half clampResult13 = clamp( step( temp_output_82_0 , ( temp_output_2_0 + 0.1 ) ) , 0.0 , 1.0 );
				half clampResult85 = clamp( pow( ( temp_output_76_0 * -1.0 * _DissolveSquareScale ) , _DissolveSquarePower ) , 0.0 , 1.0 );
				half lerpResult79 = lerp( clampResult13 , 1.0 , clampResult85);
				half2 temp_cast_2 = (_FirstTextureTiling).xx;
				half2 texCoord182 = IN.ase_texcoord2.xy * temp_cast_2 + float2( 0,0 );
				half2 panner184 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord182);
				half4 tex2DNode75 = tex2D( _Noises, panner184 );
				half clampResult86 = clamp( abs( ( ( vpY32 + temp_output_71_0 + _FirstTextureHight ) * -1.0 * _FirstTextureScale ) ) , 0.0 , 1.0 );
				half lerpResult199 = lerp( tex2DNode75.g , 1.0 , clampResult86);
				half2 temp_cast_3 = (_SecondTextureTiling).xx;
				half2 texCoord179 = IN.ase_texcoord2.xy * temp_cast_3 + float2( 0,0 );
				half2 panner183 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + texCoord179);
				half4 tex2DNode167 = tex2D( _Noises, panner183 );
				half clampResult166 = clamp( abs( ( ( vpY32 + temp_output_71_0 + _SecondTextureHight ) * -1.0 * _SecondTextureScale ) ) , 0.0 , 1.0 );
				half lerpResult200 = lerp( tex2DNode167.b , 1.0 , clampResult166);
				half clampResult207 = clamp( pow( ( temp_output_76_0 * -1.0 * _OpacityScale ) , _OpacityPower ) , 0.0 , 1.0 );
				half lerpResult201 = lerp( ( lerpResult79 * lerpResult199 * lerpResult200 ) , 1.0 , clampResult207);
				half temp_output_59_0 = ( -0.3 + temp_output_54_0 );
				half smoothstepResult46 = smoothstep( ( temp_output_59_0 - 0.5 ) , ( temp_output_59_0 + 0.5 ) , vpY32);
				half clampResult247 = clamp( ( ( tex2DNode126.a * lerpResult201 ) + smoothstepResult46 + -1.2 ) , 0.0 , 1.0 );
				
				
				float3 Albedo = lerpResult136.rgb;
				float Alpha = ( 1.0 - clampResult247 );
				float AlphaClipThreshold = 0.5;

				half4 color = half4( Albedo, Alpha );

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				return color;
			}
			ENDHLSL
		}
		
	}
	/*ase_lod*/
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18500
-118;450;1502;628;484.1147;1082.655;1.280601;True;False
Node;AmplifyShaderEditor.RangedFloatNode;53;-3064.572,189.4937;Float;False;Property;_ObjectLow;ObjectLow;11;0;Create;True;0;0;False;0;False;0;-45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-3065.768,271.6888;Float;False;Property;_ObjectHigh;ObjectHigh;10;0;Create;True;0;0;False;0;False;0;40;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-3179.321,106.7301;Float;False;Global;_ShaderDisplacement;_ShaderDisplacement;9;0;Create;True;0;0;False;0;False;0;0.005105305;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2684.178,-974.984;Float;False;Constant;_Const4;Const4;12;0;Create;True;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;54;-2830.536,108.1034;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;29;-2909.303,823.0121;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;90;-3114.039,-2342.804;Float;False;Property;_DissolveTiling;DissolveTiling;13;0;Create;True;0;0;False;0;False;1;18.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;-2623.588,862.7499;Float;False;vpY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-2492.365,-970.7411;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;87;-2854.703,-2360.003;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;169;-2124.522,-521.1563;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-2792.722,-2101.98;Float;False;Property;_DissolvePower;DissolvePower;15;0;Create;True;0;0;False;0;False;0;0.33;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;77;-2152.69,-1541.649;Inherit;False;32;vpY;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;89;-2571.735,-2358.736;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-1980.877,-874.433;Float;False;Property;_FirstTextureHight;FirstTextureHight;20;0;Create;True;0;0;False;0;False;0;-5.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;160;-1962.382,-343.8934;Float;False;Property;_SecondTextureHight;SecondTextureHight;24;0;Create;True;0;0;False;0;False;0;-22.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;74;-1962.605,-1012.7;Inherit;False;32;vpY;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;161;-1919.356,-505.4121;Inherit;False;32;vpY;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-2157.578,-1400.337;Float;False;Constant;_Const6;Const6;16;0;Create;True;0;0;False;0;False;0.4;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;135;-2259.349,-1390.222;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;163;-1660.955,-454.2181;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;76;-1920.564,-1511.635;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;151;-1699.766,-830.1476;Float;False;Property;_FirstTextureScale;FirstTextureScale;21;0;Create;True;0;0;False;0;False;0;0.1;0.1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;180;-1905.148,-694.7529;Float;False;Property;_SecondTextureTiling;SecondTextureTiling;26;0;Create;True;0;0;False;0;False;0;24.33;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-1690.271,-302.6078;Float;False;Property;_SecondTextureScale;SecondTextureScale;25;0;Create;True;0;0;False;0;False;0;0.1;0.1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;2;-2446.693,-2096.773;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;-0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-2267.807,-1982.189;Float;False;Constant;_OpacityPowerConst;OpacityPowerConst;10;0;Create;True;0;0;False;0;False;0.1;0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;123;-2007.609,-1342.877;Float;False;Property;_DissolveSquareScale;DissolveSquareScale;17;0;Create;True;0;0;False;0;False;0;0.047;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;181;-1768.607,-1198.311;Float;False;Property;_FirstTextureTiling;FirstTextureTiling;22;0;Create;True;0;0;False;0;False;0;14.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-1670.451,-981.7579;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-2352.462,-2387.743;Inherit;True;Property;_Noises;Noises;12;0;Create;True;0;0;False;0;False;-1;None;d13ca39b41162ec409f248ce502bd9dc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;182;-1508.908,-1215.927;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;82;-2021.693,-2361.025;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;-1702.104,-1343.205;Float;False;Property;_DissolveSquarePower;DissolveSquarePower;18;0;Create;True;0;0;False;0;False;1;0.84;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;147;-1401.591,-981.7869;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;179;-1607.818,-711.7132;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;-1431.72,-454.247;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;-1692.489,-1512.4;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-1963.244,-2048.934;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;208;-1655.069,-91.7218;Float;False;Property;_OpacityScale;OpacityScale;29;0;Create;True;0;0;False;0;False;0;0.14;0.01;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;184;-1238.644,-1215.87;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;122;-1457.957,-1512.218;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;26;-1147.334,-981.7818;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;165;-1165.751,-455.2575;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;183;-1327.559,-709.9534;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;12;-1750.319,-2070.921;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;167;-1100.969,-736.4238;Inherit;True;Property;_SecondTexture;SecondTexture;12;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;5;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;166;-918.9913,-455.5774;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;13;-1533.909,-2069.516;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;204;-1337.839,-78.9095;Float;False;Property;_OpacityPower;OpacityPower;30;0;Create;True;0;0;False;0;False;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;85;-1236.578,-1512.556;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;75;-1000.334,-1240.665;Inherit;True;Property;_FirstTexture;FirstTexture;12;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;5;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;205;-1287.982,-239.0935;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;86;-911.9623,-980.7171;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-2500.86,497.955;Float;False;Constant;_Const8;Const8;27;0;Create;True;0;0;False;0;False;-0.3;-0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;79;-952.0895,-1695.641;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;206;-1053.45,-238.9115;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;199;-587.0931,-1028.132;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;200;-636.1179,-504.6269;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-2240.56,504.5043;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;207;-832.0707,-239.2495;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;-152.7149,-803.7125;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-1923.596,256.404;Float;False;Constant;_Const3;Const3;5;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;142;-1599.708,87.17714;Inherit;False;32;vpY;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;43;-1563.435,188.0176;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;126;-882.5004,-2435.056;Inherit;True;Property;_Albedo;Albedo;2;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;201;241.4608,-704.0665;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-1561.198,322.523;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;140;-1855.454,-1770.352;Float;False;Property;_DarkAreaScale;DarkAreaScale;27;0;Create;True;0;0;False;0;False;0;0.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;249;507.3444,-578.983;Inherit;False;Constant;_Const5;Const5;32;0;Create;True;0;0;False;0;False;-1.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;46;-1218.142,283.7689;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;228;551.2578,-804.8745;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;141;-1597.59,-1661.058;Float;False;Property;_DarkAreaPower;DarkAreaPower;28;0;Create;True;0;0;False;0;False;1;3.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-1567.363,-1811.357;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;232;709.8028,-780.1196;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;138;-1370.794,-1811.176;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;125;-841.5622,-2245.002;Float;False;Property;_AlbedoColor;AlbedoColor;3;0;Create;True;0;0;False;0;False;0,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;247;857.0994,-819.7338;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-501.2606,-2199.842;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;139;-1162.599,-1811.832;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;221;620.1107,-1231.787;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;-2624.795,772.1302;Float;False;vpX;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;-2624.818,952.5828;Float;False;vpZ;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;136;-162.5546,-2201.388;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;171;-181.4835,-1197.442;Float;False;Property;_SecondTextureColor;SecondTextureColor;23;1;[HDR];Create;True;0;0;False;0;False;0,0.07736176,0.8773585,0;0.3804152,0.5349588,2.270603,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;27;-593.0814,-1187.504;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;101;-946.7266,-1970.933;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;168;-633.8624,-659.8631;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;227;306.3863,-1362.585;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1168.177,-2226.772;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;250;1009.855,-870.3688;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;131;-69.11788,-1336.691;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;220;829.8907,-1153.826;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;212;223.3008,-1010.384;Inherit;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;False;0;0.253;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;6;-1669.651,-2207.523;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;223;453.8795,-1480.208;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;222;463.577,-1318.638;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;214;-306.9906,-1999.957;Inherit;False;Property;_NormalPower;NormalPower;31;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;132;-363.2149,-1396.56;Float;False;Property;_FirstTextureColor;FirstTextureColor;19;1;[HDR];Create;True;0;0;False;0;False;0,0.07736176,0.8773585,0;0.9057248,1.289886,5.486105,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;170;138.0131,-1215.853;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;10;-1448.613,-2205.864;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;91;-344.5497,-1669.205;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;175;-91.24333,-2005.258;Inherit;True;Property;_Normal;Normal;6;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;176;209.9737,-932.2772;Inherit;True;Property;_AO;AO;7;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;99;-1278.452,-2068.698;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;174;-2.21844,-999.6563;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-1575.142,-2422.35;Float;False;Property;_DissolveColor;DissolveColor;14;1;[HDR];Create;True;0;0;False;0;False;0.180188,4.594794,0,0;0.9057248,1.289886,5.486105,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;218;-28.16073,-1597.008;Inherit;True;Property;_Emission;Emission;8;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-2285.197,-2187.716;Float;False;Property;_DissolveColorPower;DissolveColorPower;16;0;Create;True;0;0;False;0;False;0.02;-0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;4;-1973.861,-2179.033;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;224;137.7501,-1782.019;Inherit;False;Property;_EmissionColor;EmissionColor;9;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;211;218.3422,-1094.12;Inherit;False;Property;_Metallic;Metallic;4;0;Create;True;0;0;False;0;False;0;0.064;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;241;1166.589,-1121.183;Half;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;SciFiURP/Sci-FiRefraction;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;17;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;0;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;36;Workflow;1;Surface;1;  Refraction Model;0;  Blend;0;Two Sided;1;Fragment Normal Space,InvertActionOnDeselection;0;Transmission;0;  Transmission Shadow;0.5,False,-1;Translucency;0;  Translucency Strength;1,False,-1;  Normal Distortion;0.5,False,-1;  Scattering;2,False,-1;  Direct;0.9,False,-1;  Ambient;0.1,False,-1;  Shadow;0.5,False,-1;Cast Shadows;1;  Use Shadow Threshold;0;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;1;Built-in Fog;1;_FinalColorxAlpha;0;Meta Pass;1;Override Baked GI;0;Extra Pre Pass;0;DOTS Instancing;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Vertex Position,InvertActionOnDeselection;1;0;6;False;True;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;244;1166.589,-1121.183;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;243;1166.589,-1121.183;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;False;False;False;False;0;False;-1;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;242;1166.589,-1121.183;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;240;1166.589,-1121.183;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;245;1166.589,-1121.183;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;54;0;57;0
WireConnection;54;3;53;0
WireConnection;54;4;52;0
WireConnection;32;0;29;2
WireConnection;71;0;72;0
WireConnection;71;1;54;0
WireConnection;87;0;90;0
WireConnection;169;0;71;0
WireConnection;89;0;87;0
WireConnection;135;0;71;0
WireConnection;163;0;161;0
WireConnection;163;1;169;0
WireConnection;163;2;160;0
WireConnection;76;0;77;0
WireConnection;76;1;135;0
WireConnection;76;2;78;0
WireConnection;2;0;1;0
WireConnection;18;0;74;0
WireConnection;18;1;71;0
WireConnection;18;2;73;0
WireConnection;5;1;89;0
WireConnection;182;0;181;0
WireConnection;82;0;5;1
WireConnection;147;0;18;0
WireConnection;147;2;151;0
WireConnection;179;0;180;0
WireConnection;164;0;163;0
WireConnection;164;2;162;0
WireConnection;121;0;76;0
WireConnection;121;2;123;0
WireConnection;9;0;2;0
WireConnection;9;1;7;0
WireConnection;184;0;182;0
WireConnection;122;0;121;0
WireConnection;122;1;107;0
WireConnection;26;0;147;0
WireConnection;165;0;164;0
WireConnection;183;0;179;0
WireConnection;12;0;82;0
WireConnection;12;1;9;0
WireConnection;167;1;183;0
WireConnection;166;0;165;0
WireConnection;13;0;12;0
WireConnection;85;0;122;0
WireConnection;75;1;184;0
WireConnection;205;0;76;0
WireConnection;205;2;208;0
WireConnection;86;0;26;0
WireConnection;79;0;13;0
WireConnection;79;2;85;0
WireConnection;206;0;205;0
WireConnection;206;1;204;0
WireConnection;199;0;75;2
WireConnection;199;2;86;0
WireConnection;200;0;167;3
WireConnection;200;2;166;0
WireConnection;59;0;124;0
WireConnection;59;1;54;0
WireConnection;207;0;206;0
WireConnection;198;0;79;0
WireConnection;198;1;199;0
WireConnection;198;2;200;0
WireConnection;43;0;59;0
WireConnection;43;1;38;0
WireConnection;201;0;198;0
WireConnection;201;2;207;0
WireConnection;44;0;59;0
WireConnection;44;1;38;0
WireConnection;46;0;142;0
WireConnection;46;1;43;0
WireConnection;46;2;44;0
WireConnection;228;0;126;4
WireConnection;228;1;201;0
WireConnection;137;0;76;0
WireConnection;137;2;140;0
WireConnection;232;0;228;0
WireConnection;232;1;46;0
WireConnection;232;2;249;0
WireConnection;138;0;137;0
WireConnection;138;1;141;0
WireConnection;247;0;232;0
WireConnection;127;0;126;0
WireConnection;127;1;125;0
WireConnection;139;0;138;0
WireConnection;221;0;222;0
WireConnection;30;0;29;1
WireConnection;31;0;29;3
WireConnection;136;1;127;0
WireConnection;136;2;139;0
WireConnection;27;0;75;2
WireConnection;27;2;86;0
WireConnection;101;0;99;0
WireConnection;101;2;85;0
WireConnection;168;0;167;3
WireConnection;168;2;166;0
WireConnection;227;0;218;0
WireConnection;227;1;170;0
WireConnection;11;0;8;0
WireConnection;11;1;10;0
WireConnection;250;0;247;0
WireConnection;131;0;91;0
WireConnection;131;1;132;0
WireConnection;131;2;27;0
WireConnection;220;0;170;0
WireConnection;220;1;223;0
WireConnection;220;2;221;0
WireConnection;6;0;82;0
WireConnection;6;1;4;0
WireConnection;223;0;218;0
WireConnection;223;1;224;0
WireConnection;222;0;227;0
WireConnection;170;0;131;0
WireConnection;170;1;171;0
WireConnection;170;2;174;0
WireConnection;10;0;6;0
WireConnection;91;0;11;0
WireConnection;91;2;101;0
WireConnection;175;5;214;0
WireConnection;99;0;13;0
WireConnection;174;0;168;0
WireConnection;4;0;2;0
WireConnection;4;1;3;0
WireConnection;241;0;136;0
WireConnection;241;1;175;0
WireConnection;241;2;220;0
WireConnection;241;3;211;0
WireConnection;241;4;212;0
WireConnection;241;5;176;1
WireConnection;241;6;250;0
ASEEND*/
//CHKSM=241C25E2B9A847398FF603BB1017B35980796C14