using System;
using System.Windows.Forms;
using static Lab3.OpenGL;

namespace Lab3
{
    public partial class MainForm : Form
    {
        private DomainUpDown xMinCountUpDown;
        private DomainUpDown xMaxCountUpDown;
        private DomainUpDown pointsCountUpDown;
        private GroupBox radioButtonGroup;
        private RadioButton firstFuncRadioButton;
        private RadioButton secondFuncRadioButton;
        private RenderControl render;
        // додаткові значення для визначення меж xMin, xMax які може задати користувач
        // це для того щоб користувач не міг вказати мінімальне значення яке дорівнює чи перевищує максимальне і навпаки
        private double lastValidXMin = -1.0;
        private double lastValidXMax = 1.0;

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
            render.BackColor = System.Drawing.Color.White;
            render.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(render);
        }

        private void CreateLabels()
        {
            Label xMinlLabel = new Label();
            xMinlLabel.Text = "X(min)";
            xMinlLabel.Location = new System.Drawing.Point(640, 50);
            xMinlLabel.AutoSize = true;
            xMinlLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.Controls.Add(xMinlLabel);

            Label xMaxlLabel = new Label();
            xMaxlLabel.Text = "X(max)";
            xMaxlLabel.Location = new System.Drawing.Point(640, 83);
            xMaxlLabel.AutoSize = true;
            xMaxlLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.Controls.Add(xMaxlLabel);

            Label pointsLabel = new Label();
            pointsLabel.Text = "Points";
            pointsLabel.Location = new System.Drawing.Point(640, 150);
            pointsLabel.AutoSize = true;
            pointsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.Controls.Add(pointsLabel);
        }

        private void CreateDomainUpDown()
        {
            xMinCountUpDown = new DomainUpDown();
            xMinCountUpDown.Location = new System.Drawing.Point(727, 47);
            xMinCountUpDown.Size = new System.Drawing.Size(57, 27);
            xMinCountUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            for (double i = -10; i <= 10; i += 0.1)
                xMinCountUpDown.Items.Add(i.ToString("F1"));

            xMinCountUpDown.SelectedItem = "-1,0";
            xMinCountUpDown.ReadOnly = true;
            xMinCountUpDown.SelectedItemChanged += xMinCountUpDown_SelectedItemChanged;
            this.Controls.Add(xMinCountUpDown);

            xMaxCountUpDown = new DomainUpDown();
            xMaxCountUpDown.Location = new System.Drawing.Point(727, 80);
            xMaxCountUpDown.Size = new System.Drawing.Size(57, 27);
            xMaxCountUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            for (double i = -10; i <= 10; i += 0.1)
                xMaxCountUpDown.Items.Add(i.ToString("F1"));

            xMaxCountUpDown.SelectedIndex = xMaxCountUpDown.Items.IndexOf("1,0");
            xMaxCountUpDown.ReadOnly = true;
            xMaxCountUpDown.SelectedItemChanged += xMaxCountUpDown_SelectedItemChanged;
            this.Controls.Add(xMaxCountUpDown);

            pointsCountUpDown = new DomainUpDown();
            pointsCountUpDown.Location = new System.Drawing.Point(727, 147);
            pointsCountUpDown.Size = new System.Drawing.Size(57, 27);
            pointsCountUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            for (int i = 3; i <= 1000; i++)
                pointsCountUpDown.Items.Add(i.ToString());

            pointsCountUpDown.SelectedItem = "100";
            pointsCountUpDown.ReadOnly = true;
            pointsCountUpDown.SelectedItemChanged += pointsCountUpDown_SelectedItemChanged;
            this.Controls.Add(pointsCountUpDown);
        }

        private void CreateRadioButtonGroup()
        {
            radioButtonGroup = new GroupBox();
            radioButtonGroup.Text = "Functions";
            radioButtonGroup.Location = new System.Drawing.Point(625, 290);
            radioButtonGroup.Size = new System.Drawing.Size(170, 75);
            radioButtonGroup.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            firstFuncRadioButton = new RadioButton();
            firstFuncRadioButton.Text = "𝑓₁(𝑥) = cos(𝜋𝑥)/\n       ((sin(5𝜋𝑥³)+1.5)³)";
            firstFuncRadioButton.Location = new System.Drawing.Point(3, 3);
            firstFuncRadioButton.AutoSize = true;
            firstFuncRadioButton.Checked = true;
            firstFuncRadioButton.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

            secondFuncRadioButton = new RadioButton();
            secondFuncRadioButton.Text = "𝑓₂(𝑥) = tg(2 sin(𝑥))";
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

        private void xMinCountUpDown_SelectedItemChanged(object sender, EventArgs e)
        {
            double selectedMin = Convert.ToDouble(xMinCountUpDown.SelectedItem);
            double selectedMax = Convert.ToDouble(xMaxCountUpDown.SelectedItem);

            // Перевіряємо, чи мінімальне значення менше або дорівнює максимальному
            if (selectedMin >= selectedMax)
            {
                MessageBox.Show("Minimum value cannot be greater than or equal to the maximum value.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                xMinCountUpDown.SelectedItem = lastValidXMin.ToString("F1"); // повертаємо до останнього валідного значення
            }
            else
            {
                lastValidXMin = selectedMin; // оновлюємо останнє валідне значення
                UpdateRenderControl();
            }
        }

        private void xMaxCountUpDown_SelectedItemChanged(object sender, EventArgs e)
        {
            double selectedMax = Convert.ToDouble(xMaxCountUpDown.SelectedItem);
            double selectedMin = Convert.ToDouble(xMinCountUpDown.SelectedItem);

            // Перевіряємо, чи максимальне значення менше або дорівнює мінімальному
            if (selectedMax <= selectedMin)
            {
                MessageBox.Show("Maximum value cannot be less than or equal to the minimum value.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                xMaxCountUpDown.SelectedItem = lastValidXMax.ToString("F1"); // повертаємо до останнього валідного значення
            }
            else
            {
                lastValidXMax = selectedMax; // оновлюємо останнє валідне значення
                UpdateRenderControl();
            }
        }

        private void pointsCountUpDown_SelectedItemChanged(object sender, EventArgs e)
        {
            UpdateRenderControl();
        }

        // метод для оновлення значень в RenderControl
        private void UpdateRenderControl()
        {
            double xMinCount = double.Parse(xMinCountUpDown.SelectedItem.ToString());
            double xMaxCount = double.Parse(xMaxCountUpDown.SelectedItem.ToString());
            int pointsCount = int.Parse(pointsCountUpDown.SelectedItem.ToString());
            
            // перевіряємо, яка саме RadioButton вибрана
            int func = firstFuncRadioButton.Checked ? 1 : (secondFuncRadioButton.Checked ? 2 : 0);
            render.SetValues(xMinCount, xMaxCount, pointsCount, func);
        }
    }
}
