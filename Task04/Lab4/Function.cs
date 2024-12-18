using System;
using System.Collections.Generic;
using System.Drawing;
using static Lab4.OpenGL;

namespace Lab4
{
    internal class Function
    {
        private static double xMin = -10.0;
        private static double xMax = 10.0;
        private static double yMin = -10.0;
        private static double yMax = 10.0;
        private  static int points = 1000;
        private int currentselectedFunc;
        private double[] pointsX, pointsY;

        public double GetXMin()
        {
            return xMin;
        }
        public double GetXMax()
        {
            return xMax;
        }
        public double GetYMin()
        {
            return yMin;
        }
        public double GetYMax()
        {
            return yMax;
        }

        public double[] GetPointsX()
        {
            return pointsX;
        }
        public double[] GetPointsY()
        {
            return pointsY;
        }
        public int GetPoints()
        {
            return points;
        }

        public int GetCurrentSelectedFunc()
        {
            return currentselectedFunc;
        }
        public void SetCurrentSelectedFunc(int value)
        {
            currentselectedFunc = value;
        }

        //конструктор класу 
        public Function(int currentSelectedFunc)
        {
            SetCurrentSelectedFunc(currentSelectedFunc);
        }

        // метод для обчислення кола (параметричне подання)
        public void FirstFunction()
        {
            pointsX = new double[points];
            pointsY = new double[points];

            double h = 0; // центр кола (x-координата)
            double k = 0; // центр кола (y-координата)
            double r = 5; // радіус кола

            // крок зміни параметра t
            double step = 2 * Math.PI / (points - 1);

            // обчислюємо точки X і Y
            for (int i = 0; i < points; i++)
            {
                double t = i * step; // параметр t
                pointsX[i] = h + r * Math.Cos(t);
                pointsY[i] = k + r * Math.Sin(t);
            }
        }

        // метод для обчислення гіперболи (явне подання)
        public void SecondFunction()
        {
            pointsX = new double[points];
            pointsY = new double[points];

            // Масштаб для Y і X
            double a = 5; // Зменшуємо a, щоб контролювати вертикальне розтягнення
            double b = 5; // Можна експериментувати з різними значеннями для кращого вигляду

            // Визначаємо діапазон параметра t
            double step = 4.0 / (points - 1); // t від -2 до 2 (можна збільшити діапазон)

            // Обчислюємо точки X і Y
            for (int i = 0; i < points; i++)
            {
                double t = -2 + i * step; // параметр t
                pointsX[i] = b * t;       // X координата
                pointsY[i] = a * t * t;   // Y координата
            }
        }

        // метод для малювання графіку
        public void DrawGraph()
        {
            glLineWidth(2);
            glColor3d(0, 0, 0);

            if(currentselectedFunc == 1)
                FirstFunction();
            else 
                SecondFunction();
            //КОД МАЛЮВАННЯ ГРАФІКУ
            glBegin(GL_LINES);
            for(int i = 0; i < pointsX.Length - 1; i++) 
            {
                glVertex2d(pointsX[i], pointsY[i]);
                glVertex2d(pointsX[i+1], pointsY[i+1]);
            }
            glEnd();
        }

        // метод для малювання точок перетину графіку з віссю X
        public void DrawIntersectionPoints(List<PointF> points)
        {
            glPointSize(6);
            glColor3d(1.0, 0.55, 0.0); // помаранчевий колір
            glBegin(GL_POINTS);
            foreach (var point in points)
            {
                glVertex2d(point.X, point.Y);
            }
            glEnd();
            glColor3d(0, 0, 0); // повертаємо чорний колір
        }

        // метод для малювання осей координат
        public void DrawCoordinateAxis()
        {
            glLineWidth(2);
            glColor3d(0.25, 0.25, 0.25);

            glBegin(GL_LINES);
            // Вісь X
            glVertex2d(xMin, 0);
            glVertex2d(xMax, 0);
            // Вісь Y
            glVertex2d(0, yMin);
            glVertex2d(0, yMax);
            glEnd();
        }

        // метод для малювання фонової розмітки
        public void DrawBackgroundMarkup()
        {
            glLineWidth(1);
            glColor3d(0.75, 0.75, 0.75);

            glBegin(GL_LINES);
            // Вертикальні лінії
            for (double x = xMin; x <= xMax; x += 1.0)
            {
                glVertex2d(x, yMin);
                glVertex2d(x, yMax);
            }

            // Горизонтальні лінії
            for (double y = yMin; y <= yMax; y += 1.0)
            {
                glVertex2d(xMin, y);
                glVertex2d(xMax, y);
            }
            glEnd();
        }
    }
}
