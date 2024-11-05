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
        // метод для обчислення першої функції
        public (double[] pointsX, double[] pointsY) FirstFunction(double xMin, double xMax, int numPoints)
        {
            double[] pointsX = new double[numPoints];
            double[] pointsY = new double[numPoints];

            //вираховуємо крок на основі діапазону X і кількості точок
            double step = (double)(xMax - xMin) / (numPoints - 1);
            // генерація точок X та обчислення відповідних значень Y
            for (int i = 0; i < numPoints; i++)
            {
                pointsX[i] = xMin + i * step;
                pointsY[i] = Math.Cos(Math.PI * pointsX[i]) / Math.Pow((Math.Sin(5 * Math.PI * Math.Pow(pointsX[i], 3)) + 1.5), 3);
            }
            return (pointsX, pointsY);
        }

        // метод для обчислення другої функції
        public (double[] pointsX, double[] pointsY) SecondFunction(double xMin, double xMax, int numPoints)
        {
            double[] pointsX = new double[numPoints];
            double[] pointsY = new double[numPoints];

            // вираховуємо крок на основі діапазону X і кількості точок
            double step = (double)(xMax - xMin) / (numPoints - 1);
            // генерація точок X та обчислення відповідних значень Y
            for (int i = 0; i < numPoints; i++)
            {
                pointsX[i] = xMin + i * step;
                pointsY[i] = Math.Tan(2 * Math.Sin(pointsX[i]));
            }
            return (pointsX, pointsY);
        }

        // метод для малювання графіку
        public void DrawGraph((double[] pointsX, double[] pointsY) points)
        {
            double[] pointsX = points.pointsX;
            double[] pointsY = points.pointsY;
            var (ymin, ymax) = GetYRange(pointsY); // отримуємо значення ymin, ymax для методу DrawDashedLine
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
                        DrawDashedLine(pointsX[i], ymin, ymax);
                        hasDrawnGap = true; // оновлюємо значення, щоб уникнути повторного малювання точки розриву на поточному сегменті
                    }
                }
            }
            EndSegment();
        }

        // метод для малювання точок перетину графіку з віссю X
        private void DrawIntersectionPoint(double x, double y)
        {
            glPointSize(6);
            glColor3d(1.0, 0.55, 0.0); // помаранчевий колір
            glBegin(GL_POINTS);
            glVertex2d(x, y);
            glEnd();
            glColor3d(0, 0, 0); // повертаємо чорний колір для основного графіку
        }

        // метод для малювання темно-зеленої пунктирної лінії на місці розриву
        private void DrawDashedLine(double x, double ymin, double ymax)
        {
            glLineWidth(1);
            glColor3d(0, 0.5, 0); // темно-зелений колір
            glEnable(GL_LINE_STIPPLE);
            glLineStipple(1, 0x00FF); // пунктирна лінія

            glBegin(GL_LINES);
            glVertex2d(x, ymin);
            glVertex2d(x, ymax);
            glEnd();

            glDisable(GL_LINE_STIPPLE);
            glColor3d(0, 0, 0); // повертаємо чорний колір для основного графіку
        }

        // метод для отримання yMin, yMax
        public (double yMin, double yMax) GetYRange(double[] pointsY)
        {
            double yMin = double.PositiveInfinity;
            double yMax = double.NegativeInfinity;

            foreach (double y in pointsY)
            {
                if (y < yMin) yMin = y;
                if (y > yMax) yMax = y;
            }

            //обмежуємо Y в межах -10, +10 якщо yMin yMax перевищують ці значення
            yMin = Math.Max(yMin, -10);
            yMax = Math.Min(yMax, 10);

            return (yMin, yMax);
        }

        // метод для малювання осей координат
        public void DrawCoordinateAxis(double xMin, double xMax, double yMin, double yMax)
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
        public void DrawBackgroundMarkup(double xMin, double xMax, double yMin, double yMax)
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
