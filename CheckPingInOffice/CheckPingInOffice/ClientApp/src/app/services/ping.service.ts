import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IpModel } from '../models/ip.model'
import { PingModel } from '../models/ping.model'

@Injectable({
  providedIn: 'root'
})
export class PingService {

  constructor(private http: HttpClient) { }


  public async getPing() {
    const url: string = "/ping";
    const ping = await this.http.get<PingModel>(url).toPromise();

    return ping;
  }

  public async getIp(): Promise<IpModel>{
    const url: string = "/ping/getIp";
    const ip = await this.http.get<IpModel>(url).toPromise();

    return ip;
  }
  public async setIp(ipAddress: string): Promise<IpModel>{
    const url: string ="/ping/setIp?ip=" + ipAddress;
    const ip = await this.http.get<IpModel>(url).toPromise();

    return ip;
  }
}