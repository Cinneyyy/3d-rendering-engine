using System.Drawing;

namespace src;

public interface IRenderableObject
{
    void RenderToScreen(Graphics canvas);
}