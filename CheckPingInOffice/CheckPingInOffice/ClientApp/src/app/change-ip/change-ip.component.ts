import { Component, OnInit } from '@angular/core';
import { PingService } from '../services/ping.service';

@Component({
  selector: 'app-change-ip',
  templateUrl: './change-ip.component.html',
  styleUrls: ['./change-ip.component.css']
})
export class ChangeIpComponent implements OnInit {

  actualIp: string;
  ipMass: string[];
  ip: string;
  ipNew: string;
  result: string;


  constructor(private pingService : PingService) { }

  async ngOnInit() {

    let ip = await this.pingService.getIp();
    this.ipMass = ip.ipAddress;
    this.actualIp = this.ipMass[0];
  }

  async addIp(){
    let result = await this.pingService.addIp(this.ipNew);
    this.result = result.response;
    
    let ip = await this.pingService.getIp();
    this.ipMass = ip.ipAddress;
  }

  async updateIp(){
    let result = await this.pingService.updateIp(this.actualIp, this.ipNew);
    this.result = result.response;

    let ip = await this.pingService.getIp();
    this.ipMass = ip.ipAddress;
  }

  async deleteIp(){
    let result = await this.pingService.deleteIp(this.actualIp);
    this.result = result.response;

    let ip = await this.pingService.getIp();
    this.ipMass = ip.ipAddress;
  }
}