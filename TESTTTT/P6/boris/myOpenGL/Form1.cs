using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenGL;
using System.Runtime.InteropServices; 

namespace myOpenGL
{
    public partial class Form1 : Form
    {
        cOGL cGL;

        public Form1()
        {

            InitializeComponent();
            cGL = new cOGL(panel1);
            //apply the bars values as cGL.ScrollValue[..] properties 
                                         //!!!
            hScrollBarScroll(hScrollBar1, null);
            hScrollBarScroll(hScrollBar2, null);
            hScrollBarScroll(hScrollBar3, null);
            hScrollBarScroll(hScrollBar4, null);
            hScrollBarScroll(hScrollBar5, null);
            hScrollBarScroll(hScrollBar6, null);
            hScrollBarScroll(hScrollBar7, null);
            hScrollBarScroll(hScrollBar8, null);
            hScrollBarScroll(hScrollBar9, null);
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            cGL.Draw();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            //cGL.OnResize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void hScrollBarScroll(object sender, ScrollEventArgs e)
        {
            cGL.intOptionC = 0;
            HScrollBar hb = (HScrollBar)sender;
            int n = int.Parse(hb.Name.Substring(hb.Name.Length - 1));
            cGL.ScrollValue[n - 1] = (hb.Value - 100) / 10.0f;
            if (e != null)
                cGL.Draw();
        }

        public float[] oldPos = new float[7];

        private void numericUpDownValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nUD = (NumericUpDown)sender;
            int i = int.Parse(nUD.Name.Substring(nUD.Name.Length - 1));
            int pos = (int)nUD.Value; 
            switch(i)
            {
                case 1:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.xShift += 0.25f;
                        cGL.intOptionC = 4;
                    }
                    else
                    {
                        cGL.xShift -= 0.25f;
                        cGL.intOptionC = -4;
                    }
                    break;
                case 2:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.yShift += 0.25f;
                        cGL.intOptionC = 5;
                    }
                    else
                    {
                        cGL.yShift -= 0.25f;
                        cGL.intOptionC = -5;
                    }
                    break;
                case 3:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.zShift += 0.25f;
                        cGL.intOptionC = 6;
                    }
                    else
                    {
                        cGL.zShift -= 0.25f;
                        cGL.intOptionC = -6;
                    }
                    break;
                case 4:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.xAngle += 5;
                        cGL.intOptionC = 1;
                    }
                    else
                    {
                        cGL.xAngle -= 5;
                        cGL.intOptionC = -1;
                    }
                    break;
                case 5:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.yAngle += 5;
                        cGL.intOptionC = 2;
                    }
                    else
                    {
                        cGL.yAngle -= 5;
                        cGL.intOptionC = -2;
                    }
                    break;
                case 6: 
	                if (pos>oldPos[i-1]) 
	                {
		                cGL.zAngle+=5;
		                cGL.intOptionC=3;
	                }
	                else
	                {
                        cGL.zAngle -= 5;
                        cGL.intOptionC = -3;
                    }
                    break;
            }
            cGL.Draw();
            oldPos[i - 1] = pos;
            cGL.intOptionC = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cGL.alpha -= 15;
            cGL.Draw();
        }

        int oldPos7 = 0;
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            int pos = (int)numericUpDown7.Value;  
	        if (pos>oldPos7)
                cGL.ROBOT_angle += 10;
	        else
                cGL.ROBOT_angle -= 10;
            cGL.Draw();
            oldPos7=pos;
        }

        int oldPos8 = 0;
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            int pos = (int)numericUpDown8.Value;
            if (pos > oldPos8)
                cGL.SHOULDER_angle += 10;
            else
                cGL.SHOULDER_angle -= 10;
            cGL.CreateRobotList();
            cGL.Draw();
            oldPos8 = pos;
        }

        int oldPos9 = 0;
        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            int pos = (int)numericUpDown9.Value;
            if (pos > oldPos9)
                cGL.ARM_angle += 10;
            else
                cGL.ARM_angle -= 10;
            cGL.CreateRobotList();
            cGL.Draw();
            oldPos9 = pos;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked; 
        }

        private void hScrollBar10_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }
}