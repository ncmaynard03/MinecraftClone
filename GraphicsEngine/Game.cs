using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GraphicsEngine;
using OpenTK.Mathematics;
using GraphicsEngine.Graphics;

internal class Game : GameWindow
{

    List<Vector3> vertices = new List<Vector3>
    {
        // Front Face
        new Vector3(0.5f,  0.5f,  0.5f), // top right
        new Vector3(0.5f, -0.5f,  0.5f), // bottom right
        new Vector3(-0.5f, -0.5f,  0.5f), // bottom left
        new Vector3(-0.5f,  0.5f,  0.5f), // top left

        // Right Face
        new Vector3(0.5f,  0.5f,  0.5f), // top right
        new Vector3(0.5f, -0.5f,  0.5f), // bottom right
        new Vector3(0.5f, -0.5f, -0.5f), // bottom left
        new Vector3(0.5f,  0.5f, -0.5f), // top left

        // Left Face
        new Vector3(-0.5f,  0.5f, -0.5f), // top right
        new Vector3(-0.5f, -0.5f, -0.5f), // bottom right
        new Vector3(-0.5f, -0.5f,  0.5f), // bottom left
        new Vector3(-0.5f,  0.5f,  0.5f), // top left

        // Back Face
        new Vector3(0.5f,  0.5f, -0.5f), // top right
        new Vector3(0.5f, -0.5f, -0.5f), // bottom right
        new Vector3(-0.5f, -0.5f, -0.5f), // bottom left
        new Vector3(-0.5f,  0.5f, -0.5f), // top left

        // Top Face
        new Vector3(0.5f,  0.5f, -0.5f), // top right
        new Vector3(0.5f,  0.5f,  0.5f), // bottom right
        new Vector3(-0.5f,  0.5f,  0.5f), // bottom left
        new Vector3(-0.5f,  0.5f, -0.5f), // top left

        // Bottom Face
        new Vector3(0.5f, -0.5f,  0.5f), // top right
        new Vector3(0.5f, -0.5f, -0.5f), // bottom right
        new Vector3(-0.5f, -0.5f, -0.5f), // bottom left
        new Vector3(-0.5f, -0.5f,  0.5f), // top left
    };

    List<Vector2> texCoords = new List<Vector2>
    {
        // Front Face
        new Vector2(1.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),

        // Right Face
        new Vector2(1.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),

        // Left Face
        new Vector2(1.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),

        // Back Face
        new Vector2(1.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),

        // Top Face
        new Vector2(1.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),

        // Bottom Face
        new Vector2(1.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),
    };

    List<uint> indices = new List<uint>
    {
        0, 1, 3,
        1, 2, 3,

        4, 5, 7,
        5, 6, 7,

        8, 9, 11,
        9, 10, 11,

        12, 13, 15, 
        13, 14, 15,

        16, 17, 19, 
        17, 18, 19,

        20, 21, 23, 
        21, 22, 23,
    };

    VBO vbo;
    VAO vao;
    IBO ibo;
    int TextureVBO;
    Shader shader;
    Texture texture;
    double time;
    Matrix4 view;
    Matrix4 projection;

    float speed = 0.05f;
    Vector3 position;
    Vector3 front;
    Vector3 up;
    Vector2 lastMousePos;
    Camera camera;
    


    public Game(int width, int height, string title)
        : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
    {
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        if (!IsFocused) return;


        base.OnUpdateFrame(args);

        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        if (input.IsKeyDown(Keys.W))
        {
            camera.Position += camera.Front * speed;
        }
        if (input.IsKeyDown(Keys.S))
        {
            camera.Position -= camera.Front * speed;

        }
        if (input.IsKeyDown(Keys.A))
        {
            camera.Position -= Vector3.Normalize(Vector3.Cross(camera.Front, up)) * speed; 
        }
        if (input.IsKeyDown(Keys.D))
        {
            camera.Position += Vector3.Normalize(Vector3.Cross(camera.Front, up)) * speed; 
        }
        if (input.IsKeyDown(Keys.Space))
        {
            camera.Position += up * speed;
        }
        if (input.IsKeyDown(Keys.LeftShift))
        {
            camera.Position -= up * speed;
        }



        var deltaMouse = lastMousePos - MousePosition;
        var mouse = MousePosition;
        float sensitivity = 0.4f;
        lastMousePos = MousePosition;
        camera.RotateRelative(deltaMouse.Y * sensitivity, deltaMouse.X * sensitivity);
        




    }

    protected override void OnLoad()
    {
        base.OnLoad();

        //Camera values
        position = new Vector3(0.0f, 0.0f, 3.0f);
        front = new Vector3(0.0f, 0.0f, -1.0f);
        up = new Vector3(0.0f, 1.0f, 0.0f);
        camera = new(position);

        view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
        projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100.0f);

        //Input states
        CursorState = CursorState.Grabbed;
        lastMousePos = MousePosition;


        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        // Generate and bind a Vertex Array Object
        vao = new();

        // Generate and bind a Vertex Buffer Object
        vbo = new(vertices);
        vao.LinkToVAO(0, 3, vbo);

        ibo = new(indices);

        shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        shader.Bind();

        
        var vertexLocation = shader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(vertexLocation);

        TextureVBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, TextureVBO);
        GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Count * sizeof(float), texCoords, BufferUsageHint.StaticDraw);

        var texCoordLocation = shader.GetAttribLocation("aTexCoord");
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(texCoordLocation);
        

        // Initialize and use the shader

        texture = new Texture("Textures/cobblestone.png");
        texture.Bind();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        time += 20.0 * args.Time;

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        shader.Bind();
        texture.Bind();

        var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(time));

        shader.SetMatrix4("model", model);
        shader.SetMatrix4("view", camera.View);
        shader.SetMatrix4("projection", projection);
        //Console.WriteLine("Camera: \n" + camera.View);
        //Console.WriteLine("Static: \n" + view);


        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y); 
        projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100.0f);

    }

    protected override void OnUnload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        GL.DeleteVertexArray(vao);
        GL.DeleteBuffer(vbo);
        GL.DeleteBuffer(ibo);
        GL.DeleteTexture(texture.ID);
        GL.DeleteProgram(shader.ID);

        base.OnUnload();

    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        base.OnMouseMove(e);

        if (IsFocused) // check to see if the window is focused  
        {
            //MousePosition = (e.X + Size.X / 2f, e.Y + Size.Y / 2f);
        }
    }


    private void CheckShaderCompile(int shader)
    {
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error compiling shader: {infoLog}");
        }
    }

    private void CheckProgramLink(int program)
    {
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(program);
            throw new Exception($"Error linking program: {infoLog}");
        }
    }
}