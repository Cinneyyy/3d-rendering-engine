using System.Drawing;

namespace src;

public interface IRenderableObject
{
    int renderLayer { get; protected set; }
    bool renderEnabled { get; protected set; }


    void RenderToScreen(Graphics canvas, Camera cam);

    void EnableRendering(bool enabled)
    {
        if(renderEnabled == enabled)
            return;

        renderEnabled = enabled;
        if(enabled) Renderer.RegisterRenderer(this, renderLayer);
        else Renderer.UnregisterRenderer(this, renderLayer);
    }
    void ChangeLayer(int newLayer)
    {
        if(!renderEnabled)
        {
            renderLayer = newLayer;
            return;
        }

        EnableRendering(false);
        renderLayer = newLayer;
        EnableRendering(true);
    }
}