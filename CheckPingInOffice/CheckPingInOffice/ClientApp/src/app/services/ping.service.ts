import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PingModel } from '../models/ping.model'

@Injectable({
  providedIn: 'root'
})
export class PingService {

  constructor(private http: HttpClient) { }


  public async getPing() {
    const url: string = "https://localhost:44369/Ping"
    const ping = await this.http.get<PingModel>(url).toPromise();

    return ping;
  }
}