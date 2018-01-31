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

using Microsoft.Azure.Commands.Network.Models;
using System;
using System.Management.Automation;
using MNM = Microsoft.Azure.Management.Network.Models;

namespace Microsoft.Azure.Commands.Network
{
    [Cmdlet(VerbsCommon.New, "AzureRmVpnClientIpsecParameters"), OutputType(typeof(PSVpnClientIPsecParameters))]
    public class NewAzureRmVpnClientIpsecParametersCommand : NetworkBaseCmdlet
    {
        [Parameter(
            Mandatory = false,
            HelpMessage = "The Vpnclient IPSec Security Association (also called Quick Mode or Phase 2 SA) lifetime in seconds")]
        [ValidateRange(300, 172799)]
        public int SALifeTimeSeconds { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "The Vpnclient IPSec Security Association (also called Quick Mode or Phase 2 SA) payload size in KB")]
        [ValidateRange(1024, int.MaxValue)]
        public int SADataSizeKilobytes { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The Vpnclient IPSec encryption algorithm (IKE Phase 1)")]
        [ValidateNotNullOrEmpty]
        [ValidateSet(
            MNM.IpsecEncryption.GCMAES256,
            MNM.IpsecEncryption.GCMAES128,
            MNM.IpsecEncryption.AES256,
            MNM.IpsecEncryption.AES128,
            IgnoreCase = false)]
        public string IpsecEncryption { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The Vpnclient IPSec integrity algorithm (IKE Phase 1)")]
        [ValidateNotNullOrEmpty]
        [ValidateSet(
            MNM.IpsecEncryption.GCMAES256,
            MNM.IpsecEncryption.GCMAES128,
            MNM.IpsecIntegrity.SHA256,
            IgnoreCase = false)]
        public string IpsecIntegrity { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The Vpnclient IKE encryption algorithm (IKE Phase 2)")]
        [ValidateNotNullOrEmpty]
        [ValidateSet(
            MNM.IkeEncryption.AES256,
            MNM.IkeEncryption.AES128,
            IgnoreCase = false)]
        public string IkeEncryption { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The Vpnclient IKE integrity algorithm (IKE Phase 2)")]
        [ValidateNotNullOrEmpty]
        [ValidateSet(
            MNM.IkeIntegrity.SHA384,
            MNM.IkeIntegrity.SHA256,
            IgnoreCase = false)]
        public string IkeIntegrity { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The Vpnclient DH Groups used in IKE Phase 1 for initial SA")]
        [ValidateNotNullOrEmpty]
        [ValidateSet(
            MNM.DhGroup.DHGroup24,
            MNM.DhGroup.ECP384,
            MNM.DhGroup.ECP256,
            MNM.DhGroup.DHGroup14,
            MNM.DhGroup.DHGroup2,
            IgnoreCase = false)]
        public string DhGroup { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The Vpnclient PFS Groups used in IKE Phase 2 for new child SA")]
        [ValidateNotNullOrEmpty]
        [ValidateSet(
            MNM.PfsGroup.PFSMM,
            MNM.PfsGroup.PFS24,
            MNM.PfsGroup.ECP384,
            MNM.PfsGroup.ECP256,
            MNM.PfsGroup.PFS14,
            MNM.PfsGroup.PFS2,
            MNM.PfsGroup.None,
            IgnoreCase = false)]
        public string PfsGroup { get; set; }

        public override void Execute()
        {
            base.Execute();
            var vpnclientIPsecParameters = new PSVpnClientIPsecParameters();

            // default SA values
            vpnclientIPsecParameters.SaLifeTimeSeconds = (!this.MyInvocation.BoundParameters.ContainsKey("SaLifeTimeSeconds")) ? 27000 : this.SALifeTimeSeconds;
            vpnclientIPsecParameters.SaDataSizeKilobytes = (!this.MyInvocation.BoundParameters.ContainsKey("SaDataSizeKilobytes")) ? 102400000 : this.SADataSizeKilobytes;

            // GCM matching check
            if ((this.IpsecEncryption.Contains("GCM") || this.IpsecIntegrity.Contains("GCM")) && this.IpsecEncryption != this.IpsecIntegrity)
            {
                throw new ArgumentException("Vpnclient IpsecEncryption and IpsecIntegrity must use matching GCM algorithms");
            }

            vpnclientIPsecParameters.IpsecEncryption = this.IpsecEncryption;
            vpnclientIPsecParameters.IpsecIntegrity = this.IpsecIntegrity;
            vpnclientIPsecParameters.IkeEncryption = this.IkeEncryption;
            vpnclientIPsecParameters.IkeIntegrity = this.IkeIntegrity;
            vpnclientIPsecParameters.DhGroup = this.DhGroup;
            vpnclientIPsecParameters.PfsGroup = this.PfsGroup;

            WriteObject(vpnclientIPsecParameters);
        }
    }
}
