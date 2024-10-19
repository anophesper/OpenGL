using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;

namespace Lab1
{
    public partial class RenderControl : OpenGL
    {
        Draw draw;
        static double x1 = -3.5;
        static double x2 = 1;
        static double y1 = -0.5;
        static double y2 = 1.5;

        double[] axesXandY = { x1 - 0.5, y2 + 0.5, x1 - 0.5, y1 - 0.5, x2 + 0.5, y1 - 0.5 };
        double[] arrowsY = { -4.1, 1.8, -4, 2, -3.9, 1.8 };
        double[] arrowsX = { 1.3, -0.9, 1.5, -1, 1.3, -1.1, };
        double[] lineLoop = { x1, 0, x1, 1, -2.5, y2, -2, 0.5, -2, y1 };
        double[] points = { (x1 + 2.5), 0, (x1 + 2.5), 1, (-2.5 + 2.5), y2, (-2 + 2.5), 0.5, (-2 + 2.5), y1 };

        public RenderControl()
        {
            InitializeComponent();
        }
        
        private void RenderControl_Render(object sender, EventArgs e)
        {
            glClear(GL_COLOR_BUFFER_BIT);
            glLoadIdentity();
            glViewport(0, 0, Width, Height);
            gluOrtho2D(x1 - 1, x2 + 1, y1 - 1, y2 + 1);

            draw.DrawBackgroundMarkup();//ДОДАЄМО ФОНОВУ РОЗМІТКУ

            glLineWidth(3);
            glColor3d(0, 0, 0);

            draw.DrawItem(GL_LINE_STRIP, axesXandY);//СТВОРЮЄМО ВІСІ X ТА Y
            draw.DrawItem(GL_LINE_STRIP, arrowsY);//СТРІЛОЧКА ДЛЯ ВІСІ Y
            draw.DrawItem(GL_LINE_STRIP, arrowsX);//СТРІЛОЧКА ДЛЯ ВІСІ X
            draw.DrawStreaks();//ДОДАЄМО РИСОЧКИ

            DrawText("x1", -3.55, -1.3);
            DrawText("x2", 0.95, -1.3);
            DrawText("y1", -4.3, -0.55);
            DrawText("y2", -4.3, 1.45);

            glColor3d(0, 0, 1);
            glPointSize(5);
            glDisable(GL_POINT_SMOOTH);  //КВАДРАТНІ ТОЧКИ

            draw.DrawItem(GL_LINE_LOOP, lineLoop); //СТВОРЮЄМО ФІГУРУ
            draw.DrawItem(GL_POINTS, points); //СТВОРЮЄМО точки
        }

        private void RenderControl_ContextCreated(object sender, EventArgs e)
        {
            draw = new Draw(x1, x2, y1, y2);
        }
    }
}
