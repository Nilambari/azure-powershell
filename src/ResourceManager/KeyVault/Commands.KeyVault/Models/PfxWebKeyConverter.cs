﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Security;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure.KeyVault.WebKey;
using KeyVaultProperties = Microsoft.Azure.Commands.KeyVault.Properties;

namespace Microsoft.Azure.Commands.KeyVault.Models
{
    internal class PfxWebKeyConverter : IWebKeyConverter
    {
        public PfxWebKeyConverter(IWebKeyConverter next = null)
        {
            this.next = next;
        }

        public JsonWebKey ConvertKeyFromFile(FileInfo fileInfo, SecureString password)
        {
            if (CanProcess(fileInfo, password))
                return Convert(fileInfo.FullName, password);
            else if (next != null)
                return next.ConvertKeyFromFile(fileInfo, password);
            else
                throw new ArgumentException(string.Format(KeyVaultProperties.Resources.UnsupportedFileFormat, fileInfo.Name));
        }

        private bool CanProcess(FileInfo fileInfo, SecureString password)
        {
            if (fileInfo == null ||
                string.IsNullOrEmpty(fileInfo.Extension))
            {
                return false;
            }

            return PfxFileExtension.Equals(fileInfo.Extension, StringComparison.OrdinalIgnoreCase);
        }

        private JsonWebKey Convert(string pfxFileName, SecureString pfxPassword)
        {
            X509Certificate2 certificate;

            if (pfxPassword != null)
                certificate = new X509Certificate2(pfxFileName, pfxPassword, X509KeyStorageFlags.Exportable);
            else
                certificate = new X509Certificate2(pfxFileName);
          
            if (!certificate.HasPrivateKey)
                throw new ArgumentException(string.Format(KeyVaultProperties.Resources.InvalidKeyBlob, "pfx"));
          
            var key = certificate.PrivateKey as RSA;

            if (key == null)
                throw new ArgumentException(string.Format(KeyVaultProperties.Resources.InvalidKeyBlob, "pfx"));

            return CreateJWK(key);
        }

        private static JsonWebKey CreateJWK(RSA rsa)
        {
            if (rsa == null)
                throw new ArgumentNullException("rsa");
            RSAParameters rsaParameters = rsa.ExportParameters(true);
            var webKey = new JsonWebKey() 
            { 
                Kty = JsonWebKeyType.Rsa,
                E = rsaParameters.Exponent,
                N = rsaParameters.Modulus,
                D = rsaParameters.D,
                DP = rsaParameters.DP,
                DQ = rsaParameters.DQ,
                QI = rsaParameters.InverseQ,
                P = rsaParameters.P,
                Q = rsaParameters.Q
            };
            
            return webKey;
        }

        private IWebKeyConverter next;
        private const string PfxFileExtension = ".pfx";
    }
}
