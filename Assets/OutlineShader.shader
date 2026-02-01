Shader "Sprites/Outline2D"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0.2, 0.8, 1, 1)
        _OutlineSize ("Outline Size", Float) = 1.0
        _OutlineEnabled ("Outline Enabled", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Sprite"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color    : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _Color;
            float4 _OutlineColor;
            float _OutlineSize;
            float _OutlineEnabled;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;

                if (_OutlineEnabled < 0.5)
                    return col;

                if (col.a > 0)
                    return col;

                float2 offset = _MainTex_TexelSize.xy * _OutlineSize;

                float alpha =
                    tex2D(_MainTex, i.texcoord + float2( offset.x, 0)).a +
                    tex2D(_MainTex, i.texcoord + float2(-offset.x, 0)).a +
                    tex2D(_MainTex, i.texcoord + float2(0,  offset.y)).a +
                    tex2D(_MainTex, i.texcoord + float2(0, -offset.y)).a;

                if (alpha > 0)
                    return _OutlineColor;

                return col;
            }
            ENDCG
        }
    }
}
