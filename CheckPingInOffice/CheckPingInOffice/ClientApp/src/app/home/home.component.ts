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

  ping: number;

  constructor(private pingService : PingService){}

  async ngOnInit(){
    const source = interval(1000);
    const subscribe = source.subscribe(val => this.getTime());
  }

  public async getTime(){
    let ping = await this.pingService.getPing();
    this.ping = ping.percents;
    console.log(this.ping);
  }
}
