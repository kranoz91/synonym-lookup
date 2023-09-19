import { SharedState } from "../../App";
import { ResolveError, isProblemDetails } from "../../problemDetails";
import AddWord, { Word } from "./AddWord";
import { Error } from "../../problemDetails";
import { useState } from "react";
import { useMsal } from "@azure/msal-react";
import { AccountInfo } from "@azure/msal-browser";
import { protectedResources } from "../../authConfig";

export interface CreateWordProps {
    UpdateState: (newState: SharedState) => void
}

export const CreateWord = (Props: CreateWordProps) => {
    const [error, setError] = useState<Error|undefined>(undefined);
    const { instance } = useMsal();

    let activeAccount: AccountInfo|null;

    if (instance) {
        activeAccount = instance.getActiveAccount();
    }

    async function create(word: Word): Promise<string> {
        const headers: Headers = new Headers()

        headers.set('Content-Type', 'application/json')
        headers.set('Accept', 'application/json')
        if (activeAccount) {
            let response = await instance.acquireTokenSilent({scopes: protectedResources.synonymLookupAPI.scopes.write, account: activeAccount});
            let accessToken = response.accessToken;
            headers.set('Authorization', 'Bearer ' + accessToken);
        }

        const request: RequestInfo = new Request('https://apim-synonym-lookup-dev.azure-api.net/words/v1/words/', {
          method: 'POST',
          body: JSON.stringify(word),
          headers: headers
        })

        return fetch(request)
            .then(res => {
                if (res.status !== 201) {
                    res.json().then(json => {
                        if (isProblemDetails(json)) {
                            setError(ResolveError(json));
                            return "";
                        }
                    })
                }
                
                setError(undefined);
                return res.headers.get("Location") ?? "";
            })
    }
    
    const handleCreate = (word: Word) => {
        create(word)
            .then(location => {
                if (location !== "") {
                    searchWithLocation(location)
                        .then(synonyms => {
                            var newState: SharedState = {
                                LatestSearch: word.word,
                                Synonyms: synonyms
                            };

                            Props.UpdateState(newState);
                        });
                }
            })
    }

    function searchWithLocation(location: string) : Promise<string[]> {
        const headers: Headers = new Headers()
    
        headers.set('Content-Type', 'application/json')
        headers.set('Accept', 'application/json')
    
        const request: RequestInfo = new Request('https://apim-synonym-lookup-dev.azure-api.net/words' + location, {
          method: 'GET',
          headers: headers
        })
    
        return fetch(request)
          .then(res => res.json())
          .then(res => res as string[])
    }

    return (
        <AddWord onAddWord={handleCreate} error={error}/>
    )
}