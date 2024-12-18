using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab2.OpenGL;

namespace Lab2
{
    class Figure
    {
        double side = 100; // Сторона трикутника
        double height = 86.6; // Висота рівностороннього трикутника (sqrt(3)/2) * 100

        //метод для малювання фігури
        public void DrawFigure(double X, double Y, uint renderMode)
        {
            glLineWidth(3);
            glPointSize(5);

            // визначення вершин шестикутника
            double[] vertices = new double[]
            {
                X, Y, // центр
                X - side, Y, // ліва точка
                X - side / 2, Y - height, // нижня ліва точка
                X + side / 2, Y - height, // нижня права точка
                X + side, Y, // права точка
                X + side / 2, Y + height, // верхня права точка
                X - side / 2, Y + height  // верхня ліва точка
            };

            glBegin(renderMode);
            // Малюємо трикутники для шестикутника
            glColor3d(0, 1, 0); // зелений колір
            glVertex2d(vertices[0], vertices[1]);
            glVertex2d(vertices[2], vertices[3]);
            glVertex2d(vertices[4], vertices[5]);

            glColor3d(1, 1, 0); // жовтий колір
            glVertex2d(vertices[0], vertices[1]);
            glVertex2d(vertices[4], vertices[5]);
            glVertex2d(vertices[6], vertices[7]);

            glVertex2d(vertices[0], vertices[1]);
            glVertex2d(vertices[12], vertices[13]);
            glVertex2d(vertices[2], vertices[3]);

            glVertex2d(vertices[0], vertices[1]);
            glVertex2d(vertices[10], vertices[11]);
            glVertex2d(vertices[8], vertices[9]);

            glColor3d(1, 0, 0); // червоний колір
            glVertex2d(vertices[0], vertices[1]);
            glVertex2d(vertices[6], vertices[7]);
            glVertex2d(vertices[8], vertices[9]);

            glColor3d(0, 0, 1); // синій колір
            glVertex2d(vertices[0], vertices[1]);
            glVertex2d(vertices[10], vertices[11]);
            glVertex2d(vertices[12], vertices[13]);

            glEnd();
        }

        //метод для обчислення центральних точок фігур
        public void SetFigureCenters(int verticalCount, int horizontalCount, uint renderMode)
        {
            // метод обчислює центральні точки фігур, для кожної пари вертикальних (i) і горизонтальних (j) значень генерує координати (centerX, centerY)
            // та передає їх у метод DrawFigure для малювання фігури.
            double centerX = 0, centerY = 0;
            for (int i = 0; i < verticalCount; i++)
            {
                for (int j = 0; j < horizontalCount; j++)
                {
                    centerX = j * 150 + i * 150;
                    centerY = (i - j) * 86.6;
                    DrawFigure(centerX, centerY, renderMode);
                }
            }
        }
    }
}
