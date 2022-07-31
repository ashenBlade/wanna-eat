import {CookingAppliance} from "../entities/cooking-appliance";

export interface ICookingApplianceRepository {
    getCookingAppliancesAsync(pageNumber: number, pageSize: number): Promise<CookingAppliance[]>
    getCookingApplianceById(id: number): Promise<CookingAppliance | null>
}

export class CookingApplianceRepository implements ICookingApplianceRepository {
    getCookingApplianceById(id: number): Promise<CookingAppliance | null> {
        return Promise.resolve(null);
    }

    getCookingAppliancesAsync(pageNumber: number, pageSize: number): Promise<CookingAppliance[]> {
        return Promise.resolve([]);
    }
    
}