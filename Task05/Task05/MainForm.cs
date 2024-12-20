using System.Drawing;
using System.Windows.Forms;

namespace Task05
{
    public partial class MainForm : Form
    {
        private RenderControl render;

        public MainForm()
        {
            InitializeComponent();
            CreateRenderControl();
            CreateToggleButton();
            CreateRadioButtons();
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

        private void CreateToggleButton()
        {
            // Створення кнопки для включення/вимкнення площини відсічення
            Button toggleClipPlaneButton = new Button
            {
                Text = "Включити площину відсічення",
                Location = new Point(635, 50),
                Size = new Size(150, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Обробник натискання кнопки
            toggleClipPlaneButton.Click += (sender, e) =>
            {
                if (toggleClipPlaneButton.Text == "Включити площину відсічення")
                {
                    render.EnableClippingPlane(true);  // Увімкнути площину відсічення
                    toggleClipPlaneButton.Text = "Вимкнути площину відсічення"; // Оновлення тексту кнопки
                }
                else
                {
                    render.EnableClippingPlane(false);  // Вимкнути площину відсічення
                    toggleClipPlaneButton.Text = "Включити площину відсічення"; // Оновлення тексту кнопки
                }
            };

            this.Controls.Add(toggleClipPlaneButton);
        }

        private void CreateRadioButtons()
        {
            RadioButton wireframeButton = new RadioButton
            {
                Text = "Каркас",
                Location = new Point(635, 100),
                Checked = true, // За замовчуванням вибрано Wireframe
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            wireframeButton.CheckedChanged += (sender, e) =>
            {
                render.SetRenderingMode(true); // True - Wireframe
                render.Invalidate();
            };

            RadioButton solidButton = new RadioButton
            {
                Text = "Суцільний",
                Location = new Point(635, 130),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            solidButton.CheckedChanged += (sender, e) =>
            {
                render.SetRenderingMode(false); // False - Solid
                render.Invalidate();
            };

            this.Controls.Add(wireframeButton);
            this.Controls.Add(solidButton);
        }
    }
}
