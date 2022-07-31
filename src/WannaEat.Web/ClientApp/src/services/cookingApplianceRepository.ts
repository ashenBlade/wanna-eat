import {CookingAppliance} from "../entities/cooking-appliance";

export interface ICookingApplianceRepository {
    getCookingAppliancesAsync(pageNumber: number, pageSize: number): Promise<CookingAppliance[]>
    getCookingApplianceById(id: number): Promise<CookingAppliance | null>
}

export class CookingApplianceRepository implements ICookingApplianceRepository {
    getCookingApplianceById(id: number): Promise<CookingAppliance | null> {
        return fetch(`/api/v1/appliances/${id}`).then(res => res.status === 404 ? null : res.json())
    }

    getCookingAppliancesAsync(pageNumber: number, pageSize: number): Promise<CookingAppliance[]> {
        return fetch(`/api/v1/appliances?n=${pageNumber}&s=${pageSize}`).then(res => res.json())
    }
}