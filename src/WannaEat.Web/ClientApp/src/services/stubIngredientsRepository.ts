import {IIngredientsRepository} from "../interfaces/iIngredientRepository";
import {Ingredient} from "../entities/ingredient";

export class StubIngredientsRepository implements IIngredientsRepository {
    static ingredients: Ingredient[] = [
        {id:478,name:"Брюссельская капуста"},
        {id:479,name:"Буженина"},
        {id:480,name:"Буйвол"},
        {id:481,name:"Буйволятина"},
        {id:482,name:"Буковый орешек"},
        {id:483,name:"Булгур"},
        {id:484,name:"Бычок-песочник"},
        {id:485,name:"Вакциниум сетчатый"},
        {id:486,name:"Валерианелла"},
        {id:487,name:"Ванилин"},
        {id:488,name:"Васаби"},
        {id:489,name:"Васаби игай-яки"},
        {id:490,name:"Ветчина"},
        {id:491,name:"Ветчина маложирная"},
        {id:492,name:"Ветчина постная"},
        {id:493,name:"Вешенки"},
        {id:494,name:"Визига сухая"},
        {id:495,name:"Вилочковая железа телячья"},
        {id:496,name:"Вино"},
        {id:497,name:"Виноград"},
        {id:498,name:"Виноград американский"},
        {id:499,name:"Виноград киш-миш"},
        {id:500,name:"Виноградно-яблочный сок"},
        {id:501,name:"Виноградные листья"},
        {id:502,name:"Виноградный сок"},
        {id:503,name:"Виноградный сок-напиток"},
        {id:504,name:"Вишня"},
        {id:505,name:"Вобла"},
        {id:506,name:"Водка"},
        {id:507,name:"Водный шпинат"},
        {id:508,name:"Водоросли"},
        {id:509,name:"Водяника"},
        {id:510,name:"Водяной буйвол"},
        {id:511,name:"Водяной кресс"},
        {id:512,name:"Воздушная кукуруза"},
        {id:513,name:"Воздушная пшеница"},
        {id:514,name:"Воздушный рис"},
        {id:515,name:"Волованы"},
        {id:516,name:"Восковая тыква"},
        {id:517,name:"Галеты"}
    ];

    ingredients: Ingredient[];

    constructor(ingredients?: Ingredient[]) {
        this.ingredients = ingredients ?? StubIngredientsRepository.ingredients;
    }
    
    findWithName(name: string, pageNumber: number, max: number): Promise<Ingredient[]> {
        return Promise.resolve(this.ingredients.filter(i => i.name.match(name)).slice((pageNumber - 1) * max, pageNumber * max));
    }

    getProductById(id: number): Promise<Ingredient | null> {
        return Promise.resolve(this.ingredients.find(i => i.id === id) || null);
    }

    getProductsAsync(pageNumber: number, pageSize: number): Promise<Ingredient[]> {
        return Promise.resolve(this.ingredients.slice((pageNumber - 1) * pageSize, pageNumber * pageSize));
    }

}