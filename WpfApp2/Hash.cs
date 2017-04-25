using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Hash
    {

        public string CalculateHash(string pw)
        {
            string hash = "";

            using (SHA512Managed shaM = new SHA512Managed())
            {
                byte[] data = shaM.ComputeHash(Encoding.UTF8.GetBytes(pw));
                StringBuilder strBuilder = new StringBuilder();

                foreach (byte b in data)
                {
                    strBuilder.AppendFormat("{0:x2}", b);
                }
                hash = strBuilder.ToString();
            }

            return hash;
        }

    }
}
