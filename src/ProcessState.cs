using System;
using System.Windows.Forms;

namespace src;

public static class ProcessState
{
    public static Camera cam
    {
        get => Renderer.cam;
        set => Renderer.cam = value;
    }


    static ProcessState()
    {
        EdgeMesh[] meshes = [
            ResourceLoader.edgeMeshes["cube"].Clone<EdgeMesh>(),
            ResourceLoader.edgeMeshes["cube"].Clone<EdgeMesh>()
        ];
        meshes[0].TranslateX(-1.5f);
        meshes[1].TranslateX(1.5f);
        Renderer.renderObjects.Add(meshes[0]);
        Renderer.renderObjects.Add(meshes[1]);
        Window.curr!.update += dt => {
            foreach(var m in meshes)
            {
                m.RotateY(dt);
                m.scl = new((MathF.Sin(Window.ticksPassed / Window.curr!.targeTps) + 1f) / 2f);
            }
        };

        //EdgeMesh em = ResourceLoader.edgeMeshes["th_txt"].Clone<EdgeMesh>();
        //Window.curr!.update += em.RotateY;
        //Renderer.renderObjects.Add(em);
    }


    public static void Tick(float dt)
    {
        if(Input.KeyHelt(Keys.Escape))
        {
            Window.Exit();
            return;
        }

        if(Input.KeyDown(Keys.C))
            ConsoleWindow.windowMode = ConsoleWindow.windowMode == ConsoleWindow.WindowMode.Hide ? ConsoleWindow.WindowMode.Restore : ConsoleWindow.WindowMode.Hide;

        if(Input.KeyDown(Keys.F2))
            Input.lockCursor ^= true;

        if(Input.KeyHelt(Keys.Left)) cam.rot.y -= dt;
        if(Input.KeyHelt(Keys.Right)) cam.rot.y +=dt;
        if(Input.KeyHelt(Keys.Up)) cam.rot.x += dt;
        if(Input.KeyHelt(Keys.Down)) cam.rot.x -= dt;
        cam.rot %= MathF.Tau;
        cam.rot.x = UtilFuncs.Clamp(cam.rot.x, -MathF.PI/2f, MathF.PI/2f);

        Vec3f move = new();
        if(Input.KeyHelt(Keys.W)) move.z += dt;
        if(Input.KeyHelt(Keys.S)) move.z -= dt;
        if(Input.KeyHelt(Keys.D)) move.x += dt;
        if(Input.KeyHelt(Keys.A)) move.x -= dt;
        if(Input.KeyHelt(Keys.Space)) move.y += dt;
        if(Input.KeyHelt(Keys.ShiftKey)) move.y -= dt;
        cam.pos += cam.forward * move.z + cam.right * move.x + cam.up * move.y;

        if(Input.KeyHelt(Keys.I)) cam.fov += dt;
        if(Input.KeyHelt(Keys.U)) cam.fov -= dt;
    }
}