import { Injectable } from '@angular/core';
import { Storage } from '@ionic/storage';

@Injectable({
  providedIn: 'root',
})
export class DatabaseService {
  constructor(private storage: Storage) {
    this.storage.create();
  }

  /** Devuelve la promesa para poder esperar la escritura; los llamadores que no la usan no cambian. */
  setItem(key, _object) {
    return this.storage.set(key, _object);
  }

  getItem(key) {
    return this.storage.get(key);
  }

  removeItem(key) {
    return this.storage.remove(key);
  }
}
