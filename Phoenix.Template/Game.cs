using Phoenix.Framework;
using Phoenix.Framework.AssetImport;
using Phoenix.Framework.Cameras;
using Phoenix.Framework.Maths;
using Phoenix.Framework.Rendering;
using Phoenix.Framework.Rendering.Geometry.Model;
using Phoenix.Framework.Rendering.Primitives;
using Phoenix.Framework.ShaderHelpers;
using Silk.NET.Input;
using System.Numerics;
using Plane = Phoenix.Framework.Rendering.Primitives.Plane;

namespace Phoenix.Template;
public class Game : PhoenixGame
{
    private Matrix4x4 _logoWorld = Matrix4x4.Identity;
    private Model _logo = default!;
    private ShaderBasic _shaderBasic = default!;
    private ShaderPlane _shaderPlane = default!;

    private Plane _plane = default!;
    protected override void Initialize()
    {
        // Asset loading ...
        var cam = new FreeCamera(
            game: this,
            position: new Vector3(0, 0, -10),
            yaw: MathHelper.PiOver2,
            pitch: 0f,
            fov: MathHelper.PiOver2,
            nearPlane: .1f,
            farPlane: 1000f,
            aspectRatio: WindowWidth / (float)WindowHeight
        );
        cam.MouseAim = true;
        cam.SetMoveKeys(Key.W, Key.S, Key.A, Key.D, Key.Space, Key.AltLeft, Key.ShiftLeft, 2f);
        cam.SetPitchYawKeys(Key.Up, Key.Down, Key.Left, Key.Right, Vector2.One);

        Camera = cam;

        Gizmos.Enabled = true;

        _logo = AssetLoader.LoadModel("3D/phnx");

        _plane = Plane.Create(new InfoPlane { Uv = true });
        
        _shaderBasic = new ShaderBasic();
        _shaderBasic.AttachUBO(CommonUboHandle, "CommonData");

        _shaderPlane = new ShaderPlane();
        _shaderPlane.AttachUBO(CommonUboHandle, "CommonData");

    }

    protected override void Update(double deltaTime)
    {
        if (InputManager.KeyDownOnce(Key.Escape))
            Stop();

        // Game logic ...
        var t = (float)Graphics.Time + MathF.PI;
        ((FreeCamera)Camera).Update(deltaTime);
        _logoWorld = Matrix4x4.CreateScale(50f)
            * MathHelper.RotationMxFromYawPitchRoll(t,0,0);
    }

    protected override void Render(double deltaTime)
    {
        // Render logic ...
        Graphics.SetClearColor(new Vector4(0, 0, 0, 1));
        Graphics.ClearRenderTarget();
        Graphics.SetDepthTest(true);
        
        _shaderBasic.Use();
        _shaderBasic.Color.Set(new Vector3(0.85f, 0.1f, 0.1f));

        foreach (var p in _logo.Parts)
        {
            foreach(var m in p.Meshes)
            {
                _shaderBasic.World.Set(m.Transform * _logoWorld);
                m.Draw();
            }
        }

        _shaderPlane.Use();
        _shaderPlane.World.Set(Matrix4x4.CreateScale(100f) * Matrix4x4.CreateTranslation(new Vector3(0,-5f,0)));
        _plane.Draw();

    }

    protected override void RenderUI()
    {
        // UI pass...
        UI.DrawCenteredText("Welcome to Phoenix Framework! (Esc to exit)",
            position: new Vector2(WindowWidth / 2, 10),
            color: Vector4.One,
            size: 20);
    }

    protected override void OnWindowResize(Vector2 size)
    {
        // Something needs resizing...
    }

    protected override void OnClose()
    {
        // Something needs disposing...
    }
}