class Dish {
    constructor(readonly id: number, readonly name: string) {
        if (!Number.isInteger(id)) {
            throw Error(`Dish id must be integer. Given: ${id}`)
        }
    }
}