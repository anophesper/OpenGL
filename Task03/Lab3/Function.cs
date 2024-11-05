using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab3.OpenGL;

namespace Lab3
{
    class Function
    {
        private double xMin;
        private double xMax;
        private double yMin;
        private double yMax;
        private int points;
        private int currentselectedFunc;
        private double[] pointsX, pointsY;

        //геттери та сеттери
        public double GetXMin()
        {
            return xMin;
        }
        public double GetXMax()
        {
            return xMax;
        }
        public void SetX(double minValue, double maxValue)
        {
            xMin = minValue;
            xMax = maxValue;
            if (currentselectedFunc == 1)
                FirstFunction();
            else
                SecondFunction();
            GetYRange();
        }

        public double GetYMin()
        {
            return yMin;
        }
        public double GetYMax()
        {
            return yMax;
        }
        public void SetY(double minValue, double maxValue)
        {
            yMin = minValue;
            yMax = maxValue;
        }

        public int GetPoints()
        {
            return points;
        }
        public void SetPoints(int value)
        {
            points = value;
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
        public Function (double xMin, double xMax, int points, int currentSelectedFunc)
        {
            SetPoints(points);
            SetCurrentSelectedFunc(currentSelectedFunc);
            SetX(xMin, xMax);
        }

        // метод для отримання yMin, yMax
        public void GetYRange()
        {
            yMin = 0; yMax = 0;
            foreach (double y in pointsY)
            {
                if (y < yMin) yMin = y;
                if (y > yMax) yMax = y;
            }

            //обмежуємо Y в межах -10, +10 якщо yMin yMax перевищують ці значення
            yMin = Math.Max(yMin, -10);
            yMax = Math.Min(yMax, 10);

            SetY(yMin, yMax);
        }

        // метод для обчислення першої функції
        public void FirstFunction()
        {
            pointsX = new double[points];
            pointsY = new double[points];

            //вираховуємо крок на основі діапазону X і кількості точок
            double step = (double)(xMax - xMin) / (points - 1);
            // генерація точок X та обчислення відповідних значень Y
            for (int i = 0; i < points; i++)
            {
                pointsX[i] = xMin + i * step;
                pointsY[i] = Math.Cos(Math.PI * pointsX[i]) / Math.Pow((Math.Sin(5 * Math.PI * Math.Pow(pointsX[i], 3)) + 1.5), 3);
            }
        }

        // метод для обчислення другої функції
        public void SecondFunction()
        {
            pointsX = new double[points];
            pointsY = new double[points];

            // вираховуємо крок на основі діапазону X і кількості точок
            double step = (double)(xMax - xMin) / (points - 1);
            // генерація точок X та обчислення відповідних значень Y
            for (int i = 0; i < points; i++)
            {
                pointsX[i] = xMin + i * step;
                pointsY[i] = Math.Tan(2 * Math.Sin(pointsX[i]));
            }
        }

        // метод для малювання графіку
        public void DrawGraph()
        {
            glLineWidth(2);
            glColor3d(0, 0, 0);

            bool isDrawing = false; // додаткова змінна для відслідковування чи є не закритий сегмент glBegin(GL_LINE_STRIP)
            bool hasDrawnGap = false; // додаткова змінна для уникнення малювання дубльованих розривів

            // початок малювання графіку
            void StartSegment()
            {
                if (!isDrawing)
                {
                    glBegin(GL_LINE_STRIP);
                    isDrawing = true;
                }
            }

            // завершення малювання графіку
            void EndSegment()
            {
                if (isDrawing)
                {
                    glEnd();
                    isDrawing = false;
                }
            }

            for (int i = 0; i < pointsX.Length - 1; i++)
            {
                // якщо різниця між поточною точкою та наступною перевищує 10 вважаємо це точкою розриву, або якщо значення Y виходить за межі
                if (Math.Abs(pointsY[i] - pointsY[i + 1]) < 10 && Math.Abs(pointsY[i]) < 1e3)
                {
                    StartSegment();
                    glVertex2d(pointsX[i], pointsY[i]);
                    hasDrawnGap = false; // оновлюємо значення для малювання майбутніх точок розриву

                    // якщо поточна та наступна точки мають різні знаки чи одна з них дорівнює нулю, вважаємо це точкою розриву
                    if ((pointsY[i] >= 0 && pointsY[i + 1] <= 0) || (pointsY[i] <= 0 && pointsY[i + 1] >= 0))
                    {
                        double xIntersection = pointsX[i] + (pointsX[i + 1] - pointsX[i]) * Math.Abs(pointsY[i]) / (Math.Abs(pointsY[i]) + Math.Abs(pointsY[i + 1])); // знаходимо середню точку між X, це буде точка перетину

                        // малюємо графік до точки перетину, закртваємо поточний сегмент графіку, малюємо точку перетину і відкриваємо новий сегмент, починаючи з точки перетину.
                        glVertex2d(xIntersection, 0);
                        EndSegment();
                        DrawIntersectionPoint(xIntersection, 0);
                        StartSegment();
                        glVertex2d(xIntersection, 0);
                    }
                }
                else
                {
                    // закриваємо поточний сегмент, якщо точка розриву не була намальована на поточному відрізку графіку  малюємо її
                    EndSegment();
                    if (!hasDrawnGap)
                    {
                        DrawDashedLine(pointsX[i]);
                        hasDrawnGap = true; // оновлюємо значення, щоб уникнути повторного малювання точки розриву на поточному сегменті
                    }
                }
            }
            EndSegment();
        }

        // метод для малювання точок перетину графіку з віссю X
        private static void DrawIntersectionPoint(double x, double y)
        {
            glPointSize(6);
            glColor3d(1.0, 0.55, 0.0); // помаранчевий колір
            glBegin(GL_POINTS);
            glVertex2d(x, y);
            glEnd();
            glColor3d(0, 0, 0); // повертаємо чорний колір для основного графіку
        }

        // метод для малювання темно-зеленої пунктирної лінії на місці розриву
        private void DrawDashedLine(double x)
        {
            glLineWidth(1);
            glColor3d(0, 0.5, 0); // темно-зелений колір
            glEnable(GL_LINE_STIPPLE);
            glLineStipple(1, 0x00FF); // пунктирна лінія

            glBegin(GL_LINES);
            glVertex2d(x, yMin);
            glVertex2d(x, yMax);
            glEnd();

            glDisable(GL_LINE_STIPPLE);
            glColor3d(0, 0, 0); // повертаємо чорний колір для основного графіку
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

            //використовуємо Math.Ceiling((yMin або xMin) * 2) / 2, 
            // щоб округлити yMin до найближчого більшого півочисла, 
            // що гарантує, що перша лінія починається з точки кратної 0.5.

            glBegin(GL_LINES);
            // Вертикальні лінії
            for (double x = Math.Ceiling(xMin * 2) / 2; x <= xMax; x += 0.5)
            {
                glVertex2d(x, yMin);
                glVertex2d(x, yMax);
            }

            // Горизонтальні лінії
            for (double y = Math.Ceiling(yMin * 2) / 2; y <= yMax; y += 0.5)
            {
                glVertex2d(xMin, y);
                glVertex2d(xMax, y);
            }
            glEnd();
        }
    }
}
