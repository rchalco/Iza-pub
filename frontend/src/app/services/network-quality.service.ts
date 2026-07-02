import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NetworkQualityService {

  isOnline(): boolean {
    return navigator?.onLine !== false;
  }

  getEffectiveType(): string {
    if (typeof navigator === 'undefined') return 'unknown';
    const conn = (navigator as any).connection;
    return conn?.effectiveType || 'unknown';
  }

  getRtt(): number {
    if (typeof navigator === 'undefined') return 0;
    const conn = (navigator as any).connection;
    return conn?.rtt || 0;
  }

  getDownlink(): number {
    if (typeof navigator === 'undefined') return 0;
    const conn = (navigator as any).connection;
    return conn?.downlink || 0;
  }

  getNetworkTypeLabel(): string {
    const labels: Record<string, string> = {
      '4g': '4G',
      '3g': '3G',
      '2g': '2G',
      'slow-2g': '2G Lento',
    };
    return labels[this.getEffectiveType()] || 'Desconocido';
  }

  getTimeoutMultiplier(): number {
    const multipliers: Record<string, number> = {
      '4g': 0.5,
      '3g': 0.8,
      '2g': 1.2,
      'slow-2g': 1.5,
    };
    return multipliers[this.getEffectiveType()] || 1;
  }

  getAdaptiveTimeout(baseMs: number): number {
    return Math.round(baseMs * this.getTimeoutMultiplier());
  }
}