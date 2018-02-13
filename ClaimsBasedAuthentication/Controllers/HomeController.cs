using ClaimsBasedAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace ClaimsBasedAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private object newIdentity;
        private object db;
        public const int NUMBER_OF_BYTES_FOR_THE_SALT = 8;
        public const int PBKDF2_ITERATIONS = 2;
        public const int NUMBEROFBYTESINHASH = 4;

        public const int ITERATION_INDEX = 1;
        public const int SALT_INDEX = 2;
        public const int PBKDF2_INDEX = 3;


        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ClaimsIdentityCollection ValidateToken(SecurityToken token)
        {
            ClaimsIdentityCollection claimsIdentityCollection = new ClaimsIdentityCollection();
            if (token is MyCustomToken)
            {
                MyCustomToken mycustomtoken = token as MyCustomToken;
                if (mycustomtoken.ValidateThisSignature())
                {
                    ClaimsIdentity newIdentity = new ClaimsIdentity((token as
                    MyCustomToken).Claims);
                }
            }
            claimsIdentityCollection.Add(newIdentity);
            return claimsIdentityCollection;
        }

        //using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
        //{
        //// assumes that the key and initialization vectors are already configured
        //CryptoStream crypoStream = new CryptoStream(myManagedStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write);
        //};

        //// create the hash code of the text to sign
        //SHA1 sha = SHA1.Create();
        //byte[] hashcode = sha.ComputeHash(TextToConvert);
        //// use the CreateSignature method to sign the data
        //DSA dsa = DSA.Create();
        //byte[] signature = dsa.CreateSignature(hashcode);

        public static string CreateTheHash(string passwordToHash)
        {
            // Generate the random salt
            RNGCryptoServiceProvider RNGcsp = new RNGCryptoServiceProvider();
            byte[] salt = new byte[NUMBER_OF_BYTES_FOR_THE_SALT];
            RNGcsp.GetBytes(salt);
            // Hash the password and encode the parameters
            byte[] hash = PBKDF2(passwordToHash, salt, PBKDF2_ITERATIONS,
            NUMBEROFBYTESINHASH);
            return PBKDF2_ITERATIONS + ":" +
            Convert.ToBase64String(salt) + ":" +
            Convert.ToBase64String(hash);
        }
        /// <summary>
        /// Computes the PBKDF2-SHA1 hash of a password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The PBKDF2 iteration count.</param>
        /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
        /// <returns>A hash of the password.</returns>
        private static byte[] PBKDF2(string password, byte[] salt, int iterations,
        int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
        /// <summary>
        /// Validates a password against the stored, hashed value.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="goodHash">A hash of the correct password.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public static bool ValidatePassword(string password, string goodHash)
        {
            // Extract the parameters from the hash
            char[] delimiter = { ':' };
            string[] split = goodHash.Split(delimiter);
            int iterations = Int32.Parse(split[ITERATION_INDEX]);
            byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
            byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);
            byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
            return hash == testHash;
        }

        public ActionResult Create(string message)
        {
            var newEntry = new Entry();
            newEntry.Message = Server.HtmlEncode(message);
            newEntry.EntryDate = DateTime.Now;
            //db.Entries.InsertOnSubmit(newEntry);
            //db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}
