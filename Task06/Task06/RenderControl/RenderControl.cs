using System;
using System.Drawing;
using System.Windows.Forms;

namespace Task06
{
    public partial class RenderControl : OpenGL
    {
        double AspectRatio => (double)Width / Height;
        double ax = 10, ay = -20;
        double M = 1;

        private Manipulator manipulator;

        public RenderControl()
        {
            InitializeComponent();
            MouseWheel += RenderControl_MouseWheel;
            KeyDown += RenderControl_KeyDown;
        }

        private void RenderControl_Render(object sender, EventArgs e)
        {
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
            glLoadIdentity();

            glViewport(0, 0, Width, Height);

            // Встановлюємо перспективну проекцію
            glMatrixMode(GL_PROJECTION);
            glLoadIdentity();
            gluPerspective(45.0, AspectRatio, 0.1, 100.0); // Кут огляду 45°, ближня і дальня площини
            glMatrixMode(GL_MODELVIEW);

            // Камера (позиція спостерігача)
            gluLookAt(
                2.0, 2.0, 3.0, // Позиція камери
                0.0, 0.0, 0.0, // Точка, на яку дивиться камера
                0.0, 1.0, 0.0 // Вектор вгору (y-вісь)
            );

            // Встановлюємо статичну позицію світла перед обертанням сцени
            float[] lightPosition = { 1.0f, 1.0f, 1.0f, 0.0f };
            glLightfv(GL_LIGHT0, GL_POSITION, lightPosition);

            glScaled(M, M, M);
            glRotated(ax, 1, 0, 0); //обретаємо по вісі X
            glRotated(ay, 0, 1, 0); //обретаємо по вісі Y

            manipulator.Axes();
            DrawText();
            manipulator.Draw();
        }

        // малюємо вісі
        private void DrawText()
        {
            DrawText("+X", 1, 0, 0);
            DrawText("+Y", 0, 1, 0);
            DrawText("+Z", 0, 0, 1);
        }

        private void RenderControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W: // Збільшення кута першого сегмента
                    manipulator.AngleA += 5;
                    break;
                case Keys.S: // Зменшення кута першого сегмента
                    manipulator.AngleA -= 5;
                    break;
                case Keys.D: // Збільшення кута другого сегмента
                    manipulator.AngleB += 5;
                    break;
                case Keys.A: // Зменшення кута другого сегмента
                    manipulator.AngleB -= 5;
                    break;
                case Keys.Q: // Збільшення кута обертання навколо осі Y
                    manipulator.AngleY += 5;
                    break;
                case Keys.E: // Зменшення кута обертання навколо осі Y
                    manipulator.AngleY -= 5;
                    break;
            }
            Invalidate();
        }

        bool flag = false;
        Point start;

        private void RenderControl_MouseDown(object sender, MouseEventArgs e)
        {
            flag = e.Button == MouseButtons.Left;
            start = e.Location;
        }

        private void RenderControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (flag)
                flag = !(e.Button == MouseButtons.Left);
        }

        private void RenderControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (flag)
            {
                Point current = e.Location;
                ax += (current.Y - start.Y) / 2.0;
                ay += (current.X - start.X) / 2.0;
                start = current;
                Invalidate();
            }
        }

        private void RenderControl_MouseWheel(object sender, MouseEventArgs e)
        {
            M -= e.Delta / 1000.0;
            Invalidate();
        }

        private void RenderControl_ContextCreated(object sender, EventArgs e)
        {
            glEnable(GL_DEPTH_TEST); // Глибина для 3D об'єктів
            glEnable(GL_LIGHTING); // Увімкнути освітлення
            glEnable(GL_LIGHT0); // Увімкнути перше джерело світла

            // Ввімкнути матеріали
            glEnable(GL_COLOR_MATERIAL);
            glColorMaterial(GL_FRONT, GL_AMBIENT_AND_DIFFUSE);

            // Ввімкнення прозорості
            glEnable(GL_BLEND);
            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

            manipulator = new Manipulator(0.6, 0.8);
        }
    }
}
