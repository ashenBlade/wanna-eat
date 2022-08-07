import {Food} from "./food";

export interface Dish extends Food {
    readonly imageUrl?: string
    readonly link: string
}