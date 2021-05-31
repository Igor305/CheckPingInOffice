import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiAllResponseModel } from '../models/api.all.response.model';
import { IpModel } from '../models/ip.model';
import { PingModel } from '../models/ping.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  public async getPersent(name: string, path: string) {
    const url: string = "/api?name=" + name + "&path=" + path;
    const ping = await this.http.get<PingModel>(url).toPromise();

    return ping;
  }

  public async getApi(): Promise<ApiAllResponseModel>{
    const url: string = "/api/getApi";
    const model = await this.http.get<ApiAllResponseModel>(url).toPromise();
    
    return model;
  }

  public async addApi(name: string, path: string): Promise<IpModel>{
    const url: string = "/api/addApi?name=" + name + "&path="+ path;
    const model = await this.http.get<IpModel>(url).toPromise();

    return model;
  }

  public async updateApi(path: string, pathNew: string): Promise<IpModel>{
    const url: string ="/api/updateApi?path="+ path + "&pathNew=" + pathNew;
    const model = await this.http.get<IpModel>(url).toPromise();

    return model;
  }

  public async deleteApi(name: string, path: string): Promise<IpModel>{
    const url: string = "/api/deleteApi?name=" + name + "&path="+ path;
    const model = await this.http.get<IpModel>(url).toPromise();

    return model;
  }
}