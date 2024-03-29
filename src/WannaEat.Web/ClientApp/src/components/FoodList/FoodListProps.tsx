import {Food} from "../../entities/food";

export interface FoodListProps<TFood extends Food> {
    foods: TFood[],
    onChoose?: ((product: TFood) => void),
    emptyListPlaceholder?: string,
    onScrollToEnd?: () => (void)
    additionalAction?: {
        sign?: string
        hint?: string
    }
    isLoading?: boolean
}