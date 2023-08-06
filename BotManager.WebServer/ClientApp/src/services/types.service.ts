import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {TypeInfoDto} from "../dto/type-info-dto";

@Injectable({
  providedIn: 'root'
})
export class TypesService {

  constructor(private http: HttpClient) { }

  public get(): Observable<TypeInfoDto[]> {
    return this.http.get<TypeInfoDto[]>(`/api/types`);
  }
}
