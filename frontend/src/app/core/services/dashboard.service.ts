import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ManagementDashboardDto } from '../models/managementDashboard/management-dashboard.dto';



@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  private apiUrl =
    `${environment.apiUrl}/managementdashboard`;


  constructor(
    private http: HttpClient
  ) {}


  getManagementDashboard():
    Observable<ManagementDashboardDto> {

    return this.http.get<ManagementDashboardDto>(
      this.apiUrl
    );

  }

}