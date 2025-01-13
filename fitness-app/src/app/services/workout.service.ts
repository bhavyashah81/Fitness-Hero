import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class WorkoutService {
  private apiUrl = 'http://localhost:5251/api/workout';

  constructor(private http: HttpClient) {}

  getWorkouts(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
