import {useState} from "react";

export const useToggle = (init: boolean = false): [boolean, () => void] => {
    const [state, setState] = useState<boolean>(init)
    return [state, () => setState(!state)]
}