import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ScientificPaperDto, ScientificPaperService } from '../service';
import { SignalRService } from '../service/signalr.service';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'sci-paper',
  templateUrl: './sci-paper.component.html',
  styleUrls: ['./sci-paper.component.scss']
})
export class ScientificPaperComponent {
  papers: Record<number, ScientificPaperDto> = {};
  private hubConnection?: signalR.HubConnection;
  pageLen?: number;
  constructor(private route: ActivatedRoute, signalRService: SignalRService, private sciPaperService: ScientificPaperService) {
    this.route.params.subscribe(params => {
      const keywordId = params['idKeywords'];
      const arr: string[] = [];
      if (keywordId) {
        arr.push(keywordId);
        this.sciPaperService.scientificPaperGetByKeywordsGet(arr).subscribe((result) => {
          result.forEach((paper: any) => {
            this.papers[paper.id] = paper;
            // console.log(this.papers);
            this.pagedPapers = Object.values(this.papers).slice(0, 8);
          });
          this.pagedPapers = result.slice(0, 8);
          console.log(this.pagedPapers)
          this.pageLen = result.length;
          console.log(this.pageLen)
        });
      }
      else {
        const jwt = localStorage.getItem('jwt');
        if (jwt) {
          this.sciPaperService.scientificPaperGetAllUsersPapersGet().subscribe((result) => {
            result.forEach((paper: any) => {
              this.papers[paper.id] = paper;
              //console.log(this.papers);
            });
            this.pagedPapers = result.slice(0, 8);
            console.log(this.pagedPapers)
            this.pageLen = result.length;
            console.log(this.pageLen)
          });
        }
      }
    });
    const jwt = localStorage.getItem('jwt');
    if (jwt) {
      this.hubConnection = signalRService.startConnection();
      this.hubConnection?.on("AddedAsRole", (message) => {
        message.new = true;
        this.papers[message.id] = message;
        this.pageLen=Object.values(this.papers).length;
        this.pagedPapers = Object.values(this.papers).slice(0, 8);
        console.log(message);
        console.log(this.papers);
      });
      this.hubConnection?.on("EditedPaper", (message) => {
        message.new = true;
        this.papers[message.id] = message;
        this.pagedPapers = Object.values(this.papers).slice(0, 8);
        console.log(message);
        console.log(this.papers);
      });
    }

  }

  pagedPapers: any;

  onPageChanged(event: PageEvent) {
    const startIndex = event.pageIndex * event.pageSize;
    const endIndex = startIndex + event.pageSize;
    this.pagedPapers = Object.values(this.papers).slice(startIndex, endIndex);
  }
}