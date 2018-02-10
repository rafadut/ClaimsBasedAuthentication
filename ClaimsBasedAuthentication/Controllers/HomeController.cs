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
    }
}
