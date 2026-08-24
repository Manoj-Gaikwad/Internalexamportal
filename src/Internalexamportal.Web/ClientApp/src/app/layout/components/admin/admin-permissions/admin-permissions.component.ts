import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AdminService } from 'app/layout/services/admin.service';
import { PrivilegeService } from 'app/layout/services/privilege.service';
import { isNullOrUndefined, isNull } from 'util';
import { SaveRolePermissionModel } from 'app/layout/entities/models';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { isEmpty } from 'lodash';
import { AddRoleComponent } from './add-role/add-role.component';
import { Roles } from 'app/layout/entities/globalConstants';
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'cg-admin-permissions',
    templateUrl: './admin-permissions.component.html',
    styleUrls: ['./admin-permissions.component.scss'],
    providers: [PrivilegeService],
    standalone: false
})
export class AdminPermissionsComponent implements OnInit {

  public isSpinner = false;
  public roles: Array<any>;
  public roleId: string = null;
  public permissions: Array<any> = [];
  public parentPermissions: Array<any> = [];
  public childPermissions: Array<any> = [];
  public rolesEnum = Roles;

  isSearchText: boolean;
  searchText: string;
  public searchTerm = new Subject<string>();
  public dataSource = new MatTableDataSource<any>();
  arrays: any;
  displayedColumns: string[] = ['id', 'name','count', 'createdDate', 'isActive', 'action'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  public saveRolePermissionModel = new SaveRolePermissionModel();

  constructor(private _privilegeService: PrivilegeService, private _snackbar: SnackBarService,
    public dialog: MatDialog) { }

  ngOnInit() {
    this.getAllPermission();
    this.getAllRoles();

    //search box on change.
    this.searchTerm.pipe(
      debounceTime(100),
      distinctUntilChanged())
      .subscribe(value => {
        this.isSearchText = (!isEmpty(value) && !isNull(value));
        this.applyFilter(value);
      });
  }


  applyFilter(value: string) {
    this.dataSource.filter = value
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  clearSearch() {
    this.searchText = '';
    this.isSearchText = false;
    this.paginator.firstPage();
    this.getAllRoles();
  }

  openAddRoleDialog() {
    const dialogRef = this.dialog.open(AddRoleComponent, {
      width: '400px',
      panelClass: ['app-no-padding-dialog']
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getAllPermission();
        this.getAllRoles();
      }
    });
  }

  openEditRoleDialog(role) {   
    const dialogRef = this.dialog.open(AddRoleComponent, {
      width: '400px',
      panelClass: ['app-no-padding-dialog'],
      data: {
        id: role.id,
        name: role.name
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getAllPermission();
        this.getAllRoles();
      }
    });
  }

  getAllRoles() {
    this.showSpinner()
    this._privilegeService.getAllRoles()
      .subscribe(res => {
        this.arrays = res.roles;
        this.dataSource = new MatTableDataSource(this.arrays);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;

        this.hideSpinner();
      }, err => {
        this.hideSpinner();
      })
  }

  toggleRoleStatus(role) {  
    this._privilegeService.updateRoleStatus(role.id)
      .subscribe((res: any) => {
        this._snackbar.success(`Role made ${(role.isActive ? 'Inactive' : 'Active')} Successfully.`);
        this.getAllPermission();
        this.getAllRoles();
      }, () => { }
      )

  }

  getAllPermission() {
    this.showSpinner()
    this._privilegeService.getAllPermissions(this.roleId)
      .subscribe(res => {
        this.roles = res.roleResult;
        this.permissions = res.rolePermissions;

        this.parentPermissions = this.permissions.filter(prop => prop.parentId == 0);
        this.childPermissions = this.permissions.filter(prop => prop.parentId != 0);

        this.hideSpinner();
      }, err => {
        this.hideSpinner();
      })

  }

  setPermission(permissionId, event) {

    if (isNullOrUndefined(this.roleId)) {
      return;
    }
    var checked = event.checked;
    if (isNullOrUndefined(checked)) { checked = event.target.checked; }

    var index = this.getPermissionIndex(permissionId);
    if (index < 0) {
      return;
    }

    this.permissions[index].isChecked = checked;
  }

  onParentPermissionChange(permissionId, event) {

    if (isNullOrUndefined(this.roleId)) {
      return;
    }
    var checked = event.checked;
    if (isNullOrUndefined(checked)) { checked = event.target.checked; }

    var i = this.getPermissionIndex(permissionId);
    if (i < 0) {
      return;
    }
    this.permissions[i].isChecked = checked;

    this.permissions.forEach((item, index) => {
      if (item.parentId == permissionId)
        this.permissions[index].isChecked = checked;
    });
  }

  checkParentIndertiminateState(permissionId): boolean {

    var children = this.permissions.filter(prop => prop.parentId == permissionId);

    var total = children.length;

    var checked = children.filter(prop => prop.isChecked).length;

    if (total == checked || checked == 0) {
      if (checked == 0) {
        var index = this.getPermissionIndex(permissionId);
        if (index < 0) {
          return;
        }
        this.permissions[index].isChecked = false;
      }
      return false;
    } else {
      return true;
    }
  }

  onSubmit() {
    this.showSpinner();
    this.saveRolePermissionModel.roleId = this.roleId;
    this.saveRolePermissionModel.rolePermissions = this.permissions;
    this._privilegeService.saveRolePermission(this.saveRolePermissionModel)
      .subscribe(
        res => {
          this._snackbar.success('Roles and Permissions saved successfully.');
          this.hideSpinner();
        }, onerror => {
          this.hideSpinner();
        }
      );

  }

  getPermissionIndex(permissionId: number): number {
    return this.permissions.findIndex(prop => prop.id == permissionId);
  }

  showSpinner() { this.isSpinner = true }
  hideSpinner() { this.isSpinner = false }
}
