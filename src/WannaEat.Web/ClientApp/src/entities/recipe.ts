import {Food} from "./food";

export interface Recipe extends Food {
    readonly imageUrl?: string
    readonly originUrl: string
}