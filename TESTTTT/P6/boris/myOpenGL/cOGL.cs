using System;
using System.Collections.Generic;
using System.Windows.Forms;

//2
using System.Drawing;

namespace OpenGL
{
    class cOGL
    {
        Control p;
        int Width;
        int Height;
        float[] lightPosition = { 0.0f, 0.0f, 1.0f, 0.0f }; // Default light position
        float[] lightDiffuse = { 1.0f, 1.0f, 1.0f, 1.0f }; // Default light color

        GLUquadric obj;

        public cOGL(Control pb)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;
            InitializeGL();
            obj = GLU.gluNewQuadric(); //!!!

            PrepareLists();
        }

        ~cOGL()
        {
            GLU.gluDeleteQuadric(obj); //!!!
            WGL.wglDeleteContext(m_uint_RC);
        }

        uint m_uint_HWND = 0;

        public uint HWND
        {
            get { return m_uint_HWND; }
        }

        uint m_uint_DC = 0;

        public uint DC
        {
            get { return m_uint_DC; }
        }
        uint m_uint_RC = 0;

        public uint RC
        {
            get { return m_uint_RC; }
        }


        void DrawOldAxes()
        {
            //for this time
            //Lights positioning is here!!!
            float[] pos = new float[4];
            pos[0] = 10; pos[1] = 10; pos[2] = 10; pos[3] = 1;
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);
            GL.glDisable(GL.GL_LIGHTING);

            //INITIAL axes
            GL.glEnable(GL.GL_LINE_STIPPLE);
            GL.glLineStipple(1, 0xFF00);  //  dotted   
            GL.glBegin(GL.GL_LINES);
            //x  RED
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(-3.0f, 0.0f, 0.0f);
            GL.glVertex3f(3.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, -3.0f, 0.0f);
            GL.glVertex3f(0.0f, 3.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, -3.0f);
            GL.glVertex3f(0.0f, 0.0f, 3.0f);
            GL.glEnd();
            GL.glDisable(GL.GL_LINE_STIPPLE);
        }
        void DrawAxes()
        {
            GL.glBegin(GL.GL_LINES);
            //x  RED
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(-3.0f, 0.0f, 0.0f);
            GL.glVertex3f(3.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, -3.0f, 0.0f);
            GL.glVertex3f(0.0f, 3.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, -3.0f);
            GL.glVertex3f(0.0f, 0.0f, 3.0f);
            GL.glEnd();
        }
        public uint[] Textures = new uint[6];

        void GenerateTextures()
        {
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glGenTextures(6, Textures);
            string[] imagesName ={ "front.bmp","back.bmp",
                                    "left.bmp","right.bmp","top.bmp","bottom.bmp",};
            for (int i = 0; i < 6; i++)
            {
                Bitmap image = new Bitmap(imagesName[i]);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY); //Y axis in Windows is directed downwards, while in OpenGL-upwards
                System.Drawing.Imaging.BitmapData bitmapdata;
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

                bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[i]);
                //2D for XYZ
                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, image.Width, image.Height,
                                                              0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

                image.UnlockBits(bitmapdata);
                image.Dispose();
            }
        }
        void drawBackGorund(int width)
        {

            GL.glPushMatrix();

            GL.glTranslatef(-width / 2, -width / 2, -width / 2);


            GL.glColor3f(1f, 1f, 1f);
            GL.glEnable(GL.GL_TEXTURE_2D);

            // front
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(0f, 0f, 0f);
            GL.glTexCoord2f(0f, 1.0f); GL.glVertex3f(0f, 0f, width);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(width, 0f, width);
            GL.glTexCoord2f(1f, 0f); GL.glVertex3f(width, 0f, 0f);
            GL.glEnd();
            // back
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(0f, width, 0f);
            GL.glTexCoord2f(0f, 1f); GL.glVertex3f(0f, width, width);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(width, width, width);
            GL.glTexCoord2f(1f, 0f); GL.glVertex3f(width, width, 0f);
            GL.glEnd();
            // left
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(0f, 0f, 0f);
            GL.glTexCoord2f(0f, 1f); GL.glVertex3f(0f, width, 0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(0f, width, width);
            GL.glTexCoord2f(1f, 0f); ; GL.glVertex3f(0f, 0f, width);
            GL.glEnd();
            // right
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(width, 0f, 0f);
            GL.glTexCoord2f(0f, 1f); GL.glVertex3f(width, 0f, width);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(width, width, width);
            GL.glTexCoord2f(1f, 0f); GL.glVertex3f(width, width, 0f);
            GL.glEnd();
            // top
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(0f, 0f, width);
            GL.glTexCoord2f(0f, 1f); GL.glVertex3f(0f, width, width);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(width, width, width);
            GL.glTexCoord2f(1f, 0f); GL.glVertex3f(width, 0f, width);
            GL.glEnd();
            // bottom
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(0f, 0f, 0f);
            GL.glTexCoord2f(0f, 1f); GL.glVertex3f(0f, width, 0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(width, width, 0f);
            GL.glTexCoord2f(1f, 0f); GL.glVertex3f(width, 0f, 0f);
            GL.glEnd();

            GL.glPopMatrix();
        }
        void DrawFloor()
        {
            GL.glEnable(GL.GL_LIGHTING);
            GL.glBegin(GL.GL_QUADS);
            //!!! for blended REFLECTION
            GL.glColor4f(0.5f, 0.5f, 1.0f, 0.5f); // Adjusted color and transparency
            GL.glVertex3d(-3, -3, 0);
            GL.glVertex3d(-3, 3, 0);
            GL.glVertex3d(3, 3, 0);
            GL.glVertex3d(3, -3, 0);
            GL.glEnd();
        }


        void DrawFigures()
        {
            //====================================
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);

            //Draw Light Source
            GL.glDisable(GL.GL_LIGHTING);
            GL.glTranslatef(pos[0], pos[1], pos[2]);
            //Yellow Light source
            GL.glColor3f(1, 1, 0);
            GLUT.glutSolidSphere(0.05, 8, 8);
            GL.glTranslatef(-pos[0], -pos[1], -pos[2]);
            //projection line from source to plane
            GL.glBegin(GL.GL_LINES);
            GL.glColor3d(0.5, 0.5, 0);
            GL.glVertex3d(pos[0], pos[1], 0);
            GL.glVertex3d(pos[0], pos[1], pos[2]);
            GL.glEnd();
            //=======================================

            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_LIGHTING);

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            GL.glRotatef(ROBOT_angle, 0, 0, 1);
            GL.glCallList(ROBOT_LIST);
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            // saw 
            GL.glLineStipple(1, 0x1243);
            GL.glLineWidth(3);
            GL.glEnable(GL.GL_LINE_STIPPLE);

            GL.glColor3f(1, 1, 0); //yellow
            GL.glBegin(GL.GL_LINES);
            float angle;
            for (int i = 0; i <= 9; i++)
            {
                angle = alpha + i * 6.283f / 10;
                GL.glVertex3d(0.5f * r * Math.Cos(angle), 0.5f * r * Math.Sin(angle), 0.01f);
                GL.glVertex3d(1.5f * r * Math.Cos(angle + 0.6), 1.5f * r * Math.Sin(angle + 0.6), 0.01f);
            }
            GL.glEnd();
            GL.glLineWidth(1);
            GL.glDisable(GL.GL_LINE_STIPPLE);
            GL.glPopMatrix();
        }
        public float[] pos = new float[4];
        public float[] ScrollValue = new float[14];
        public float zShift = 0.0f;
        public float yShift = 0.0f;
        public float xShift = 0.0f;
        public float zAngle = 0.0f;
        public float yAngle = 0.0f;
        public float xAngle = 0.0f;
        public int intOptionC = 0;
        public int intOptionB = 1;
        double[] AccumulatedRotationsTraslations = new double[16];

        public void Draw()
        {
            pos[0] = ScrollValue[11];
            pos[1] = ScrollValue[12];
            pos[2] = ScrollValue[13];
            pos[3] = 1.0f; 
            //Shadows
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;

            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);

            GL.glLoadIdentity();

            // Increase the distance between the eye position and the target position
            GLU.gluLookAt(ScrollValue[0], ScrollValue[1], ScrollValue[2] + 10, // Increase the z-coordinate by 10
                           ScrollValue[3], ScrollValue[4], ScrollValue[5],
                           ScrollValue[6], ScrollValue[7], ScrollValue[8]);

            GL.glTranslatef(0.0f, 0.0f, -1.0f);

            // Draw the background
            drawBackGorund(50); // Pass the desired width of the background here

            // Save the current matrix state
            GL.glPushMatrix();

            // Draw the object
            GL.glTranslatef(xShift, yShift, zShift); // Translate the object
            GL.glRotatef(xAngle, 1.0f, 0.0f, 0.0f); // Rotate around the x-axis
            GL.glRotatef(yAngle, 0.0f, 1.0f, 0.0f); // Rotate around the y-axis
            GL.glRotatef(zAngle, 0.0f, 0.0f, 1.0f); // Rotate around the z-axis
            DrawFigures(); // Assuming this method draws your dynamic object

            // Restore the matrix state
            GL.glPopMatrix();

            // Save the current matrix state for reflection
            GL.glPushMatrix();

            // Apply reflection transformation
            GL.glScalef(1, 1, -1); // Scale on Z axis to reverse

            //=========================================
            //only floor, draw only to STENCIL buffer
            GL.glEnable(GL.GL_STENCIL_TEST);
            GL.glStencilOp(GL.GL_REPLACE, GL.GL_REPLACE, GL.GL_REPLACE);
            GL.glStencilFunc(GL.GL_ALWAYS, 1, 0xFFFFFFFF); // draw floor always
            GL.glColorMask((byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE);
            GL.glDisable(GL.GL_DEPTH_TEST);

            DrawFloor();

            // restore regular settings
            GL.glColorMask((byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE);
            GL.glEnable(GL.GL_DEPTH_TEST);

            // reflection is drawn only where STENCIL buffer value equal to 1
            GL.glStencilFunc(GL.GL_EQUAL, 1, 0xFFFFFFFF);
            GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);

            GL.glEnable(GL.GL_STENCIL_TEST);
            //=========================================
            // Draw the reflected object
            GL.glTranslatef(xShift, yShift, -zShift); // Translate the object
            GL.glRotatef(xAngle, 1.0f, 0.0f, 0.0f); // Reflect the rotation
            GL.glRotatef(yAngle, 0.0f, 1.0f, 0.0f);
            GL.glRotatef(zAngle, 0.0f, 0.0f, 1.0f);

            // really draw floor 
            //( half-transparent ( see its color's alpha byte)))
            // in order to see reflected objects 
            GL.glDepthMask((byte)GL.GL_FALSE);
            DrawFloor();
            GL.glDepthMask((byte)GL.GL_TRUE);
            // Disable GL.GL_STENCIL_TEST to show All, else it will be cut on GL.GL_STENCIL
            GL.glDisable(GL.GL_STENCIL_TEST);

            DrawFigures(); // Assuming this method draws your dynamic object

            // Restore the matrix state
            GL.glPopMatrix();

            WGL.wglSwapBuffers(m_uint_DC);
        }




        protected virtual void InitializeGL()
        {


            m_uint_HWND = (uint)p.Handle.ToInt32();
            m_uint_DC = WGL.GetDC(m_uint_HWND);

            // Not doing the following WGL.wglSwapBuffers() on the DC will
            // result in a failure to subsequently create the RC.
            WGL.wglSwapBuffers(m_uint_DC);

            WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
            WGL.ZeroPixelDescriptor(ref pfd);
            pfd.nVersion = 1;
            pfd.dwFlags = (WGL.PFD_DRAW_TO_WINDOW | WGL.PFD_SUPPORT_OPENGL | WGL.PFD_DOUBLEBUFFER);
            pfd.iPixelType = (byte)(WGL.PFD_TYPE_RGBA);
            pfd.cColorBits = 32;
            pfd.cDepthBits = 32;
            pfd.iLayerType = (byte)(WGL.PFD_MAIN_PLANE);

            // for stencil support 
            pfd.cStencilBits = 32;


            int pixelFormatIndex = 0;
            pixelFormatIndex = WGL.ChoosePixelFormat(m_uint_DC, ref pfd);
            if (pixelFormatIndex == 0)
            {
                MessageBox.Show("Unable to retrieve pixel format");
                return;
            }

            if (WGL.SetPixelFormat(m_uint_DC, pixelFormatIndex, ref pfd) == 0)
            {
                MessageBox.Show("Unable to set pixel format");
                return;
            }
            //Create rendering context
            m_uint_RC = WGL.wglCreateContext(m_uint_DC);
            if (m_uint_RC == 0)
            {
                MessageBox.Show("Unable to get rendering context");
                return;
            }
            if (WGL.wglMakeCurrent(m_uint_DC, m_uint_RC) == 0)
            {
                MessageBox.Show("Unable to make rendering context current");
                return;
            }


            initRenderingGL();
        }

        public void OnResize()
        {
            Width = p.Width;
            Height = p.Height;
            GL.glViewport(0, 0, Width, Height);
            Draw();
        }

        protected virtual void initRenderingGL()
        {
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;
            if (this.Width == 0 || this.Height == 0)
                return;
            GL.glClearColor(0.5f, 0.5f, 0.5f, 0.0f);
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);

            GL.glViewport(0, 0, this.Width, this.Height);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();

            //nice 3D
            GLU.gluPerspective(45.0, 1.0, 0.4, 3000);


            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();

            //save the current MODELVIEW Matrix (now it is Identity)
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);
            GenerateTextures();
        }
        //test to see if its commited


        public float ARM_angle;
        public float SHOULDER_angle;
        public float ROBOT_angle;
        public float alpha;




        uint ROBOT_LIST, ARM_LIST, SHOULDER_LIST;
        float r;

        void PrepareLists()
        {
            // Default Material Properties
            float[] matAmbient = { 0.7f, 0.7f, 0.7f, 1.0f };
            float[] matDiffuse = { 0.8f, 0.8f, 0.8f, 1.0f };
            float[] matSpecular = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] highShininess = { 100.0f };

            GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, matAmbient);
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, matDiffuse);
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_SPECULAR, matSpecular);
            GL.glMaterialfv(GL.GL_FRONT, GL.GL_SHININESS, highShininess);

            float ARM_length, SHOULDER_length;
            ARM_length = 2;
            ARM_angle = 90;
            SHOULDER_length = 2.5f;
            SHOULDER_angle = 10;
            ROBOT_angle = 45;
            r = 0.2f;

            ROBOT_LIST = GL.glGenLists(3);
            ARM_LIST = ROBOT_LIST + 1;
            SHOULDER_LIST = ROBOT_LIST + 2;

            GL.glPushMatrix();
            GL.glNewList(ARM_LIST, GL.GL_COMPILE);
            GL.glColor3f(0.0f, 0.0f, 1.0f); // Blue
            GLU.gluCylinder(obj, r * 0.6, r * 0.6, ARM_length, 20, 5); // Adjusted radius
            GL.glTranslated(0, 0, ARM_length);
            // internal disk
            GL.glColor3f(1.0f, 1.0f, 0.0f); // Yellow
            GLU.gluDisk(obj, 0, r * 0.5, 20, 20);
            // external disk
            GL.glColor3f(0.0f, 1.0f, 0.0f); // Green
            GLU.gluDisk(obj, r * 0.5, r * 1.5, 20, 20);
            GL.glEndList();
            GL.glPopMatrix();

            GL.glPushMatrix();
            GL.glNewList(SHOULDER_LIST, GL.GL_COMPILE);
            GL.glColor3f(0.0f, 0.0f, 1.0f); // Blue
            GLU.gluCylinder(obj, r, r, SHOULDER_length, 20, 20);
            GL.glTranslated(0, 0, SHOULDER_length);
            GL.glColor3f(0.0f, 0.0f, 1.0f); // Blue
            GLU.gluSphere(obj, r * 1.2, 20, 20);
            GL.glEndList();
            GL.glPopMatrix();

            CreateRobotList();
        }


        public void CreateRobotList()
        {

            GL.glPushMatrix();
            GL.glNewList(ROBOT_LIST, GL.GL_COMPILE);
            GL.glColor3f(0.0f, 0.0f, 1.0f); // Blue
            GLU.gluCylinder(obj, 3 * r, 3 * r, r * 1.2, 40, 20);
            GL.glTranslated(0, 0, r * 1.2);
            GLU.gluDisk(obj, 0, 3 * r, 40, 20);
            GL.glColor3f(0.0f, 0.0f, 1.0f); // Blue
            GLU.gluSphere(obj, r * 1.2, 20, 20);
            GL.glRotatef(SHOULDER_angle, 1, 0, 0);
            GL.glCallList(SHOULDER_LIST);
            GL.glRotatef(ARM_angle, 1, 0, 0);
            GL.glCallList(ARM_LIST);
            GL.glEndList();
            GL.glPopMatrix();
        }


    }

}