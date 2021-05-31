import { Component, OnInit } from '@angular/core';
import { PingService } from '../services/ping.service';
import { IpAllModel } from '../models/ip.all.model';
import { ApiService } from '../services/api.service';
import { ApiAllModel } from '../models/api.all.model';

@Component({
  selector: 'app-change-ip',
  templateUrl: './change-ip.component.html',
  styleUrls: ['./change-ip.component.css']
})
export class ChangeIpComponent implements OnInit {

  nameIp: string;
  address: IpAllModel;
  MassIp: IpAllModel[];

  nameApi: string;
  api: ApiAllModel;
  MassApi: ApiAllModel[];

  nameNewIp: string;
  ipNew: string;

  nameNewApi: string;
  pathNew: string;

  result: string;

  constructor(private pingService : PingService, private apiService: ApiService) { }

  async ngOnInit() {

    let ip = await this.pingService.getIp();
    this.MassIp = ip.ipAddress;
    this.address = this.MassIp[0];
    this.nameIp = this.MassIp[0].nameConnect + " - " + this.MassIp[0].ipAddress;

    let api = await this.apiService.getApi();
    this.MassApi = api.apiModels;
    this.api = this.MassApi[0];
    this.nameApi = this.MassApi[0].name + " - " + this.MassApi[0].path;

  }

  public async addIp(){

    if(this.nameNewIp === undefined){
      this.nameNewIp = "";
    }

    if(this.ipNew === undefined){
      this.ipNew = "";
    }

    let result = await this.pingService.addIp(this.nameNewIp,this.ipNew);
    this.result = result.response;
      
    let ip = await this.pingService.getIp();
    this.MassIp = ip.ipAddress;
  }

  public async addApi(){

    if(this.nameNewApi === undefined){
      this.nameNewApi = "";
    }

    if(this.pathNew === undefined){
      this.pathNew;
    }

    this.pathNew = this.pathNew.replace("&", "%26");

    let result = await this.apiService.addApi(this.nameNewApi, this.pathNew);
    this.result = result.response;

    let api = await this.apiService.getApi();
    this.MassApi = api.apiModels;

  }

  public async updateIp(){
    
    if(this.ipNew === undefined){
      this.ipNew = "";
    }

    let result = await this.pingService.updateIp(this.address.ipAddress, this.ipNew);
    this.result = result.response;

    let ip = await this.pingService.getIp();
    this.MassIp = ip.ipAddress;
  }

  public async updateApi(){

    if(this.pathNew === undefined){
      this.pathNew = "";
    }

    this.pathNew = this.pathNew.replace("&", "%26");
    
    let result = await this.apiService.updateApi(this.api.path, this.pathNew);
    this.result = result.response;

    let api = await this.apiService.getApi();
    this.MassApi = api.apiModels;
  }

  public async deleteIp(){

    let result = await this.pingService.deleteIp(this.address.nameConnect, this.address.ipAddress);
    this.result = result.response;

    let ipAddress = await this.pingService.getIp();
    this.MassIp = ipAddress.ipAddress;
  }

  public async deleteApi(){
    
    this.api.path = this.api.path.replace("&", "%26");

    let result = await this.apiService.deleteApi(this.api.name, this.api.path);
    this.result = result.response;

    let api = await this.apiService.getApi();
    this.MassApi = api.apiModels;
  }
}