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


    public static void Tick(object? sender, EventArgs args)
    {
        if(Input.KeyDown(Keys.Escape))
        {
            Window.Exit();
            return;
        }

        if(Input.KeyDown(Keys.C))
            ConsoleWindow.windowMode = ConsoleWindow.windowMode == ConsoleWindow.WindowMode.Hide ? ConsoleWindow.WindowMode.Restore : ConsoleWindow.WindowMode.Hide;

        if(Input.KeyDown(Keys.F2))
            Input.lockCursor ^= true;

        if(Input.KeyHelt(Keys.Left)) cam.rot.y -= Window.deltaTime;
        if(Input.KeyHelt(Keys.Right)) cam.rot.y += Window.deltaTime;
        if(Input.KeyHelt(Keys.Up)) cam.rot.x += Window.deltaTime;
        if(Input.KeyHelt(Keys.Down)) cam.rot.x -= Window.deltaTime;

        Vec3f move = new();
        if(Input.KeyHelt(Keys.W)) move.z += Window.deltaTime;
        if(Input.KeyHelt(Keys.S)) move.z -= Window.deltaTime;
        if(Input.KeyHelt(Keys.D)) move.x += Window.deltaTime;
        if(Input.KeyHelt(Keys.A)) move.x -= Window.deltaTime;
        if(Input.KeyHelt(Keys.Space)) move.y += Window.deltaTime;
        if(Input.KeyHelt(Keys.ShiftKey)) move.y -= Window.deltaTime;
        cam.pos += cam.forward * move.z + cam.right * move.x + cam.up * move.y;

        if(Input.KeyHelt(Keys.I)) cam.fov += Window.deltaTime;
        if(Input.KeyHelt(Keys.U)) cam.fov -= Window.deltaTime;
    }
}