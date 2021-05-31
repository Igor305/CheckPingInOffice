import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IpAllResponseModel } from '../models/ip.all.response.model';
import { IpModel } from '../models/ip.model'
import { PingModel } from '../models/ping.model'

@Injectable({
  providedIn: 'root'
})
export class PingService {

  constructor(private http: HttpClient) { }


  public async getPing(name: string, ip: string) {
    const url: string = "/ping?name=" + name + "&&ip=" + ip;
    const ping = await this.http.get<PingModel>(url).toPromise();

    return ping;
  }

  public async getIp(): Promise<IpAllResponseModel>{
    const url: string = "/ping/getIp";
    const model = await this.http.get<IpAllResponseModel>(url).toPromise();

    return model;
  }

  public async addIp(name: string, ip: string): Promise<IpModel>{
    const url: string = "/ping/addIp?name=" + name + "&&ip="+ ip;
    const model = await this.http.get<IpModel>(url).toPromise();

    return model;
  }

  public async updateIp(ip: string, ipNew: string): Promise<IpModel>{
    const url: string ="/ping/updateIp?ip="+ ip + "&&ipNew=" + ipNew;
    const model = await this.http.get<IpModel>(url).toPromise();

    return model;
  }

  public async deleteIp(name: string, ip: string): Promise<IpModel>{
    const url: string = "/ping/deleteIp?name=" + name + "&&ip="+ ip;
    const model = await this.http.get<IpModel>(url).toPromise();

    return model;
  }
}