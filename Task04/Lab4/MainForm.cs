using System;
using System.Drawing;
using System.Windows.Forms;
using static Lab4.OpenGL;

namespace Lab4
{
    public partial class MainForm : Form
    {
        private GroupBox radioButtonGroup;
        private RadioButton firstFuncRadioButton;
        private RadioButton secondFuncRadioButton;
        private RenderControl render;

        public MainForm()
        {
            InitializeComponent();
            //Cтворення елементів інтерфейсу для взаємодії з користувачем
            CreateRenderControl();
            CreateRadioButtonGroup();
        }

        // методи для створення компонентів інтерфейсу
        private void CreateRenderControl()
        {
            render = new RenderControl();
            render.Location = new System.Drawing.Point(15, 15);
            render.Size = new System.Drawing.Size(600, 400);
            render.BackColor = System.Drawing.Color.White;
            render.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(render);
        }
        
        private void CreateRadioButtonGroup()
        {
            radioButtonGroup = new GroupBox();
            radioButtonGroup.Text = "Functions";
            radioButtonGroup.Location = new System.Drawing.Point(640, 25);
            radioButtonGroup.Size = new System.Drawing.Size(105, 75);
            radioButtonGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            firstFuncRadioButton = new RadioButton();
            firstFuncRadioButton.Text = "Circle";
            firstFuncRadioButton.Location = new System.Drawing.Point(3, 23);
            firstFuncRadioButton.AutoSize = true;
            firstFuncRadioButton.Checked = true;
            firstFuncRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

            secondFuncRadioButton = new RadioButton();
            secondFuncRadioButton.Text = "Hyperbola";
            secondFuncRadioButton.Location = new System.Drawing.Point(3, 48);
            secondFuncRadioButton.AutoSize = true;
            secondFuncRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

            radioButtonGroup.Controls.Add(firstFuncRadioButton);
            radioButtonGroup.Controls.Add(secondFuncRadioButton);
            this.Controls.Add(radioButtonGroup);
        }

        // методи для відслідковування змін в компонентах
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRenderControl();
        }
                
        private void pointsCountUpDown_SelectedItemChanged(object sender, EventArgs e)
        {
            UpdateRenderControl();
        }

        // метод для оновлення значень в RenderControl
        private void UpdateRenderControl()
        {
            // перевіряємо, яка саме RadioButton вибрана
            int func = firstFuncRadioButton.Checked ? 1 : (secondFuncRadioButton.Checked ? 2 : 0);
            render.SetValues(func);
        }
    }
}
