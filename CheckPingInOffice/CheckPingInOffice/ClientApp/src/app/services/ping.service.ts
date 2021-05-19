import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IpAllModel } from '../models/ip.all.model';
import { IpModel } from '../models/ip.model'
import { PingModel } from '../models/ping.model'

@Injectable({
  providedIn: 'root'
})
export class PingService {

  constructor(private http: HttpClient) { }


  public async getPing(ip: string) {
    const url: string = "/ping?ip=" + ip;
    const ping = await this.http.get<PingModel>(url).toPromise();

    return ping;
  }

  public async getIp(): Promise<IpAllModel>{
    const url: string = "/ping/getIp";
    const model = await this.http.get<IpAllModel>(url).toPromise();

    return model;
  }

  public async addIp(ip: string): Promise<IpModel>{
    const url: string = "/ping/addIp?ip=" + ip;
    const model = await this.http.get<IpModel>(url).toPromise();

    return model;
  }

  public async updateIp(ip: string, ipNew: string): Promise<IpModel>{
    const url: string ="/ping/updateIp?ip=" + ip + "&&ipNew=" + ipNew;
    const model = await this.http.get<IpModel>(url).toPromise();

    return model;
  }

  public async deleteIp(ip: string): Promise<IpModel>{
    const url: string = "/ping/deleteIp?ip=" + ip;
    const model = await this.http.get<IpModel>(url).toPromise();

    return model;
  }
}