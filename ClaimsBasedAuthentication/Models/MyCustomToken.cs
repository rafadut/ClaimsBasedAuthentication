using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Xml;

namespace ClaimsBasedAuthentication.Models
{
    public class MyCustomToken : SecurityToken
    {
        public List<Claim> Claims { get; set; }
        public XmlElement Signature { get; set; }
        public bool ValidateThisSignature()
        {
            // code to validate the signature
            return true;
        }
    }
}