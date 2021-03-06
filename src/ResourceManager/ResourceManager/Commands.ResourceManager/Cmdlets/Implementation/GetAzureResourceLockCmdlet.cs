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

namespace Microsoft.Azure.Commands.ResourceManager.Cmdlets.Implementation
{
    using System.Management.Automation;
    using System.Threading.Tasks;
    using Microsoft.Azure.Commands.ResourceManager.Cmdlets.Components;
    using Microsoft.Azure.Commands.ResourceManager.Cmdlets.Extensions;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Gets the resource lock.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureResourceLock"), OutputType(typeof(PSObject))]
    public class GetAzureResourceLockCmdlet : ResourceLockManagementCmdletBase
    {
        /// <summary>
        /// Gets or sets the at-scope filter.
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "When specified returns all locks at or above the specified scope, otherwise returns all locks at, above or below the scope.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter AtScope { get; set; }


        /// <summary>
        /// Gets or sets the extension resource name parameter.
        /// </summary>
        [Alias("ExtensionResourceName")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "The name of the lock.")]
        [ValidateNotNullOrEmpty]
        public string LockName { get; set; }

        /// <summary>
        /// Gets the resource Id from the supplied PowerShell parameters.
        /// </summary>
        protected string GetResourceId()
        {
            return !string.IsNullOrWhiteSpace(this.Scope)
                ? ResourceIdUtility.GetResourceId(
                    resourceId: this.Scope,
                    extensionResourceType: Constants.MicrosoftAuthorizationLocksType,
                    extensionResourceName: this.LockName)
                : ResourceIdUtility.GetResourceId(
                    subscriptionId: this.SubscriptionId,
                    resourceGroupName: this.ResourceGroupName,
                    resourceType: this.ResourceType,
                    resourceName: this.ResourceName,
                    extensionResourceType: Constants.MicrosoftAuthorizationLocksType,
                    extensionResourceName: this.LockName);
        }

        /// <summary>
        /// Executes the cmdlet.
        /// </summary>
        protected override void OnProcessRecord()
        {
            base.OnProcessRecord();
            this.RunCmdlet();
        }

        /// <summary>
        /// Contains the cmdlet's execution logic.
        /// </summary>
        private void RunCmdlet()
        {
            PaginatedResponseHelper.ForEach(
                getFirstPage: () => this.GetResources(),
                getNextPage: nextLink => this.GetNextLink<JObject>(nextLink),
                cancellationToken: this.CancellationToken,
                action: resources => this.WriteObject(sendToPipeline: resources.CoalesceEnumerable().SelectArray(resource => resource.ToPsObject()), enumerateCollection: true));
        }

        /// <summary>
        /// Queries the ARM cache and returns the cached resource that match the query specified.
        /// </summary>
        private async Task<ResponseWithContinuation<JObject[]>> GetResources()
        {
            if (!string.IsNullOrWhiteSpace(this.LockName))
            {
                var resource = await this.GetResource().ConfigureAwait(continueOnCapturedContext: false);
                return new ResponseWithContinuation<JObject[]> { Value = resource.AsArray() };
            }

            return await this.ListResourcesTypeCollection().ConfigureAwait(continueOnCapturedContext: false);
        }

        /// <summary>
        /// Gets a resource.
        /// </summary>
        private async Task<JObject> GetResource()
        {
            var resourceId = this.GetResourceId();

            var apiVersion = await this
                .DetermineApiVersion(resourceId: resourceId)
                .ConfigureAwait(continueOnCapturedContext: false);

            return await this
                .GetResourcesClient()
                .GetResource<JObject>(
                    resourceId: resourceId,
                    apiVersion: apiVersion,
                    cancellationToken: this.CancellationToken.Value)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        /// <summary>
        /// Lists resources in a type collection.
        /// </summary>
        private async Task<ResponseWithContinuation<JObject[]>> ListResourcesTypeCollection()
        {
            var resourceCollectionId = this.GetResourceId();

            var apiVersion = await this
                .DetermineApiVersion(resourceId: resourceCollectionId)
                .ConfigureAwait(continueOnCapturedContext: false);

            var filter = this.AtScope
                ? "$filter=atScope()"
                : null;

            return await this
                .GetResourcesClient()
                .ListObjectColleciton<JObject>(
                    resourceCollectionId: resourceCollectionId,
                    apiVersion: apiVersion,
                    cancellationToken: this.CancellationToken.Value,
                    odataQuery: filter)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        /// <summary>
        /// Gets the next set of resources using the <paramref name="nextLink"/>
        /// </summary>
        /// <param name="nextLink">The next link.</param>
        private Task<ResponseWithContinuation<TType[]>> GetNextLink<TType>(string nextLink)
        {
            return this
                .GetResourcesClient()
                .ListNextBatch<TType>(nextLink: nextLink, cancellationToken: this.CancellationToken.Value);
        }
    }
}
