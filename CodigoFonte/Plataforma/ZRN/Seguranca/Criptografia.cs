using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZRN.Seguranca
{
    public class Criptografia
    {

        public static string Criptografar(string senha)
        {
            string senhazeeng = "zeeng" + senha;

            MD5 md5Hash = MD5.Create();
            
            return GetMd5Hash(md5Hash, senhazeeng);

        }

        static bool VerificarSenha(MD5 md5Hash, string senha, string hash)
        {
            string senhazeeng = "zeeng" + senha;

            string hashOfInput = GetMd5Hash(md5Hash, senhazeeng);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

    }
}
