import { Component, OnInit } from '@angular/core';
import { PingService } from '../services/ping.service';

@Component({
  selector: 'app-change-ip',
  templateUrl: './change-ip.component.html',
  styleUrls: ['./change-ip.component.css']
})
export class ChangeIpComponent implements OnInit {

  ip: string;
  ipNew: string;
  result: string;

  constructor(private pingService : PingService) { }

  async ngOnInit() {

    let ip = await this.pingService.getIp();
    this.ip = ip.response;

  }

  async setIp(){
    this.ip = this.ipNew ;
    let ip = await this.pingService.setIp(this.ip);
    this.result = ip.response;
  }
}
