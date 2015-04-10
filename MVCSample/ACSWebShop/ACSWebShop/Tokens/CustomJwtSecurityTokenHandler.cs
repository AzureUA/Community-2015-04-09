using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel.Security.Tokens;
using System.Web;

namespace ACSWebShop.Tokens
{
    public class CustomJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        //public override ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        //{
        //    //var identity = new GenericIdentity("alex@test.com");

        //    //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "alex@test.com"));


        //    //ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

        //    //validatedToken = new JwtSecurityToken("https://asemshop1.accesscontrol.windows.net/", "https://localhost/ACSWebShop/", new List<Claim>(), DateTime.Now.AddDays(-1), DateTime.Now.AddDays(10));

        //    //return claimsPrincipal;

        //    return base.ValidateToken(securityToken, validationParameters, out validatedToken);
        //}

        public override System.Collections.ObjectModel.ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            JwtSecurityToken jwtToken = (JwtSecurityToken)token;

            // Get the configuration from the configuration file (element "issuerNameRegistry").
            ValidatingIssuerNameRegistry issuerNameRegistry = (ValidatingIssuerNameRegistry)
                      Configuration.IssuerNameRegistry;

            IssuingAuthority issuingAuthority = issuerNameRegistry.IssuingAuthorities.First();

            // Set the validation parameters from the configuration.
            var validationParameters = new TokenValidationParameters
            {
                // Get the audiences that are expected.
                ValidAudiences = Configuration.AudienceRestriction.AllowedAudienceUris.Select(s => s.ToString()),

                // Get the issuer that are expected.
                ValidIssuers = issuingAuthority.Issuers,                

                // Get the symmetric key token that is used to sign (if configured).
                // Did not get this one working though.
                IssuerSigningToken = new BinarySecretSecurityToken(Convert.FromBase64String(issuingAuthority.SymmetricKeys.FirstOrDefault())),

                // Get how to validate the certificate.
                CertificateValidator = Configuration.CertificateValidator,

                // Get if the token should be preserved.
                SaveSigninToken = Configuration.SaveBootstrapContext
            };

            // Call the correct validation method.
            SecurityToken validatedToken;
            ClaimsPrincipal validated = ValidateToken(jwtToken.RawData, validationParameters, out validatedToken);

            // Return the claim identities.
            return new ReadOnlyCollection<ClaimsIdentity>(validated.Identities.ToList());
        }
    }
}