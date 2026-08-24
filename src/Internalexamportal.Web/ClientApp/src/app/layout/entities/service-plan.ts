import { Injectable } from '@angular/core';
export class ServicePlan {
    public name: string;
    public ownerId: string;
    public id: number;
    public statusId: number;
    public licenses: Array<License>;
    public servicePlanTypeId: number
}

export class License {
    public Id: number;
    public servicePlanId: number;
    public licenseTypeId: number
    public quantity: number;
}

@Injectable()
export class ServicePlanTypeService {
    getServicePlanTypes() {
        return servicePlanType;
    }

    getServicePlanStatus() {
        return servicePlanStatus;
    }
}

const servicePlanType = { Basic: 1, Standard: 2, Premium: 3 };

const servicePlanStatus = { Incomplete: 1, Complete: 2 };