using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectNeptulon
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            // Перевірка, чи введені значення для червоного, зеленого та синього каналів коректні
            if (!double.TryParse(guna2TextBox1.Text, out double red) || !double.TryParse(guna2TextBox2.Text, out double green) || !double.TryParse(guna2TextBox3.Text, out double blue))
            {
                MessageBox.Show("Некоректні введені значення для каналів RGB.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"D:\Games"; 
                openFileDialog.Filter = "Зображення|*.jpg;*.jpeg;*.png;*.bmp|Всі файли|*.*";
                openFileDialog.Title = "Виберіть зображення";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string imagePath = openFileDialog.FileName; // Отримуємо шлях до обраного зображення

                    // Перевірка, чи шлях не є null або пустим
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        Mat processedImage;

                        Mat image = Cv2.ImRead(imagePath, ImreadModes.Grayscale);

                        image = Cv2.ImRead(imagePath, ImreadModes.AnyColor);
                        processedImage = RGBchannel(image, red, green, blue);

                        Cv2.ImShow("Processed Image", processedImage);
                        Cv2.WaitKey(0);
                        Cv2.DestroyAllWindows();

                        SaveAndDisplayImage(processedImage);
                    }
                }
            }
        }


        private Mat RGBchannel(Mat image, double redChannel, double greenChannel, double blueChannel)
        {
            // Проверка наличия каналов изображения
            if (image.Channels() != 3)
            {
                MessageBox.Show("Ошибка: изображение должно иметь три канала (RGB)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Разделение изображения на отдельные каналы
            Mat[] channels = Cv2.Split(image);

            // Весовые коэффициенты для каждого канала RGB
            double[] weights = { redChannel, greenChannel, blueChannel };

            // Проверка наличия трех каналов в изображении и их размеров
            if (channels.Length != 3 || channels[0].Size() != channels[1].Size() || channels[1].Size() != channels[2].Size())
            {
                MessageBox.Show("Ошибка: размеры каналов изображения не совпадают", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            // Матрица для хранения результирующего изображения
            Mat result = new Mat(image.Size(), MatType.CV_32F);

            // Проход по каждому каналу и применение весового коэффициента
            for (int i = 0; i < channels.Length; i++)
            {
                channels[i].ConvertTo(channels[i], MatType.CV_32F);
                channels[i] *= weights[i];
            }

            // Сложение каналов для получения карты высот
            Cv2.Add(channels[0], channels[1], result);
            Cv2.Add(result, channels[2], result);

            // Нормализация значений карты высот
            Cv2.Normalize(result, result, 0, 255, NormTypes.MinMax);

            // Конвертация результирующей матрицы в тип CV_8U
            result.ConvertTo(result, MatType.CV_8U);

            return result;
        }


        private void SaveAndDisplayImage(Mat image)
        {
            // Перевірка наявності зображення
            if (image == null)
            {
                MessageBox.Show("Помилка: неправильний шлях до зображення", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Діалогове вікно для вибору місця збереження
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
            saveFileDialog.Title = "Виберіть місце для збереження зображення";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Зберігаємо оброблене зображення
                string outputPath = saveFileDialog.FileName;
                Cv2.ImWrite(outputPath, image);

                // Відображаємо оброблене зображення в pictureBox
                using (var fs = new FileStream(outputPath, FileMode.Open, FileAccess.Read))
                {
                    pictureBox1.Image = System.Drawing.Image.FromStream(fs);
                }
            }
        }

    }
}
