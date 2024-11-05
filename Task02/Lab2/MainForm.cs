using System;
using System.Windows.Forms;
using static Lab2.OpenGL;

namespace Lab2
{
    public partial class MainForm : Form
    {
        private DomainUpDown horizontalCountUpDown;
        private DomainUpDown verticalCountUpDown;
        private GroupBox radioButtonGroup;
        private RadioButton fillModeRadioButton;
        private RadioButton lineModeRadioButton;
        private RadioButton pointModeRadioButton;
        private RenderControl render;

        public MainForm()
        {
            InitializeComponent();
            //Cтворення елементів інтерфейсу для взаємодії з користувачем
            CreateRenderControl();
            CreateLabels();
            CreateDomainUpDown();
            CreateRadioButtonGroup();
        }

        // методи для створення компонентів інтерфейсу
        private void CreateRenderControl()
        {
            render = new RenderControl();
            render.Location = new System.Drawing.Point(15, 15);
            render.Size = new System.Drawing.Size(600, 400);
            render.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(render);
        }

        private void CreateLabels()
        {
            Label tileCountLabel = new Label();
            tileCountLabel.Text = "Tile Count:";
            tileCountLabel.Location = new System.Drawing.Point(668, 14);
            tileCountLabel.AutoSize = true;
            tileCountLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.Controls.Add(tileCountLabel);

            Label horizontalLabel = new Label();
            horizontalLabel.Text = "Horizontal";
            horizontalLabel.Location = new System.Drawing.Point(650, 50);
            horizontalLabel.AutoSize = true;
            horizontalLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.Controls.Add(horizontalLabel);

            Label verticalLabel = new Label();
            verticalLabel.Text = "Vertical";
            verticalLabel.Location = new System.Drawing.Point(650, 83);
            verticalLabel.AutoSize = true;
            verticalLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.Controls.Add(verticalLabel);
        }

        private void CreateDomainUpDown()
        {
            horizontalCountUpDown = new DomainUpDown();
            horizontalCountUpDown.Location = new System.Drawing.Point(737, 47);
            horizontalCountUpDown.Size = new System.Drawing.Size(50, 27);
            horizontalCountUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            for (int i = 1; i <= 100; i++)
                horizontalCountUpDown.Items.Add(i.ToString());

            horizontalCountUpDown.SelectedItem = "1";
            horizontalCountUpDown.ReadOnly = true;
            horizontalCountUpDown.SelectedItemChanged += HorizontalCountUpDown_SelectedItemChanged;
            this.Controls.Add(horizontalCountUpDown);

            verticalCountUpDown = new DomainUpDown();
            verticalCountUpDown.Location = new System.Drawing.Point(737, 80);
            verticalCountUpDown.Size = new System.Drawing.Size(50, 27);
            verticalCountUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            for (int i = 1; i <= 100; i++)
                verticalCountUpDown.Items.Add(i.ToString());

            verticalCountUpDown.SelectedItem = "1";
            verticalCountUpDown.ReadOnly = true;
            verticalCountUpDown.SelectedItemChanged += VerticalCountUpDown_SelectedItemChanged;
            this.Controls.Add(verticalCountUpDown);
        }

        private void CreateRadioButtonGroup()
        {
            radioButtonGroup = new GroupBox();
            radioButtonGroup.Text = "Mods";
            radioButtonGroup.Location = new System.Drawing.Point(653, 290);
            radioButtonGroup.Size = new System.Drawing.Size(130, 100);
            radioButtonGroup.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            fillModeRadioButton = new RadioButton();
            fillModeRadioButton.Text = "Fill Mode";
            fillModeRadioButton.Location = new System.Drawing.Point(3, 3);
            fillModeRadioButton.AutoSize = true;
            fillModeRadioButton.Checked = true;
            fillModeRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

            lineModeRadioButton = new RadioButton();
            lineModeRadioButton.Text = "Line Mode";
            lineModeRadioButton.Location = new System.Drawing.Point(3, 33);
            lineModeRadioButton.AutoSize = true;
            lineModeRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

            pointModeRadioButton = new RadioButton();
            pointModeRadioButton.Text = "Point Mode";
            pointModeRadioButton.Location = new System.Drawing.Point(3, 63);
            pointModeRadioButton.AutoSize = true;
            pointModeRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

            radioButtonGroup.Controls.Add(fillModeRadioButton);
            radioButtonGroup.Controls.Add(lineModeRadioButton);
            radioButtonGroup.Controls.Add(pointModeRadioButton);
            this.Controls.Add(radioButtonGroup);
        }

        // методи для відслідковування змін в компонентах
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRenderControl();
        }

        private void HorizontalCountUpDown_SelectedItemChanged(object sender, EventArgs e)
        {
            UpdateRenderControl();
        }

        private void VerticalCountUpDown_SelectedItemChanged(object sender, EventArgs e)
        {
            UpdateRenderControl();
        }

        // метод для оновлення значень в RenderControl
        private void UpdateRenderControl()
        {
            int horizontalCount = int.Parse(horizontalCountUpDown.SelectedItem.ToString());
            int verticalCount = int.Parse(verticalCountUpDown.SelectedItem.ToString());

            // перевіряємо, яка саме RadioButton вибрана
            uint mode = lineModeRadioButton.Checked ? GL_LINE_STRIP :
                pointModeRadioButton.Checked ? GL_POINTS : 
                GL_TRIANGLES;
            render.SetTileCounts(horizontalCount, verticalCount, mode);
        }
    }
}
