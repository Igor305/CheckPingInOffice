import { Component, OnInit } from '@angular/core';
import { PingService } from '../services/ping.service';
import { IpAllModel } from '../models/ip.all.model';

@Component({
  selector: 'app-change-ip',
  templateUrl: './change-ip.component.html',
  styleUrls: ['./change-ip.component.css']
})
export class ChangeIpComponent implements OnInit {

  name: string;
  actualIp: string;
  
  address: IpAllModel;
  Mass: IpAllModel[];

  nameNew: string;
  ipNew: string;
  result: string;


  constructor(private pingService : PingService) { }

  async ngOnInit() {

    let ip = await this.pingService.getIp();
    this.Mass = ip.ipAddress;
    this.address = this.Mass[0];
    this.name = this.Mass[0].nameConnect + " - " + this.Mass[0].ipAddress;

  }

  async addIp(){

    if(this.nameNew === undefined){
      this.nameNew = "";
    }

    if(this.ipNew === undefined){
      this.ipNew = "";
    }

    let result = await this.pingService.addIp(this.nameNew,this.ipNew);
    this.result = result.response;
    
    
    let ip = await this.pingService.getIp();
    this.Mass = ip.ipAddress;
  }

  async updateIp(){
    
    if(this.ipNew === undefined){
      this.ipNew = "";
    }

    let result = await this.pingService.updateIp(this.address.ipAddress, this.ipNew);
    this.result = result.response;

    let ip = await this.pingService.getIp();
    this.Mass = ip.ipAddress;
  }

  async deleteIp(){

    let result = await this.pingService.deleteIp(this.address.nameConnect, this.address.ipAddress);
    this.result = result.response;

    let ipAddress = await this.pingService.getIp();
    this.Mass = ipAddress.ipAddress;
  }
}