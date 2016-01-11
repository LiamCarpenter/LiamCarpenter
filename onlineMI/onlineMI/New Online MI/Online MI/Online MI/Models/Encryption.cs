using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_MI.Models
{
    public class Encryption
    {
        public static string DecryptParam(string EncodedKey, string Token)
        {
            string Result = "";
            Chilkat.Crypt2 crypt = new Chilkat.Crypt2();
            bool success = crypt.UnlockComponent("NYSGROCrypt_pafhxYhyTVKZ");

            if (!success)
            {
                return Result;
            }

            crypt.CipherMode = "ecb";
            crypt.KeyLength = 256;
            crypt.PaddingScheme = 3;
            crypt.SetEncodedKey(EncodedKey, "hex");
            crypt.EncodingMode = "hex";

            Result = crypt.DecryptStringENC(Token);

            return Result;
        }
        
        public static string GetKey(string PlainKey)
        {
            byte[] Input = new byte[31];
            Input = CreateKey(PlainKey);
            return BytetoHexString(Input);
        }

        public static byte[] CreateKey(string Key)
        {
            char[] charData = Key.ToCharArray();
            int Length = charData.GetUpperBound(0) + 1;
            byte[] DatatoHash = new byte[Length];

            for (int i = 0; i < Length; i++)
            {
                DatatoHash[i] = (byte)((int)charData[i]);
            }

            System.Security.Cryptography.SHA512Managed SHA512 = new System.Security.Cryptography.SHA512Managed();

            byte[] ByteResult = SHA512.ComputeHash(DatatoHash);
            byte[] ResultKey = new byte[32];

            for (int i = 0; i < 32; i++)
            {
                ResultKey[i] = ByteResult[i];
            }

            return ResultKey;
        }

        public static string BytetoHexString(byte[] targetbytes)
        {
            string Result = "";

            for (int i = 0; i < targetbytes.Length; i++)
            {
                Result += string.Format("{0:X2}", targetbytes[i]);
            }

            return Result;
        }


    }
}