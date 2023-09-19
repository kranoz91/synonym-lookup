metadata description = 'Creates an Azure API Management instance.'
param name string
param location string = resourceGroup().location
param tags object = {}

@description('The email address of the owner of the service')
@minLength(1)
param publisherEmail string = 'noreply@microsoft.com'

@description('The name of the owner of the service')
@minLength(1)
param publisherName string = 'n/a'

@description('The pricing tier of this API Management service')
@allowed([
  'Consumption'
  'Developer'
  'Standard'
  'Premium'
])
param sku string = 'Consumption'

@description('The instance size of this API Management service.')
@allowed([ 0, 1, 2 ])
param skuCount int = 0

@description('Azure Application Insights Name')
param applicationInsightsName string

param webClientId string
param backendClientId string
param audience string

resource apimService 'Microsoft.ApiManagement/service@2021-08-01' = {
  name: name
  location: location
  tags: union(tags, { 'azd-service-name': name })
  sku: {
    name: sku
    capacity: (sku == 'Consumption') ? 0 : ((sku == 'Developer') ? 1 : skuCount)
  }
  properties: {
    publisherEmail: publisherEmail
    publisherName: publisherName
    // Custom properties are not supported for Consumption SKU
    customProperties: sku == 'Consumption' ? {} : {
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_RSA_WITH_AES_128_GCM_SHA256': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_RSA_WITH_AES_256_CBC_SHA256': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_RSA_WITH_AES_128_CBC_SHA256': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_RSA_WITH_AES_256_CBC_SHA': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TLS_RSA_WITH_AES_128_CBC_SHA': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Ciphers.TripleDes168': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Tls10': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Tls11': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Ssl30': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Tls10': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Tls11': 'false'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Ssl30': 'false'
    }
  }
}

resource namedValueAppInsightsKey 'Microsoft.ApiManagement/service/namedValues@2020-06-01-preview' = if (!empty(applicationInsightsName)) {
  name: 'appinsights-key'
  parent: apimService
  properties: {
    tags: []
    secret: false
    displayName: 'appinsights-key'
    value: applicationInsights.properties.InstrumentationKey
  }
}

resource namedValueTenantId 'Microsoft.ApiManagement/service/namedValues@2020-06-01-preview' = {
  name: 'tenantId-namedValue'
  parent: apimService
  properties: {
    tags: []
    secret: false
    displayName: 'tenantId'
    value: tenant().tenantId
  }
}

resource namedValueWebClientId 'Microsoft.ApiManagement/service/namedValues@2020-06-01-preview' = {
  name: 'webClientId-namedValue'
  parent: apimService
  properties: {
    tags: []
    secret: false
    displayName: 'webClientId'
    value: webClientId
  }
}

resource namedValueAudience 'Microsoft.ApiManagement/service/namedValues@2020-06-01-preview' = {
  name: 'audience-namedValue'
  parent: apimService
  properties: {
    tags: []
    secret: false
    displayName: 'audience'
    value: audience
  }
}

resource namedValueBackendClientId 'Microsoft.ApiManagement/service/namedValues@2020-06-01-preview' = {
  name: 'backendClientId-namedValue'
  parent: apimService
  properties: {
    tags: []
    secret: false
    displayName: 'backendClientId'
    value: backendClientId
  }
}

resource apimLogger 'Microsoft.ApiManagement/service/loggers@2021-12-01-preview' = if (!empty(applicationInsightsName)) {
  name: 'app-insights-logger'
  parent: apimService
  properties: {
    credentials: {
      instrumentationKey: '{{appinsights-key}}'
    }
    description: 'Logger to Azure Application Insights'
    isBuffered: false
    loggerType: 'applicationInsights'
    resourceId: applicationInsights.id
  }
  dependsOn:  [
    namedValueAppInsightsKey
  ]
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' existing = if (!empty(applicationInsightsName)) {
  name: applicationInsightsName
}

output apimServiceName string = apimService.name
