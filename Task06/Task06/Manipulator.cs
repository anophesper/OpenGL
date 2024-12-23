using System;
using static Task06.OpenGL;

namespace Task06
{
    public class Manipulator
    {
        private double segmentA;
        private double segmentB;
        private double angleA; // Кут першого сегмента
        private double angleB; // Кут другого сегмента
        private double angleY; // Новий кут для обертання навколо осі Y
        
        // Властивості для зміни параметрів
        public double AngleA { get => angleA; set => angleA = value; }
        public double AngleB { get => angleB; set => angleB = value; }
        public double AngleY { get => angleY; set => angleY = value; }
        public double SegmentA { get => segmentA; set => segmentA = value; }
        public double SegmentB { get => segmentB; set => segmentB = value; }

        public Manipulator(double a, double b)
        {
            segmentA = a;
            segmentB = b;
            angleA = 0;
            angleB = 0;
            angleY = 0;
        }

        public void Draw()
        {
            glRotated(angleY, 0, 1, 0);  // Обертання навколо осі Y
            glPushMatrix();
            DrawSphere(0.05);// Сфера на початку першого сегменту

            // Перший сегмент
            glRotated(angleA, 0, 0, 1);
            glRotated(90, 0, 1, 0);
            DrawSegment(segmentA);
            glTranslated(0, segmentA, 0);
            DrawSphere(0.05);// Сфера на з'єднанні сегментів

            // Другий сегмент
            glRotated(angleB, 1, 0, 0);
            glRotated(90, 0, 1, 0);
            DrawSegment(segmentB);
            glTranslated(0, segmentB, 0);
            DrawSphere(0.05);// Третя сфера на кінці другого сегмента

            glPopMatrix();
        }

        private void DrawSegment(double length)
        {
            // Створюємо об'єкт циліндра
            IntPtr quadric = gluNewQuadric();

            // Матеріал для сегмента
            float[] ambient = { 0.2f, 0.2f, 0.5f, 1.0f }; // Синюватий відтінок
            float[] diffuse = { 0.0f, 0.5f, 1.0f, 0.8f }; // Прозорість 0.8
            float[] specular = { 1.0f, 1.0f, 1.0f, 1.0f }; // Білі відблиски
            float shininess = 64.0f; // Високий блиск
            ApplyMaterial(ambient, diffuse, specular, shininess, diffuse[3]);

            // Трансляція та орієнтація циліндра
            glPushMatrix();
            glTranslated(0, 0, 0);
            glRotated(270, 1, 0, 0);
            gluCylinder(quadric, 0.05, 0.05, length, 32, 32);
            glPopMatrix();
            gluDeleteQuadric(quadric);
        }

        private void DrawSphere(double radius)
        {
            IntPtr quadric = gluNewQuadric();

            // Матеріал для сфер
            float[] ambient = { 0.2f, 0.2f, 0.5f, 1.0f }; // Синюватий відтінок
            float[] diffuse = { 0.0f, 0.5f, 1.0f, 0.8f }; // Прозорість 0.8
            float[] specular = { 1.0f, 1.0f, 1.0f, 1.0f }; // Білі відблиски
            float shininess = 64.0f; // Високий блиск
            ApplyMaterial(ambient, diffuse, specular, shininess, diffuse[3]);

            // Малюємо сферу
            gluSphere(quadric, radius, 16, 16);
            gluDeleteQuadric(quadric);
        }

        private void ApplyMaterial(float[] ambient, float[] diffuse, float[] specular, float shininess, float alpha)
        {
            // Встановлення прозорості
            glEnable(GL_BLEND);
            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

            // Встановлення матеріалів
            glMaterialfv(GL_FRONT, GL_AMBIENT, ambient); // Фонове світло
            glMaterialfv(GL_FRONT, GL_DIFFUSE, diffuse); // Розсіяне світло
            glMaterialfv(GL_FRONT, GL_SPECULAR, specular); // Дзеркальне світло
            glMaterialf(GL_FRONT, GL_SHININESS, shininess); // Блискучість

            // Встановлення кольору з прозорістю
            glColor4f(diffuse[0], diffuse[1], diffuse[2], alpha);
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
