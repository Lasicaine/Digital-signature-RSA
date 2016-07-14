using System;
using System.Security.Cryptography;

namespace ECP
{
    class ECP
    {
        
        private readonly BigInteger n;
        /// <summary>
        /// Модуль n
        /// </summary>
        public string N
        {
            get { return n.ToString(); }
        }

        private readonly BigInteger e;
        /// <summary>
        /// Открытый ключ
        /// </summary>
        public string E
        {
            get { return e.ToString(); }
        }
   
        private readonly BigInteger d;
        /// <summary>
        /// Закрытый ключ
        /// </summary>
        public string D
        {
            get { return d.ToString(); }
        }

        public ECP()
        {
            Random uy = new Random();

            BigInteger p = BigInteger.genPseudoPrime(129, 13, uy);
            BigInteger q = BigInteger.genPseudoPrime(128, 13, uy);

            n = p * q;
            p--;
            q--;
            BigInteger w = p * q;

            e = new BigInteger();
            BigInteger one = new BigInteger(1);
            do
            {
            ar:
                e.genRandomBits(256, uy);
                if (e > n) goto ar;
            }
            while (BigInteger.NOD(e, w) != one);
            // d - закрытый ключ (обратный к е по модулю w)
            d = BigInteger.Inverse(e, w);
        }

        /// <summary>
        /// Вычисляет хэш исходных данных
        /// </summary>
        /// <param name="data">Исходные данные</param>
        /// <returns>160-битный хэш</returns>
        private static byte[] SHA1hash(byte[] data)
        {
            // This is one implementation of the abstract class SHA1.
            SHA1 sha = new SHA1CryptoServiceProvider();
            return sha.ComputeHash(data);
        }   
        /// <summary>
        /// Вырабатывает ЭЦП сообщения-Text
        /// </summary>
        /// <param name="Text">Сообщение</param>
        /// <returns>Подпись</returns>
        public byte[] CreateSignature(byte[] Text)
        {
            byte[] hash = SHA1hash(Text);
            BigInteger BI_Text = new BigInteger(hash);
            return BI_Text.modPow(d, n).getBytes();
        }

        /// <summary>
        /// Проверка цифровой подписи Signature текста Text
        /// </summary>
        /// <param name="Text">Текст</param>
        /// <param name="Signature">Подпись</param>
        /// <param name="e">Открытый ключ</param>
        /// <param name="n">Модуль</param>
        /// <returns>true если подпись верна, false в обратном случае</returns>
        public bool VerifySignature(byte[] Text, byte[] Signature, byte[] e, byte[] n)
        {
            BigInteger R = new BigInteger(SHA1hash(Text));
            BigInteger S = new BigInteger(Signature).modPow(new BigInteger(e),new BigInteger(n));
            if (R==S)
                return true;
            return false;
        }
    }
}
