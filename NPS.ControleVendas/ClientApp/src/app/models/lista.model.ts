export class Lista<T> {
  total: number;
  lst: T[] = [];

  constructor(values: any = {}) {
    this.total = 0;
    if (Object.entries(values).length === 0 && values.constructor === Object) {
      return;
    }
    Object.assign(this, values);
  }
}
