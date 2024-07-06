// eslint-disable-next-line @typescript-eslint/no-namespace
namespace wraper {
  export class ResponseObject<T> extends wraper.Response {
    code: string;
    object: T;
  }
}
