export type TransportProtocol = 'http' | 'https' | 'ws' | 'wss';

export function getTransportProtocol(isSecureContext: boolean, kind: 'http' | 'ws'): TransportProtocol {
  if (kind === 'ws') {
    return isSecureContext ? 'wss' : 'ws';
  }

  return isSecureContext ? 'https' : 'http';
}

export function buildProtocolAwareUrl(url: string, protocol: TransportProtocol): string {
  const value = url?.trim() ?? '';

  if (!value) {
    return '';
  }

  if (value.startsWith('ws://') || value.startsWith('wss://')) {
    return value;
  }

  if (value.startsWith('http://') || value.startsWith('https://')) {
    const parsedUrl = new URL(value);
    return `${protocol}://${parsedUrl.host}${parsedUrl.pathname}${parsedUrl.search}${parsedUrl.hash}`;
  }

  return `${protocol}://${value}`;
}
