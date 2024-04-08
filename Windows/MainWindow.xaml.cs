using CryptoStream.Services;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CryptoStream.Windows
{
    public partial class MainWindow : Window
    {
        private RadioButton selectedRadioButton { get; set; } = null;

        public MainWindow() => InitializeComponent();

        private void executeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedRadioButton == null)
                    throw new NullReferenceException("Выберите провайдер шифрования.");

                SymmetricAlgorithm algorithm = GetAlgorithm(selectedRadioButton.Tag.ToString())
                    ?? throw new NullReferenceException("Введите провайдер шифрования.");

                CryptographyService cryptographyService = new CryptographyService(algorithm);

                string sourceData = sourceDataTxt.Text;
                string encryptedData = cryptographyService.Encrypt(sourceData);
                encryptedDataTxt.Text = encryptedData;

                string decryptedData = cryptographyService.Decrypt(encryptedData);
                decryptedDataTxt.Text = decryptedData;

                keyTxt.Text = Encoding.UTF7.GetString(algorithm.Key);
                initVectorTxt.Text = Encoding.UTF7.GetString(algorithm.IV);

                keySizeTxt.Text = algorithm.KeySize.ToString();
                blockSizeTxt.Text = algorithm.BlockSize.ToString();

                StringBuilder stringBuilder = new StringBuilder();
                foreach (KeySizes sizes in algorithm.LegalKeySizes)
                    stringBuilder.Append(sizes.MinSize.ToString() + "-" + sizes.MaxSize.ToString() + " (" + sizes.SkipSize.ToString() + ") ");

                legalKeySizesTxt.Text = stringBuilder.ToString();

                stringBuilder = new StringBuilder();
                foreach (KeySizes sizes in algorithm.LegalBlockSizes)
                    stringBuilder.Append(sizes.MinSize.ToString() + "-" + sizes.MaxSize.ToString() + " (" + sizes.SkipSize.ToString() + ") ");
                
                legalBlockSizesTxt.Text = stringBuilder.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Произошла ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private SymmetricAlgorithm GetAlgorithm(string value)
        {
            switch (value)
            {
                case "DES":
                    return new DESCryptoServiceProvider();
                case "RC2":
                    return new RC2CryptoServiceProvider();
                case "Rijndael":
                    return new RijndaelManaged();
                case "TripleDES":
                    return new TripleDESCryptoServiceProvider();
                case "SymmetricAlgorithm":
                    return SymmetricAlgorithm.Create();
                default:
                    return null;
            }
        }

        private void DESCryptoServiceProvider_Checked(object sender, RoutedEventArgs e)
        {
            selectedRadioButton = sender as RadioButton;
        }

        private void RC2CryptoServiceProvider_Checked(object sender, RoutedEventArgs e)
        {
            selectedRadioButton = sender as RadioButton;
        }

        private void rijndaelManagedRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            selectedRadioButton = sender as RadioButton;
        }

        private void tripleDESCryptoServiceProviderRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            selectedRadioButton = sender as RadioButton;
        }

        private void symmetricAlgorithmRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            selectedRadioButton = sender as RadioButton;
        }
    }
}