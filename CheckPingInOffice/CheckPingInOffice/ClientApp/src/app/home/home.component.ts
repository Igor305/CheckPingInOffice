import { Component, OnInit } from '@angular/core';
import { PingModel } from '../models/ping.model';
import { PingService } from '../services/ping.service';
import { interval } from 'rxjs'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {

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

  timeNow:string;


  constructor(private pingService : PingService){}

  async ngOnInit(){

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
    
    this.timeNow = now.getHours() + ":" + min;
  }

  public async getPing(){
    let ping = await this.pingService.getPing();

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