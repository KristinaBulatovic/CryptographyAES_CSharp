using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace AES
{
    public partial class Form1 : Form
    {
        private string IV = "qgp065mlsy3ep064"; // 16 chars = 128 bytes
        public Form1()
        {
            InitializeComponent();
        }

        public string Encrypt_AES(string key, string text) // key -> 32 chars = 256 bytes
        {
            byte[] textbytes = ASCIIEncoding.ASCII.GetBytes(text);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = ASCIIEncoding.ASCII.GetBytes(key);
            aes.IV = ASCIIEncoding.ASCII.GetBytes(IV);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform trans = aes.CreateEncryptor(aes.Key, aes.IV);

            byte[] encrypt = trans.TransformFinalBlock(textbytes, 0, textbytes.Length);
            trans.Dispose();

            return Convert.ToBase64String(encrypt);

        }

        public string Decrypt_AES(string key, string text) // key -> 32 chars = 256 bytes
        {
            byte[] textbytes = Convert.FromBase64String(text);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            try
            {
                aes.Key = ASCIIEncoding.ASCII.GetBytes(key);
                aes.IV = ASCIIEncoding.ASCII.GetBytes(IV);
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                ICryptoTransform trans = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] decrypt = trans.TransformFinalBlock(textbytes, 0, textbytes.Length);
                trans.Dispose();

                return ASCIIEncoding.ASCII.GetString(decrypt);
            }
            catch
            {
                return "";
            }


        }
        public string Key(string text)
        {
            string k = text;
            string key = "";
            if (k.Length == 0)
            {
                for (int i = 0; i < 32; i++)
                {
                    key += " ";
                }
            }
            else if (k.Length < 32)
            {
                key = k;
                for (int i = 32; i > k.Length; i--)
                {
                    key += " ";
                }
            }
            else key = k;
            return key;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string key = Key(textBox4.Text);
            textBox2.Text = Encrypt_AES(key, textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string key = Key(textBox4.Text);
            textBox3.Text = Decrypt_AES(key, textBox2.Text);
        }
    }
}