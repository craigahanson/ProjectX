import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { switchMap } from 'rxjs/operators';
import { Person } from '../people/people.component'

@Component({
  selector: 'app-person',
  templateUrl: './person.component.html',
  styleUrls: ['./person.component.css']
})
export class PersonComponent implements OnInit {
  public person: Person;

  constructor(private route: ActivatedRoute, private router: Router, private http: HttpClient) {
  }

  ngOnInit() {
    let id = this.route.snapshot.paramMap.get('id');
    this.http.get<Person>('https://localhost:1001/api/People/' + id).subscribe(result => {
      this.person = result;
    }, error => console.error(error));
  }
}
