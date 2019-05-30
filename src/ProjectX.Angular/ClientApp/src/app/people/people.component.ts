import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-people',
  templateUrl: './people.component.html',
  styleUrls: ['./people.component.css']
})
export class PeopleComponent {
  public people: Person[];

  constructor(http: HttpClient) {
    http.get<Person[]>('https://localhost:1001/api/People').subscribe(result => {
      this.people = result;
    }, error => console.error(error));
  }
}

export interface Person {
  id: number;
  firstName: string;
  lastName: string;
}
