import React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';
import { AccountInfo, AuthenticationResult, AuthorizationCodeRequest, BrowserConfiguration, ClearCacheRequest, EndSessionPopupRequest, EndSessionRequest, EventCallbackFunction, INavigationClient, IPublicClientApplication, ITokenCache, Logger, PerformanceCallbackFunction, PopupRequest, RedirectRequest, SilentRequest, SsoSilentRequest, WrapperSKU } from '@azure/msal-browser';
import { msalConfig } from './authConfig';

test('renders learn react link', () => {
  const mockPublicClientApplication: IPublicClientApplication = {
    initialize: function (): Promise<void> {
      throw new Error('Function not implemented.');
    },
    acquireTokenPopup: function (request: PopupRequest): Promise<AuthenticationResult> {
      throw new Error('Function not implemented.');
    },
    acquireTokenRedirect: function (request: RedirectRequest): Promise<void> {
      throw new Error('Function not implemented.');
    },
    acquireTokenSilent: function (silentRequest: SilentRequest): Promise<AuthenticationResult> {
      throw new Error('Function not implemented.');
    },
    acquireTokenByCode: function (request: AuthorizationCodeRequest): Promise<AuthenticationResult> {
      throw new Error('Function not implemented.');
    },
    addEventCallback: function (callback: EventCallbackFunction): string | null {
      throw new Error('Function not implemented.');
    },
    removeEventCallback: function (callbackId: string): void {
      throw new Error('Function not implemented.');
    },
    addPerformanceCallback: function (callback: PerformanceCallbackFunction): string {
      throw new Error('Function not implemented.');
    },
    removePerformanceCallback: function (callbackId: string): boolean {
      throw new Error('Function not implemented.');
    },
    enableAccountStorageEvents: function (): void {
      throw new Error('Function not implemented.');
    },
    disableAccountStorageEvents: function (): void {
      throw new Error('Function not implemented.');
    },
    getAccountByHomeId: function (homeAccountId: string): AccountInfo | null {
      throw new Error('Function not implemented.');
    },
    getAccountByLocalId: function (localId: string): AccountInfo | null {
      throw new Error('Function not implemented.');
    },
    getAccountByUsername: function (userName: string): AccountInfo | null {
      throw new Error('Function not implemented.');
    },
    getAllAccounts: function (): AccountInfo[] {
      throw new Error('Function not implemented.');
    },
    handleRedirectPromise: function (hash?: string | undefined): Promise<AuthenticationResult | null> {
      throw new Error('Function not implemented.');
    },
    loginPopup: function (request?: PopupRequest | undefined): Promise<AuthenticationResult> {
      throw new Error('Function not implemented.');
    },
    loginRedirect: function (request?: RedirectRequest | undefined): Promise<void> {
      throw new Error('Function not implemented.');
    },
    logout: function (logoutRequest?: EndSessionRequest | undefined): Promise<void> {
      throw new Error('Function not implemented.');
    },
    logoutRedirect: function (logoutRequest?: EndSessionRequest | undefined): Promise<void> {
      throw new Error('Function not implemented.');
    },
    logoutPopup: function (logoutRequest?: EndSessionPopupRequest | undefined): Promise<void> {
      throw new Error('Function not implemented.');
    },
    ssoSilent: function (request: SsoSilentRequest): Promise<AuthenticationResult> {
      throw new Error('Function not implemented.');
    },
    getTokenCache: function (): ITokenCache {
      throw new Error('Function not implemented.');
    },
    getLogger: function (): Logger {
      throw new Error('Function not implemented.');
    },
    setLogger: function (logger: Logger): void {
      throw new Error('Function not implemented.');
    },
    setActiveAccount: function (account: AccountInfo | null): void {
      throw new Error('Function not implemented.');
    },
    getActiveAccount: function (): AccountInfo | null {
      throw new Error('Function not implemented.');
    },
    initializeWrapperLibrary: function (sku: WrapperSKU, version: string): void {
      throw new Error('Function not implemented.');
    },
    setNavigationClient: function (navigationClient: INavigationClient): void {
      throw new Error('Function not implemented.');
    },
    getConfiguration: function (): BrowserConfiguration {
      throw new Error('Function not implemented.');
    },
    hydrateCache: function (result: AuthenticationResult, request: PopupRequest | RedirectRequest | SilentRequest | SsoSilentRequest): Promise<void> {
      throw new Error('Function not implemented.');
    },
    clearCache: function (logoutRequest?: ClearCacheRequest | undefined): Promise<void> {
      throw new Error('Function not implemented.');
    }
  }

  render(<App Instance={mockPublicClientApplication}/>);
  const linkElement = screen.getByText(/learn react/i);
  expect(linkElement).toBeInTheDocument();
});
