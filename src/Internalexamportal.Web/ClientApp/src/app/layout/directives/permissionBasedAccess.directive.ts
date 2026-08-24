import { Directive, Input, ElementRef, AfterViewInit } from '@angular/core';
import * as CryptoJS from 'crypto-js';
import { cryptoKey } from 'app/layout/entities/globalConstants';

export interface Permission {
    PermissionId: number;
    [key: string]: any; // other properties if needed
}

@Directive({
    selector: '[permission-based-access]',
    standalone: false
})
export class PermissionBasedAccessDirective implements AfterViewInit {

    @Input() permissionKey!: number; // mandatory input

    private userRolePermissions: Permission[] = [];

    constructor(private elRef: ElementRef<HTMLElement>) { }

    ngAfterViewInit(): void {
        const encryptedPermissions = localStorage.getItem('permissions');
        if (!encryptedPermissions) {
            this.hideElement();
            return;
        }

        let decodedPermissions: Permission[] = [];
        try {
            const decrypted = CryptoJS.AES
                .decrypt(encryptedPermissions.trim(), cryptoKey.trim())
                .toString(CryptoJS.enc.Utf8);

            if (decrypted) {
                decodedPermissions = JSON.parse(decrypted);
            }
        } catch (error) {
            console.error('Failed to decrypt or parse permissions:', error);
            this.hideElement();
            return;
        }

        this.userRolePermissions = decodedPermissions;

        const hasPermission = this.userRolePermissions.some(
            perm => perm.PermissionId == this.permissionKey
        );

        if (!hasPermission) {
            this.hideElement();
        }
    }

    private hideElement(): void {
        // fully remove element from DOM
        this.elRef.nativeElement.remove();

        // or just hide it (optional)
        // this.elRef.nativeElement.style.display = 'none';
    }
}
