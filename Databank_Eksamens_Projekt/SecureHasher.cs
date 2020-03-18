﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Databank_Eksamens_Projekt
{
    public static class SecureHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;


        public static string Hash(string password, int interactions)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, interactions);
            var hash = pbkdf2.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            var base64Hash = Convert.ToBase64String(hashBytes);

            return string.Format("$MYHASH$V1${0}${1}", interactions, base64Hash);
        }

        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }
        
        public static bool Verify(string password, string hashedPassword)
        {
            if (!IsHashSupported(hashedPassword));
            {
                throw new NotSupportedException("Error: Hashtype not dupported");
            }

            var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
            var iterations = int.TryParse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);

            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i+SaltSize]!=Hash[i])
                {
                    return false;
                }
            }
            return true;
        }
        
    }

}