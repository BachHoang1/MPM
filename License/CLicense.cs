// author: hoan chau
// purpose: allow Display to run based on a registered machine license

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;


namespace License
{
    public class CLicense
    {
        const string ENCRYPTION_KEY = "MyLittlePony";

        private string m_sUserName;
        private string m_sOrganizationName;
        private string m_sDate;
        private string m_sDaysValid;

        public void Load()
        {
            StreamReader sr = new StreamReader("C:\\APS\\Data\\License.txt");
            string sCipherText = sr.ReadLine();
            string sClearText = Decrypt(sCipherText);
            sr.Close();
        }

        public void Save(string sCipherText_)
        {
            StreamWriter sw = new StreamWriter("C:\\APS\\Data\\License.txt", false);
            sw.WriteLine(sCipherText_);
            sw.Close();
        }

        public string Encrypt(string sClearText_)
        {            
            byte[] byteArrClear = Encoding.Unicode.GetBytes(sClearText_);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ENCRYPTION_KEY, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(byteArrClear, 0, byteArrClear.Length);
                        cs.Close();
                    }
                    sClearText_ = Convert.ToBase64String(ms.ToArray());
                }
            }
            return sClearText_;
        }
        public string Decrypt(string sCipherText_)
        {            
            sCipherText_ = sCipherText_.Replace(" ", "+");
            byte[] byteArrCipher = Convert.FromBase64String(sCipherText_);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ENCRYPTION_KEY, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(byteArrCipher, 0, byteArrCipher.Length);
                        cs.Close();
                    }
                    sCipherText_ = Encoding.Unicode.GetString(ms.ToArray());
                    string[] sVal = sCipherText_.Split('#');
                    if (sVal.Length > 3)
                    {
                        m_sUserName = sVal[0];
                        m_sOrganizationName = sVal[1];
                        m_sDate = sVal[2];
                        m_sDaysValid = sVal[3];
                    }
                    else
                        m_sDaysValid = "0";

                }
            }
            return sCipherText_;
        }

        public bool IsValid()
        {
            int iDaysValid = 0;
            bool bRetVal = false;

            if (int.TryParse(m_sDaysValid, out iDaysValid))
            {
                if (iDaysValid > 0)
                    bRetVal = true;
            }

            return bRetVal;
        }

        public string GetUserName()
        {
            return m_sUserName;
        }

        public string GetOrganizationName()
        {
            return m_sOrganizationName;
        }

    }
}
