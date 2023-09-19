import AddWord, { Word } from "./AddWord";

export interface CreateWordProps {
    Callback: (synonyms: string[]) => void
}

export const CreateWord = (Props: CreateWordProps) => {
    function create(word: Word): Promise<string> {
        const headers: Headers = new Headers()
    
        headers.set('Content-Type', 'application/json')
        headers.set('Accept', 'application/json')
    
        const request: RequestInfo = new Request('https://apim-synonym-lookup-dev.azure-api.net/words/v1/words/', {
          method: 'POST',
          body: JSON.stringify(word),
          headers: headers
        })
    
        return fetch(request)
          .then(res => res.headers.get("Location") ?? "")
    }
    
    const handleCreate = (word: Word) => {
        create(word)
            .then(location => {
                if (location !== "") {
                    searchWithLocation(location)
                        .then(synonyms => {
                            Props.Callback(synonyms);
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
        <AddWord onAddWord={handleCreate}/>
    )
}