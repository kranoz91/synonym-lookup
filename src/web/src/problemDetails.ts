export interface ProblemDetails {
    type: string,
    title: string,
    status: number,
    errorCode: string
}

export enum ErrorCode {
    WordNotFound,
    WordCannotBeEmpty,
    MaxLengthExceeded,
    UnsupportedCharacters
}

export function isProblemDetails(object: any): object is ProblemDetails {
    return 'errorCode' in object;
}

export function ResolveError(error: ProblemDetails) : Error | undefined {
    switch (error.errorCode){
        case 'SL101':
            return Errors.find(error => error.code === ErrorCode.WordNotFound);
        case 'SL201':
            return Errors.find(error => error.code === ErrorCode.WordCannotBeEmpty);
        case 'SL202':
            return Errors.find(error => error.code === ErrorCode.MaxLengthExceeded);
        case 'SL203':
            return Errors.find(error => error.code === ErrorCode.UnsupportedCharacters);
    }
}

export interface Error {
    code: ErrorCode,
    message: string
}

export const Errors: Error[] = [
    { code: ErrorCode.WordNotFound, message: "The word that you are searching for could not be found." },
    { code: ErrorCode.WordCannotBeEmpty, message: "You can't create an empty word." },
    { code: ErrorCode.MaxLengthExceeded, message: "A word is not allowed to exceed 64 characters." },
    { code: ErrorCode.UnsupportedCharacters, message: "A word is only allowed to consist of characters A - Ö" }
]