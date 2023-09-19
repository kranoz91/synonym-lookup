targetScope = 'subscription'

// The main bicep module to provision Azure resources.
// For a more complete walkthrough to understand how this file works with azd,
// see https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/make-azd-compatible?pivots=azd-create

@minLength(1)
@maxLength(64)
@description('Name of the the environment which is used to generate a short unique hash used in all resources.')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
param location string

param clientId string
param instance string
param swaggerClientId string
param audience string
param scopes string

var abbrs = loadJsonContent('./abbreviations.json')

// Name of the service defined in azure.yaml
var serviceName = 'synonym-lookup'

// tags that should be applied to all resources.
var tags = {
  'azd-env-name': environmentName
}

// Organize resources in a resource group
resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: '${abbrs.resourcesResourceGroups}${serviceName}-${environmentName}'
  location: location
  tags: tags
}

module staticWebApp 'core/host/staticwebapp.bicep' = {
  name: 'StaticWebApp'
  scope: rg
  params: {
    name: '${abbrs.webStaticSites}${serviceName}-${environmentName}'
    location: location
    tags: union(tags, { 'azd-service-name': '${abbrs.webStaticSites}${serviceName}'})
  }
}

module monitoring './core/monitor/monitoring.bicep' = {
  name: 'monitoring'
  scope: rg
  params: {
    location: location
    tags: tags
    logAnalyticsName: '${abbrs.operationalInsightsWorkspaces}${serviceName}-${environmentName}'
    applicationInsightsName: '${abbrs.insightsComponents}${serviceName}-${environmentName}'
  }
}

module apim './core/gateway/apim.bicep' = {
  name: 'apim-deployment'
  scope: rg
  params: {
    name: '${abbrs.apiManagementService}${serviceName}-${environmentName}'
    location: location
    tags: tags
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    webClientId: swaggerClientId
    backendClientId: clientId
    audience: audience
  }
}

module apimApi './app/apim-api.bicep' = {
  name: 'apim-api-deployment'
  scope: rg
  params: {
    name: apim.outputs.apimServiceName
    apiName: 'synonym-lookup-api'
    apiDisplayName: 'Synonym Lookup API'
    apiDescription: 'CRUD for words and their synonyms'
    apiPath: 'words'
    webFrontendUrl: staticWebApp.outputs.uri
    apiBackendUrl: api.outputs.uri
    apiAppName: api.outputs.name
  }
}

module appServicePlan './core/host/appserviceplan.bicep' = {
  name: 'appserviceplan'
  scope: rg
  params: {
    name: '${abbrs.webServerFarms}${serviceName}-${environmentName}'
    location: location
    tags: tags
    sku: {
      name: 'B1'
    }
  }
}

module api './core/host/appservice.bicep' = {
  name: '${serviceName}-app-module'
  scope: rg
  params: {
    name: '${abbrs.webSitesAppService}${serviceName}'
    location: location
    tags: union(tags, { 'azd-service-name': '${abbrs.webSitesAppService}${serviceName}' })
    applicationInsightsName: monitoring.outputs.applicationInsightsName
    appServicePlanId: appServicePlan.outputs.id
    runtimeName: 'dotnetcore'
    runtimeVersion: '7.0'
    scmDoBuildDuringDeployment: false
    appSettings: {
      AzureAd__ClientId: clientId
      AzureAd__TenantId: tenant().tenantId
      AzureAd__Instance: instance
      AzureAd__SwaggerClientId: swaggerClientId
      AzureAd__Audience: audience
      AzureAd__Scopes: scopes
    }
  }
}

// Add outputs from the deployment here, if needed.
//
// This allows the outputs to be referenced by other bicep deployments in the deployment pipeline,
// or by the local machine as a way to reference created resources in Azure for local development.
// Secrets should not be added here.
//
// Outputs are automatically saved in the local azd environment .env file.
// To see these outputs, run `azd env get-values`,  or `azd env get-values --output json` for json output.
output AZURE_LOCATION string = location
output AZURE_TENANT_ID string = tenant().tenantId
