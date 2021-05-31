import { Component, OnInit } from '@angular/core';
import { interval } from 'rxjs';
import { ApiAllModel } from '../models/api.all.model';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-status-api',
  templateUrl: './status-api.component.html',
  styleUrls: ['./status-api.component.css']
})
export class StatusApiComponent implements OnInit {

  api: ApiAllModel;
  Mass: ApiAllModel[];

  name: string;

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

  constructor(private apiService : ApiService){}

  async ngOnInit(){

    let api = await this.apiService.getApi();
    this.Mass = api.apiModels;
    this.api = this.Mass[0];
    this.name = this.api.name;
    console.log(this.name);
    
    this.api.path = this.Mass[0].path;

    setInterval(() => this.getTime(), 1000);

    const source = interval(1000);
    const subscribe = source.subscribe(val => this.getPing());

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
