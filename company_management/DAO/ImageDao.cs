using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Guna.UI2.WinForms;
// ReSharper disable All

namespace company_management.DAO
{
    public class ImageDao
    {
        private readonly string _connString = Properties.Settings.Default.connStr;

        public void ChooseImageToPictureBox(Guna2CirclePictureBox pictureBox)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = @"Image Files (*.bmp;*.jpg;*.jpeg,*.png,*.gif)|*.BMP;*.JPG;*.JPEG;*.PNG;*.GIF";

            // Cho phép chọn nhiều tệp cùng lúc
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imagePath = openFileDialog.FileName;

                pictureBox.Image = Image.FromFile(imagePath);
            }
        }

        public void ShowImageInPictureBox(byte[] imageBytes, Guna2CirclePictureBox pictureBox)
        {
            if (imageBytes != null)
            {
                using (var ms = new MemoryStream(imageBytes))
                {
                    pictureBox.Image = Image.FromStream(ms);
                }
            }
        }

        public void SaveUserAvatar(byte[] imageBytes, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                string query = "UPDATE users SET avatar=@avatar WHERE id=@idUser";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@avatar", imageBytes);
                    command.Parameters.AddWithValue("@idUser", userId);

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Lưu ảnh thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Lưu ảnh thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void SaveTeamAvatar(byte[] imageBytes, int idTeam)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                string query = "UPDATE teams SET avatar=@avatar WHERE id=@idTeam";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@avatar", imageBytes);
                    command.Parameters.AddWithValue("@idTeam", idTeam);

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Lưu ảnh thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Lưu ảnh thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public byte[] ImageToByte(Guna2CirclePictureBox pictureBox)
        {
            Bitmap bmp = new Bitmap(pictureBox.Image);
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}
