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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"D:\Games"; // Змініть шлях на необхідний
                openFileDialog.Filter = "Зображення|*.jpg;*.jpeg;*.png;*.bmp|Всі файли|*.*";
                openFileDialog.Title = "Виберіть зображення";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string imagePath = openFileDialog.FileName; // Отримуємо шлях до обраного зображення

                    // Перевірка, чи шлях не є null або пустим
                    if (!string.IsNullOrEmpty(imagePath))
                    {

                        Mat processedImage;
                        Mat invertedImage;


                        Mat image = Cv2.ImRead(imagePath, ImreadModes.Grayscale);

                        
                        image = Cv2.ImRead(imagePath, ImreadModes.Grayscale);
                        processedImage = ProcessWithSobelAndGaussian(image);
                        // invertedImage = InvertColors(processedImage);

                       
                        Cv2.ImShow("Processed Image", processedImage);
                        invertedImage = InvertColors(processedImage);
                        Cv2.ImShow("Reversed Image", invertedImage);
                        Cv2.WaitKey(0);
                        Cv2.DestroyAllWindows();

                        SaveAndDisplayImage(invertedImage);


                    }
                }
            }
        }

        private Mat ProcessWithSobelAndGaussian(Mat image)
        {
            // Применяем фильтр Гаусса для сглаживания изображения
            Mat blurredImage = new Mat();
            Cv2.GaussianBlur(image, blurredImage, new OpenCvSharp.Size(3, 3), sigmaX: 0);

            // Применяем оператор Собеля для выделения границ
            Mat Gx = blurredImage.Sobel(MatType.CV_64F, 1, 0, ksize: 3);
            Mat Gy = blurredImage.Sobel(MatType.CV_64F, 0, 1, ksize: 3);

            // Вычисляем величину градиента
            Mat gradientMagnitude = new Mat();
            Cv2.Sqrt(Gx.Mul(Gx) + Gy.Mul(Gy), gradientMagnitude);

            // Нормализуем величину градиента и преобразуем в 8-битное изображение
            Cv2.Normalize(gradientMagnitude, gradientMagnitude, 0, 255, NormTypes.MinMax);
            gradientMagnitude.ConvertTo(gradientMagnitude, MatType.CV_8U);

            return gradientMagnitude;
        }

        private static Mat InvertColors(Mat image)
        {
            Mat invertedImage = new Mat(image.Rows, image.Cols, MatType.CV_8U);

            for (int y = 0; y < image.Rows; y++)
            {
                for (int x = 0; x < image.Cols; x++)
                {
                    // Получаем текущее значение интенсивности пикселя
                    byte intensity = image.At<byte>(y, x);

                    // Инвертируем интенсивность пикселя
                    byte invertedIntensity = (byte)(255 - intensity);

                    // Устанавливаем инвертированную интенсивность для пикселя на изображении
                    invertedImage.Set<byte>(y, x, invertedIntensity);
                }
            }

            return invertedImage;
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
