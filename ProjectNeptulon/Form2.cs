using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ProjectNeptulon
{
    public partial class Form2 : Form
    {

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        public Form2()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        public void Loadform(object Form)
        {
            if(this.formshow.Controls.Count > 0)
            {
                this.formshow.Controls.RemoveAt(0);
            }
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.formshow.Controls.Add(f);
            this.formshow.Tag = f;
            f.Show();
        }


        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void QuestionButton_Click(object sender, EventArgs e)
        {
            // Замените "https://example.com" на URL-адрес вашей веб-страницы
            string url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ&ab_channel=RickAstley";

            // Открыть ссылку в браузере по умолчанию
            System.Diagnostics.Process.Start(url);
        }

        // Обработчик события нажатия на кнопку TensorButton
        private void TensorButton_Click(object sender, EventArgs e)
        {
            // Установить цвет фона кнопки TensorButton на (30, 170, 231)
            TensorButton.FillColor = Color.FromArgb(30, 170, 231);

            // Вернуть цвет фона других кнопок на (15, 15, 23)
            RGBButton.FillColor = Color.FromArgb(15, 15, 23);
            SobelButton.FillColor = Color.FromArgb(15, 15, 23);
            INH.FillColor = Color.FromArgb(15, 15, 23);

            Loadform(new Form6());
        }

        // Обработчик события нажатия на кнопку RGBButton
        private void RGBButton_Click(object sender, EventArgs e)
        {
            // Установить цвет фона кнопки RGBButton на (30, 170, 231)
            RGBButton.FillColor = Color.FromArgb(30, 170, 231);

            // Вернуть цвет фона других кнопок на (15, 15, 23)
            TensorButton.FillColor = Color.FromArgb(15, 15, 23);
            SobelButton.FillColor = Color.FromArgb(15, 15, 23);
            INH.FillColor = Color.FromArgb(15, 15, 23);

            Loadform(new Form4());
        }

        // Обработчик события нажатия на кнопку SobelButton
        private void SobelButton_Click(object sender, EventArgs e)
        {
            // Установить цвет фона кнопки SobelButton на (30, 170, 231)
            SobelButton.FillColor = Color.FromArgb(30, 170, 231);

            // Вернуть цвет фона других кнопок на (15, 15, 23)
            TensorButton.FillColor = Color.FromArgb(15, 15, 23);
            RGBButton.FillColor = Color.FromArgb(15, 15, 23);
            INH.FillColor = Color.FromArgb(15, 15, 23);

            Loadform(new Form3());
        }

        private void INH_Click(object sender, EventArgs e)
        {
            INH.FillColor = Color.FromArgb(30, 170, 231);
            TensorButton.FillColor = Color.FromArgb(15, 15, 23);
            RGBButton.FillColor = Color.FromArgb(15, 15, 23);
            SobelButton.FillColor = Color.FromArgb(15, 15, 23);

            Loadform(new Form1());
        }
    }
}
