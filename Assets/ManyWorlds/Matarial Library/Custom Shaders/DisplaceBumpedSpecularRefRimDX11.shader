// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:Bumped Specular,lico:1,lgpr:1,nrmq:1,limd:1,uamb:False,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32585,y:32774|diff-2-OUT,spec-18-OUT,gloss-25-OUT,normal-103-OUT,emission-70-OUT,lwrap-262-OUT,amdfl-47-OUT,disp-343-OUT,tess-84-OUT;n:type:ShaderForge.SFN_Multiply,id:2,x:33013,y:32399|A-12-RGB,B-5-OUT,C-11-RGB;n:type:ShaderForge.SFN_Multiply,id:3,x:32284,y:32759|A-4-OUT,B-7-OUT;n:type:ShaderForge.SFN_Lerp,id:4,x:32284,y:32893|A-10-OUT,B-8-A,T-9-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:5,x:32344,y:32575,ptlb:AO Active,ptin:_AOActive,on:False|A-6-OUT,B-230-OUT;n:type:ShaderForge.SFN_Vector1,id:6,x:32613,y:32608,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:7,x:32451,y:32793,ptlb:AO Multiplier,ptin:_AOMultiplier,glob:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:8,x:32284,y:33041,ptlb:AO Map (Alpha),ptin:_AOMapAlpha,tex:3bf4e5eff2e9d194486928c5ffa52498,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:9,x:32451,y:32996,ptlb:AO Burn,ptin:_AOBurn,glob:False,v1:1;n:type:ShaderForge.SFN_Vector1,id:10,x:32613,y:32548,v1:0;n:type:ShaderForge.SFN_Color,id:11,x:33243,y:32296,ptlb:Main Color,ptin:_MainColor,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:12,x:33243,y:32456,ptlb:Base,ptin:_Base,tex:11d4bad88ca2b8c4dbd6ecb4651d244b,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:13,x:33320,y:32668,ptlb:Specular Custom Map,ptin:_SpecularCustomMap,ntxv:0,isnm:False;n:type:ShaderForge.SFN_SwitchProperty,id:14,x:33168,y:32668,ptlb:Custom Specular,ptin:_CustomSpecular,on:False|A-12-A,B-13-A;n:type:ShaderForge.SFN_Lerp,id:15,x:32993,y:32632|A-10-OUT,B-14-OUT,T-17-OUT;n:type:ShaderForge.SFN_ValueProperty,id:17,x:33168,y:32830,ptlb:Spec Power,ptin:_SpecPower,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:18,x:32940,y:32784|A-15-OUT,B-5-OUT,C-19-OUT,D-124-RGB;n:type:ShaderForge.SFN_ValueProperty,id:19,x:33168,y:32903,ptlb:Spec Burn,ptin:_SpecBurn,glob:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:20,x:33478,y:32871,ptlb:Gloss,ptin:_Gloss,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:21,x:33302,y:32988|A-20-A,B-24-OUT;n:type:ShaderForge.SFN_Slider,id:24,x:33399,y:33067,ptlb:Shininess,ptin:_Shininess,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_SwitchProperty,id:25,x:33145,y:32998,ptlb:Use Gloss Map,ptin:_UseGlossMap,on:False|A-24-OUT,B-21-OUT;n:type:ShaderForge.SFN_Fresnel,id:41,x:32872,y:33357|EXP-45-OUT;n:type:ShaderForge.SFN_AmbientLight,id:42,x:32681,y:33508;n:type:ShaderForge.SFN_SwitchProperty,id:43,x:32507,y:33437,ptlb:Custom Ambient,ptin:_CustomAmbient,on:False|A-42-RGB,B-44-OUT;n:type:ShaderForge.SFN_Multiply,id:44,x:32681,y:33357|A-48-OUT,B-51-RGB,C-247-A;n:type:ShaderForge.SFN_ValueProperty,id:45,x:33040,y:33374,ptlb:Rim Fresnel,ptin:_RimFresnel,glob:False,v1:3;n:type:ShaderForge.SFN_ValueProperty,id:46,x:32521,y:33607,ptlb:Ambient Power,ptin:_AmbientPower,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:47,x:32351,y:33462|A-43-OUT,B-46-OUT,C-5-OUT;n:type:ShaderForge.SFN_Multiply,id:48,x:32872,y:33219|A-50-OUT,B-41-OUT;n:type:ShaderForge.SFN_ValueProperty,id:50,x:33040,y:33451,ptlb:Rim Power,ptin:_RimPower,glob:False,v1:1;n:type:ShaderForge.SFN_Color,id:51,x:32872,y:33508,ptlb:Rim Color,ptin:_RimColor,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:64,x:33505,y:33406,ptlb:Reflection Mask,ptin:_ReflectionMask,tex:0ed2929df606c1a449d8f30ec17a1a47,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Cubemap,id:65,x:33696,y:33394,ptlb:Reflection,ptin:_Reflection,cube:a596436b21c6d484bb9b3b6385e3e666,pvfc:0;n:type:ShaderForge.SFN_Fresnel,id:66,x:33530,y:33571|EXP-67-OUT;n:type:ShaderForge.SFN_ValueProperty,id:67,x:33712,y:33594,ptlb:Ref Fresnel Rim,ptin:_RefFresnelRim,glob:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:68,x:33712,y:33676,ptlb:Ref Fresnel Power,ptin:_RefFresnelPower,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:69,x:33367,y:33571|A-66-OUT,B-68-OUT;n:type:ShaderForge.SFN_Multiply,id:70,x:33303,y:33438|A-65-RGB,B-69-OUT,C-64-A,D-71-OUT,E-77-RGB;n:type:ShaderForge.SFN_ValueProperty,id:71,x:33347,y:33366,ptlb:Reflection Power,ptin:_ReflectionPower,glob:False,v1:1;n:type:ShaderForge.SFN_Color,id:77,x:33381,y:33750,ptlb:Reflection Color,ptin:_ReflectionColor,glob:False,c1:0.6029412,c2:0.7699797,c3:1,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:84,x:33145,y:33185,ptlb:Tesellation Level,ptin:_TesellationLevel,glob:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:90,x:34315,y:32849,ptlb:Displacement,ptin:_Displacement,tex:c6355e4a358959e4c9adf10cb59dcdaf,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Lerp,id:91,x:34124,y:32866|A-10-OUT,B-90-A,T-92-OUT;n:type:ShaderForge.SFN_ValueProperty,id:92,x:34219,y:33044,ptlb:Displace Burn,ptin:_DisplaceBurn,glob:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:93,x:33957,y:33116,ptlb:Displace Power,ptin:_DisplacePower,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:94,x:33874,y:32866|A-303-OUT,B-93-OUT,C-95-OUT;n:type:ShaderForge.SFN_NormalVector,id:95,x:34113,y:32751,pt:True;n:type:ShaderForge.SFN_Lerp,id:103,x:33681,y:32495|A-104-RGB,B-105-OUT,T-106-OUT;n:type:ShaderForge.SFN_Tex2d,id:104,x:33912,y:32317,ptlb:Normal,ptin:_Normal,tex:5466d8610b088e44986230ccb663dc41,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Vector3,id:105,x:33912,y:32495,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_Slider,id:106,x:33912,y:32615,ptlb:Normal Burn,ptin:_NormalBurn,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_Color,id:124,x:33710,y:32715,ptlb:Specular Color,ptin:_SpecularColor,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Max,id:229,x:31913,y:32834|A-3-OUT,B-10-OUT;n:type:ShaderForge.SFN_Min,id:230,x:31913,y:32698|A-229-OUT,B-6-OUT;n:type:ShaderForge.SFN_Tex2d,id:247,x:32802,y:33716,ptlb:Rim Mask,ptin:_RimMask,ntxv:0,isnm:False;n:type:ShaderForge.SFN_SwitchProperty,id:262,x:32029,y:33369,ptlb:Use Light Wrap,ptin:_UseLightWrap,on:False|A-10-OUT,B-278-OUT;n:type:ShaderForge.SFN_ValueProperty,id:277,x:32320,y:33802,ptlb:Light Wrap,ptin:_LightWrap,glob:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:278,x:32137,y:33711|A-279-A,B-277-OUT;n:type:ShaderForge.SFN_Tex2d,id:279,x:32320,y:33617,ptlb:Wrap Mask,ptin:_WrapMask,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Max,id:302,x:34203,y:33138|A-91-OUT,B-10-OUT;n:type:ShaderForge.SFN_Min,id:303,x:34049,y:33197|A-302-OUT,B-6-OUT;n:type:ShaderForge.SFN_Add,id:319,x:33833,y:32644|A-103-OUT,B-95-OUT;n:type:ShaderForge.SFN_Add,id:343,x:33752,y:33014|A-94-OUT,B-345-OUT;n:type:ShaderForge.SFN_ValueProperty,id:344,x:33863,y:33327,ptlb:Offset Displace,ptin:_OffsetDisplace,glob:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:345,x:33842,y:33156|A-95-OUT,B-344-OUT;proporder:11-12-104-106-124-17-19-14-13-24-25-20-5-8-7-9-71-77-65-64-67-68-46-43-45-50-51-247-262-277-279-84-90-92-93-344;pass:END;sub:END;*/

Shader "ManyWorlds/Tessellation/DisplaceRefRimAODX11" {
    Properties {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _Base ("Base", 2D) = "white" {}
        _Normal ("Normal", 2D) = "bump" {}
        _NormalBurn ("Normal Burn", Range(-1, 1)) = 0
        _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
        _SpecPower ("Spec Power", Float ) = 1
        _SpecBurn ("Spec Burn", Float ) = 1
        [MaterialToggle] _CustomSpecular ("Custom Specular", Float ) = 0.4666667
        _SpecularCustomMap ("Specular Custom Map", 2D) = "white" {}
        _Shininess ("Shininess", Range(0, 1)) = 0.5
        [MaterialToggle] _UseGlossMap ("Use Gloss Map", Float ) = 0.5
        _Gloss ("Gloss", 2D) = "white" {}
        [MaterialToggle] _AOActive ("AO Active", Float ) = 1
        _AOMapAlpha ("AO Map (Alpha)", 2D) = "white" {}
        _AOMultiplier ("AO Multiplier", Float ) = 1
        _AOBurn ("AO Burn", Float ) = 1
        _ReflectionPower ("Reflection Power", Float ) = 1
        _ReflectionColor ("Reflection Color", Color) = (0.6029412,0.7699797,1,1)
        _Reflection ("Reflection", Cube) = "_Skybox" {}
        _ReflectionMask ("Reflection Mask", 2D) = "white" {}
        _RefFresnelRim ("Ref Fresnel Rim", Float ) = 2
        _RefFresnelPower ("Ref Fresnel Power", Float ) = 1
        _AmbientPower ("Ambient Power", Float ) = 1
        [MaterialToggle] _CustomAmbient ("Custom Ambient", Float ) = 0.4191176
        _RimFresnel ("Rim Fresnel", Float ) = 3
        _RimPower ("Rim Power", Float ) = 1
        _RimColor ("Rim Color", Color) = (0.5,0.5,0.5,1)
        _RimMask ("Rim Mask", 2D) = "white" {}
        [MaterialToggle] _UseLightWrap ("Use Light Wrap", Float ) = 0
        _LightWrap ("Light Wrap", Float ) = 0
        _WrapMask ("Wrap Mask", 2D) = "white" {}
        _TesellationLevel ("Tesellation Level", Float ) = 1
        _Displacement ("Displacement", 2D) = "white" {}
        _DisplaceBurn ("Displace Burn", Float ) = 1
        _DisplacePower ("Displace Power", Float ) = 1
        _OffsetDisplace ("Offset Displace", Float ) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 5.0
            uniform float4 _LightColor0;
            uniform fixed _AOActive;
            uniform float _AOMultiplier;
            uniform sampler2D _AOMapAlpha; uniform float4 _AOMapAlpha_ST;
            uniform float _AOBurn;
            uniform float4 _MainColor;
            uniform sampler2D _Base; uniform float4 _Base_ST;
            uniform sampler2D _SpecularCustomMap; uniform float4 _SpecularCustomMap_ST;
            uniform fixed _CustomSpecular;
            uniform float _SpecPower;
            uniform float _SpecBurn;
            uniform sampler2D _Gloss; uniform float4 _Gloss_ST;
            uniform float _Shininess;
            uniform fixed _UseGlossMap;
            uniform fixed _CustomAmbient;
            uniform float _RimFresnel;
            uniform float _AmbientPower;
            uniform float _RimPower;
            uniform float4 _RimColor;
            uniform sampler2D _ReflectionMask; uniform float4 _ReflectionMask_ST;
            uniform samplerCUBE _Reflection;
            uniform float _RefFresnelRim;
            uniform float _RefFresnelPower;
            uniform float _ReflectionPower;
            uniform float4 _ReflectionColor;
            uniform float _TesellationLevel;
            uniform sampler2D _Displacement; uniform float4 _Displacement_ST;
            uniform float _DisplaceBurn;
            uniform float _DisplacePower;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float _NormalBurn;
            uniform float4 _SpecularColor;
            uniform sampler2D _RimMask; uniform float4 _RimMask_ST;
            uniform fixed _UseLightWrap;
            uniform float _LightWrap;
            uniform sampler2D _WrapMask; uniform float4 _WrapMask_ST;
            uniform float _OffsetDisplace;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float node_10 = 0.0;
                    float2 node_379 = v.texcoord0;
                    float node_6 = 1.0;
                    float3 node_95 = v.normal;
                    v.vertex.xyz +=  ((min(max(lerp(node_10,tex2Dlod(_Displacement,float4(TRANSFORM_TEX(node_379.rg, _Displacement),0.0,0)).a,_DisplaceBurn),node_10),node_6)*_DisplacePower*node_95)+(node_95*_OffsetDisplace));
                }
                float Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    return _TesellationLevel;
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o;
                    float ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts;
                    o.edge[1] = ts;
                    o.edge[2] = ts;
                    o.inside = ts;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_379 = i.uv0;
                float3 node_103 = lerp(UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(node_379.rg, _Normal))).rgb,float3(0,0,1),_NormalBurn);
                float3 normalLocal = node_103;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float node_10 = 0.0;
                float _UseLightWrap_var = lerp( node_10, (tex2D(_WrapMask,TRANSFORM_TEX(node_379.rg, _WrapMask)).a*_LightWrap), _UseLightWrap );
                float3 w = float3(_UseLightWrap_var,_UseLightWrap_var,_UseLightWrap_var)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                float3 diffuse = forwardLight * attenColor;
////// Emissive:
                float3 emissive = (texCUBE(_Reflection,viewReflectDirection).rgb*(pow(1.0-max(0,dot(normalDirection, viewDirection)),_RefFresnelRim)*_RefFresnelPower)*tex2D(_ReflectionMask,TRANSFORM_TEX(node_379.rg, _ReflectionMask)).a*_ReflectionPower*_ReflectionColor.rgb);
///////// Gloss:
                float gloss = lerp( _Shininess, (tex2D(_Gloss,TRANSFORM_TEX(node_379.rg, _Gloss)).a*_Shininess), _UseGlossMap );
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float4 node_12 = tex2D(_Base,TRANSFORM_TEX(node_379.rg, _Base));
                float node_6 = 1.0;
                float node_5 = lerp( node_6, min(max((lerp(node_10,tex2D(_AOMapAlpha,TRANSFORM_TEX(node_379.rg, _AOMapAlpha)).a,_AOBurn)*_AOMultiplier),node_10),node_6), _AOActive );
                float3 specularColor = (lerp(node_10,lerp( node_12.a, tex2D(_SpecularCustomMap,TRANSFORM_TEX(node_379.rg, _SpecularCustomMap)).a, _CustomSpecular ),_SpecPower)*node_5*_SpecBurn*_SpecularColor.rgb);
                float3 specular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                diffuseLight += (lerp( UNITY_LIGHTMODEL_AMBIENT.rgb, ((_RimPower*pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimFresnel))*_RimColor.rgb*tex2D(_RimMask,TRANSFORM_TEX(node_379.rg, _RimMask)).a), _CustomAmbient )*_AmbientPower*node_5); // Diffuse Ambient Light
                finalColor += diffuseLight * (node_12.rgb*node_5*_MainColor.rgb);
                finalColor += specular;
                finalColor += emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 5.0
            uniform float4 _LightColor0;
            uniform fixed _AOActive;
            uniform float _AOMultiplier;
            uniform sampler2D _AOMapAlpha; uniform float4 _AOMapAlpha_ST;
            uniform float _AOBurn;
            uniform float4 _MainColor;
            uniform sampler2D _Base; uniform float4 _Base_ST;
            uniform sampler2D _SpecularCustomMap; uniform float4 _SpecularCustomMap_ST;
            uniform fixed _CustomSpecular;
            uniform float _SpecPower;
            uniform float _SpecBurn;
            uniform sampler2D _Gloss; uniform float4 _Gloss_ST;
            uniform float _Shininess;
            uniform fixed _UseGlossMap;
            uniform sampler2D _ReflectionMask; uniform float4 _ReflectionMask_ST;
            uniform samplerCUBE _Reflection;
            uniform float _RefFresnelRim;
            uniform float _RefFresnelPower;
            uniform float _ReflectionPower;
            uniform float4 _ReflectionColor;
            uniform float _TesellationLevel;
            uniform sampler2D _Displacement; uniform float4 _Displacement_ST;
            uniform float _DisplaceBurn;
            uniform float _DisplacePower;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float _NormalBurn;
            uniform float4 _SpecularColor;
            uniform fixed _UseLightWrap;
            uniform float _LightWrap;
            uniform sampler2D _WrapMask; uniform float4 _WrapMask_ST;
            uniform float _OffsetDisplace;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float node_10 = 0.0;
                    float2 node_380 = v.texcoord0;
                    float node_6 = 1.0;
                    float3 node_95 = v.normal;
                    v.vertex.xyz +=  ((min(max(lerp(node_10,tex2Dlod(_Displacement,float4(TRANSFORM_TEX(node_380.rg, _Displacement),0.0,0)).a,_DisplaceBurn),node_10),node_6)*_DisplacePower*node_95)+(node_95*_OffsetDisplace));
                }
                float Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    return _TesellationLevel;
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o;
                    float ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts;
                    o.edge[1] = ts;
                    o.edge[2] = ts;
                    o.inside = ts;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_380 = i.uv0;
                float3 node_103 = lerp(UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(node_380.rg, _Normal))).rgb,float3(0,0,1),_NormalBurn);
                float3 normalLocal = node_103;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float node_10 = 0.0;
                float _UseLightWrap_var = lerp( node_10, (tex2D(_WrapMask,TRANSFORM_TEX(node_380.rg, _WrapMask)).a*_LightWrap), _UseLightWrap );
                float3 w = float3(_UseLightWrap_var,_UseLightWrap_var,_UseLightWrap_var)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                float3 diffuse = forwardLight * attenColor;
///////// Gloss:
                float gloss = lerp( _Shininess, (tex2D(_Gloss,TRANSFORM_TEX(node_380.rg, _Gloss)).a*_Shininess), _UseGlossMap );
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float4 node_12 = tex2D(_Base,TRANSFORM_TEX(node_380.rg, _Base));
                float node_6 = 1.0;
                float node_5 = lerp( node_6, min(max((lerp(node_10,tex2D(_AOMapAlpha,TRANSFORM_TEX(node_380.rg, _AOMapAlpha)).a,_AOBurn)*_AOMultiplier),node_10),node_6), _AOActive );
                float3 specularColor = (lerp(node_10,lerp( node_12.a, tex2D(_SpecularCustomMap,TRANSFORM_TEX(node_380.rg, _SpecularCustomMap)).a, _CustomSpecular ),_SpecPower)*node_5*_SpecBurn*_SpecularColor.rgb);
                float3 specular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * (node_12.rgb*node_5*_MainColor.rgb);
                finalColor += specular;
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCollector"
            Tags {
                "LightMode"="ShadowCollector"
            }
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCOLLECTOR
            #define SHADOW_COLLECTOR_PASS
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcollector
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 5.0
            uniform float _TesellationLevel;
            uniform sampler2D _Displacement; uniform float4 _Displacement_ST;
            uniform float _DisplaceBurn;
            uniform float _DisplacePower;
            uniform float _OffsetDisplace;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_COLLECTOR;
                float2 uv0 : TEXCOORD5;
                float3 normalDir : TEXCOORD6;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_COLLECTOR(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float node_10 = 0.0;
                    float2 node_381 = v.texcoord0;
                    float node_6 = 1.0;
                    float3 node_95 = v.normal;
                    v.vertex.xyz +=  ((min(max(lerp(node_10,tex2Dlod(_Displacement,float4(TRANSFORM_TEX(node_381.rg, _Displacement),0.0,0)).a,_DisplaceBurn),node_10),node_6)*_DisplacePower*node_95)+(node_95*_OffsetDisplace));
                }
                float Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    return _TesellationLevel;
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o;
                    float ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts;
                    o.edge[1] = ts;
                    o.edge[2] = ts;
                    o.inside = ts;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                SHADOW_COLLECTOR_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Cull Off
            Offset 1, 1
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 5.0
            uniform float _TesellationLevel;
            uniform sampler2D _Displacement; uniform float4 _Displacement_ST;
            uniform float _DisplaceBurn;
            uniform float _DisplacePower;
            uniform float _OffsetDisplace;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float node_10 = 0.0;
                    float2 node_382 = v.texcoord0;
                    float node_6 = 1.0;
                    float3 node_95 = v.normal;
                    v.vertex.xyz +=  ((min(max(lerp(node_10,tex2Dlod(_Displacement,float4(TRANSFORM_TEX(node_382.rg, _Displacement),0.0,0)).a,_DisplaceBurn),node_10),node_6)*_DisplacePower*node_95)+(node_95*_OffsetDisplace));
                }
                float Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    return _TesellationLevel;
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o;
                    float ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts;
                    o.edge[1] = ts;
                    o.edge[2] = ts;
                    o.inside = ts;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Bumped Specular"
    CustomEditor "ShaderForgeMaterialInspector"
}
