using System.Windows.Forms;

namespace src;

public class Canvas : Form
{
    public Canvas()
        => DoubleBuffered = true;


    public void Center()
        => CenterToScreen();
}