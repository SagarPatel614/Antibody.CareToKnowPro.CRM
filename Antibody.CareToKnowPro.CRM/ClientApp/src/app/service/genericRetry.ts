import { Observable, throwError, timer } from "rxjs";
import { mergeMap, finalize } from "rxjs/operators";

export const genericRetryStrategy = ({
  maxRetryAttempts = 4,
  scalingDuration = 300,
  excludedStatusCodes = []
}: {
  maxRetryAttempts?: number,
  scalingDuration?: number,
  excludedStatusCodes?: number[];
} = {}) => (attempts: Observable<any>) => {
  return attempts.pipe(
    mergeMap((error: any, i) => {
      const retryAttempt = i + 1;
      // if maximum number of retries have been met
      // or response is a status code we don't wish to retry, throw error
      if (
        retryAttempt > maxRetryAttempts ||
          excludedStatusCodes.find(e => e === error.status)
      ) {
        return throwError(error);
      }
      console.log(
        `Attempt ${retryAttempt}: retrying in ${retryAttempt *
        scalingDuration}ms`
      );
      // retry after 1s, 2s, etc...
      return timer(retryAttempt * scalingDuration);
    }),
    finalize(() => console.log('We are done!'))
  );
};
