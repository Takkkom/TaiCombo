using System.Drawing;
using Silk.NET.Maths;
using Silk.NET.OpenGLES;
using SkiaSharp;
using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Helpers;
using TaiCombo.Engine.Struct;
using Rectangle = System.Drawing.Rectangle;

namespace TaiCombo.Engine;

/// <summary>
/// 2Dの画像を描画するクラス
/// </summary>
public class Sprite : IDisposable
{
    /// <summary>
    /// バッファの集まり
    /// </summary>
    private static uint VAO;

    /// <summary>
    /// 頂点バッファ
    /// </summary>
    private static uint VBO;

    /// <summary>
    /// 頂点バッファの使用順バッファ
    /// </summary>
    private static uint EBO;

    /// <summary>
    /// テクスチャで使用するUV座標バッファ
    /// </summary>
    private static uint UVBO;

    /// <summary>
    /// 頂点バッファの使用順の数
    /// </summary>
    private static uint IndicesCount;

    /// <summary>
    /// シェーダー
    /// </summary>
    private static uint ShaderProgram;

    /// <summary>
    /// 移動、回転、拡大縮小に使うMatrixのハンドル
    /// </summary>
    private static int MVPID;

    /// <summary>
    /// 色合いのハンドル
    /// </summary>
    private static int ColorID;

    /// <summary>
    /// テクスチャの切り抜きのハンドル
    /// </summary>
    private static int TextureRectID;

    /// <summary>
    /// テクスチャのサイズ
    /// </summary>
    public Size TextureSize { get; private set; } 

    /// <summary>
    /// 通常の場合使用されるRect
    /// </summary>
    public Rectangle DefaultRect { get; private set; } 

    public bool Failed { get; private set; }

    /// <summary>
    /// 描画に使用する共通のバッファを作成
    /// </summary>
    public static void Init()
    {
        
        //シェーダーを作成、実際のコードはCreateShaderProgramWithShaderを見てください
        ShaderProgram = ShaderHelper.CreateShaderProgramFromSource(
            @"#version 100
            precision mediump float;

            attribute vec3 aPosition;
            attribute vec2 aUV;

            uniform mat4 mvp;

            varying vec2 texcoord;

            void main()
            {
                vec4 position = vec4(aPosition, 1.0);
                position = mvp * position;

                texcoord = vec2(aUV.x, aUV.y);
                gl_Position = position;
            }"
            ,
            @"#version 100
            precision mediump float;

            uniform vec4 color;
            uniform sampler2D texture1;
            uniform vec4 textureRect;

            varying vec2 texcoord;

            void main()
            {
                vec2 rect = vec2(textureRect.xy + (texcoord * textureRect.zw));
                gl_FragColor = texture2D(texture1, rect) * color;
            }"
        );
        //------

        //シェーダーに値を送るためのハンドルを取得------
        MVPID = GameEngine.Gl.GetUniformLocation(ShaderProgram, "mvp"); //拡大縮小、移動、回転のMatrix
        ColorID = GameEngine.Gl.GetUniformLocation(ShaderProgram, "color"); //色合い
        TextureRectID = GameEngine.Gl.GetUniformLocation(ShaderProgram, "textureRect"); //テクスチャの切り抜きの座標と大きさ
        //------
        
        //2DSprite専用のバッファーを作成する... なんとVAOは一つでOK!



        //VAOを作成----
        VAO = GameEngine.Gl.GenVertexArray();
        GameEngine.Gl.BindVertexArray(VAO);
        //----

        //VBOを作成-----
        float[] vertices = new float[] //頂点データ
        {
            //x, y, z
            -1.0f, 1.0f, 0.0f,
            1.0f, 1.0f, 0.0f,
            -1.0f, -1.0f, 0.0f,
            1.0f, -1.0f, 0.0f,
        };
        VBO = GameEngine.Gl.GenBuffer(); //頂点バッファを作る
        GameEngine.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, VBO); //頂点バッファをバインドをする
        unsafe 
        {
            fixed(float* data = vertices) 
            {
                GameEngine.Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), data, BufferUsageARB.StaticDraw); //VRAMに頂点データを送る
            }
        }

        uint locationPosition = (uint)GameEngine.Gl.GetAttribLocation(ShaderProgram, "aPosition");
        GameEngine.Gl.EnableVertexAttribArray(locationPosition); //layout (location = 0)を使用可能に
        unsafe 
        {
            GameEngine.Gl.VertexAttribPointer(locationPosition, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), (void*)0); //float3個で一つのxyzの塊として頂点を作る
        }
        //-----



        //EBOを作成------
        //普通に四角を描画すると頂点データのxyzの塊が6個も必要だけど四つだけ作成して読み込む順番をこうやって登録すればメモリが少なくなる!

        EBO = GameEngine.Gl.GenBuffer(); //頂点バッファの使用順バッファを作る
        GameEngine.Gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, EBO); //頂点バッファの使用順バッファをバインドする

        uint[] indices = new uint[] //
        {
            0, 1, 2,
            2, 1, 3
        };
        IndicesCount = (uint)indices.Length; //数を登録する
        unsafe 
        {
            fixed(uint* data = indices) 
            {
                GameEngine.Gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Length * sizeof(uint)), data, BufferUsageARB.StaticDraw); //VRAMに送る
            }
        }
        //-----

        //テクスチャの読み込みに使用するUV座標のバッファを作成、処理はVBOと大体同じ
        UVBO = GameEngine.Gl.GenBuffer();
        GameEngine.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, UVBO);

        float[] uvs = new float[] 
        {
            0.0f, 0.0f,
            1.0f, 0.0f,
            0.0f, 1.0f,
            1.0f, 1.0f,
        };
        unsafe 
        {
            fixed(float* data = uvs) 
            {
                GameEngine.Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(uvs.Length * sizeof(float)), data, BufferUsageARB.StaticDraw);
            }
        }

        uint locationUV = (uint)GameEngine.Gl.GetAttribLocation(ShaderProgram, "aUV");
        GameEngine.Gl.EnableVertexAttribArray(locationUV);
        unsafe 
        {
            GameEngine.Gl.VertexAttribPointer(locationUV, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), (void*)0);
        }
        //-----


        //バインドを解除 厳密には必須ではないが何かのはずみでバインドされたままBufferSubDataでデータが更新されたらとかされたらまあ大変-----
        GameEngine.Gl.BindVertexArray(0); 
        GameEngine.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        GameEngine.Gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
        //-----


    }

    /// <summary>
    /// 描画に使用する共通のバッファを解放
    /// </summary>
    public static void Terminate()
    {
        //ちゃんとバッファは解放すること
        GameEngine.Gl.DeleteVertexArray(VAO);
        GameEngine.Gl.DeleteBuffer(VBO);
        GameEngine.Gl.DeleteBuffer(EBO);
        GameEngine.Gl.DeleteBuffer(UVBO);
        GameEngine.Gl.DeleteProgram(ShaderProgram);
    }

    /// <summary>
    /// テクスチャのハンドル
    /// </summary>
    private uint Handle;


    /// <summary>
    /// ファイルから作成
    /// </summary>
    /// <param name="fileName"></param>
    public Sprite(string fileName)
    {
        using SKBitmap bitmap = SKBitmap.Decode(fileName); //画像ファイルを読み込む
        MakeTexture(bitmap);
    }

    /// <summary>
    /// すでに読み込んだSKBitmapから作成
    /// </summary>
    /// <param name="bitmap"></param>
    public Sprite(SKBitmap bitmap)
    {
        MakeTexture(bitmap); //こっちはそのまま
    }

    /// <summary>
    /// テクスチャを作成
    /// </summary>
    /// <param name="bitmap"></param>
    public unsafe void MakeTexture(SKBitmap bitmap)
    {
        if (bitmap == null) 
        {
            Failed = true;
            return;
        }
        TextureSize = new Size(bitmap.Width, bitmap.Height);
        DefaultRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

        void create(SKBitmap bm, bool disposeBitmap)
        {
            //テクスチャハンドルの作成-----
            Handle = GameEngine.Gl.GenTexture();
            GameEngine.Gl.BindTexture(TextureTarget.Texture2D, Handle);
            //-----

            //テクスチャのデータをVramに送る
            fixed(byte* bytes = bm.Bytes)
            {
                GameEngine.Gl.TexImage2D(TextureTarget.Texture2D, 0, (int)PixelFormat.Bgra, (uint)bm.Width, (uint)bm.Height, 0, PixelFormat.Bgra, GLEnum.UnsignedByte, bytes);
            }
            //-----

            //拡大縮小の時の補完を指定------
            GameEngine.Gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)TextureMinFilter.Nearest); //この場合は補完しない
            GameEngine.Gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int)TextureMinFilter.Nearest);
            //------

            GameEngine.Gl.BindTexture(TextureTarget.Texture2D, 0); //バインドを解除することを忘れないように
            
            if (disposeBitmap)
            {
                bm.Dispose();
            }
        }

        fixed(byte* bytes = bitmap.Bytes)
        {
            if (Thread.CurrentThread.ManagedThreadId == GameEngine.MainThreadID)
            {
                create(bitmap, false);
            }
            else 
            {
                SKBitmap bm = bitmap.Copy();
                Action createInstance = () => 
                {
                    create(bm, true);
                };
                GameEngine.ASyncActions.Add(createInstance);
                while(GameEngine.ASyncActions.Contains(createInstance))
                {

                }
            }
        }
    }
    
    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    /// <param name="rectangle"></param>
    /// <param name="flipX"></param>
    /// <param name="flipY"></param>
    /// <param name="rotation"></param>
    /// <param name="color"></param>
    public void Draw(Matrix4X4<float> mvp, RectangleF? rectangle = null, Color4? color = null, BlendType blendType = BlendType.Normal)
    {
        if (Failed) return;

        //カラーを登録------
        Color4 _color;
        if (color == null)
        {
            _color = Color4.White;
        }
        else
        {
            _color = color.Value;
        }
        //------


        //Rectを登録------
        RectangleF _rectangle;
        if (rectangle == null)
        {
            _rectangle = DefaultRect;
        }
        else
        {
            _rectangle = rectangle.Value;
        }
        //------
        
        BlendHelper.SetBlend(blendType);

        GameEngine.Gl.UseProgram(ShaderProgram);//Uniform4よりこれが先

        GameEngine.Gl.BindTexture(TextureTarget.Texture2D, Handle); //テクスチャをバインド

        //MVPを設定----
        unsafe 
        {
            GameEngine.Gl.UniformMatrix4(MVPID, 1, false, (float*)&mvp); //MVPに値を設定
        }
        //------

        GameEngine.Gl.Uniform4(ColorID, new System.Numerics.Vector4(_color.R, _color.G, _color.B, _color.A)); //変色用のカラーを設定
        
        //テクスチャの切り抜きの座標と大きさを設定
        GameEngine.Gl.Uniform4(TextureRectID, new System.Numerics.Vector4(
            _rectangle.X / TextureSize.Width, _rectangle.Y / TextureSize.Height, //始まり
            _rectangle.Width / TextureSize.Width, _rectangle.Height / TextureSize.Height)); //大きさ、終わりではない

        //描画-----
        GameEngine.Gl.BindVertexArray(VAO);
        unsafe 
        {
            GameEngine.Gl.DrawElements(PrimitiveType.Triangles, IndicesCount, DrawElementsType.UnsignedInt, (void*)0);//描画!
        }
        
        BlendHelper.SetBlend(BlendType.Normal);
        //-----
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    /// <param name="rectangle"></param>
    /// <param name="flipX"></param>
    /// <param name="flipY"></param>
    /// <param name="rotation"></param>
    /// <param name="color"></param>
    public void Draw(float x, float y, float scaleX = 1.0f, float scaleY = 1.0f, RectangleF? rectangle = null, bool flipX = false, bool flipY = false, float rotation = 0.0f, Color4? color = null, DrawOriginType drawOriginType = DrawOriginType.Left_Up, BlendType blendType = BlendType.Normal)
    {
        if (Failed) return;

        //Rectを登録------
        RectangleF _rectangle;
        if (rectangle == null)
        {
            _rectangle = DefaultRect;
        }
        else
        {
            _rectangle = rectangle.Value;
        }
        //------

        Matrix4X4<float> mvp = MatrixHelper.Get2DMatrix(x, y, scaleX, scaleY, _rectangle, flipX, flipY, rotation, drawOriginType, blendType);

        Draw(mvp, rectangle, color, blendType);
    }

    /// <summary>
    /// テクスチャを解放します
    /// シーンから抜けるときや不要になったときはこれを必ず呼び出してください
    /// </summary>
    public void Dispose()
    {
        GameEngine.Gl.DeleteTexture(Handle); //解放
    }
}