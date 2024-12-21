using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Task06
{
    public partial class RenderControl : OpenGL
    {
        double size = 1.1;
        double AspectRatio => (double)Width / Height;
        double xMin => (AspectRatio > 1) ? -size * AspectRatio : -size;
        double xMax => (AspectRatio > 1) ? size * AspectRatio : size;
        double yMin => (AspectRatio < 1) ? -size / AspectRatio : -size;
        double yMax => (AspectRatio < 1) ? size / AspectRatio : size;
        double zMin => -size * 10;
        double zMax => +size * 10;
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

            glOrtho(xMin, xMax, yMin, yMax, zMin, zMax);

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
                case Keys.Q: // Збільшення кута обертання навколо осі Z для першого сегмента
                    manipulator.AngleZ += 5;
                    break;
                case Keys.E: // Зменшення кута обертання навколо осі Z для першого сегмента
                    manipulator.AngleZ -= 5;
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

            // Позиція джерела світла (статичне положення)
            float[] lightPosition = { 1.0f, 1.0f, 1.0f, 0.0f }; // X, Y, Z, W (0.0 - нескінченно далеке джерело світла)
            glLightfv(GL_LIGHT0, GL_POSITION, lightPosition);

            // Колір світла
            float[] lightAmbient = { 0.2f, 0.2f, 0.2f, 1.0f }; // Фонове світло
            float[] lightDiffuse = { 0.8f, 0.8f, 0.8f, 1.0f }; // Розсіяне світло
            float[] lightSpecular = { 1.0f, 1.0f, 1.0f, 1.0f }; // Дзеркальне світло

            glLightfv(GL_LIGHT0, GL_AMBIENT, lightAmbient);
            glLightfv(GL_LIGHT0, GL_DIFFUSE, lightDiffuse);
            glLightfv(GL_LIGHT0, GL_SPECULAR, lightSpecular);

            // Ввімкнути матеріали
            glEnable(GL_COLOR_MATERIAL);
            glColorMaterial(GL_FRONT, GL_AMBIENT_AND_DIFFUSE);

            manipulator = new Manipulator(0.6, 0.8);
        }
    }
}

