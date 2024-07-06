// eslint-disable-next-line @typescript-eslint/no-namespace
namespace wraper {
  export class ResponseQuery<T> extends wraper.Response {
    listEntities: T[];
  }
}
