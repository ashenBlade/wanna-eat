import {IRecipeService} from "../interfaces/IRecipeService";
import {Ingredient} from "../entities/ingredient";
import {Recipe} from "../entities/recipe";

export class StubFoodService implements IRecipeService {
    static recipes: Recipe[] = [
        {"id": 775, "name": "Киви", originUrl: ''},
        {"id": 379, "name": "Ёкан", originUrl: ''},
        {"id": 380, "name": "Ёрш", originUrl: ''},
        {"id": 381, "name": "Абрикос", originUrl: ''},
        {"id": 382, "name": "Абрикосовый джем", originUrl: ''},
        {"id": 383, "name": "Абрикосы", originUrl: ''},
        {"id": 384, "name": "Абрикосы без кожуры", originUrl: ''},
        {"id": 385, "name": "Авокадо", originUrl: ''},
        {"id": 386, "name": "Агава сырая", originUrl: ''},
        {"id": 387, "name": "Агар", originUrl: ''},
        {"id": 388, "name": "Агар пищевой", originUrl: ''},
        {"id": 389, "name": "Айва", originUrl: ''},
        {"id": 390, "name": "Айвовый сок", originUrl: ''},
        {"id": 391, "name": "Айран негазированный", originUrl: ''},
        {"id": 392, "name": "Айран негазированный нежирный", originUrl: ''},
        {"id": 393, "name": "Айран сильногазированный", originUrl: ''},
        {"id": 394, "name": "Актинидия", originUrl: ''},
        {"id": 395, "name": "Альбула", originUrl: ''},
        {"id": 396, "name": "Амарант", originUrl: ''},
        {"id": 397, "name": "Амарант листья", originUrl: ''},
        {"id": 398, "name": "Амарантовые хлопья", originUrl: ''},
        {"id": 399, "name": "Амур", originUrl: ''},
        {"id": 400, "name": "Ананас", originUrl: ''},
        {"id": 401, "name": "Ананасовый сок", originUrl: ''},
        {"id": 402, "name": "Анис", originUrl: ''},
        {"id": 403, "name": "Аннона сетчатая", originUrl: ''},
        {"id": 404, "name": "Антрекот", originUrl: ''},
        {"id": 405, "name": "Анчоус атлантический", originUrl: ''},
        {"id": 406, "name": "Анчоус европейский", originUrl: ''},
        {"id": 407, "name": "Анчоусное масло", originUrl: ''},
        {"id": 408, "name": "Анчоусы атлантические", originUrl: ''},
        {"id": 409, "name": "Апельсин", originUrl: ''},
        {"id": 410, "name": "Апельсин Флорида", originUrl: ''},
        {"id": 411, "name": "Апельсин валенсийский", originUrl: ''},
        {"id": 412, "name": "Апельсин пупочный", originUrl: ''},
        {"id": 413, "name": "Апельсиново-ананасовый сок", originUrl: ''},
        {"id": 414, "name": "Апельсиново-грейпфрутовый сок", originUrl: ''},
        {"id": 415, "name": "Апельсиново-клубнично-банановый сок", originUrl: ''},
        {"id": 416, "name": "Апельсиновое молоко", originUrl: ''},
        {"id": 417, "name": "Апельсиновый джем", originUrl: ''}]
    recipes: Recipe[]
    constructor(recipes?: Recipe[]) {
        this.recipes = recipes || StubFoodService.recipes;
    }
    findRelevantRecipes(products: Ingredient[]): Promise<Recipe[]> {
        return new Promise(r => setTimeout(() => r(this.recipes), 1000));
    }
}