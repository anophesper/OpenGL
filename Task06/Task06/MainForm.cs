using System.Drawing;
using System.Windows.Forms;
using static Task06.OpenGL;


namespace Task06
{
    public partial class MainForm : Form
    {
        private RenderControl render;

        public MainForm()
        {
            InitializeComponent();
            CreateRenderControl();
        }

        private void CreateRenderControl()
        {
            render = new RenderControl
            {
                Location = new Point(15, 15),
                Size = new Size(600, 400),
                BackColor = Color.SlateGray,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(render);
        }
    }
}
