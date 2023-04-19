import { Component, OnInit, ViewChild } from '@angular/core';
import { ConfigService } from './config/config.service';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';

export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [ConfigService]
})
export class AppComponent implements OnInit {
  constructor(public service: ConfigService) {}
  displayedColumns: string[] = ['id', 'fullname', 'name', 'surname', 'email', 'gender', 'address', 'birthdate', 'username'];
  displayedColumnsGames: string[] = ['id', 'fullname', 'name', 'surname', 'email', 'gender', 'address', 'birthdate', 'username'];
  dataSource = new MatTableDataSource<PeriodicElement>([]);
  playedGames = new MatTableDataSource<PeriodicElement>([]);

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;

  ngAfterViewInit(){
    this.dataSource.paginator = this.paginator;
  }
  ngOnInit(){
    this.service.getUsers().subscribe(x => {
      this.dataSource = new MatTableDataSource<PeriodicElement>(x);
      this.dataSource.paginator = this.paginator;
    });
    this.service.getUsers().subscribe(x => console.log(x));
  }

  selectedRow(event: any){
    this.playedGames = new MatTableDataSource<PeriodicElement>(event.PlayedGames);
  }
}