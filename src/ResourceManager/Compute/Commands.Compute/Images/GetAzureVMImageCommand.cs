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

using Microsoft.Azure.Commands.Compute.Common;
using Microsoft.Azure.Commands.Compute.Models;
using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Compute.Models;
using System.Linq;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.Compute
{
    [Cmdlet(VerbsCommon.Get, ProfileNouns.VirtualMachineImage)]
    [OutputType(typeof(PSVirtualMachineImage))]
    public class GetAzureVMImageCommand : VirtualMachineImageBaseCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true), ValidateNotNullOrEmpty]
        public string Location { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true), ValidateNotNullOrEmpty]
        public string PublisherName { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true), ValidateNotNullOrEmpty]
        public string Offer { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true), ValidateNotNullOrEmpty]
        public string Skus { get; set; }

        [Parameter, ValidateNotNullOrEmpty]
        public string FilterExpression { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            var parameters = new VirtualMachineImageListParameters
            {
                Location = Location.Canonicalize(),
                Offer = Offer,
                PublisherName = PublisherName,
                Skus = Skus,
                FilterExpression = FilterExpression
            };

            VirtualMachineImageResourceList result = this.VirtualMachineImageClient.List(parameters);

            var images = from r in result.Resources
                         select new PSVirtualMachineImage
                         {
                             RequestId = result.RequestId,
                             StatusCode = result.StatusCode,
                             Id = r.Id,
                             Location = r.Location,
                             Version = r.Name,
                             PublisherName = this.PublisherName,
                             Offer = this.Offer,
                             Skus = this.Skus,
                             FilterExpression = this.FilterExpression
                         };

            WriteObject(images, true);
        }
    }
}
