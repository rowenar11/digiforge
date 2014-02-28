Shader "Texture Only" {
 
Properties {
	_MainTex ("Texture", 2D) = ""
}
 
SubShader {Pass {	// iPhone 3GS and later
	// GLSL combinations: 1
Program "vp" {
SubProgram "opengl " {
Keywords { }
"!!GLSL

#ifndef SHADER_API_OPENGL
    #define SHADER_API_OPENGL 1
#endif
#ifndef SHADER_API_DESKTOP
    #define SHADER_API_DESKTOP 1
#endif
#define highp
#define mediump
#define lowp
#line 9

	varying mediump vec2 uv;
 
	#ifdef VERTEX
	void main() {
		gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
		uv = gl_MultiTexCoord0.xy;
	}
	#endif
 
	#ifdef FRAGMENT
	uniform lowp sampler2D _MainTex;
	void main() {
		gl_FragColor = texture2D(_MainTex, uv);
	}
	#endif		
	"
}
SubProgram "gles " {
Keywords { }
"!!GLES

#ifndef SHADER_API_GLES
    #define SHADER_API_GLES 1
#endif
#ifndef SHADER_API_MOBILE
    #define SHADER_API_MOBILE 1
#endif
#line 9

	varying mediump vec2 uv;
 
	 
		
	
#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform highp mat4 glstate_matrix_mvp;
#define gl_Vertex _glesVertex
attribute vec4 _glesVertex;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
attribute vec4 _glesMultiTexCoord0;

 void main() {
  gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
  uv = gl_MultiTexCoord0.xy;
 }
 
#endif
#ifdef FRAGMENT

 uniform lowp sampler2D _MainTex;
 void main() {
  gl_FragColor = texture2D(_MainTex, uv);
 }
 
#endif"
}
SubProgram "glesdesktop " {
Keywords { }
"!!GLES

#ifndef SHADER_API_GLES
    #define SHADER_API_GLES 1
#endif
#ifndef SHADER_API_DESKTOP
    #define SHADER_API_DESKTOP 1
#endif
#line 9

	varying mediump vec2 uv;
 
	 
		
	
#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform highp mat4 glstate_matrix_mvp;
#define gl_Vertex _glesVertex
attribute vec4 _glesVertex;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
attribute vec4 _glesMultiTexCoord0;

 void main() {
  gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
  uv = gl_MultiTexCoord0.xy;
 }
 
#endif
#ifdef FRAGMENT

 uniform lowp sampler2D _MainTex;
 void main() {
  gl_FragColor = texture2D(_MainTex, uv);
 }
 
#endif"
}
}

#LINE 24

}}
 
SubShader {Pass {	// pre-3GS devices, including the September 2009 8GB iPod touch
	SetTexture[_MainTex]
}}
 
}