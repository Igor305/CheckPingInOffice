import { Component, OnInit } from '@angular/core';
import { PingService } from '../services/ping.service';
import { ApiService } from '../services/api.service';
import { interval } from 'rxjs'
import { IpAllModel } from '../models/ip.all.model';
import { ApiAllModel } from '../models/api.all.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {

  type:string;
  types: string[] = ["Ip address","Web service"]

  nameIp: string;
  address: IpAllModel;
  massAddress: IpAllModel[];

  nameApi: string;
  api: ApiAllModel;
  massApi:ApiAllModel[];

  percentsLastHour: number;
  nAllSendLastHour: number;
  nTrueSendLastHour: number;
  nFalseSendLastHour: number;
  percentsLostLastHour: string;

  percentsForDay: number;
  nAllSendForDay: number;
  nTrueSendForDay: number;
  nFalseSendForDay: number;
  percentsLostForDay: string;

  percentsYesterday:  number;
  nAllSendYesterday: number;
  nTrueSendYesterday: number;
  nFalseSendYesterday: number;
  percentsLostYesterday: string;

  hours: number;
  minutes: string;
  secondPoint: string;
  second:boolean = false;

  constructor(private pingService : PingService, private apiService : ApiService){}

  async ngOnInit(){

    this.type = "Ip address";

    setInterval(() => this.getTime(), 1000);
    setInterval(() => this.getPing(), 1000);
  }

  public async getTime() {

    const now = new Date();
    
    let min, sec = "";
    if (now.getMinutes()<10) {min = "0"+ now.getMinutes();}
    if (now.getMinutes()>=10) {min = now.getMinutes();}
    if (now.getSeconds()<10) {sec = "0"+ now.getSeconds();}
    if (now.getSeconds()>=10) {sec = now.getSeconds().toString();}

    this.hours = now.getHours();
    this.minutes = min;
  
    switch(this.second){
      case true:{
        this.secondPoint =":";
        this.second = false;
      } ;break;
      case false:{
        this.secondPoint =" ";
        this.second = true;
      } ;break;
    }
  }

  public async getPing(){

    this.isWork();

    if (this.type == "Ip address")
    {
      let ping = await this.pingService.getPing(this.address.nameConnect, this.address.ipAddress);

      this.percentsLastHour = ping.percentsLastHour;
      this.nAllSendLastHour = ping.nAllSendLastHour;
      this.nTrueSendLastHour = ping.nTrueSendLastHour;
      this.nFalseSendLastHour = ping.nFalseSendLastHour;
      let persentsLostLastHour = 100 - this.percentsLastHour;
  
      this.percentsLostLastHour = persentsLostLastHour.toFixed(2);
  
  
      this.percentsForDay = ping.percentsForDay;
      this.nAllSendForDay = ping.nAllSendForDay;
      this.nTrueSendForDay = ping.nTrueSendForDay;
      this.nFalseSendForDay = ping.nFalseSendForDay;
      let percentsLostForDay = 100 - this.percentsForDay;
  
      this.percentsLostForDay = percentsLostForDay.toFixed(2);
  
      this.percentsYesterday = ping.percentsYesterday;
      this.nAllSendYesterday = ping.nAllSendYesterday;
      this.nTrueSendYesterday = ping.nTrueSendYesterday;
      this.nFalseSendYesterday = ping.nFalseSendYesterday;
      let percentsLostYesterday = 100 - ping.percentsYesterday;
  
      this.percentsLostYesterday = percentsLostYesterday.toFixed(2);
    }

    if (this.type == "Web service")
    {
      this.api.path = this.api.path.replace("&", "%26");
      let ping = await this.apiService.getPersent(this.api.name, this.api.path);

      this.percentsLastHour = ping.percentsLastHour;
      this.nAllSendLastHour = ping.nAllSendLastHour;
      this.nTrueSendLastHour = ping.nTrueSendLastHour;
      this.nFalseSendLastHour = ping.nFalseSendLastHour;
      let persentsLostLastHour = 100 - this.percentsLastHour;
  
      this.percentsLostLastHour = persentsLostLastHour.toFixed(2);
  
  
      this.percentsForDay = ping.percentsForDay;
      this.nAllSendForDay = ping.nAllSendForDay;
      this.nTrueSendForDay = ping.nTrueSendForDay;
      this.nFalseSendForDay = ping.nFalseSendForDay;
      let percentsLostForDay = 100 - this.percentsForDay;
  
      this.percentsLostForDay = percentsLostForDay.toFixed(2);
  
      this.percentsYesterday = ping.percentsYesterday;
      this.nAllSendYesterday = ping.nAllSendYesterday;
      this.nTrueSendYesterday = ping.nTrueSendYesterday;
      this.nFalseSendYesterday = ping.nFalseSendYesterday;
      let percentsLostYesterday = 100 - ping.percentsYesterday;
  
      this.percentsLostYesterday = percentsLostYesterday.toFixed(2);
    }
  }
  
  public async isWork(){

    let ip = await this.pingService.getIp();

    if (ip.ipAddress.length != 0){

    this.massAddress = ip.ipAddress;
    this.address = this.massAddress[0];
    this.nameIp = this.address.nameConnect;
    this.address.ipAddress = this.massAddress[0].ipAddress;

    }
 
    let api = await this.apiService.getApi();

    if (api.apiModels.length != 0){

    this.massApi = api.apiModels;
    this.api = this.massApi[0];
    this.nameApi = this.api.name;
    this.api.path = this.massApi[0].path;
    
    }
  }
}