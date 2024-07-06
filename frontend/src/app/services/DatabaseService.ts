import { Injectable } from '@angular/core';
import { Storage } from '@ionic/storage';

@Injectable({
  providedIn: 'root',
})
export class DatabaseService {
  constructor(private storage: Storage) {
    this.storage.create();
  }

  setItem(key, _object) {
    this.storage.set(key, _object).then();
  }

  getItem(key) {
    return this.storage.get(key);
  }

  removeItem(key) {
    return this.storage.remove(key);
  }
}
