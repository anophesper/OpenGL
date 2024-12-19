using System;
using System.Windows.Forms;
using System.Drawing;

namespace Task05
{
    public partial class RenderControl : OpenGL
    {
        double size = 5.1;
        double AspectRatio => (double)Width / Height;
        double xMin => (AspectRatio > 1) ? -size * AspectRatio : -size;
        double xMax => (AspectRatio > 1) ? size * AspectRatio : size;
        double yMin => (AspectRatio < 1) ? -size / AspectRatio : -size;
        double yMax => (AspectRatio < 1) ? size / AspectRatio : size;
        double zMin => -size * 10;
        double zMax => +size * 10;
        double ax = 10, ay = -20;
        double M = 1;

        public RenderControl()
        {
            InitializeComponent();
            MouseWheel += RenderControl_MouseWheel;
        }

        private void RenderControl_MouseWheel(object sender, MouseEventArgs e)
        {
            M -= e.Delta / 1000.0;
            Invalidate();
        }

        private void RenderControl_Render(object sender, EventArgs e)
        {
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
            glLoadIdentity();

            glViewport(0, 0, Width, Height);

            glOrtho(xMin, xMax, yMin, yMax, zMin, zMax);

            glScaled(M, M, M);
            glRotated(ax, 1, 0, 0); //обретаємо по вісі X
            glRotated(ay, 0, 1, 0); //обретаємо по вісі Y

            SetClippingPlane();

            glEnable(GL_DEPTH_TEST); // Увімкнення перевірки глибини

            Axes();
            Grid();
            DrawFigures();
        }

        // малюємо вісі
        private void Axes()
        {
            glBegin(GL_LINES);
            glVertex3d(0.0, 0.0, 0.0);
            glVertex3d(1.0, 0.0, 0.0);
            glVertex3d(0.0, 0.0, 0.0);
            glVertex3d(0.0, 1.0, 0.0);
            glVertex3d(0.0, 0.0, 0.0);
            glVertex3d(0.0, 0.0, 1.0);
            glEnd();

            DrawText("+X", 1, 0, 0);
            DrawText("+Y", 0, 1, 0);
            DrawText("+Z", 0, 0, 1);
        }

        //малюємо сітку X0Z
        private void Grid()
        {
            int gridSize = 10;
            double step = 1.0;

            glColor3d(0.8, 0.8, 0.8);

            // Малюємо сітку в площині X0Z
            glBegin(GL_LINES);
            for (double x = -gridSize; x <= gridSize; x += step)
            {
                // Вертикальні лінії (по осі Z)
                glVertex3d(x, 0.0, -gridSize);
                glVertex3d(x, 0.0, gridSize);
            }

            for (double z = -gridSize; z <= gridSize; z += step)
            {
                // Горизонтальні лінії (по осі X)
                glVertex3d(-gridSize, 0.0, z);
                glVertex3d(gridSize, 0.0, z);
            }
            glEnd();
        }

        private bool isWireframe = true; // За замовчуванням каркас

        // метод для зміни режиму відображення
        public void SetRenderingMode(bool wireframe)
        {
            isWireframe = wireframe;
        }

        // метод для малювання фігур
        private void DrawFigures()
        {
            if (!isWireframe)
            {
                glEnable(GL_COLOR_MATERIAL);
                SetupLighting(); // додаємо освітлення
            }
            else
            {
                glDisable(GL_COLOR_MATERIAL);
                glDisable(GL_LIGHTING); // вимикаємо освітлення якщо обраний каркас
            }

            // Сфера
            glPushMatrix();
            glTranslated(4.0, 1.5, 2.5);
            if (isWireframe) 
                glPolygonMode(GL_FRONT_AND_BACK, GL_LINE); // каркас
            else
                glPolygonMode(GL_FRONT_AND_BACK, GL_FILL); // повна фігура
            gluSphere(gluNewQuadric(), 2.0, 20, 20);
            glPopMatrix();

            // Циліндр
            glPushMatrix();
            glRotated(90, 1, 0, 0);
            glTranslated(-3.5, 1.5, -4.5);
            if (isWireframe)
                glPolygonMode(GL_FRONT_AND_BACK, GL_LINE); // каркас
            else
                glPolygonMode(GL_FRONT_AND_BACK, GL_FILL); // повна фігура
            gluCylinder(gluNewQuadric(), 1.5, 1.5, 3.0, 20, 20);
            glPopMatrix();

            // Диск
            glPushMatrix();
            glRotated(90, 0, 1, 0);
            glTranslated(3.5, -1.0, -3.5);
            if (isWireframe)
                glPolygonMode(GL_FRONT_AND_BACK, GL_LINE); // каркас
            else
                glPolygonMode(GL_FRONT_AND_BACK, GL_FILL); // повна фігура
            gluDisk(gluNewQuadric(), 1.5, 3.0, 20, 20);
            glPopMatrix();
        }

        // метод для налаштування світла
        private void SetupLighting()
        {
            glEnable(GL_LIGHTING);
            glEnable(GL_LIGHT0);

            // параметри світла
            float[] lightPosition = { 5.0f, 5.0f, 5.0f, 1.0f };
            float[] lightColor = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] lightAmbient = { 0.1f, 0.1f, 0.1f, 1.0f };
            float[] lightSpecular = { 1.0f, 1.0f, 1.0f, 1.0f };

            // встановлюємо позицію, дифузне та спекулярне світло
            glLightfv(GL_LIGHT0, GL_POSITION, lightPosition);
            glLightfv(GL_LIGHT0, GL_DIFFUSE, lightColor);
            glLightfv(GL_LIGHT0, GL_SPECULAR, lightSpecular);
            glLightfv(GL_LIGHT0, GL_AMBIENT, lightAmbient);

            glEnable(GL_COLOR_MATERIAL);
            glColorMaterial(GL_FRONT_AND_BACK, GL_DIFFUSE);
        }

        private bool isClippingPlaneEnabled = false;

        public void EnableClippingPlane(bool enable)
        {
            isClippingPlaneEnabled = enable;
            Invalidate();
        }

        //метод для відтину
        private void SetClippingPlane()
        {
            if (isClippingPlaneEnabled)
            {
                double[] planeEquation = { 0.0, 1.0, 0.0, 0.01 };
                glClipPlane(GL_CLIP_PLANE0, planeEquation);
                glEnable(GL_CLIP_PLANE0); // активуємо площину відсічення
            }
            else
            {
                glDisable(GL_CLIP_PLANE0); // вимикаємо площину відсічення
            }
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
    }
}
