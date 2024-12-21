using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Task06.OpenGL;

namespace Task06
{
    public class Manipulator
    {
        private double segmentA;
        private double segmentB;
        private double angleA; // Кут першого сегмента
        private double angleB; // Кут другого сегмента
        private double angleZ; // Новий кут для обертання навколо осі Y
        
        // Властивості для зміни параметрів
        public double AngleA { get => angleA; set => angleA = value; }
        public double AngleB { get => angleB; set => angleB = value; }
        public double AngleZ { get => angleZ; set => angleZ = value; }
        public double SegmentA { get => segmentA; set => segmentA = value; }
        public double SegmentB { get => segmentB; set => segmentB = value; }

        public Manipulator(double a, double b)
        {
            segmentA = a;
            segmentB = b;
            angleA = 0; // Початковий кут
            angleB = 0; // Початковий кут
        }

        public void Draw()
        {
            glColor3d(0.0, 0.0, 1.0);
            glRotated(angleZ, 0, 1, 0);  // Обертання навколо осі Y

            glPushMatrix();
            // Сфера на початку першого сегменту
            glTranslated(0, 0, 0);
            gluSphere(gluNewQuadric(), 0.05, 16, 16);

            // Перший сегмент
            glRotated(angleA, 0, 0, 1);
            glRotated(90, 0, 1, 0);
            DrawSegment(segmentA);
            glTranslated(0, segmentA, 0);

            // Сфера на з'єднанні сегментів
            glTranslated(0, 0, 0);
            gluSphere(gluNewQuadric(), 0.05, 16, 16);

            // Другий сегмент
            glRotated(angleB, 1, 0, 0);
            glRotated(90, 0, 1, 0);
            DrawSegment(segmentB);
            glTranslated(0, segmentB, 0);

            // Третя сфера на кінці другого сегмента
            glTranslated(0, 0, 0);
            gluSphere(gluNewQuadric(), 0.05, 16, 16);

            glPopMatrix();
        }

        private void DrawSegment(double length)
        {
            // Створюємо об'єкт циліндра
            IntPtr quadric = gluNewQuadric();

            // Встановлюємо параметри циліндра
            gluQuadricNormals(quadric, GLU_SMOOTH);
            gluQuadricDrawStyle(quadric, GLU_FILL);

            // Трансляція та орієнтація циліндра
            glPushMatrix();
            glTranslated(0, 0, 0);
            glRotated(270, 1, 0, 0);
            gluCylinder(quadric, 0.05, 0.05, length, 32, 32);
            glPopMatrix();

            gluDeleteQuadric(quadric);
        }

        // малюємо вісі
        public void Axes()
        {
            glColor3d(0.8, 0.8, 0.8);
            glBegin(GL_LINES);
            glVertex3d(0.0, 0.0, 0.0);
            glVertex3d(1.0, 0.0, 0.0);
            glVertex3d(0.0, 0.0, 0.0);
            glVertex3d(0.0, 1.0, 0.0);
            glVertex3d(0.0, 0.0, 0.0);
            glVertex3d(0.0, 0.0, 1.0);
            glEnd();
        }
    }
}
