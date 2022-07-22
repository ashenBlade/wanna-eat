interface ICookingApplianceRepository {
    getCookingAppliancesAsync(pageNumber: number, pageSize: number): Promise<CookingAppliance[]>
    getCookingApplianceById(id: number): Promise<CookingAppliance | null>
}

class CookingApplianceRepository implements ICookingApplianceRepository {
    getCookingApplianceById(id: number): Promise<CookingAppliance | null> {
        return Promise.resolve(null);
    }

    getCookingAppliancesAsync(pageNumber: number, pageSize: number): Promise<CookingAppliance[]> {
        return Promise.resolve([]);
    }
    
}