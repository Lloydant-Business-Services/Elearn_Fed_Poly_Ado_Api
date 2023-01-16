using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Infrastructure
{
    public static class ElearnRole
    {
        public const string SCHOOLADMIN = "2";
        public const string HOD = "4";
        public const string INSTRUCTOR = "3";
        public const string STUDENT = "4";
        public const string SERVICE_ADMIN = "7";

    }
    public static class CustomClaim
    {
        public const string USER_ID = "USER_ID";
        public const string USER_ROLE = "USER_ROLE";
        public const string NAME = "FULL NAME";
        public const string USER_NAME = "USER_NAME";
        public const string TOKEN_ISSUANCE_DATE = "TOKEN_ISSUANCE_DATE";
        public const string TOKEN_EXPIRY_DATE = "TOKEN_EXPIRY_DATE";
    }

    public class SecurityHashing
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
    //private string GenerateRandomPassword(PasswordOptions opts = null)
    //{
    //    var jwtSettings = _conf.GetSection("JwtSettings");


    //    if (opts == null) opts = new PasswordOptions()
    //    {
    //        RequiredLength = Int32.Parse(jwtSettings.GetSection("size").Value),
    //        RequiredUniqueChars = Int32.Parse(jwtSettings.GetSection("size").Value),
    //        RequireDigit = bool.Parse(jwtSettings.GetSection("number").Value),
    //        RequireLowercase = bool.Parse(jwtSettings.GetSection("lowcase").Value),
    //        RequireNonAlphanumeric = bool.Parse(jwtSettings.GetSection("specialcharacters").Value),
    //        RequireUppercase = bool.Parse(jwtSettings.GetSection("uppercase").Value)
    //    };

    //    string[] randomChars = new[] {
    //            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
    //            "abcdefghijkmnopqrstuvwxyz",    // lowercase
    //            "0123456789",                   // digits
    //            "!@$?_-"                        // non-alphanumeric
    //        };
    //    CryptoRandom rand = new CryptoRandom();
    //    List<char> chars = new List<char>();

    //    if (opts.RequireUppercase)
    //        chars.Insert(rand.Next(0, chars.Count),
    //            randomChars[0][rand.Next(0, randomChars[0].Length)]);

    //    if (opts.RequireLowercase)
    //        chars.Insert(rand.Next(0, chars.Count),
    //            randomChars[1][rand.Next(0, randomChars[1].Length)]);

    //    if (opts.RequireDigit)
    //        chars.Insert(rand.Next(0, chars.Count),
    //            randomChars[2][rand.Next(0, randomChars[2].Length)]);

    //    if (opts.RequireNonAlphanumeric)
    //        chars.Insert(rand.Next(0, chars.Count),
    //            randomChars[3][rand.Next(0, randomChars[3].Length)]);

    //    for (int i = chars.Count; i < opts.RequiredLength
    //        || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
    //    {
    //        string rcs = randomChars[rand.Next(0, randomChars.Length)];
    //        chars.Insert(rand.Next(0, chars.Count),
    //            rcs[rand.Next(0, rcs.Length)]);
    //    }

    //    return new string(chars.ToArray());
    //}
}
